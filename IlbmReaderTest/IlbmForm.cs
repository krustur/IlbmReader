using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IlbmReaderTest
{
    public partial class IlbmForm : Form
    {
        public string IlbmFileName { get; set; }

        public IlbmForm()
        {
            InitializeComponent();

            
        }

        private void IlbmForm_Load(object sender, EventArgs e)
        {
            var depacker = new IlbmReader(IlbmFileName);

            var ilbm = depacker.Read();
            pictureBox1.Image = ilbm.Bitmap;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            Width = pictureBox1.Width = ilbm.Bmhd.Width * 2;
            Height = pictureBox1.Height = ilbm.Bmhd.Height * 2;
            
        }
    }
}
