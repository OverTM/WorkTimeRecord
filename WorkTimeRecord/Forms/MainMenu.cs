using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Utility;
using Get;
using System.Windows.Forms;

namespace WorkTimeRecord
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
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
            if (Settings.Default.isFirstStart)
            {
                Settings.Default.isFirstStart = false;
                Settings.Default.Save();
                MessageBox.Show("需要先文件保存路径才能使用");
                FileOperations.SelectFolder();
            }
            else
            {
                GlobalVariables.SavePath = Settings.Default.SavePath;
            }
            this.Top = int.Parse(GlobalVariables.MainLocationY);
            this.Left = int.Parse(GlobalVariables.MainLocationX);
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.isFirstStart = GlobalVariables.isFirstStart;
            Settings.Default.StartWorkTime = GlobalVariables.StartWorkTime;
            Settings.Default.SavePath = GlobalVariables.SavePath;
            Settings.Default.Save();
        }
        
        private void 本机时间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalVariables.isLocalTime = true;
            (sender as ToolStripMenuItem).Checked = true;
            网络时间ToolStripMenuItem.Checked = false;
        }

        private void 网络时间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalVariables.isLocalTime = false;
            (sender as ToolStripMenuItem).Checked = true;
            本机时间ToolStripMenuItem.Checked = false;
        }
    }
}
