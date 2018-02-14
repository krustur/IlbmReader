namespace IlbmReaderTest
{
    public class Anhd
    {
        public Anhd(IffChunk anhdChunk)
        {
            Operation = ContentReader.ReadUByte(anhdChunk.Content, 0);
            Mask = ContentReader.ReadUByte(anhdChunk.Content, 1);
            Width = ContentReader.ReadUShort(anhdChunk.Content, 2);
            Height = ContentReader.ReadUShort(anhdChunk.Content, 4);
            X = ContentReader.ReadShort(anhdChunk.Content, 6);
            Y = ContentReader.ReadShort(anhdChunk.Content, 8);
            Abstime = ContentReader.ReadUInt(anhdChunk.Content, 10);
            Reltime = ContentReader.ReadUInt(anhdChunk.Content, 14);
            Interleave = ContentReader.ReadUByte(anhdChunk.Content, 18);
            // UBYTE pad0
            Bits = ContentReader.ReadUInt(anhdChunk.Content, 20);
            // UBYTE pad[16]       
        }

        public byte Operation { get; }
        public byte Mask { get; }
        public ushort Width { get; }
        public ushort Height { get; }
        public short X { get; }
        public short Y { get; }
        public uint Abstime { get; }
        public uint Reltime { get; }
        public byte Interleave { get; }
        public uint Bits { get; }

        // Operation
        // 0	           set directly(normal ILBM BODY)
        // 1	           XOR ILBM mode
        // 2	           Long Delta mode
        // 3	           Short Delta mode
        // 4	           Generalized short/long Delta mode
        // 5	           Byte Vertical Delta mode
        // 6	           Stereo op 5 (third party)
        // 7	           short/long Vertical Delta mode
        // 74 (ASCII 'J')  reserved for Eric Graham's compression technique (details to be released later).

        // bits
        // bit # set =0	                       set =1
        // 0     short data                    long data
        // 1     set                           XOR
        // 2     separate info for each plane  one info list for all planes
        // 3     not RLC                       RLC(run length coded)
        // 4     horizontal                    vertical
        // 5     short info offsets            long info offsets

    }
}