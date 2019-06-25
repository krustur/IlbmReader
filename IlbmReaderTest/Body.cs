using System;

namespace IlbmReaderTest
{
    internal class Body
    {
        public Body(IffChunk innerIlbmChunk, Ilbm ilbm)
        {
            if (ilbm.Bmhd == null)
            {
                throw new Exception("BMHD chunk not loaded error");
            }
            var pos = 0;
            var targetPos = 0;
            var writtenBytes = 0;
            ActualNumberOfPlanes = ilbm.Bmhd.NumberOfPlanes;
            if (ilbm.Bmhd.Masking == 1)
            {
                ActualNumberOfPlanes++;
            }
            BytesPerRowPerPlane = ((ilbm.Bmhd.Width + 15) & 0xfffffff0) / 8;
            BytesPerRowAllPlanes = BytesPerRowPerPlane * ActualNumberOfPlanes;
            var targetSize = BytesPerRowAllPlanes * ilbm.Bmhd.Height;
            InterleavedBitmapData = new byte[targetSize];
            while (pos < innerIlbmChunk.ContentLength)
                //while (targetPos < targetSize)
            {
                var n = ContentReader.ReadSByte(innerIlbmChunk.Content, pos);
                pos++;

                if (n == -128)
                {
                    throw new Exception("No operation?!?");
                }
                else
                if (n < 0)
                {
                    var newn = -n;
                    for (int i = 0; i <= newn; i++)
                    {
                        InterleavedBitmapData[targetPos++] = innerIlbmChunk.Content[pos];
                    }
                    writtenBytes += newn + 1;
                    pos++;
                }
                else
                {
                    for (int i = 0; i <= n; i++)
                    {
                        InterleavedBitmapData[targetPos++] = innerIlbmChunk.Content[pos++];
                    }
                    writtenBytes += n + 1;
                }
            }            
        }
        
        public byte[] InterleavedBitmapData { get; }
        public byte ActualNumberOfPlanes { get; }
        public long BytesPerRowPerPlane { get; }
        public long BytesPerRowAllPlanes { get; }
    }
}