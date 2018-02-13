using System;
using System.Linq;

namespace IlbmReaderTest
{
    internal class Dlta
    {
        public Dlta(IffChunk innerIlbmChunk, Ilbm delta, IffFile iffFile)
        {
            var deltaFrom = iffFile.Ilbms.Last();
            var bmhd = iffFile.Ilbms.Single(x => x.Bmhd != null).Bmhd;
            if (bmhd == null)
            {
                throw new Exception("BMHD chunk not loaded error");
            }
            var pos = 0;
            var targetPos = 0;
            var writtenBytes = 0;
            //var actualNumberOfPlanes = deltaFrom.Body.ActualNumberOfPlanes;
            //var bytesPerRowPerPlane = deltaFrom.Body.BytesPerRowPerPlane;
            //var bytesPerRowAllPlanes = deltaFrom.Body.BytesPerRowAllPlanes;
            //var targetSize = bytesPerRowAllPlanes * deltaFrom.Bmhd.Height;

            InterleavedBitmapData = deltaFrom.Body != null ? 
                deltaFrom.Body.InterleavedBitmapData.ToArray() :
                deltaFrom.Dlta.InterleavedBitmapData.ToArray();// new byte[targetSize];


            //while (pos < innerIlbmChunk.ContentLength)
            ////while (targetPos < targetSize)
            //{
            //    var n = ContentReader.ReadSByte(innerIlbmChunk.Content, pos);
            //    pos++;

            //    if (n == -128)
            //    {
            //        throw new Exception("No operation?!?");
            //    }
            //    else
            //    if (n < 0)
            //    {
            //        var newn = -n;
            //        for (int i = 0; i <= newn; i++)
            //        {
            //            InterleavedBitmapData[targetPos++] = innerIlbmChunk.Content[pos];
            //        }
            //        writtenBytes += newn + 1;
            //        pos++;
            //    }
            //    else
            //    {
            //        for (int i = 0; i <= n; i++)
            //        {
            //            InterleavedBitmapData[targetPos++] = innerIlbmChunk.Content[pos++];
            //        }
            //        writtenBytes += n + 1;
            //    }
            //}
        }

        public byte[] InterleavedBitmapData { get; }
    }
}