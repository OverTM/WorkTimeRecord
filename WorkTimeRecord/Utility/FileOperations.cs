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
using Get;
using System.Threading;
using WorkTimeRecord;

namespace Utility
{
    class FileOperations
    {
        /// <summary>
        /// 设置log文件路径
        /// </summary>
        /// <returns>log文件路径</returns>
        public static void SelectFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择配置文件保存路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                GlobalVariables.SavePath = dialog.SelectedPath;
                GlobalVariables.SavePath = GlobalVariables.SavePath.Replace(@"\", "//");
                DialogResult result = MessageBox.Show("是否需要下当前路径下生成一个设置文件？\n如果当前路径下没有设置文件，请选择是，将按默认设置生成一个文件。", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    //相应的逻辑处理
                    MainSettings settings = new MainSettings();
                    settings.RestoreDefaultSettings();
                }
                else
                {
                    //相应的逻辑处理
                }
            }
            else
            {
                MessageBox.Show("设置失败,请重新设置");
                SelectFolder();
            }
            Settings.Default.SavePath = GlobalVariables.SavePath;
            Settings.Default.Save();
        }
        
        /// <summary>
        /// 重启程序
        /// </summary>
        private static void Restart()
        {
            Application.ExitThread();

            Thread thtmp = new Thread(new ParameterizedThreadStart(Run));
            object appName = Application.ExecutablePath;
            Thread.Sleep(2000);
            thtmp.Start(appName);
        }

        /// <summary>
        /// 启动程序
        /// </summary>
        /// <param name="obj"></param>
        private static void Run(Object obj)
        {
            Process ps = new Process();
            ps.StartInfo.FileName = obj.ToString();
            ps.Start();
        }

        /// <summary>
        /// 工作开始后的操作
        /// </summary>
        public static void Start()
        {
            GetTime.GetSelectedTime();
            string Lock = "工作开始时间：";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(GlobalVariables.SavePath + "/log.txt", true))
            {
                file.WriteLine(Lock + NowTime.FullTime);
                file.Close();
            }
        }

        /// <summary>
        /// 工作结束后的操作
        /// </summary>
        public static void End()
        {
            GetTime.GetSelectedTime();
            string UnLock = "工作结束时间：";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(GlobalVariables.SavePath + "/log.txt", true))
            {
                file.WriteLine(UnLock + NowTime.FullTime);
                file.Close();
            }
            ReadLog();
        }

        /// <summary>
        /// 读取log，找到昨天下班时间和今天上班时间，存入kaoqin.txt
        /// </summary>
        public static void ReadLog()
        {
            string EndWorkTime = "Not found";
            StreamReader sr = new StreamReader(GlobalVariables.SavePath + "/log.txt");
            bool flag = false;
            string lasttime = "not found";
            while (!sr.EndOfStream)
            {
                string[] date;
                string tempdate = sr.ReadLine();
                date = tempdate.Split(new char[] { '：', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (date[1] == NowTime.YearMonthDay)
                {
                    GlobalVariables.StartWorkTime = date[1] + " " + date[2];
                    flag = true;
                    break;
                }
                else
                {
                    lasttime = date[1] + " " + date[2];
                    flag = false;
                    EndWorkTime = lasttime;
                }
            }
            sr.Close();
            if (flag)
            {
                bool WriteInToKaoqin = false;
                if (File.Exists(GlobalVariables.SavePath + "/kaoqin.txt"))
                {
                    StreamReader kq = new StreamReader(GlobalVariables.SavePath + "/kaoqin.txt");
                    while(!kq.EndOfStream)
                    {
                        string[] date;
                        string tempdate = kq.ReadLine();
                        date = tempdate.Split(new char[] { '：', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if(date[1] == NowTime.YearMonthDay)
                        {
                            WriteInToKaoqin = false;
                            break;
                        }
                        else
                        {
                            WriteInToKaoqin = true;
                        }
                    }
                    kq.Close();
                }
                else
                {
                    WriteInToKaoqin = true;
                }
                
                if(WriteInToKaoqin)
                {
                    using (System.IO.StreamWriter kq = new System.IO.StreamWriter(GlobalVariables.SavePath + "/kaoqin.txt", true))
                    {
                        kq.WriteLine("下班时间：" + EndWorkTime);
                        kq.WriteLine("上班时间：" + GlobalVariables.StartWorkTime);
                        kq.Close();
                        Settings.Default.StartWorkTime = GlobalVariables.StartWorkTime;
                        //WorkTimeRecord.MainMenu mainMenu = new WorkTimeRecord.MainMenu();
                        //mainMenu.UpDataForm();
                        Settings.Default.StartWorkTime = GlobalVariables.StartWorkTime;
                        Settings.Default.Save();
                        Restart();
                    }
                }
            }
        }

        /// <summary>
        /// 当天上班开始时间
        /// </summary>
        /// <returns>上班开始时间</returns>
        public static string Getdate()
        {
            StreamReader sr = new StreamReader(GlobalVariables.SavePath + "/kaoqin.txt");
            string line = "";
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
            }
            string[] data = line.Split(new char[] { '：', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string starttime = data[2];
            return starttime;
        }
    }
}
