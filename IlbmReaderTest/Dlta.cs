using System.Linq;

namespace IlbmReaderTest
{
    internal class Dlta
    {
        public Dlta(IffChunk ilbmChunk, Ilbm delta, IffFile iffFile)
        {
            var deltaFrom = iffFile.Ilbms.Last();
            var bmhd = iffFile.GetBmhd();
            var body = iffFile.GetBody();            

            InterleavedBitmapData = deltaFrom.Body != null ? 
                deltaFrom.Body.InterleavedBitmapData.ToArray() :
                deltaFrom.Dlta.InterleavedBitmapData.ToArray();

            switch (delta.Anhd.Operation)
            {
                case 4:
                    Operation4(ilbmChunk, body);
                    break;
                case 5:
                    Operation5(ilbmChunk, body);
                    break;
            }           
        }

        private void Operation5(IffChunk ilbmChunk, Body body)
        {
            /*
                ; This file contains a single function which is set up to be called from
                ; C.  Ie the parameters are on the stack instead of registers.
                ;       decode_vkplane(in, out, linebytes)
                ; where in is a bit-plane's worth of vertical-byte-run-with-skips data
                ; and out is a bit-plane that STILL has the image from last frame on it.
                ; Linebytes is the number of bytes-per-line in the out bitplane, and it
                ; should certainly be noted that the external pointer variable ytable
                ; must be initialized to point to a multiplication table of
                ; 0*linebytes, 1*linebytes ... n*linebytes  before this routine is called.
                ; 
                ; The format of "in":
                ;   Each column of the bitplane is compressed separately.  A 320x200
                ;   bitplane would have 40 columns of 200 bytes each.  The linebytes
                ;   parameter is used to count through the columns, it is not in the
                ;   "in" data, which is simply a concatenation of columns.
                ;
                ;   Each columns is an op-count followed by a number of ops.
                ;   If the op-count is zero, that's ok, it just means there's no change
                ;   in this column from the last frame.
                ;   The ops are of three classes, and followed by a varying amount of
                ;   data depending on which class.
                ;       1. Skip ops - this is a byte with the hi bit clear that says how many
                ;          rows to move the "dest" pointer forward, ie to skip. It is non-
                ;          zero
                ;       2. Uniq ops - this is a byte with the hi bit set.  The hi bit is
                ;          masked down and the remainder is a count of the number of bytes
                ;          of data to copy literally.  It's of course followed by the
                ;          data to copy.
                ;       3. Same ops - this is a 0 byte followed by a count byte, followed
                ;          by a byte value to repeat count times.
                ;   Do bear in mind that the data is compressed vertically rather than
                ;   horizontally, so to get to the next byte in the destination (out)
                ;   we add linebytes instead of one!
            */
            
            for (var plane = 0; plane < body.ActualNumberOfPlanes; plane++)
            {
                var planeOffset = plane * body.BytesPerRowPerPlane;
                
                var dataOffset = ContentReader.ReadUInt(ilbmChunk.Content, plane * 4);
                if (dataOffset == 0)
                {
                    // Undocumented, seem to work though!
                    continue;
                }

                for (var column = 0; column < body.BytesPerRowPerPlane; column++)
                {
                    var writeOffset = 0L;
                    var opCount = ContentReader.ReadUByte(ilbmChunk.Content, dataOffset++);

                    for (var iOp = 0; iOp < opCount; iOp++)
                    {
                        var op = ContentReader.ReadUByte(ilbmChunk.Content, dataOffset++);
                        if (op == 0)
                        {
                            // 3. Same ops
                            var repeatCount = ContentReader.ReadUByte(ilbmChunk.Content, dataOffset++);
                            var repeatValue = ContentReader.ReadUByte(ilbmChunk.Content, dataOffset++);
                            for (var iRpt = 0; iRpt < repeatCount; iRpt++)
                            {
                                ContentWriter.WriteUByte(InterleavedBitmapData, planeOffset + column + writeOffset, repeatValue);
                                writeOffset += body.BytesPerRowAllPlanes;
                            }

                        }
                        else if ((op & 0x80) != 0)
                        {
                            // 2. Uniq ops
                            var uniqueCount = op & 0x7f;
                            for (var iUnq = 0; iUnq < uniqueCount; iUnq++)
                            {
                                var uniqueValue = ContentReader.ReadUByte(ilbmChunk.Content, dataOffset++);
                                ContentWriter.WriteUByte(InterleavedBitmapData, planeOffset + column + writeOffset, uniqueValue);
                                writeOffset += body.BytesPerRowAllPlanes;
                            }
                        }
                        else
                        {
                            // 1. Skip ops
                            writeOffset += op * body.BytesPerRowAllPlanes;
                        }                        
                    }
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