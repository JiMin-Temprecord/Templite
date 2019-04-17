using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace TempLite
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Assembly.LoadFrom("FTD2XX_NET.dll");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TempLite());
        }
    }
}
