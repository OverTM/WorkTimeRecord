using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Get;
using System.Threading;

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
                FileOperations.End(); 
            }

            else if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionUnlock)
            {
                // 屏幕解锁
                FileOperations.Start();
            }
        }

        #region 键盘和鼠标没有操作所经过的时间
        /// <summary>
        /// 创建结构体用于返回捕获时间
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            /// <summary>
            /// 设置结构体块容量
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;

            /// <summary>
            /// 抓获的时间
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        /// <summary>
        /// 获取键盘和鼠标没有操作的时间
        /// </summary>
        /// <returns>用户上次使用系统到现在的时间间隔，单位为秒</returns>
        public static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            if (!GetLastInputInfo(ref vLastInputInfo))
            {
                return 0;
            }
            else
            {
                long count = Environment.TickCount - (long)vLastInputInfo.dwTime;
                long icount = count / 1000;
                return icount;
            }
        }
        #endregion

        public static void TriggerMethod()
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
                    Thread.Sleep(2000);//2秒
                    long a = GetLastInputTime();
                    if (a>1)
                    {
                        FileOperations.Start();
                    }
                    else
                        FileOperations.End();
                }
            }
        }
    }
}
