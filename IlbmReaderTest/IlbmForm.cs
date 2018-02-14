using System;
using System.Windows.Forms;
using Autofac;
using System.Linq;
using System.IO;

namespace IlbmReaderTest
{
    public partial class IlbmForm : Form
    {
        private readonly IffReader.Factory iffReaderFactory;

        public delegate IlbmForm Factory();

        public string IffFileName { get; set; }

        public IlbmForm(IffReader.Factory ilbmReaderFactory)
        {
            InitializeComponent();
            this.iffReaderFactory = ilbmReaderFactory;
        }

        private void IlbmForm_Load(object sender, EventArgs e)
        {
            var iffReader = iffReaderFactory();

            var iff = iffReader.Read(IffFileName);

            //foreach (var xxx in iff.Ilbms)
            //{
                
            //    var bitmapFile = $@"C:\temp\iffbitmaps\{Path.GetFileName(IffFileName)}_{DateTime.Now.Ticks}.bmp";
            //    xxx.Bitmap.Save(bitmapFile);
            //}
            Ilbm ilbm;
            if (iff.Ilbms.Count > 1)
            {
                var x = (new Random().Next() % iff.Ilbms.Count);
                ilbm = iff.Ilbms[x];
            }
            else
            {
                ilbm = iff.Ilbms.FirstOrDefault();
                
            }

            var bmhd = iff.GetBmhd();
            if (ilbm != null)
            {
                pictureBox1.Image = ilbm.Bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                Width = pictureBox1.Width = bmhd.Width;
                Height = pictureBox1.Height = bmhd.Height;
            }
        }
    }
}
