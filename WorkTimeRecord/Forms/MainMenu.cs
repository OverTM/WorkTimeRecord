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
            GetTime.GetSelectedTime();
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
    }
}
