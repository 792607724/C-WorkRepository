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
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Threading;

namespace SXW0301_Production_line
{
    public partial class Fom1 : Form
    {
        public Fom1()
        {
            InitializeComponent();
        }

        public static void DeleteDir1(string file)
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
                            DeleteDir1(f);
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
        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            vlcControl1.Stop();
            vlcControl2.Stop();
            if (player_1_open != null)
            {
                player_1_open.Interrupt();
                player_1_open = null;
            }
            if (player_2_open != null)
            {
                player_2_open.Interrupt();
                player_2_open = null;
            }
            vlcControl1 = null;
            vlcControl2= null;
            Application.Exit();
        }
        private unsafe void button1_Click(object sender, EventArgs e)
        {
            //播放1
            string[] options1 = { ":network-caching=200", "--rtsp-frame-buffer-size=100000", ":rtsp-tcp", ":no-audio" };// { ":network-caching=100", ":rtsp -tcp", ":no-audio" }; //  --avcodec-hw={any,d3d11va,dxva2,none} 
            var videoUri1 = new Uri(textBox1.Text.Trim());
            //vlcControl1.Play(videoUri1, options1);

            //执行标定算法,并生成calib_out_json文件
            //Process myPro = new Process();
            //myPro.StartInfo.FileName = "C_production_line_tool.exe";
            //myPro.StartInfo.UseShellExecute = false;
            //myPro.StartInfo.RedirectStandardInput = true;
            ////myPro.StartInfo.RedirectStandardOutput = true;
            //myPro.StartInfo.RedirectStandardError = true;
            //myPro.StartInfo.CreateNoWindow = true;
            //myPro.Start();
            //myPro.WaitForExit();//本行代码不是必须，但是很关键，限制等待外部程序退出后才能往下执行
            if (!vlcControl2.IsPlaying && !vlcControl1.IsPlaying)
            {
                MessageBox.Show("请先点击左右两侧的播放按钮，两侧视频流播放后，再点击标定按钮进行标定！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(textBox2.Text.Trim() != textBox1.Text.Trim())
            {
                //播放2
                string[] options2 = { ":network-caching=500", ":rtsp-tcp", ":no-audio" };// { ":network-caching=100", ":rtsp -tcp", ":no-audio" }; //  --avcodec-hw={any,d3d11va,dxva2,none} 
                var videoUri2 = new Uri(textBox2.Text.Trim());
                //vlcControl2.Play(videoUri2, options2);
                //MessageBox.Show("标定文件正在生成");
                if (vlcControl2.IsPlaying && vlcControl1.IsPlaying)
                {
                    DeleteDir1("20221017-plc//stitch");
                    DeleteDir1("20221017-plc//extr//pair_0_1");
                    DeleteDir1("20221017-plc//intr");
                    vlcControl1.Pause();
                    vlcControl2.Pause();
                    vlcControl1.TakeSnapshot("20221017-plc//stitch//camera0_stitch.jpg");
                    vlcControl2.TakeSnapshot("20221017-plc//stitch//camera1_stitch.jpg");
                    vlcControl1.TakeSnapshot("20221017-plc//extr//pair_0_1//camera0_01_1.jpg");
                    vlcControl2.TakeSnapshot("20221017-plc//extr//pair_0_1//camera1_01_1.jpg");
                    vlcControl1.TakeSnapshot("20221017-plc//intr//camera0_1.jpg");
                    vlcControl2.TakeSnapshot("20221017-plc//intr//camera1_1.jpg");
                    vlcControl1.Play(videoUri1, options1);
                    vlcControl2.Play(videoUri2, options2);
                    //执行标定算法,并生成calib_out_json文件
                    //参数
                    string argument1 = "\"" + "-t" + "\"";
                    string argument2 = "\"" + "-m" + "\"";
                    string argument3 = "\"" + "-l" + "\"";
                    Process myPro = new Process();
                    myPro.StartInfo.FileName = "C_production_line_tool.exe";
                    myPro.StartInfo.Arguments = argument1 + " " + argument2 + " " + argument3;
                    myPro.StartInfo.UseShellExecute = false;
                    myPro.StartInfo.RedirectStandardInput = true;
                    //myPro.StartInfo.RedirectStandardOutput = true;
                    myPro.StartInfo.RedirectStandardError = true;
                    myPro.StartInfo.CreateNoWindow = true;
                    myPro.Start();
                    myPro.WaitForExit();//本行代码不是必须，但是很关键，限制等待外部程序退出后才能往下执行
                    myPro.Close();
                    //MessageBox.Show("标定完成");

                    /**
                        *  陈广涛 -- Add Code 增加写入数据&删除数据的功能
                        **/
                    string calib_out_json_path = ".\\20221017-plc\\calib_out.json";
                    if (File.Exists(calib_out_json_path))
                    {
                        MatchCollection result = Regex.Matches(textBox1.Text, @"rtsp:\/\/(.*)\/sec0");
                        string writeInIP = (result[0].ToString()).Replace("//", "/").Split('/')[1];
                        //MessageBox.Show($"已找到标定数据，开始写入标定数据……,写入IP：【{writeInIP}】");
                        string uploadCommand = $"curl -s -X POST --data-binary @{calib_out_json_path} \"http://{writeInIP}/writeLdcCalib_api\" -H \"Content-Type: text/plain\"";
                        string executeResult = executeCMDCommand(uploadCommand);
                        string result_0 = "标定数据写入操作未执行成功";
                        if (executeResult.Contains("OK"))
                        {
                            result_0 = "成功";
                        }
                        else
                        {
                            result_0 = "失败";
                        }
                        Thread.Sleep(3000);
                        MessageBox.Show($"标定数据写入操作结果：" + result_0);
                        executeCMDCommand($"del {calib_out_json_path}");
                    }
                    else
                    {
                        MessageBox.Show($"未找到标定数据：{calib_out_json_path},请检查！");
                    }
                }
                else
                {
                    MessageBox.Show("标定文件生成失败,请检查两路流是否都在播放");
                }
            }
            else{
                MessageBox.Show("两个流路径请勿一样");
            }
        }

        // 在cmd中执行命令操作
        /**
         * 陈广涛 -- Add Code 
         *  传入字符串的命令即可在CMD中执行相应的指令
         */
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void vlcControl2_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void player1_open_func()
        {
            string[] options1 = { ":network-caching=1000", ":rtsp-frame-buffer-size=1000000", ":rtsp-tcp", ":no-audio" };// { ":network-caching=100", ":rtsp -tcp", ":no-audio" }; //  --avcodec-hw={any,d3d11va,dxva2,none} 
            var videoUri1 = new Uri(textBox1.Text.Trim());
            vlcControl1.Play(videoUri1, options1);
        }

        private void player2_open_func()
        {
            string[] options2 = { ":network-caching=1000", ":rtsp-frame-buffer-size=1000000", ":rtsp-tcp", ":no-audio" };// { ":network-caching=100", ":rtsp -tcp", ":no-audio" }; //  --avcodec-hw={any,d3d11va,dxva2,none} 
            var videoUri2 = new Uri(textBox2.Text.Trim());
            vlcControl2.Play(videoUri2, options2);
        }
        Thread player_1_open;
        Thread player_2_open;

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (player_1_open != null && vlcControl1.IsPlaying) 
            {
                MessageBox.Show("请勿重复打开出流哦！");
            }
            else
            {
                player_1_open = new Thread(player1_open_func);
                player_1_open.IsBackground = true;
                player_1_open.Start();

            }
            

            /**
            string[] options1 = { ":network-caching=500", ":rtsp-tcp", ":no-audio" };// { ":network-caching=100", ":rtsp -tcp", ":no-audio" }; //  --avcodec-hw={any,d3d11va,dxva2,none} 
            var videoUri1 = new Uri(textBox1.Text.Trim());
            vlcControl1.Play(videoUri1, options1);
            */
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (player_2_open != null && vlcControl2.IsPlaying)
            {
                MessageBox.Show("请勿重复打开出流哦！");
            }
            else
            {
                player_2_open = new Thread(player2_open_func);
                player_2_open.IsBackground = true;
                player_2_open.Start();
            }
            
            /**
            string[] options2 = { ":network-caching=500",  ":rtsp-tcp", ":no-audio" };// { ":network-caching=100", ":rtsp -tcp", ":no-audio" }; //  --avcodec-hw={any,d3d11va,dxva2,none} 
            var videoUri2 = new Uri(textBox2.Text.Trim());
            vlcControl2.Play(videoUri2, options2);
            */
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void vlcControl2_Click(object sender, EventArgs e)
        {

        }
    }
}
