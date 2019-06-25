using System;

namespace IlbmReaderTest
{
    internal class IffChunkIterator
    {
        private byte[] _content;
        private int _pos;

        public IffChunkIterator(byte[] content)
        {
            _content = content;
            _pos = 0;
        }

        public IffChunkIterator(byte[] content, int pos)
        {
            _content = content;
            _pos = pos;
        }

        internal bool EndOfChunk()
        {
            if (_pos + 4 <= _content.Length)
            {
                var fileIdDizCheck = ContentReader.ReadString(_content, _pos, 4);
                if (fileIdDizCheck == "@BEG")
                {
                    // Stop reading when hitting @BEGIN_FILE_ID.DIZ
                    return true;
                }
            }

            return (_pos >= _content.Length);
        }

        internal IffChunk GetNextChunk()
        {
            var innerChunk = new IffChunk(_content, _pos);
            _pos += 8 + ((innerChunk.ContentLength + 1) & 0x7ffffffe);
            return innerChunk;

        }
    }
}