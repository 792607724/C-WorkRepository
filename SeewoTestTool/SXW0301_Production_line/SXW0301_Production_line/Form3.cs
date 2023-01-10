using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Sunny.UI;

namespace SXW0301_Production_line
{
    public partial class Form3 : Form
    {
        public Form3()
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

        private void button1_Click(object sender, EventArgs e)
        {
            string[] options = { ":network-caching=1000", ":rtsp-frame-buffer-size=1000000", ":rtsp-tcp", ":no-audio" };// { ":network-caching=100", ":rtsp -tcp", ":no-audio" }; //  --avcodec-hw={any,d3d11va,dxva2,none} 
            var videoUri = new Uri(textBox1.Text.Trim());
            vlcControl1.Play(videoUri, options);
        }

        public static void DeleteDir(string file)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                //去除文件的只读属性
                System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);
                //判断文件夹是否还存在
                if (Directory.Exists(file))
                {
                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                            Console.WriteLine(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            DeleteDir(f);
                        }
                    }
                    //删除空文件夹
                    //Directory.Delete(file);
                }
            }
            catch (Exception ex) // 异常处理
            {
                Console.WriteLine(ex.Message.ToString());// 异常信息
            }
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
        private void button2_Click(object sender, EventArgs e)
        {
            if (vlcControl1.IsPlaying)
            {
                /**
                if (System.IO.File.Exists("config_threshold.txt"))
                {
                    executeCMDCommand("del config_threshold.txt");
                }
                */
                //删除图片
                DeleteDir("20221017-plc//splicing");
                //拍照
                if (!System.IO.Directory.Exists("20221017-plc//splicing"))
                {
                    Directory.CreateDirectory("20221017-plc//splicing");
                }
                vlcControl1.TakeSnapshot("20221017-plc//splicing//splicing_cam1.jpg");

                //检测拼接图正确性
                string argument1 = "\"" + "-t" + "\"";
                string argument2 = "\"" + "-m" + "\"";
                string argument3 = "\"" + "-l" + "\"";
                Process myPro = new Process();
                myPro.StartInfo.FileName = "Splicing_test.exe";
                myPro.StartInfo.Arguments = argument1 + " " + argument2 + " " + argument3;
                myPro.StartInfo.UseShellExecute = false;
                myPro.StartInfo.RedirectStandardInput = true;
                //myPro.StartInfo.RedirectStandardOutput = true;
                myPro.StartInfo.RedirectStandardError = true;
                myPro.StartInfo.CreateNoWindow = true;
                myPro.Start();
                myPro.WaitForExit();
                //myPro.Close();
                if (myPro.ExitCode == 0)
                {
                    label1.ForeColor = Color.Green;
                    label1.Text = "PASS";
                }
                else
                {
                    label1.ForeColor = Color.Red;
                    label1.Text = "FAIL";
                }
                if (!System.IO.File.Exists("config_threshold.txt"))
                {
                    MessageBox.Show("拼接图测试失败，请重新检测试试！");
                }
                else
                {
                    string output_string = executeCMDCommand("type config_threshold.txt");
                    if (output_string != null)
                    {
                        try
                        {
                            string[] data_ex = output_string.Split("exit")[1].Split("\n");
                            currentValueLabel.Text = data_ex[1].Replace(" ", "").Replace("\n", "").Replace("\r", "");
                            standardValueLabel.Text = data_ex[2].Replace(" ", "").Replace("\n", "").Replace("\r", "");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("当前拍摄拼接图异常，请重新调整后拍摄！" + ex.ToString() + "\n" + "返回内容：\n" + output_string);
                        }
                    }
                    else
                    {
                        MessageBox.Show("当前拍摄拼接图异常内容为空！");
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("请先播放");
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
        private void button1_Click_1(object sender, EventArgs e)
        {
            /**
            string[] options = { ":network-caching=200", ":rtsp-tcp", ":no-audio" };// { ":network-caching=100", ":rtsp -tcp", ":no-audio" }; //  --avcodec-hw={any,d3d11va,dxva2,none} 
            var videoUri = new Uri(textBox1.Text.Trim());
            vlcControl1.Play(videoUri, options);
            */

            if (player_1_open != null && vlcControl1.IsPlaying)
            {
                MessageBox.Show("请勿重复打开出流哦！");
            }
            else
            {
                player_1_open = new Thread(player_1_open_func);
                player_1_open.IsBackground = false;
                player_1_open.Start();
            }

        }

        private void vlcControl1_Click(object sender, EventArgs e)
        {

        }

        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            vlcControl1.Stop();
            if (player_1_open!=null)
            {
                player_1_open.Interrupt();
                player_1_open=null;
            }
        }
    }
}
