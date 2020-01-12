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
        public static string LogPath, Website, MainLocationX, MainLocationY, TriggerMethod;
        /// <summary>
        /// ini文件内的布尔设置变量
        /// </summary>
        public static bool isLocalTime;
    }
}
