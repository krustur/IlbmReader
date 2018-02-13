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
                var topLevelChunk2 = topLevelIterator.GetNextChunk();
                HandleFormChunk(topLevelChunk2, iffFile); // Assume top level chunks are form's for now
            }


            var ilbm = iffFile.Ilbms.FirstOrDefault();
            if (ilbm != null && ilbm.Bmhd != null && ilbm.Body != null)
            {
                var width = ilbm.Bmhd.Width;
                var height = ilbm.Bmhd.Height;
                var numberOfPlanes = ilbm.Bmhd.NumberOfPlanes;

                var bytesPerRowPerPlane = ilbm.Body.BytesPerRowPerPlane;
                var bytesPerRowAllPlanes = ilbm.Body.BytesPerRowAllPlanes;

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
                            var planeByte = ilbm.Body.InterleavedBitmapData[yoffset + xoffset + plane * bytesPerRowPerPlane];
                            var planeValue = ((planeByte & bitMask) >> xbit) << plane;
                            clutIndex = clutIndex + planeValue;

                        }
                        var pixelCol = ilbm.Cmap.Colors[clutIndex];
                        bitmap.SetPixel(pixelX, pixelY, pixelCol);
                    }
                }
                ilbm.Bitmap = bitmap;
            }


            return iffFile;
        }

        private void HandleFormChunk(IffChunk formChunk, IffFile iffFile)
        {
            if (formChunk.TypeId != "FORM")
            {
                _logger.Error($"Not a FORM type [{formChunk.TypeId}]");
                return;
            }

            var formType = ContentReader.ReadString(formChunk.Content, 0, 4);

            switch (formType)
            {
                case "ILBM":
                    var ilbm = new Ilbm();
                    var ilbmIterator = new IffChunkIterator(formChunk.Content, 4);
                    while (ilbmIterator.EndOfChunk() == false)
                    {
                        var innerIlbmChunk = ilbmIterator.GetNextChunk();
                        iffFile.AllChunks.Add(innerIlbmChunk);

                        HandleInnerIlbmChunk(innerIlbmChunk, ilbm);
                    }

                    iffFile.Ilbms.Add(ilbm);
                    break;
                case "ANIM":
                    iffFile.IsAnim = true;
                    var animIterator = new IffChunkIterator(formChunk.Content, 4);
                    while (animIterator.EndOfChunk() == false)
                    {
                        var innerIlbmChunk = animIterator.GetNextChunk();
                        iffFile.AllChunks.Add(innerIlbmChunk);

                        HandleFormChunk(innerIlbmChunk, iffFile);
                    }
                    break;
                default:
                    _logger.Information($"Unsupported FORM type [{formType}]");
                    break;
            }
        }

        public IffChunk ReadTopLevelChunk(string fileName)
        {
            var fileContent = File.ReadAllBytes(fileName);
            var iffChunk = new IffChunk(fileContent, 0);
            return iffChunk;
        }


        private void HandleInnerIlbmChunk(IffChunk innerIlbmChunk, Ilbm ilbm)
        {
            switch (innerIlbmChunk.TypeId)
            {
                case "ANNO":
                    ilbm.Anno = Encoding.UTF8.GetString(innerIlbmChunk.Content, 0, (int)innerIlbmChunk.ContentLength);
                    break;
                case "BMHD":
                    ilbm.Bmhd = new Bmhd(innerIlbmChunk);
                    break;
                case "CMAP":
                    ilbm.Cmap = new Cmap(innerIlbmChunk, ilbm);
                    break;
                case "CAMG":
                    ilbm.Camg = new Camg(innerIlbmChunk);
                    break;
                case "BODY":
                    ilbm.Body = new Body(innerIlbmChunk, ilbm);
                    break;
                case "DPPS":
                    //todo: Handle DPPS
                    _logger.Information($"Unsupported ILBM inner chunk [{innerIlbmChunk.TypeId}]");
                    break;
                case "FORM":
                    //todo: Handle inner FORMs
                    _logger.Information($"Unsupported ILBM inner chunk [{innerIlbmChunk.TypeId}]");
                    break;
                case "DRNG":
                    //DPaint IV enhanced color cycle chunk (EA)
                    // http://wiki.amigaos.net/wiki/ILBM_IFF_Interleaved_Bitmap
                    _logger.Information($"Unsupported ILBM inner chunk [{innerIlbmChunk.TypeId}]");
                    break;
                case "BRNG":
                    //unknown
                    _logger.Information($"Unsupported ILBM inner chunk [{innerIlbmChunk.TypeId}]");
                    break;
                case "CRNG":
                    // color register range
                    // http://wiki.amigaos.net/wiki/ILBM_IFF_Interleaved_Bitmap
                    _logger.Information($"Unsupported ILBM inner chunk [{innerIlbmChunk.TypeId}]");
                    break;
                case "DPI ":
                    // Dots per inch chunk
                    // http://wiki.amigaos.net/wiki/ILBM_IFF_Interleaved_Bitmap
                    _logger.Information($"Unsupported ILBM inner chunk [{innerIlbmChunk.TypeId}]");
                    break;
                case "GRAB":
                    // locates a “handle” or “hotspot”
                    // http://wiki.amigaos.net/wiki/ILBM_IFF_Interleaved_Bitmap
                    _logger.Information($"Unsupported ILBM inner chunk [{innerIlbmChunk.TypeId}]");
                    break;
                case "DPXT":
                    // unknown
                    _logger.Information($"Unsupported ILBM inner chunk [{innerIlbmChunk.TypeId}]");
                    break;
                case "TINY":
                    // Thumbnail
                    // https://en.m.wikipedia.org/wiki/ILBM
                    _logger.Information($"Unsupported ILBM inner chunk [{innerIlbmChunk.TypeId}]");
                    break;
                default:
                    _logger.Information($"Unsupported ILBM inner chunk [{innerIlbmChunk.TypeId}]");
                    break;
                    //throw new Exception($"Unknown inner Ilbm chunk type id [{innerIlbmChunk.TypeId}]");
            }

        }
    }
}
