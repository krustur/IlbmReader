using System;
using System.Drawing;
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
            //AllowDrop = true;
            //DragEnter += new DragEventHandler(Form1_DragEnter);
            //DragDrop += new DragEventHandler(Form1_DragDrop);
            //LoadIlbm(@"D:\github\IlbmReader\Ilbm files\Erland\On_the_edge.lbm");
            //LoadIlbm(@"D:\github\IlbmReader\Ilbm files\Erland\Temp\Flare.iff");
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            var clientPoint = PointToClient(new Point(e.X, e.Y));


            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                LoadIlbm(files[0], clientPoint.X, clientPoint.Y);
            //foreach (string file in files) Console.WriteLine(file);
            }
        }

        private void buttonLoadIlbm_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            //openFileDialog.Title = "C# Corner Open File Dialog";
            openFileDialog.InitialDirectory = @"d:\github\IlbmReader\Ilbm Files";
            openFileDialog.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = false;

            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var fileName = openFileDialog.FileName;
                LoadIlbm(fileName, _drawX, _drawY);

            }
                    
        }

        private void LoadIlbm(string fileName, int x, int y)
        {

            var depacker = new IlbmReader(fileName);

            var ilbm = depacker.Read();

            if (ilbm.Bitmap != null)
            {
                var gfx = CreateGraphics();
                gfx.DrawImageUnscaled(ilbm.Bitmap, x, y);
            }
        }
       
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            _drawX = e.X;
            _drawY = e.Y;
        }
    }
}
