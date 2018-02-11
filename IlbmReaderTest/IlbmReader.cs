using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IlbmReaderTest
{
    public class IlbmReader
    {
        public delegate IlbmReader Factory();

        private readonly ILogger _logger;

        public IlbmReader(ILogger logger)
        {
            _logger = logger;
        }

        internal Ilbm Read(string fileName)
        {
            _logger.Information($"Loading IFF ILBM file {fileName}");

            var iffFile = new IffFileReader();
            var topLevelchunk = iffFile.ReadAsTopLevelChunk(fileName);
            if (topLevelchunk.TypeId != "FORM")
            {
                _logger.Error($"Unknown top level chunk type id [{topLevelchunk.TypeId}]");
                return null;
            }

            var ilbm = new Ilbm();

            var ilbmChunk = new IffChunk(topLevelchunk.Content, 0, topLevelchunk.ContentLength-4);

            var iffChunkParser = new IffChunkParser(ilbmChunk);
            var innerIlbmChunks = new List<IffChunk>();
            while (iffChunkParser.EndOfChunk() == false)
            {
                var innerIlbmChunk = iffChunkParser.GetNextChunk();
                innerIlbmChunks.Add(innerIlbmChunk);

                HandleInnerIlbmChunk(innerIlbmChunk, ilbm);
            }

            //if (iffFile.EndOfFile() != true)
            //{
            //    throw new System.Exception("IFF file not empty after top level group chunk!");
            //}

            //Get
            if (ilbm.Bmhd != null && ilbm.Body != null)
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
                        //var pixelCol = Color.FromArgb(pixelX & 0xff, pixelY & 0xff, 0);
                        //var clutIndex = ilbm.Body.InterleavedBitmaps[]

                        //clutIndex = pixelY & 0xff;

                        var pixelCol = ilbm.Cmap.Colors[clutIndex];
                        bitmap.SetPixel(pixelX, pixelY, pixelCol);
                    }
                }
                ilbm.Bitmap = bitmap;
            }


            return ilbm;
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
                    break;
                case "BODY":
                    ilbm.Body = new Body(innerIlbmChunk, ilbm);
                    break;
                case "DPPS":
                    //todo: Handle DPPS
                    break;
                case "FORM":
                    //todo: Handle inner FORMs
                    break;
                case "DRNG":
                    //DPaint IV enhanced color cycle chunk (EA)
                    // http://wiki.amigaos.net/wiki/ILBM_IFF_Interleaved_Bitmap
                    break;
                case "BRNG":
                    //unknown
                    break;
                case "CRNG":
                    // color register range
                    // http://wiki.amigaos.net/wiki/ILBM_IFF_Interleaved_Bitmap
                    break;
                case "DPI ":
                    // Dots per inch chunk
                    // http://wiki.amigaos.net/wiki/ILBM_IFF_Interleaved_Bitmap
                    break;
                case "GRAB":
                    // locates a “handle” or “hotspot”
                    // http://wiki.amigaos.net/wiki/ILBM_IFF_Interleaved_Bitmap
                    break;
                case "DPXT":
                    // unknown
                    break;
                case "TINY":
                    // Thumbnail
                    // https://en.m.wikipedia.org/wiki/ILBM
                    break;
                default:
                    throw new Exception($"Unknown inner Ilbm chunk type id [{innerIlbmChunk.TypeId}]");
            }

        }
    }
}
