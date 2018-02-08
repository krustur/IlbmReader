using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IlbmReaderTest
{
    internal class IlbmReader
    {
        private readonly string _fileName;

        public IlbmReader(string fileName)
        {
            _fileName = fileName;
        }

        internal Ilbm Read()
        {
            var iffFile = new IffFileReader();
            var topLevelchunk = iffFile.ReadAsTopLevelChunk(_fileName);
            if (topLevelchunk.TypeId != "FORM")
            {
                throw new System.Exception($"Unknown top level chunk type id [{topLevelchunk.TypeId}]");
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
            var width = ilbm.Bmhd.Width;
            var height = ilbm.Bmhd.Height;
            var bitmap = new Bitmap(width, height);
            for (var pixelY = 0; pixelY < height; pixelY++)
            {
                for (var pixelX = 0; pixelX < width; pixelX++)
                {


                    //var pixelCol = Color.FromArgb(pixelX & 0xff, pixelY & 0xff, 0);
                    var pixelCol = ilbm.Cmap.Colors[pixelX & 0xff];
                    bitmap.SetPixel(pixelX, pixelY, pixelCol);
                }
            }

            ilbm.Bitmap = bitmap;

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
                    ilbm.Cmap = new Cmap(innerIlbmChunk);
                    break;
                case "CAMG":
                    break;
                case "BODY":
                    break;
                default:
                    throw new Exception($"Unknown inner Ilbm chunk type id [{innerIlbmChunk.TypeId}]");
            }

        }
    }
}
