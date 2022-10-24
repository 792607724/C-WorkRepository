using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace SeewoTestTool
{
    public partial class Form1 : Form
    {
        Socket clientSocket;
        Dictionary<string, InternetPort> internetPorts = new Dictionary<string, InternetPort>();
        public Form1()
        {
            InitializeComponent();

            FileStream fs = new FileStream("data.bin", FileMode.OpenOrCreate);
            if (fs.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                internetPorts = bf.Deserialize(fs) as Dictionary<string, InternetPort>;
                foreach (InternetPort internetPort in internetPorts.Values)
                {
                    device_ip_textbox.Text = internetPort.Deviceip;
                    for (int i = 0; i < internetPorts.Count; i++)
                    {
                        if (device_ip_textbox.Text != "")
                        {
                            if (internetPorts.ContainsKey(device_ip_textbox.Text))
                            {
                                device_port_textbox.Text = internetPorts[device_ip_textbox.Text].Deviceport;
                                rememberCheckBox.Checked = true;
                            }
                        }
                    }
                }
            }
            fs.Close();
            // 增加自动连接设备功能
            device_connect_button_Click(null, null);
        }

        // 保证设备在连接状态 网口通信
        private void device_connect_button_Click(object sender, EventArgs e)
        {
            string host = device_ip_textbox.Text;
            string port = device_port_textbox.Text;
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
            {
                output_rich_textbox.AppendText("设备网口IP地址和端口号不能为空！\n");
            }
            else
            {
                // Socket Connection Build
                try
                {
                    int port_int = Convert.ToInt32(port);
                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint ipe = new IPEndPoint(ip, port_int);
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    clientSocket.Connect(ipe);
                    if (clientSocket.Connected)
                    {
                        output_rich_textbox.AppendText($"设备ip:{host}:{port}已连接上！\n");
                        device_connect_button.Enabled = false;
                        device_disconnect_button.Enabled = true;
                        device_status_label.Text = "已连接";
                        device_ip_textbox.Enabled = false;
                        device_port_textbox.Enabled = false;
                        // 增加记住IP和端口功能
                        if (rememberCheckBox.Checked == true)
                        {
                            FileStream fileStream = new FileStream("data.bin", FileMode.OpenOrCreate);
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            InternetPort internetPort = new InternetPort();
                            internetPort.Deviceip = host;
                            internetPort.Deviceport = port;
                            if (internetPorts.ContainsKey(internetPort.Deviceip))
                            {
                                // 如果存在就清除掉
                                internetPorts.Remove(internetPort.Deviceip);
                            }
                            internetPorts.Add(internetPort.Deviceip, internetPort);
                            binaryFormatter.Serialize(fileStream, internetPorts);
                            fileStream.Close();
                        }
                    }
                    /*
                    send_Str("am start com.android.browser");
                    string rec_Str = receive_Str();
                    */
                }
                catch (Exception ex)
                {
                    device_status_label.Text = "已断开";
                    device_ip_textbox.Enabled = true;
                    device_port_textbox.Enabled = true;
                    output_rich_textbox.AppendText($"设备网口IP地址和端口号错误，请检查是否输入正确！\n问题Log如下：{ex.ToString()}\n");
                    MessageBox.Show($"设备未连接，请检查！\nIP:{host}， PORT:{port}");
                }

            }
        }

        // 在cmd中执行命令操作
        private string executeCMDCommand(string command)
        {
            Process process_cmd = new Process();
            process_cmd.StartInfo.FileName = "cmd.exe";
            process_cmd.StartInfo.RedirectStandardInput = true;
            process_cmd.StartInfo.RedirectStandardOutput = true;
            process_cmd.StartInfo.CreateNoWindow = false;
            process_cmd.StartInfo.UseShellExecute = false;
            process_cmd.Start();
            process_cmd.StandardInput.WriteLine(command + "&exit");
            process_cmd.StandardInput.AutoFlush = true;
            string output_string = process_cmd.StandardOutput.ReadToEnd();
            process_cmd.WaitForExit();
            process_cmd.Close();
            return output_string;

        }

        // Socket发送命令函数
        private void send_Str(string sendStr)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                // Socket发送命令
                try
                {
                    output_rich_textbox.AppendText($"指令发送:{sendStr}\n");
                    byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);
                    clientSocket.Send(sendBytes);
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"Socket发送命令执行失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // Socket接收返回内容函数
        private string receive_Str()
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                while (true)
                {
                    try
                    {
                        string recStr = "";
                        byte[] recBytes = new byte[4096];
                        //  以阻塞的方式接收，在收到数据前无响应。
                        int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
                        if (bytes == 0)
                        {
                            break;
                        }
                        recStr += Encoding.ASCII.GetString(recBytes, 0, bytes);
                        output_rich_textbox.AppendText($"接收内容：{recStr}\n");
                        return recStr;
                    }
                    catch (Exception ex)
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                        device_disconnect_button.Enabled = false;
                        device_connect_button.Enabled = true;
                        device_ip_textbox.Enabled = true;
                        device_port_textbox.Enabled = true;
                        device_status_label.Text = "已断开";
                        output_rich_textbox.AppendText($"Socket接收命令执行失败：\n{ex.ToString()}\n");
                        break;
                    }
                }
                return "";
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
                return "";
            }
        }

        // 断开设备连接
        private void device_disconnect_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    output_rich_textbox.AppendText("断开当前已连接设备\n");
                    device_disconnect_button.Enabled = false;
                    device_connect_button.Enabled = true;
                    clientSocket.Close();
                    device_status_label.Text = "已断开";
                    device_ip_textbox.Enabled = true;
                    device_port_textbox.Enabled = true;
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"断开设备连接失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
        }

        string filePath = null;
        // 选择升级的固件路径
        private void choose_upgrade_firmware_button_Click(object sender, EventArgs e)
        {
            //if(true)
            if (clientSocket != null && clientSocket.Connected)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                output_rich_textbox.AppendText("选择升级固件！\n");
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = dialog.FileName;
                    if (string.IsNullOrEmpty(filePath))
                    {
                        upgrade_firmware_textbox.Text = "未选择正确的固件路径";
                    }
                    else
                    {
                        upgrade_firmware_textbox.Text = filePath;
                        output_rich_textbox.AppendText($"当前选择的固件路径为：\n{filePath}\n");

                    }
                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // 固件升级操作
        private void upgrade_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                string filePath = upgrade_firmware_textbox.Text;
                if (string.IsNullOrEmpty(filePath))
                {
                    upgrade_firmware_textbox.Text = "未选择正确的固件路径";
                }
                else
                {
                    // 固件开始升级
                    try
                    {
                        output_rich_textbox.AppendText("开始升级，请耐心等待完成！\n");
                        // 使用后台线程去升级操作，防止UI阻塞卡死
                        upgrade_progressbar.Value = 0;
                        upgrade_button.Enabled = false;
                        if (backgroundworker_firmwareupgrade.IsBusy)
                        {
                            return;
                        }
                        backgroundworker_firmwareupgrade.RunWorkerAsync("Hello");
                    }
                    catch (Exception ex)
                    {
                        output_rich_textbox.AppendText($"固件升级操作失败：\n{ex.ToString()}\n");
                    }
                    finally
                    {

                    }
                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // 校验当前固件
        private void check_current_firmware_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                output_rich_textbox.AppendText("校验固件测试！\n");
                string currentFirmware = null;
                try
                {
                    // 校验固件 - 登录操作获取session - 后续IP以后续输入为主，当前暂定
                    string loginCommand = "curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{\\\"method\\\": \\\"login\\\", \\\"username\\\": \\\"admin\\\",\\\"password\\\": \\\"8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92\\\"}\"";
                    output_string = executeCMDCommand(loginCommand);
                    MatchCollection results = Regex.Matches(output_string, "\"session\" : (.*)"); 
                    string session = results[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    output_rich_textbox.AppendText("当前固件校验Session是：" + session + "\n");


                    // 校验固件 - 从session中获取固件当前版本
                    string checkVersionCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\":\\\"getParam\\\",\\\"session\\\":\\\"{session}\\\",\\\"name\\\":\\\"DevInfo\\\"}}\"";
                    output_string = executeCMDCommand(checkVersionCommand);
                    MatchCollection results_2 = Regex.Matches(output_string, "\"SoftwaveVersion\" : (.*)");
                    string currentVersion = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    output_rich_textbox.AppendText("当前版本是：" + currentVersion + "\n");

                    // 将本地Firmware压缩包解压从info.txt中获取需要升级的版本进行核对
                    if (string.IsNullOrEmpty(filePath))
                    {
                        output_rich_textbox.AppendText("请先点击【选择升级固件】后再进行核对操作！\n");
                    }
                    else
                    {
                        using (var zip = ZipFile.OpenRead(filePath))
                        {
                            foreach (var entry in zip.Entries)
                            {
                                if (entry.FullName == "info.txt")
                                {
                                    output_rich_textbox.AppendText($"当前解包的固件文件为：{entry.FullName}\n");
                                    using (var stream = entry.Open())
                                    using (var reader = new StreamReader(stream))
                                    {
                                        var content_get = reader.ReadToEnd();
                                        MatchCollection results_3 = Regex.Matches(content_get.ToString(), "project=(.*)");
                                        string toProduct = results_3[0].ToString().Split("=")[1];
                                        MatchCollection results_4 = Regex.Matches(content_get.ToString(), "version=(.*)");
                                        string toVersion = results_4[0].ToString().Split("=")[1];
                                        output_rich_textbox.AppendText($"当前核对的版本为：{toVersion}, 项目为：{toProduct}\n");

                                        if (currentVersion == toVersion && toProduct == "SXW0301")
                                        {
                                            checked_firmware_textbox.Text = $"固件校验成功，当前固件版本：{currentVersion}";
                                            output_rich_textbox.AppendText($"固件校验成功，当前固件版本：{currentVersion}\n");
                                        }
                                        else
                                        {
                                            checked_firmware_textbox.Text = $"固件校验失败，当前固件版本：{currentVersion}";
                                            output_rich_textbox.AppendText($"固件校验失败，当前固件版本：{currentVersion}\n");
                                        }
                                    }
                                }

                            }
                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"校验固件测试失败：\n{ex.ToString()}\n");
                }
                finally
                { 
                    
                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // 打开红绿灯交替闪烁
        private void start_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("打开红绿灯交替闪烁\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    stop_rg_flicker_button.Enabled = true;
                    start_rg_flicker_button.Enabled = false;

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"打开红绿灯交替闪烁功能失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // 关闭红绿灯交替闪烁
        private void stop_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("关闭红绿灯交替闪烁\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    stop_rg_flicker_button.Enabled = false;
                    start_rg_flicker_button.Enabled = true;

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"关闭红绿灯交替闪烁功能失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // 获取吊麦信息
        private void get_poe_mic_info_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("获取吊麦信息\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"获取吊麦信息失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // Audio IN 1口测试
        private void audioin1_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("Audio IN 1口测试\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"Audio IN 1口测试失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // Audio IN 2口测试
        private void audioin2_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("Audio IN 2口测试\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"Audio IN 2口测试失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // 阵列MIC音量值测试
        private void array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("阵列MIC音量值测试\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"阵列MIC音量值测试失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // 设备复位
        private void device_reset_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("设备复位\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"设备复位失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // 窗体关闭事件，将设备socket连接释放掉
        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("窗体关闭事件，将设备socket连接释放掉\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    clientSocket.Close();
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"窗体关闭将设备socket连接释放掉失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
        }

        // 输出列表始终显示最新内容Log
        private void richTextChanged_to(object sender, EventArgs e)
        {
            // 将光标位置设置到当前内容的末尾
            output_rich_textbox.SelectionStart = output_rich_textbox.Text.Length;
            // 滚动到光标位置
            output_rich_textbox.ScrollToCaret();
        }

        // 清空输出内容
        private void clear_output_button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(output_rich_textbox.Text))
            {
                output_rich_textbox.Text = "";
            }
        }

        string output_string = "";
        // 新建backgroundworker给固件升级操作，防止UI阻塞交互卡死
        private void backgroundworker_firmwareupgrade_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (backgroundworker_firmwareupgrade.CancellationPending)
            { 
                e.Cancel = true;
                return;
            }
            else
            {
                int progress_i = 0;
                // 升级操作 - 将固件推进去 后面确定下来了，这个ip从输入框里面获取
                output_string = executeCMDCommand($"curl -T {filePath} \"ftp://10.66.30.69/\"");
                progress_i += 50;
                backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Pushing\n");
                System.Threading.Thread.Sleep(1000);

                // 升级操作 - 创建文件夹 need_upgrade
                if (!System.IO.Directory.Exists("need_upgrade"))
                {
                    output_string = executeCMDCommand("touch need_upgrade");
                    progress_i += 10;
                    backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Createing\n");
                    System.Threading.Thread.Sleep(1000);
                }

                // 升级操作 - 开始升级
                output_string = executeCMDCommand("curl -T need_upgrade \"ftp://10.66.30.69/\"");
                progress_i += 40;
                backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Upgrading\n");
                System.Threading.Thread.Sleep(1000);





            }
        }
        private void backgroundworker_firmwareupgrade_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            upgrade_progressbar.Value = e.ProgressPercentage;
            output_rich_textbox.AppendText($"升级进度：{Convert.ToString(e.ProgressPercentage)}%\n");


            // 临时增加执行cmd命令验证
            output_rich_textbox.AppendText(output_string);
        }
        private void backgroundworker_firmwareupgrade_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error!=null)
            {
                output_rich_textbox.AppendText(e.Error.ToString());
                return;
            }
            if (!e.Cancelled)
            {
                output_rich_textbox.AppendText("升级完毕！\n");
                upgrade_button.Enabled = true;

            }
            else
            {
                output_rich_textbox.AppendText("升级终止！\n");
            }
        }
        
        // 获取当前电脑连接的所有设备的网口并筛选出Seewo的设备
        private void getSeewoDevice_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("获取当前电脑连接的所有设备的网口并筛选出Seewo的设备\n");
            try
            {
                
                foreach (NetworkInterface netItem in NetworkInterface.GetAllNetworkInterfaces())
                {
                    output_rich_textbox.AppendText($"接口名：{netItem.Name}，接口类型：{netItem.NetworkInterfaceType}，" +
                        $"接口MAC：{netItem.GetPhysicalAddress().ToString()}\n");
                    foreach (UnicastIPAddressInformation ipIntProp in netItem.GetIPProperties().UnicastAddresses.ToArray<UnicastIPAddressInformation>())
                    {
                        output_rich_textbox.AppendText($"   接口名：{netItem.Name}，IP：{ipIntProp.Address.ToString()}，IP类型：{ipIntProp.Address.AddressFamily}\n");
                    }
                }

                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] endPoints = ipGlobalProperties.GetActiveTcpListeners();
                foreach (IPEndPoint endPoint in endPoints)
                {
                    output_rich_textbox.AppendText($"端口：{endPoint.Port}，IP：{endPoint.Address.ToString()}，IP类型：{endPoint.Address.AddressFamily.ToString()}\n");
                }
            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"获取设备失败，当前未连接设备：\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

    }
}