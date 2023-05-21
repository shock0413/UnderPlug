using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Setting
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Utilities.ProcessChecker.IsOnlyProcess("Setting", "셋팅"))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Setting_Form());
            }
            else
            {
                MessageBox.Show("셋팅프로그램 실행중입니다.");
                return;
            }
        }
    }
}
