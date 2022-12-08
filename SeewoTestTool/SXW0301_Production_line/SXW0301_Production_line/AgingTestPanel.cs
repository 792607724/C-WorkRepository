using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Forms;

namespace SXW0301_Production_line
{
    public partial class AgingTestPanel : Form
    {
        public AgingTestPanel()
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
            string live_ip = "rtsp://" + ip_textBox.Text + "/live";
            string[] options = { ":network-caching=1000", ":rtsp-frame-buffer-size=100000", ":rtsp-tcp", ":no-audio" };// { ":network-caching=100", ":rtsp -tcp", ":no-audio" }; //  --avcodec-hw={any,d3d11va,dxva2,none} 
            var videoUri = new Uri(live_ip);

            vlcControl1.Play(videoUri, options);

        }

        Thread player_2_open;
        private void player_2_open_func()
        {
            //播放
            string live1_ip = "rtsp://" + ip_textBox.Text + "/live1";
            string[] options = { ":network-caching=1000", ":rtsp-frame-buffer-size=100000", ":rtsp-tcp", ":no-audio" };// { ":network-caching=100", ":rtsp -tcp", ":no-audio" }; //  --avcodec-hw={any,d3d11va,dxva2,none} 
            var videoUri = new Uri(live1_ip);

            vlcControl2.Play(videoUri, options);

        }

        Thread player_3_open;
        private void player_3_open_func()
        {
            //播放
            string merge_ip = "rtsp://" + ip_textBox.Text + "/merge";
            string[] options = { ":network-caching=1000", ":rtsp-frame-buffer-size=100000", ":rtsp-tcp", ":no-audio" };// { ":network-caching=100", ":rtsp -tcp", ":no-audio" }; //  --avcodec-hw={any,d3d11va,dxva2,none} 
            var videoUri = new Uri(merge_ip);

            vlcControl3.Play(videoUri, options);

        }

        private void beginAging_func()
        {
            liveVideoPlay_button.Enabled = false;
            stopAgingTest_button.Enabled = true;
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

            if (player_2_open != null && vlcControl2.IsPlaying)
            {
                MessageBox.Show("请勿重复打开出流哦！");
            }
            else
            {
                player_2_open = new Thread(player_2_open_func);
                player_2_open.IsBackground = true;
                player_2_open.Start();
            }

            if (player_3_open != null && vlcControl3.IsPlaying)
            {
                MessageBox.Show("请勿重复打开出流哦！");
            }
            else
            {
                player_3_open = new Thread(player_3_open_func);
                player_3_open.IsBackground = true;
                player_3_open.Start();
            }
            if (stopAging_t != null)
            {
                stopAging_t.Interrupt();
                stopAging_t = null;
            }

            OpenRedGreenFLashing();
        }

        private void stopAging_func()
        {
            liveVideoPlay_button.Enabled = true;
            stopAgingTest_button.Enabled = false;
            CloseReadGreenFLashing();
            vlcControl1.Stop();
            if (player_1_open != null)
            {
                player_1_open.Interrupt();
                player_1_open = null;
            }

            vlcControl2.Stop();
            if (player_2_open != null)
            {
                player_2_open.Interrupt();
                player_2_open = null;
            }

            vlcControl3.Stop();
            if (player_3_open != null)
            {
                player_3_open.Interrupt();
                player_3_open = null;
            }

            if (beginAging_t != null)
            {
                beginAging_t.Interrupt();
                beginAging_t = null;
            }

        }

        Thread beginAging_t;
        Thread stopAging_t;
        // 开始老化测试
        private void liveVideoPlay_button_Click(object sender, EventArgs e)
        {
            if (beginAging_t != null)
            {
                //MessageBox.Show("请勿重复开启老化测试哦！");
            }
            else
            {
                beginAging_t = new Thread(beginAging_func);
                beginAging_t.IsBackground = true;
                beginAging_t.Start();
            }

        }
        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            stopAgingTest_button_Click(null, null);
        }
        private string executeCMDCommand(string command)
        {
            Process process_cmd = new Process();
            string output_string = null;
            try
            {
                process_cmd.StartInfo.FileName = "cmd.exe";
                process_cmd.StartInfo.RedirectStandardInput = true;
                process_cmd.StartInfo.RedirectStandardOutput = true;
                process_cmd.StartInfo.CreateNoWindow = true;
                process_cmd.StartInfo.UseShellExecute = false;
                process_cmd.Start();
                process_cmd.StandardInput.WriteLine(command + "&exit");
                process_cmd.StandardInput.AutoFlush = true;
                output_string = process_cmd.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                output_string = ex.ToString();
            }
            finally
            {
                process_cmd.WaitForExit();
                process_cmd.Close();
            }
            return output_string;

        }

        string output_string;
        private void OpenRedGreenFLashing()
        {
            try
            {
                // 打开红绿灯交替闪烁
                string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_textBox.Text}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"ledTest\\\",\\\"open\\\": true}}\"";
                output_string = executeCMDCommand(fetchDeviceInfoCommand);
                MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                string result = "打开红绿灯交替闪烁操作未执行成功";
                if (back_code == "0")
                {
                    result = "成功";
                }
                else
                {
                    result = "失败";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开红绿灯交替闪烁功能失败：\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        // 关闭红绿灯交替闪烁
        private void CloseReadGreenFLashing()
        {
            try
            {
                // 关闭红绿灯交替闪烁
                string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_textBox.Text}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"ledTest\\\",\\\"open\\\": false}}\"";
                output_string = executeCMDCommand(fetchDeviceInfoCommand);
                MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                string result = "关闭红绿灯交替闪烁操作未执行成功";
                if (back_code == "0")
                {
                    result = "成功";
                }
                else
                {
                    result = "失败";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"关闭红绿灯交替闪烁功能失败：\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        // 停止老化测试
        private void stopAgingTest_button_Click(object sender, EventArgs e)
        {
            if (stopAging_t != null)
            {
                //MessageBox.Show("请勿重复开启老化测试哦！");
            }
            else
            {
                stopAging_t = new Thread(stopAging_func);
                stopAging_t.IsBackground = true;
                stopAging_t.Start();
            }
        }
    }
}
