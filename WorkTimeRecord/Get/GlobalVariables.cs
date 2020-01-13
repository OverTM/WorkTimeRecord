using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get
{
    class GlobalVariables
    {
        #region 保存在Settings.settings内
        /// <summary>
        /// 指示是否第一次启动
        /// </summary>
        public static bool isFirstStart = true;

        /// <summary>
        /// 文件保存路径
        /// </summary>
        public static string SavePath;

        /// <summary>
        /// 开始工作时间
        /// </summary>
        public static string StartWorkTime = "not found";
        #endregion

        /// <summary>
        /// ini文件内的设置变量
        /// </summary>
        public static string LogPath, TimeObtain, LocalIP, Website, MainLocationX, MainLocationY, TriggerMethod;
    }

    /// <summary>
    /// 时间
    /// </summary>
    static class NowTime
    {
        /// <summary>
        /// 年月日时分秒
        /// </summary>
        public static string FullTime;
        /// <summary>
        /// 年月日
        /// </summary>
        public static string YearMonthDay;
        /// <summary>
        /// 时分秒
        /// </summary>
        public static string HourMinuteSecond;
    }
}
