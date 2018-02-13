using System;
using System.Windows.Forms;
using Autofac;
using System.Linq;

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
            var ilbm = iff.Ilbms.FirstOrDefault();
            if (ilbm != null && ilbm.Bmhd != null)
            {
                pictureBox1.Image = ilbm.Bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                Width = pictureBox1.Width = ilbm.Bmhd.Width;
                Height = pictureBox1.Height = ilbm.Bmhd.Height;
            }
        }
    }
}
