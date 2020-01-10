using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkTimeRecord
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 设置log路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileOperations.FileOperationsClass.SelectFolder();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        string a = "";
        string b = "";
        private void MainMenu_Load(object sender, EventArgs e)
        {
            FileOperations.FileOperationsClass.savePath = Settings.Default.savePath;
            FileOperations.FileOperationsClass.StartWorkTime = Settings.Default.StartWorkTime;
            a = Settings.Default.a;
            this.Location = new Point(1300, 0);
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.savePath = FileOperations.FileOperationsClass.savePath;
            Settings.Default.StartWorkTime = FileOperations.FileOperationsClass.StartWorkTime;
            Settings.Default.a = a;
        }
    }
}
