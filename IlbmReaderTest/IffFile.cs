using System.Collections.Generic;

namespace IlbmReaderTest
{
    internal class IffFile
    {
        public IList<Ilbm> Ilbms { get; }
        public IList<IffChunk> AllChunks { get; }
        public bool IsAnim { get; internal set; }

        public IffFile()
        {
            Ilbms = new List<Ilbm>();
            AllChunks = new List<IffChunk>();
    }
}
}