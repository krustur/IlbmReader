namespace IlbmReaderTest
{
    public class Bmhd
    {
        public Bmhd(IffChunk innerIlbmChunk)
        {
            Width = ContentReader.ReadUShort(innerIlbmChunk.Content, 0);
            Height = ContentReader.ReadUShort(innerIlbmChunk.Content, 2);
            X = ContentReader.ReadShort(innerIlbmChunk.Content, 4);
            Y = ContentReader.ReadShort(innerIlbmChunk.Content, 6);
            NumberOfPlanes = ContentReader.ReadUByte(innerIlbmChunk.Content, 8);
            Masking = ContentReader.ReadUByte(innerIlbmChunk.Content, 9);
            Compression = ContentReader.ReadUByte(innerIlbmChunk.Content, 10);
            // UBYTE pad1
            TransparentColorNumber = ContentReader.ReadUShort(innerIlbmChunk.Content, 12);
            XAspect = ContentReader.ReadUByte(innerIlbmChunk.Content, 14);
            YAspect = ContentReader.ReadUByte(innerIlbmChunk.Content, 15);
            PageWidth = ContentReader.ReadShort(innerIlbmChunk.Content, 16);
            PageHeight = ContentReader.ReadShort(innerIlbmChunk.Content, 18);
        }

        public ushort Width { get; }
        public ushort Height { get; }
        public short X { get; }
        public short Y { get; }
        public byte NumberOfPlanes { get; }
        public byte Masking { get; }
        public byte Compression { get; }
        public ushort TransparentColorNumber { get; }
        public byte XAspect { get; }
        public byte YAspect { get; }
        public short PageWidth { get; }
        public short PageHeight { get; }
    }
}