namespace IlbmReaderTest
{
    internal class Camg
    {
       public Camg(IffChunk innerIlbmChunk)
        {
            // https://en.wikipedia.org/wiki/ILBM#CAMG:_Amiga_mode
            // Commodore Amiga viewport mode

            ViewportMode = ContentReader.ReadUInt(innerIlbmChunk.Content, 0);
        }

        public uint ViewportMode { get; }
    }
}