using System;
using System.Globalization;
using System.Net;

namespace Get
{
    class GetTime
    {
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

        public static DateTime GetSelectedTime()
        {
            DateTime NowTime = GlobalVariables.isLocalTime ? DateTime.Now : GetBeijingTime();
            return NowTime;
        }

    }
}
