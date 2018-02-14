using System;
using System.Linq;

namespace IlbmReaderTest
{
    internal class Dlta
    {
        public Dlta(IffChunk ilbmChunk, Ilbm delta, IffFile iffFile)
        {
            var deltaFrom = iffFile.Ilbms.Last();
            var bmhd = iffFile.Ilbms.Single(x => x.Bmhd != null).Bmhd;
            if (bmhd == null)
            {
                throw new Exception("BMHD chunk not loaded error");
            }
            var body = iffFile.Ilbms.Single(x => x.Body != null).Body;
            
            //var pos = 0;
            //var targetPos = 0;
            //var writtenBytes = 0;
            //var actualNumberOfPlanes = deltaFrom.Body.ActualNumberOfPlanes;
            //var bytesPerRowPerPlane = deltaFrom.Body.BytesPerRowPerPlane;
            //var bytesPerRowAllPlanes = deltaFrom.Body.BytesPerRowAllPlanes;
            //var targetSize = bytesPerRowAllPlanes * deltaFrom.Bmhd.Height;

            InterleavedBitmapData = deltaFrom.Body != null ? 
                deltaFrom.Body.InterleavedBitmapData.ToArray() :
                deltaFrom.Dlta.InterleavedBitmapData.ToArray();// new byte[targetSize];

            switch (delta.Anhd.Operation)
            {
                case 4:
                    Operation4(ilbmChunk, body);
                    break;
                case 5:
                    Operation5(ilbmChunk, body);
                    break;
            }
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

        private void Operation5(IffChunk ilbmChunk, Body body)
        {

            return;
            int i;
            //WORD* ptr
            //WORD* planeptr
            //int s, size, nw
            //WORD *data
            //WORD *dest

            //LONG* deltadata = (LONG*)deltaword;
            var nw = body.BytesPerRowAllPlanes;// >> 1; // Maybe BytesPerRowPerPlane?

            for (i = 0; i < body.ActualNumberOfPlanes; i++)
            {
                //planeptr = (WORD*)(bm.Planes[i]);
                var planeOffset = i * body.BytesPerRowPerPlane;
                //data = deltaword + deltadata[i];
                var dataOffset = ContentReader.ReadUInt(ilbmChunk.Content, i * 4);
                //ptr = deltaword + deltadata[i + 8];
                var offsetListOffset = ContentReader.ReadUInt(ilbmChunk.Content, (8 * 4) + (i * 4));

                

                //while (*ptr != 0xFFFF)
                while (ContentReader.ReadUShort(ilbmChunk.Content, offsetListOffset) != 0xFFFF)
                {

                    //dest = planeptr + *ptr++;
                    var destOffset = ContentReader.ReadUShort(ilbmChunk.Content, offsetListOffset);
                    offsetListOffset += 2;

                    //size = *ptr++;
                    var size = ContentReader.ReadUShort(ilbmChunk.Content, offsetListOffset);
                    offsetListOffset += 2;

                    if (size < 0) 
                    {
                        for (var s = size; s < 0; s++) 
                        {
                            //*dest = *data;
                            ushort value = ContentReader.ReadUShort(ilbmChunk.Content, dataOffset); 
                            ContentWriter.WriteUShort(InterleavedBitmapData, planeOffset + destOffset, value);
                            //dest += nw;
                            planeOffset += nw;
                        }
                        //data++;
                        dataOffset += 2;
                    }
                    else
                    {
                        for (var s = 0; s < size; s++) 
                        {
                            //*dest = *data++;
                            ushort value = ContentReader.ReadUShort(ilbmChunk.Content, dataOffset);
                            ContentWriter.WriteUShort(InterleavedBitmapData, planeOffset + destOffset, value);
                            dataOffset += 2;
                            //dest += nw;
                            planeOffset += nw;
                        }
                    }
                    
                    //destOffset = ContentReader.ReadUShort(ilbmChunk.Content, offsetListOffset);
                }
            }
        }

        private void Operation4(IffChunk ilbmChunk, Body body)
        {

            int i;
            //WORD* ptr
            //WORD* planeptr
            //int s, size, nw
            //WORD *data
            //WORD *dest

            //LONG* deltadata = (LONG*)deltaword;
            var nw = body.BytesPerRowAllPlanes;// >> 1; // Maybe BytesPerRowPerPlane?

            for (i = 0; i < body.ActualNumberOfPlanes; i++)
            {
                //planeptr = (WORD*)(bm.Planes[i]);
                var planeOffset = i * body.BytesPerRowPerPlane;
                //data = deltaword + deltadata[i];
                var dataOffset = ContentReader.ReadUInt(ilbmChunk.Content, i * 4);
                //ptr = deltaword + deltadata[i + 8];
                var offsetListOffset = ContentReader.ReadUInt(ilbmChunk.Content, (8 * 4) + (i * 4));



                //while (*ptr != 0xFFFF)
                while (ContentReader.ReadUShort(ilbmChunk.Content, offsetListOffset) != 0xFFFF)
                {

                    //dest = planeptr + *ptr++;
                    var destOffset = ContentReader.ReadUShort(ilbmChunk.Content, offsetListOffset);
                    offsetListOffset += 2;

                    //size = *ptr++;
                    var size = ContentReader.ReadUShort(ilbmChunk.Content, offsetListOffset);
                    offsetListOffset += 2;

                    if (size < 0)
                    {
                        for (var s = size; s < 0; s++)
                        {
                            //*dest = *data;
                            ushort value = ContentReader.ReadUShort(ilbmChunk.Content, dataOffset);
                            ContentWriter.WriteUShort(InterleavedBitmapData, planeOffset + destOffset, value);
                            //dest += nw;
                            planeOffset += nw;
                        }
                        //data++;
                        dataOffset += 2;
                    }
                    else
                    {
                        for (var s = 0; s < size; s++)
                        {
                            //*dest = *data++;
                            ushort value = ContentReader.ReadUShort(ilbmChunk.Content, dataOffset);
                            ContentWriter.WriteUShort(InterleavedBitmapData, planeOffset + destOffset, value);
                            dataOffset += 2;
                            //dest += nw;
                            planeOffset += nw;
                        }
                    }

                    //destOffset = ContentReader.ReadUShort(ilbmChunk.Content, offsetListOffset);
                }
            }
        }

        public byte[] InterleavedBitmapData { get; }
    }
}