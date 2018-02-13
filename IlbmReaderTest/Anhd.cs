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
    }
}