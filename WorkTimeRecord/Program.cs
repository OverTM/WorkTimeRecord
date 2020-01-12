using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Utility;
using System.Windows.Forms;

namespace WorkTimeRecord
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool bCreatedNew;
            Mutex m = new Mutex(false, "Product_Index_Cntvs", out bCreatedNew);
            if (bCreatedNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainMenu());
            }
            else
            {
                MessageBox.Show("该程序已经在运行");
            }
        }
    }
}
