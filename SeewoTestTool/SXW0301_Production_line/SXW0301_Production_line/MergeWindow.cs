using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SXW0301_Production_line
{
    public partial class MergeWindow : Form
    {
        public MergeWindow()
        {
            InitializeComponent();
        }
        private void vlcControl1_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssesmbly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssesmbly.Location).DirectoryName;

            if (null == currentDirectory)
            {
                e.VlcLibDirectory = new DirectoryInfo(Path.GetFullPath(@".\libvlc\win-x86\"));
            }
            else
            {
                e.VlcLibDirectory = new DirectoryInfo(Path.GetFullPath(@".\libvlc\win-x64\"));
            }

            if (!e.VlcLibDirectory.Exists)
            {
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog.Description = "Select Vlc libraries folder.";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    e.VlcLibDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                }
            }
        }

        Thread player_1_open;
        private void player_1_open_func()
        {
            //播放
            string[] options = { ":network-caching=1000", ":rtsp-frame-buffer-size=1000000", ":rtsp-tcp", ":no-audio" };// { ":network-caching=100", ":rtsp -tcp", ":no-audio" }; //  --avcodec-hw={any,d3d11va,dxva2,none} 
            var videoUri = new Uri(textBox1.Text.Trim());

            vlcControl1.Play(videoUri, options);

        }

        private void mergeVideoPlay_button_Click(object sender, EventArgs e)
        {
            if (player_1_open != null && vlcControl1.IsPlaying)
            {
                MessageBox.Show("请勿重复打开出流哦！");
            }
            else
            {
                player_1_open = new Thread(player_1_open_func);
                player_1_open.IsBackground = true;
                player_1_open.Start();
            }
        }

        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            vlcControl1.Stop();
            if (player_1_open != null)
            {
                player_1_open.Interrupt();
                player_1_open = null;
            }
        }
    }
}
