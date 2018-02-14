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

        public static sbyte ReadSByte(byte[] content, int offset)
        {
            var result = (sbyte)content[offset + 0];
            return result;
        }

        public static byte ReadUByte(byte[] content, int offset)
        {
            var result = content[offset + 0];
            return result;
        }

        public static short ReadShort(byte[] content, int offset)
        {
            var result = (short)(
                (content[offset + 0]) * 0x0100 +
                (content[offset + 1]) * 0x0001
                );
            return result;
        }

        public static ushort ReadUShort(byte[] content, int offset)
        {
            var result = (ushort)(
                (content[offset + 0]) * 0x0100 +
                (content[offset + 1]) * 0x0001
                );
            return result;
        }

        public static ushort ReadUShort(byte[] content, uint offset)
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

    public class ContentWriter
    {
        public static void WriteUShort(byte[] content, long offset, ushort value)
        {
            var byte2 = (byte)(value >> 8);
            var byte1 = (byte)(value & 0xff);

            content[offset + 0] = byte2;
            content[offset + 1] = byte1;
        }
    }
}
