using System;
using System.Windows.Forms;

namespace IlbmReaderTest
{
    public partial class Form1 : Form
    {
        private int _drawX = 0;
        private int _drawY = 0;

        public Form1()
        {
            InitializeComponent();
            LoadIlbm();
        }

        private void buttonLoadIlbm_Click(object sender, EventArgs e)
        {
            LoadIlbm();
        }

        private void LoadIlbm()
        {
            var depacker = new IlbmReader(@"D:\github\IlbmReader\Ilbm files\Erland\On_the_edge.lbm");

            var ilbm = depacker.Read();

            var gfx = CreateGraphics();
            gfx.DrawImageUnscaled(ilbm.Bitmap, _drawX, _drawY);
        }
       
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            _drawX = e.X;
            _drawY = e.Y;
        }
    }
}
