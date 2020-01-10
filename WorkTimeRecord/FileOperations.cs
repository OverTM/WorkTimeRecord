using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;//打开外部程序用
using System.Runtime.InteropServices;//全局热键用using System;
using System.IO;
using System.Timers;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace FileOperations
{
    class FileOperationsClass
    {
        /// <summary>
        /// 全局变量
        /// </summary>
        public static string savePath;
        public static string StartWorkTime;

        /// <summary>
        /// 设置log文件路径
        /// </summary>
        /// <returns>log文件路径</returns>
        public static void SelectFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                savePath = dialog.SelectedPath;
                savePath = savePath.Replace(@"\", "/");
            }
            else
            {
                savePath = string.Empty;
            }
        }

        /// <summary>
        /// 屏幕锁定后的操作
        /// </summary>
        private static void ScreenLocked()
        {
            string sDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
            string Lock = "锁定时间：";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(savePath + "/log.txt", true))
            {
                file.WriteLine(Lock + sDateTime);
                file.Close();
            }
        }

        /// <summary>
        /// 屏幕解锁后的操作
        /// </summary>
        private static void ScreenUnlocked()
        {
            
            string sDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
            string UnLock = "解锁时间：";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(savePath + "/log.txt", true))
            {
                file.WriteLine(UnLock + sDateTime);
                file.Close();
            }
            ReadLog();
        }

        /// <summary>
        /// 读取log，找到昨天下班时间和今天上班时间，存入kaoqin.txt
        /// </summary>
        public static void ReadLog()
        {
            string sDate = DateTime.Today.ToString("yyyy-MM-dd");
            string EndWork = "";
            StreamReader sr = new StreamReader(savePath + "/log.txt");
            bool flag = false;
            while (!sr.EndOfStream)
            {
                string[] date;
                string lasttime = "not found";
                string tempdate = sr.ReadLine();
                date = tempdate.Split(new char[] { '：', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (date[1] == sDate)
                {
                    StartWorkTime = date[1] + " " + date[2];
                    flag = true;
                    break;
                }
                else
                {
                    lasttime = date[1] + " " + date[2];
                    flag = true;
                    EndWork = lasttime;
                }
            }
            sr.Close();
            if (flag)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(savePath + "/kaoqin.txt", true))
                {
                    file.WriteLine("下班时间：" + EndWork);
                    file.WriteLine(sDate);
                    file.WriteLine("上班时间：" + StartWorkTime);
                    file.Close();
                }
            }
        }

        /// <summary>
        /// 当天上班开始时间
        /// </summary>
        /// <returns>上班开始时间</returns>
        public static string Getdate()
        {
            StreamReader sr = new StreamReader(savePath + "/kaoqin.txt");
            string line = "";
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
            }
            string[] data = line.Split(new char[] { '：', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string starttime = data[2];
            return starttime;
        }

        /// <summary>
        /// 监控屏幕锁定与解锁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLock)
            {
                // 屏幕锁定
                ScreenLocked();
            }

            else if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionUnlock)
            {
                // 屏幕解锁
                ScreenUnlocked();
            }
        }

    }
}
