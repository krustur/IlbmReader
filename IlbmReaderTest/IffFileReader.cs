using System.IO;

namespace IlbmReaderTest
{
    internal class IffFileReader
    {
        private byte[] _fileContent;

        public IffChunk ReadAsTopLevelChunk(string fileName)
        {
            _fileContent = File.ReadAllBytes(fileName);
            var iffChunk = new IffChunk(_fileContent, 0);
            return iffChunk;
        }
    }
}
