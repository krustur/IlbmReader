using System.Drawing;

namespace IlbmReaderTest
{
    internal class Ilbm
    {
        public Bitmap Bitmap { get; internal set; }
        public string Anno { get; internal set; }
        public Bmhd Bmhd { get; internal set; }
        public Cmap Cmap { get; internal set; }
        public Body Body { get; internal set; }
        public Camg Camg { get; internal set; }
        public Anhd Anhd { get; internal set; }
        public Dlta Dlta { get; internal set; }
        public bool IsPbm { get; internal set; }
    }
}