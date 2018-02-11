using System;
using System.Windows.Forms;
using Autofac;

namespace IlbmReaderTest
{
    public partial class IlbmForm : Form
    {
        private readonly IlbmReader.Factory ilbmReaderFactory;

        public delegate IlbmForm Factory();

        public string IlbmFileName { get; set; }

        public IlbmForm(IlbmReader.Factory ilbmReaderFactory)
        {
            InitializeComponent();
            this.ilbmReaderFactory = ilbmReaderFactory;
        }

        private void IlbmForm_Load(object sender, EventArgs e)
        {
            var ilbmReader = ilbmReaderFactory();

            var ilbm = ilbmReader.Read(IlbmFileName);
            if (ilbm.Bmhd != null)
            {
                pictureBox1.Image = ilbm.Bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                Width = pictureBox1.Width = ilbm.Bmhd.Width;
                Height = pictureBox1.Height = ilbm.Bmhd.Height;
            }
        }
    }
}
