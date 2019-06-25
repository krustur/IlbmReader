using Autofac;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace IlbmReaderTest
{
    public partial class MainForm : Form
    {
        private readonly IlbmForm.Factory _ilbmFormFactory;

        public delegate MainForm Factory();

        public MdiClientPanel MdiClientPanel { get; private set; }

        public MainForm(IlbmForm.Factory ilbmFormFactory)
        {
            _ilbmFormFactory = ilbmFormFactory;

            InitializeComponent();
            //AllowDrop = true;
            //DragEnter += new DragEventHandler(Form1_DragEnter);
            //DragDrop += new DragEventHandler(Form1_DragDrop);
            //LoadIlbm(@"D:\github\IlbmReader\Ilbm files\Erland\On_the_edge.lbm");
            //LoadIlbm(@"D:\github\IlbmReader\Ilbm files\Erland\Temp\Flare.iff");

            
           

            MdiClientPanel = new MdiClientPanel
            {
                Width = splitContainer1.Panel2.Width,
                Height = splitContainer1.Panel2.Height,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
            this.splitContainer1.Panel2.Controls.Add(MdiClientPanel);

        }

        void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var clientPoint = PointToClient(new Point(e.X, e.Y));


            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                LoadIlbm(file);
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
                LoadIlbm(fileName);

            }
                    
        }

        private void LoadIlbm(string fileName)
        {
            //try
            //{
                var form = _ilbmFormFactory();

                form.IffFileName = fileName;
                form.MdiParent = this.MdiClientPanel.MdiForm;
                form.ShowInTaskbar = false;

                form.Show();
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.ToString());
            //}
        }       
    }
}
