using System;

namespace IlbmReaderTest
{
    public class IffChunk
    {
        public string TypeId { get; private set; }
        public int ContentLength { get; private set; }
        public byte[] Content { get; private set;}

        public IffChunk(byte[] chunkContent, int offset)
        {
            TypeId = GetChunkTypeId(chunkContent, offset);
            ContentLength = GetChunkSize(chunkContent, offset);
            Content = GetChunkContent(chunkContent, offset+8, ContentLength);
        }

        public IffChunk(byte[] chunkContent, int offset, int length)
        {
            TypeId = GetChunkTypeId(chunkContent, offset);
            ContentLength = length;
            Content = GetChunkContent(chunkContent, offset+4, ContentLength);
        }

        private string GetChunkTypeId(byte[] fileBytes, int offset)
        {
            var result = ContentReader.ReadString(fileBytes, offset, 4);

            return result;
        }

        private int GetChunkSize(byte[] content, int offset)
        {
            var result = ContentReader.ReadInt(content, offset+4);
            return result;
        }

        private byte[] GetChunkContent(byte[] chunkContent, int offset, int length)
        {
            var result = new byte[length];
            Array.Copy(chunkContent, offset, result, 0, length);
            return result;
        }

    }
}
