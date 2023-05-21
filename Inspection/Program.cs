using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Utilities;

namespace Inspection
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (ProcessChecker.IsOnlyProcess("Inspection", "Main") || true)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main_Form());
            }
            else 
            {
                MessageBox.Show("Inspection Strart Error");
                return;
            }
        }
    }
}
