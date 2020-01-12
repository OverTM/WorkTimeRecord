using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Get;

namespace Utility
{
    class MainSettings
    {
        /// <summary>
        /// ini文件路径
        /// </summary>
        string file = GlobalVariables.SavePath + "//Settings.ini";
        
        /// <summary>
        /// 恢复默认设置
        /// </summary>
        public void RestoreDefaultSettings()
        {
            GetIni.INIWriteValue(file, "path", "LogPath", "WorkSapce\\WorkTimeRecord\\setting_and_log    //路径");
            GetIni.INIWriteItems(file, "get_time", "isLocalTime = true   //取本机时间还是网络时间\0Website = https://www.baidu.com    //获取时间的网站");
            GetIni.INIWriteItems(file, "location", "MainLocationX = 1200 //屏幕左上角为原点，主窗体左上顶点的横轴坐标\0MainLocationY = 0  //屏幕左上角为原点，主窗体左上顶点的纵轴坐标");
            GetIni.INIWriteValue(file, "trigger_method", "TriggerMethod", "1    //1、通过系统锁屏与解锁触发 2、通过系统一段时间有无操作触发");
        }

        /// <summary>
        /// 去掉读出值的注释
        /// </summary>
        public void RemoveComments()
        {
            GlobalVariables.LogPath = GetIni.INIGetStringValue(file, "path", "LogPath", null);
            //去掉注释
            GlobalVariables.LogPath = GlobalVariables.LogPath.Split(new char[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

            string isLocalTime = GetIni.INIGetStringValue(file, "get_time", "isLocalTime", null);
            GlobalVariables.isLocalTime = bool.Parse(isLocalTime.Split(new char[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]);

            GlobalVariables.Website = GetIni.INIGetStringValue(file, "get_time", "Website", null);
            string[] Websites = GlobalVariables.Website.Split(new char[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //本身带有”//“，需要注意
            GlobalVariables.Website = Websites[0] + "//" + Websites[1];

            GlobalVariables.MainLocationX = GetIni.INIGetStringValue(file, "location", "MainLocationX", null);
            GlobalVariables.MainLocationX = GlobalVariables.MainLocationX.Split(new char[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

            GlobalVariables.MainLocationY = GetIni.INIGetStringValue(file, "location", "MainLocationY", null);
            GlobalVariables.MainLocationY = GlobalVariables.MainLocationY.Split(new char[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

            GlobalVariables.TriggerMethod = GetIni.INIGetStringValue(file, "trigger_method", "TriggerMethod", null);
            GlobalVariables.TriggerMethod = GlobalVariables.TriggerMethod.Split(new char[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }
    }
}
