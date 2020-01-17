using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;

namespace Get
{
    class GetTime
    {
        #region 获取标准北京时间
        ///<summary>
        /// 获取标准北京时间
        ///</summary>
        ///<returns></returns>
        public static DateTime GetBeijingTime()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.baidu.com");
            /*或者试试下面这些
             * https://www.baidu.com
             * time.windows.com
             * time.asia.apple.com
             * asia.pool.ntp.org
             * ntp.nict.jp
             */
            request.Method = "HEAD";
            request.AllowAutoRedirect = false;
            HttpWebResponse reponse = (HttpWebResponse)request.GetResponse();
            string cc = reponse.GetResponseHeader("date");
            reponse.Close();
            DateTime time;
            bool s = GMTStrParse(cc, out time);
            return time.AddHours(8); //GMT要加8个小时才是北京时间
        }
        public static bool GMTStrParse(string gmtStr, out DateTime gmtTime)  //抓取的date是GMT格式的字符串，这里转成datetime
        {
            CultureInfo enUS = new CultureInfo("en-US");
            bool s = DateTime.TryParseExact(gmtStr, "r", enUS, DateTimeStyles.None, out gmtTime);
            return s;
        }
        #endregion

        #region 调用CMD获取局域网指定电脑时间
        public static string GetLocalNetworkTime()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(@"net time " + @GlobalVariables.LocalIP + " &exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令

            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();

            output = output.Split(new char[] { 'は', 'で' }, StringSplitOptions.RemoveEmptyEntries)[1];
            return output;
        }
        #endregion

        /// <summary>
        /// 刷新 NowTime
        /// </summary>
        public static void GetSelectedTime()
        {

            if(GlobalVariables.TimeObtain == "1")
            {
                NowTime.FullTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss");
            }
            else if(GlobalVariables.TimeObtain == "2")
            {
                NowTime.FullTime = GetLocalNetworkTime();
            }
            else
            {
                NowTime.FullTime = GetBeijingTime().ToString();
            }

            string[] ArrayTime = NowTime.FullTime.Split(new char[] { '/', ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
            NowTime.YearMonthDay = ArrayTime[0] +"/"+ ArrayTime[1] +"/"+ ArrayTime[2];
            NowTime.HourMinuteSecond = ArrayTime[3] + "：" + ArrayTime[4] + "：" + ArrayTime[5];
        }
    }
}
