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

            ActualNumberOfPlanes = ilbm.Bmhd.NumberOfPlanes;
            if (ilbm.Bmhd.Masking == 1)
            {
                ActualNumberOfPlanes++;
            }

            BytesPerRowPerPlane = ((ilbm.Bmhd.Width + 15) & 0xfffffff0) / 8;
            BytesPerRowAllPlanes = BytesPerRowPerPlane * ActualNumberOfPlanes;
            var targetSize = BytesPerRowAllPlanes * ilbm.Bmhd.Height;
            InterleavedBitmapData = new byte[targetSize];

            switch (ilbm.Bmhd.Compression)
            {
                case 0:
                {
                    if (targetSize != innerIlbmChunk.Content.Length)
                    {
                        throw new Exception("Expected uncompressed data not equal to chunk content length");
                    }

                    for (int i = 0; i < targetSize; i++)
                    {
                        InterleavedBitmapData[i] = innerIlbmChunk.Content[i];
                    }

                    break;
                }
                case 1:
                {
                    var pos = 0;
                    var targetPos = 0;
                    var writtenBytes = 0;

                    while (pos < innerIlbmChunk.ContentLength)
                    //while (targetPos < targetSize)
                    {
                        //if (pos > 224382 - 10)
                        //{

                        //}
                        var n = ContentReader.ReadSByte(innerIlbmChunk.Content, pos);
                        pos++;

                        //if (n == 0)
                        //{
                        //    // noop?
                        //}
                        if (n == -128)
                        {
                            throw new Exception("No operation?!?");
                        }
                        else if (n < 0)
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
                                if (pos >= innerIlbmChunk.ContentLength)
                                {
                                    return;
                                }
                                InterleavedBitmapData[targetPos++] = innerIlbmChunk.Content[pos++];
                            }

                            writtenBytes += n + 1;
                        }
                    }

                    break;
                }
                default:
                    throw new Exception($"BMHD compression {ilbm.Bmhd.Compression} is not supported");
            }
        }
        
        public byte[] InterleavedBitmapData { get; }
        public byte ActualNumberOfPlanes { get; }
        public long BytesPerRowPerPlane { get; }
        public long BytesPerRowAllPlanes { get; }
    }
}