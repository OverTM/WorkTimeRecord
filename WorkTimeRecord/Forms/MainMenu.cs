using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Utility;
using Get;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WorkTimeRecord
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            if (Settings.Default.SavePath == "")
            {
                FileOperations.SelectFolder();
            }
            else
            {
                GlobalVariables.SavePath = Settings.Default.SavePath;
            }
            MainSettings settings = new MainSettings();
            settings.RemoveComments();
            GlobalVariables.StartWorkTime = Settings.Default.StartWorkTime == "" ? "Not found" : Settings.Default.StartWorkTime;
            InitializeComponent();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 设置log路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            this.Top = int.Parse(GlobalVariables.MainLocationY);
            this.Left = int.Parse(GlobalVariables.MainLocationX);
            Utility.StatusMonitor.TriggerMethod();
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.isFirstStart = GlobalVariables.isFirstStart;
            Settings.Default.StartWorkTime = GlobalVariables.StartWorkTime;
            Settings.Default.SavePath = GlobalVariables.SavePath;
            Settings.Default.Save();
        }

        public void UpDataForm()
        {
            StartTimeLabel2.Text = Get.GlobalVariables.StartWorkTime;
        }

        #region 设置全局热键
        public class AppHotKey
        {
            [DllImport("kernel32.dll")]
            public static extern uint GetLastError();
            //如果函数执行成功，返回值不为0。
            //如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool RegisterHotKey(
                IntPtr hWnd,                //要定义热键的窗口的句柄
                int id,                     //定义热键ID（不能与其它ID重复）          
                KeyModifiers fsModifiers,   //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
                Keys vk                     //定义热键的内容
                );

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool UnregisterHotKey(
                IntPtr hWnd,                //要取消热键的窗口的句柄
                int id                      //要取消热键的ID
                );

            //定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
            [Flags()]
            public enum KeyModifiers
            {
                None = 0,
                Alt = 1,
                Ctrl = 2,
                Shift = 4,
                WindowsKey = 8
            }
            /// <summary>
            /// 注册热键
            /// </summary>
            /// <param name="hwnd">窗口句柄</param>
            /// <param name="hotKey_id">热键ID</param>
            /// <param name="keyModifiers">组合键</param>
            /// <param name="key">热键</param>
            public static void RegKey(IntPtr hwnd, int hotKey_id, KeyModifiers keyModifiers, Keys key)
            {
                try
                {
                    if (!RegisterHotKey(hwnd, hotKey_id, keyModifiers, key))
                    {
                        if (Marshal.GetLastWin32Error() == 1409) { MessageBox.Show("热键被占用 ！"); }
                        else
                        {
                            MessageBox.Show("注册热键失败！");
                        }
                    }
                }
                catch (Exception) { }
            }
            /// <summary>
            /// 注销热键
            /// </summary>
            /// <param name="hwnd">窗口句柄</param>
            /// <param name="hotKey_id">热键ID</param>
            public static void UnRegKey(IntPtr hwnd, int hotKey_id)
            {
                //注销Id号为hotKey_id的热键设定
                UnregisterHotKey(hwnd, hotKey_id);
            }
        }
        private const int WM_HOTKEY = 0x312; //窗口消息-热键
        private const int WM_CREATE = 0x1; //窗口消息-创建
        private const int WM_DESTROY = 0x2; //窗口消息-销毁
        private const int Space = 0x3572; //热键ID
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_HOTKEY: //窗口消息-热键ID
                    switch (m.WParam.ToInt32())
                    {
                        case Space: //热键ID
                            this.Visible = true;
                            this.WindowState = FormWindowState.Normal;//正常大小
                            this.Activate(); //激活窗体
                            //MessageBox.Show("我按了Control +Shift +Alt +S");//设置按下热键后的动作
                            break;
                        default:
                            break;
                    }
                    break;
                case WM_CREATE: //窗口消息-创建
                    AppHotKey.RegKey(Handle, Space, AppHotKey.KeyModifiers.Ctrl, Keys.Space); //热键为Ctrl+空格
                    break;
                case WM_DESTROY: //窗口消息-销毁
                    AppHotKey.UnRegKey(Handle, Space); //销毁热键
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            //DateTime dt;
            //System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            //dtFormat.ShortDatePattern = "yyyy/MM/dd";
            //dt = Convert.ToDateTime(GlobalVariables.StartWorkTime, dtFormat);
            //dt = dt.AddHours(1);
            //int i = DateTime.Now.CompareTo( dt );
            //if (i > 0)
            //{
            //    MessageBox.Show("下班时间到了");
            //    timer1.Enabled = false;
            //}
            //if(DateTime.Today==dt.Date)
            //{
            //    timer1.Enabled = true;
            //}
        }
    }
}
