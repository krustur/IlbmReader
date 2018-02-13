using Autofac;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IlbmReaderTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var logger = LogConfiguration.Create();
            var container = IocConfiguration.Configure(logger);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainFormFactory = container.Resolve<MainForm.Factory>();
            var form = mainFormFactory.Invoke();
            Application.Run(form);
        }
    }
}
