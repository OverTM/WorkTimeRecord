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
            GetIni.INIWriteValue(file, "path", "LogPath", "E:\\WorkSapce\\WorkTimeRecord\\setting_and_log    //路径");
            GetIni.INIWriteItems(file, "get_time", "TimeObtain = 1    //1、本机时间  2、局域网时间  3、外网时间\0LocalIP = \\10.8.1.196   //可以是局域网内某IP地址或局域网内计算机名，也可以为空\0Website = https://www.baidu.com    //获取时间的网站");
            GetIni.INIWriteItems(file, "location", "MainLocationX = 1200 //屏幕左上角为原点，主窗体左上顶点的横轴坐标\0MainLocationY = 0  //屏幕左上角为原点，主窗体左上顶点的纵轴坐标");
            GetIni.INIWriteValue(file, "trigger_method", "TriggerMethod", "1    //1、通过系统锁屏与解锁触发 2、通过系统一段时间有无操作触发");
        }

        /// <summary>
        /// 读取并解析ini设置文件
        /// </summary>
        public void RemoveComments()
        {
            GlobalVariables.LogPath = GetIni.INIGetStringValue(file, "path", "LogPath", null);
            //去掉注释
            GlobalVariables.LogPath = GlobalVariables.LogPath.Split(new char[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

            GlobalVariables.TimeObtain = GetIni.INIGetStringValue(file, "get_time", "TimeObtain", null);
            GlobalVariables.TimeObtain = GlobalVariables.TimeObtain.Split(new char[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

            GlobalVariables.LocalIP = GetIni.INIGetStringValue(file, "get_time", "LocalIP ", null);
            GlobalVariables.LocalIP = GlobalVariables.LocalIP.Split(new char[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

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
