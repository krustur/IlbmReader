using System;

namespace IlbmReaderTest
{
    internal class IffChunkParser
    {
        private IffChunk _chunk;
        private int _pos;

        public IffChunkParser(IffChunk chunk)
        {
            _chunk = chunk;
            _pos = 0;
        }

        internal bool EndOfChunk()
        {
            return (_pos >= _chunk.ContentLength);
        }

        internal IffChunk GetNextChunk()
        {
            var innerChunk = new IffChunk(_chunk.Content, _pos);
            _pos += 8 + innerChunk.ContentLength;
            return innerChunk;

        }
    }
}