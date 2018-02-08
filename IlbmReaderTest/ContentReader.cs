using System;
using System.Text;

namespace IlbmReaderTest
{
    public class ContentReader
    {
        public static string ReadString(byte[] content, int offset, int length)
        {
            return Encoding.UTF8.GetString(content, offset, length);
        }

        internal static sbyte ReadSByte(byte[] content, int offset)
        {
            var result = (sbyte)content[offset + 0];
            return result;
        }

        internal static byte ReadUByte(byte[] content, int offset)
        {
            var result = content[offset + 0];
            return result;
        }

        internal static short ReadShort(byte[] content, int offset)
        {
            var result = (short)(
                (content[offset + 0]) * 0x0100 +
                (content[offset + 1]) * 0x0001
                );
            return result;
        }

        internal static ushort ReadUShort(byte[] content, int offset)
        {
            var result = (ushort)(
                (content[offset + 0]) * 0x0100 +
                (content[offset + 1]) * 0x0001
                );
            return result;
        }

        public static int ReadInt(byte[] content, int offset)
        {
            var result =
                (content[offset + 0]) * 0x01000000 +
                (content[offset + 1]) * 0x00010000 +
                (content[offset + 2]) * 0x00000100 +
                (content[offset + 3]) * 0x00000001;
            return result;
        }

        public static uint ReadUInt(byte[] content, int offset)
        {
            var result = 
                ((uint)content[offset + 0]) * 0x01000000 +
                ((uint)content[offset + 1]) * 0x00010000 +
                ((uint)content[offset + 2]) * 0x00000100 +
                ((uint)content[offset + 3]) * 0x00000001;
            return result;
        }
    }
}
