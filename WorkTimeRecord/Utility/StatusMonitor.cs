using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Get;

namespace Utility
{
    class StatusMonitor
    {
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
                FileOperations.Start();
            }

            else if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionUnlock)
            {
                // 屏幕解锁
                FileOperations.End();
            }
        }

        #region 键盘和鼠标没有操作所经过的时间
        // 创建结构体用于返回捕获时间
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            // 设置结构体块容量
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            // 捕获的时间
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }

        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// 获取键盘和鼠标没有操作所经过的时间
        /// </summary>
        /// <returns>键盘和鼠标没有操作所经过的时间 (单位:毫秒) </returns>
        public static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            // 捕获时间
            if (!GetLastInputInfo(ref vLastInputInfo))
                return 0;
            else
                return Environment.TickCount - (long)vLastInputInfo.dwTime;
        }
        #endregion
        private void TriggerMethod(object sender, EventArgs e)
        {
            if(GlobalVariables.TriggerMethod == "1")
            {
                SystemEvents.SessionSwitch += new
                SessionSwitchEventHandler(StatusMonitor.SystemEvents_SessionSwitch);
            }
            else
            {
                while (true)
                {
                    //Thread.Sleep(2000);//10秒
                    long a = GetLastInputTime();
                }
            }
        }
    }
}
