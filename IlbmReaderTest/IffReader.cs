using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace IlbmReaderTest
{
    public class IffReader
    {
        public delegate IffReader Factory();

        private readonly ILogger _logger;

        public IffReader(ILogger logger)
        {
            _logger = logger;
        }

        internal IffFile Read(string fileName)
        {
            _logger.Information($"Loading IFF ILBM file {fileName}");
            
            var fileContent = File.ReadAllBytes(fileName);
            var topLevelIterator = new IffChunkIterator(fileContent);
            var iffFile = new IffFile();

            while (topLevelIterator.EndOfChunk() == false)
            {
                var topLevelChunk = topLevelIterator.GetNextChunk();
                HandleChunk(topLevelChunk, iffFile); // Assume top level chunks are form's for now
            }

            var bmhd = iffFile.GetBmhd();
            var body = iffFile.GetBody();
            var cmap = iffFile.GetCmap();
            foreach (var ilbm in iffFile.Ilbms)
            {
                //var ilbm = iffFile.Ilbms.FirstOrDefault();
                var interleavedBitmapData = ilbm.Body != null ? ilbm.Body.InterleavedBitmapData : ilbm.Dlta.InterleavedBitmapData;
                {
                    var width = bmhd.Width;
                    var height = bmhd.Height;
                    var numberOfPlanes = bmhd.NumberOfPlanes;

                    var bytesPerRowPerPlane = body.BytesPerRowPerPlane;
                    var bytesPerRowAllPlanes = body.BytesPerRowAllPlanes;

                    var bitmap = new Bitmap(width, height);

                    for (var pixelX = 0; pixelX < width; pixelX++)
                    {
                        var xoffset = pixelX / 8;
                        var xbit = (~pixelX) & 0x00000007;
                        var bitMask = 0x01 << xbit;

                        for (var pixelY = 0; pixelY < height; pixelY++)
                        {
                            var yoffset = pixelY * bytesPerRowAllPlanes;
                            int clutIndex = 0;
                            for (int plane = 0; plane < numberOfPlanes; plane++)
                            {
                                var planeByte = interleavedBitmapData[yoffset + xoffset + plane * bytesPerRowPerPlane];
                                var planeValue = ((planeByte & bitMask) >> xbit) << plane;
                                clutIndex = clutIndex + planeValue;

                            }

                            //if (bmhd.NumberOfPlanes == 24 && clutIndex > 0) { }
                            Color pixelCol;
                            if (bmhd.NumberOfPlanes == 24)
                            {
                                int r = (clutIndex & 0x00ff0000) >> 16;
                                int g = (clutIndex & 0x0000ff00) >> 8;
                                int b = clutIndex & 0x000000ff;
                                pixelCol = Color.FromArgb(r, g, b);
                            }
                            else
                            {
                                pixelCol = cmap.Colors[clutIndex];
                            }
                            bitmap.SetPixel(pixelX, pixelY, pixelCol);
                        }
                    }
                    ilbm.Bitmap = bitmap;
                }
            }


            return iffFile;
        }

        private void HandleChunk(IffChunk chunk, IffFile iffFile, Ilbm ilbm = null)
        {
            switch (chunk.TypeId)
            {
                case "FORM":
                    var formType = ContentReader.ReadString(chunk.Content, 0, 4);

                    switch (formType)
                    {
                        case "ILBM":
                        case "PBM ":
                            ilbm = new Ilbm();
                            var ilbmIterator = new IffChunkIterator(chunk.Content, 4);
                            while (ilbmIterator.EndOfChunk() == false)
                            {
                                var innerIlbmChunk = ilbmIterator.GetNextChunk();
                                iffFile.AllChunks.Add(innerIlbmChunk);

                                HandleChunk(innerIlbmChunk, iffFile, ilbm);
                            }

                            iffFile.Ilbms.Add(ilbm);
                            break;
                        case "ANIM":
                            iffFile.IsAnim = true;
                            var animIterator = new IffChunkIterator(chunk.Content, 4);
                            while (animIterator.EndOfChunk() == false)
                            {
                                var innerIlbmChunk = animIterator.GetNextChunk();
                                iffFile.AllChunks.Add(innerIlbmChunk);

                                HandleChunk(innerIlbmChunk, iffFile);
                            }
                            break;
                        default:
                            _logger.Information($"Unsupported FORM type [{formType}]");
                            break;
                    }

                    break;

                case "ANNO":
                    ilbm.Anno = Encoding.UTF8.GetString(chunk.Content, 0, (int)chunk.ContentLength);
                    break;
                case "BMHD":
                    ilbm.Bmhd = new Bmhd(chunk);
                    break;
                case "CMAP":
                    ilbm.Cmap = new Cmap(chunk, ilbm);
                    break;
                case "CAMG":
                    ilbm.Camg = new Camg(chunk);
                    break;
                case "BODY":
                    ilbm.Body = new Body(chunk, ilbm);
                    break;
                case "ANHD":
                    ilbm.Anhd = new Anhd(chunk);
                    break;
                case "DLTA":
                    ilbm.Dlta = new Dlta(chunk, ilbm, iffFile);
                    break;

                case "DPPS":
                    //todo: Handle DPPS
                    _logger.Information($"Unsupported ILBM inner chunk [{chunk.TypeId}]");
                    break;
                //case "FORM":
                //    //todo: Handle inner FORMs
                //    _logger.Information($"Unsupported ILBM inner chunk [{chunk.TypeId}]");
                //    break;
                case "DRNG":
                    //DPaint IV enhanced color cycle chunk (EA)
                    // http://wiki.amigaos.net/wiki/ILBM_IFF_Interleaved_Bitmap
                    _logger.Information($"Unsupported ILBM inner chunk [{chunk.TypeId}]");
                    break;
                case "BRNG":
                    //unknown
                    _logger.Information($"Unsupported ILBM inner chunk [{chunk.TypeId}]");
                    break;
                case "CRNG":
                    // color register range
                    // http://wiki.amigaos.net/wiki/ILBM_IFF_Interleaved_Bitmap
                    _logger.Information($"Unsupported ILBM inner chunk [{chunk.TypeId}]");
                    break;
                case "DPI ":
                    // Dots per inch chunk
                    // http://wiki.amigaos.net/wiki/ILBM_IFF_Interleaved_Bitmap
                    _logger.Information($"Unsupported ILBM inner chunk [{chunk.TypeId}]");
                    break;
                case "GRAB":
                    // locates a “handle” or “hotspot”
                    // http://wiki.amigaos.net/wiki/ILBM_IFF_Interleaved_Bitmap
                    _logger.Information($"Unsupported ILBM inner chunk [{chunk.TypeId}]");
                    break;
                case "DPXT":
                    // unknown
                    _logger.Information($"Unsupported ILBM inner chunk [{chunk.TypeId}]");
                    break;
                case "TINY":
                    // Thumbnail
                    // https://en.m.wikipedia.org/wiki/ILBM
                    _logger.Information($"Unsupported ILBM inner chunk [{chunk.TypeId}]");
                    break;

                default:
                    _logger.Information($"Unsupported ILBM inner chunk [{chunk.TypeId}]");
                    break;
                    //throw new Exception($"Unknown inner Ilbm chunk type id [{innerIlbmChunk.TypeId}]");
            }
        }

        public IffChunk ReadTopLevelChunk(string fileName)
        {
            var fileContent = File.ReadAllBytes(fileName);
            var iffChunk = new IffChunk(fileContent, 0);
            return iffChunk;
        }
    }
}
