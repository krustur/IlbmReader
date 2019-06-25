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
        private IffFile _iff;
        private int _frame;
        private Timer _timer;

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

            _iff = iffReader.Read(IffFileName);
            _frame = 0;

            this.Text = string.Format("{0}{1} [{2}x{3}x{4}] [{5}x{6}]",
                _iff.IsAnim ? "Anim-" : "", 
                Path.GetFileName(IffFileName),
                _iff?.GetBmhd()?.Width,
                _iff?.GetBmhd()?.Height,
                _iff?.GetBmhd()?.NumberOfPlanes,
                _iff?.GetBmhd()?.PageWidth,
                _iff?.GetBmhd()?.PageHeight
            );

            //foreach (var xxx in iff.Ilbms)
            //{

            //    var bitmapFile = $@"C:\temp\iffbitmaps\{Path.GetFileName(IffFileName)}_{DateTime.Now.Ticks}.bmp";
            //    xxx.Bitmap.Save(bitmapFile);
            //}
            UpdateImage();

            if (_iff.IsAnim)
            {
                _timer = new Timer()
                {
                    Interval = 1 * 1000 / 50,
                };
                _timer.Tick += AnimationTick;
                _timer.Start();
            }

        }

        private void AnimationTick(object sender, EventArgs e)
        {
            _frame++;
            _frame = _frame % _iff.Ilbms.Count;
            UpdateImage(resize: false);
        }

        private void UpdateImage(bool resize = true)
        {
            Ilbm ilbm;
            /*
            if (_iff.Ilbms.Count > 1)
            {
                var x = (new Random().Next() % _iff.Ilbms.Count);
                ilbm = _iff.Ilbms[x];
            }
            else
            {
            */
                ilbm = _iff.Ilbms[_frame];

            //}
            var bmhd = _iff.GetBmhd();
            if (ilbm != null)
            {
                pictureBox1.Image = ilbm.Bitmap;
                if (resize)
                {
                    Width = bmhd.Width + 16;
                    Height = bmhd.Height + 39;
                }
            }
        }
    }
}
