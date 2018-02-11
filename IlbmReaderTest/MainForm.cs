using System;
using System.Drawing;
using System.Windows.Forms;

namespace IlbmReaderTest
{
    public partial class MainForm : Form
    {
        public MdiClientPanel MdiClientPanel { get; private set; }

        public MainForm()
        {
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
            var form = new IlbmForm()
            {
                IlbmFileName = fileName,
                //Parent = this,
                MdiParent = this.MdiClientPanel.MdiForm,
                ShowInTaskbar = false,
            };
            form.Show();
        }       
    }

    public class MdiClientPanel : Panel
    {
        private Form mdiForm;
        private MdiClient ctlClient = new MdiClient();

        public MdiClientPanel()
        {
            base.Controls.Add(this.ctlClient);
        }

        public Form MdiForm
        {
            get
            {
                if (this.mdiForm == null)
                {
                    this.mdiForm = new Form();
                    /// set the hidden ctlClient field which is used to determine if the form is an MDI form
                    System.Reflection.FieldInfo field = typeof(Form).GetField("ctlClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    field.SetValue(this.mdiForm, this.ctlClient);
                }
                return this.mdiForm;
            }
        }
    }
}
