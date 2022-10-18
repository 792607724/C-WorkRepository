using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwitchResolutionTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();


        static void Main()
        {

           /// AllocConsole();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestForm());
            ////FreeConsole();
        }
    }
}
