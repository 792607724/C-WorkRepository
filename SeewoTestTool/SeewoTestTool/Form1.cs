using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace SeewoTestTool
{
    public partial class Form1 : Form
    {
        Socket clientSocket;
        Dictionary<string, InternetPort> internetPorts = new Dictionary<string, InternetPort>();
        Dictionary<string, User> users = new Dictionary<string, User>();
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
                }
                for (int i = 0; i < internetPorts.Count; i++)
                {
                    if (device_ip_textbox.Text != "")
                    {
                        if (internetPorts.ContainsKey(device_ip_textbox.Text))
                        {
                            string port_temp = internetPorts[device_ip_textbox.Text].Deviceport;
                            if (int.Parse(port_temp) == 80)
                            {
                                radioButton_80.Checked = true;
                            }
                            else
                            {
                                radioButton_8080.Checked = true;

                            }
                            rememberCheckBox.Checked = true;
                        }
                    }
                }

                
                
            }
            FileStream fs1 = new FileStream("data1.bin", FileMode.OpenOrCreate);
            if (fs1.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                users = bf.Deserialize(fs1) as Dictionary<string, User>;
                foreach (User user in users.Values)
                {
                    username_textbox.Text = user.Username;
                }
                for (int i = 0; i < users.Count; i++)
                {
                    if (username_textbox.Text != "")
                    {
                        if (users.ContainsKey(username_textbox.Text))
                        {
                            string password_temp = users[username_textbox.Text].Password;
                            password_textbox.Text = password_temp;
                        }
                    }
                }
            }
            fs.Close();
            fs1.Close();
            // 增加自动连接设备功能
            device_connect_button_Click(null, null);

            login_button_Click(null, null);
        }

        // 保证设备在连接状态 网口通信
        private void device_connect_button_Click(object sender, EventArgs e)
        {
            string host = device_ip_textbox.Text;
            string port;
            if (radioButton_80.Checked)
            {
                port = radioButton_80.Text;
            }
            else 
            {
                port = radioButton_8080.Text;
            }
            if (string.IsNullOrEmpty(host))
            {
                output_rich_textbox.AppendText("设备网口IP地址和端口号不能为空！\n");
            }
            else
            {
                // Socket Connection Build
                try
                {
                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint ipe = new IPEndPoint(ip, int.Parse(port));
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    clientSocket.Connect(ipe);
                    if (clientSocket.Connected)
                    {
                        output_rich_textbox.AppendText($"设备ip:{host}:{port}已连接上！\n");
                        device_connect_button.Enabled = false;
                        device_disconnect_button.Enabled = true;
                        device_status_label.Text = "已连接";
                        device_ip_textbox.Enabled = false;
                        radioButton_80.Enabled = false;
                        radioButton_8080.Enabled = false;
                        login_button.Enabled = true;
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
                                //internetPorts.Remove(internetPort.Deviceip);
                                internetPorts.Clear();
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
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
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
            process_cmd.StartInfo.CreateNoWindow = true;
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
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
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
                        radioButton_80.Enabled = true;
                        radioButton_8080.Enabled = true;
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
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
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
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    check_current_firmware_button.Enabled = false;
                    upgrade_button.Enabled = false;
                    getCurrentSN_button.Enabled = false;
                    writeIn_button.Enabled = false;
                    login_button.Enabled = false;
                    getCurrentPCBA_button.Enabled = false;
                    writeInPCBA_button.Enabled = false;
                    start_array_mic_audio_level_test_button.Enabled = false;
                    stop_array_mic_audio_level_test_button.Enabled = false;
                    gain_array_mic_audio_level_button.Enabled = false;
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
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
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
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
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

                                        Font font = new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold);
                                        if (currentVersion == toVersion && toProduct == "SXW0301")
                                        {
                                            output_rich_textbox.ForeColor = Color.Green;
                                            output_rich_textbox.SelectionFont = font;
                                            checked_firmware_textbox.Text = $"固件校验成功，当前固件版本：{currentVersion}";
                                            output_rich_textbox.AppendText($"固件校验成功，当前固件版本：{currentVersion}\n");
                                        }
                                        else
                                        {
                                            output_rich_textbox.ForeColor = Color.Red;
                                            output_rich_textbox.SelectionFont = font;
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
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
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
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
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
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
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
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
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
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
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
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // 开启阵列MIC音量值测试
        private void array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("开启阵列MIC音量值测试\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // 开启阵列MIC音量值测试
                    output_rich_textbox.AppendText(session);
                    string beginDeviceMICVolumeTestCommand = $"curl -X POST \"http://10.66.30.69/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"startAudioTest\\\",\\\"format\\\": 0,\\\"soundmode\\\": 8,\\\"samplerate\\\": 16000,\\\"periodsize\\\": 1024}}\"";
                    output_string = executeCMDCommand(beginDeviceMICVolumeTestCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    
                    if (Int32.Parse(backCode) == 0)
                    {
                        output_rich_textbox.AppendText($"执行结果为：PASS，已开启阵列MIC音量值测试，backCode:[{backCode}]\n");
                    }
                    else if(Int32.Parse(backCode) == -1)
                    {
                        output_rich_textbox.AppendText($"执行结果为：FAIL，未开启阵列MIC音量值测试，backCode:[{backCode}]\n");
                    }
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"开启阵列MIC音量值测试失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // 停止阵列MIC音量值测试
        private void stop_array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("停止阵列MIC音量值测试\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // 停止阵列MIC音量值测试
                    output_rich_textbox.AppendText(session);
                    string stopDeviceMICVolumeTestCommand = $"curl -X POST \"http://10.66.30.69/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"stopAudioTest\\\"}}\"";
                    output_string = executeCMDCommand(stopDeviceMICVolumeTestCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                    if (Int32.Parse(backCode) == 0)
                    {
                        output_rich_textbox.AppendText($"执行结果为：PASS，已停止阵列MIC音量值测试，backCode:[{backCode}]\n");
                    }
                    else if (Int32.Parse(backCode) == -1)
                    {
                        output_rich_textbox.AppendText($"执行结果为：FAIL，无法关闭阵列MIC音量值测试，backCode:[{backCode}]\n");
                    }
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"停止阵列MIC音量值测试失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // 获取各路MIC音频音量值
        private void gain_array_mic_audio_level_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("获取各路MIC音频音量值\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"获取各路MIC音频音量值失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
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
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
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
                output_rich_textbox.ForeColor = Color.Black;
                Font font2 = new Font(FontFamily.GenericMonospace, 9, FontStyle.Regular);
                output_rich_textbox.Font = font2;
            }
        }

        string output_string = "";
        // 新建backgroundworker给固件升级操作，防止UI阻塞交互卡死
        private void backgroundworker_firmwareupgrade_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                if (backgroundworker_firmwareupgrade.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    int progress_i = 0;
                    //升级操作 - 增加固件检验，检测压缩包中的info.txt的项目名称是否正确
                    using (var zip = ZipFile.OpenRead(filePath))
                    {
                        foreach (var entry in zip.Entries)
                        {
                            if (entry.FullName == "info.txt")
                            {
                                using (var stream = entry.Open())
                                using (var reader = new StreamReader(stream))
                                {
                                    var content_get = reader.ReadToEnd();
                                    MatchCollection results_3 = Regex.Matches(content_get.ToString(), "project=(.*)");
                                    string toProduct = results_3[0].ToString().Split("=")[1];
                                    MatchCollection results_4 = Regex.Matches(content_get.ToString(), "version=(.*)");
                                    string toVersion = results_4[0].ToString().Split("=")[1];
                                    if (toProduct != "SXW0301")
                                    {
                                        backgroundworker_firmwareupgrade.CancelAsync();
                                        output_string = $"当前所选固件异常，非本项目固件当前固件属于：【{toProduct}】，升级失败！\n";
                                    }
                                    else
                                    {
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
                                        progress_i += 5;
                                        backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Upgrading\n");
                                        System.Threading.Thread.Sleep(1000);

                                        // 升级操作 - 检测升级状态
                                        bool upgradeDone = false;
                                        if (System.IO.File.Exists("upgrade_result"))
                                        {
                                            output_string = executeCMDCommand("rm -rf upgrade_result");
                                        }
                                        while (!upgradeDone)
                                        {
                                            output_string = executeCMDCommand("curl \"ftp://10.66.30.69/upgrade_result\" -o upgrade_result");
                                            if (!System.IO.File.Exists("upgrade_result"))
                                            {
                                                // 如果2s内没有获取到这个文件直接报升级失败
                                                backgroundworker_firmwareupgrade.CancelAsync();
                                                output_string = "未检测到upgrade_result文件，升级失败！\n";
                                                break;
                                            }
                                            // 检测到该文件，进行cat读取内容
                                            output_string = executeCMDCommand("cat upgrade_result");
                                            if (output_string.Contains("start"))
                                            {
                                                continue;
                                            }
                                            if (output_string.Contains("success") || output_string.Contains("fail"))
                                            {
                                                progress_i = 100;
                                                backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Upgrading\n");
                                                backgroundworker_firmwareupgrade.CancelAsync();
                                                break;
                                            }
                                            progress_i += 5;
                                            System.Threading.Thread.Sleep(500);
                                            output_string = "正在升级中，请稍后！";
                                            backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Upgrading\n");
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                output_string = $"升级操作异常，异常报错Log如下：\n{ex.ToString()}\n";
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
                output_rich_textbox.AppendText("升级操作结束，请查看结果！\n");
                Font font = new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold);
                if (output_string.Contains("success"))
                {
                    output_rich_textbox.ForeColor = Color.Green;
                    output_rich_textbox.SelectionFont = font;
                }
                else if (output_string.Contains("fail"))
                {
                    output_rich_textbox.ForeColor = Color.Red;
                    output_rich_textbox.SelectionFont = font;
                }
                output_rich_textbox.AppendText(output_string);
                if (output_string.Contains("Upgrade finish"))
                {
                    output_rich_textbox.AppendText("等待20s设备正在重启中，期间无法操作工具……\n");
                    System.Threading.Thread.Sleep(20000);
                    // 这里升级完重启，需要重新连接设备，设备状态那边需要同步更新
                    device_disconnect_button_Click(null, null);
                }
                output_rich_textbox.AppendText("设备重启完成！\n");
                check_current_firmware_button.Enabled = false;
                upgrade_button.Enabled = false;
                getCurrentSN_button.Enabled = false;
                writeIn_button.Enabled = false;
                getCurrentPCBA_button.Enabled = false;
                writeInPCBA_button.Enabled = false;
                start_array_mic_audio_level_test_button.Enabled = false;
                stop_array_mic_audio_level_test_button.Enabled = false;
                gain_array_mic_audio_level_button.Enabled = false;
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

        // 获取当前设备的SN号
        private void getCurrentSN_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("获取当前设备的SN号:\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // 获取SN号
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"SN\" : (.*)");
                    string currentSN = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    output_rich_textbox.AppendText("获取当前设备的SN号：" + currentSN + "\n");
                    currentSN_textbox.Text = currentSN;
                }
                else
                {
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    device_status_label.Text = "已断开";
                    output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
                }

            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"获取当前设备的SN号失败，当前未连接设备：\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }


        // 写入指定的设备SN号
        private void writeIn_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("写入指定的设备SN号:\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {

                    // 写入指定的设备SN号
                    string writeINSN = writeInSN_textbox.Text;
                    if (string.IsNullOrEmpty(writeINSN) || writeINSN.Length != 22 || !new Regex("^[A-Z|0-9]+$").IsMatch(writeINSN))
                    {
                        writeInSN_textbox.Text = "请写入正确的SN号再进行刷入，示例：FCSC03V000019179E00001，只能包含大写字母和数字，且长度为22位！\n";
                        output_rich_textbox.AppendText("请写入正确的SN号再进行刷入，示例：FCSC03V000019179E00001，只能包含大写字母和数字，且长度为22位！\n");
                    }
                    else 
                    {
                        output_rich_textbox.AppendText(session);
                        string writeDeviceInfoCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\",\\\"value\\\": {{\\\"SN\\\": \\\"{writeINSN}\\\"}}}}\"";
                        output_string = executeCMDCommand(writeDeviceInfoCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                        string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                        // 获取SN号
                        string fetchDeviceInfoCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                        output_string = executeCMDCommand(fetchDeviceInfoCommand);
                        MatchCollection results_2 = Regex.Matches(output_string, "\"SN\" : (.*)");
                        string currentSN = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                        output_rich_textbox.AppendText("获取当前设备的SN号：" + currentSN + "\n");
                        if (currentSN == writeINSN && Int32.Parse(backCode) == 0)
                        {
                            output_rich_textbox.AppendText($"当前写入结果为：PASS，当前SN号：{currentSN}\n");
                            writeInSN_textbox.Text = currentSN;
                        }
                        else
                        {
                            output_rich_textbox.AppendText($"当前写入结果为：FAIL，当前SN号：{currentSN}\n");
                            writeInSN_textbox.Text = currentSN;
                        }
                    }
                }
                else
                {
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    device_status_label.Text = "已断开";
                    output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
                }
            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"写入指定的设备SN号失败，当前未连接设备：\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        // 密码转换为SHA256加密字符串 
        public static string SHA256EncryptString(string data)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);

                    using (SHA256 sha256 = SHA256.Create())
                    {
                        if (sha256 != null)
                        {
                            byte[] hash = sha256.ComputeHash(bytes);
                            if (hash.Length > 0)
                            {
                                for (int i = 0; i < hash.Length; i++)
                                {
                                    builder.Append(hash[i].ToString("x2"));
                                }
                                return builder.ToString();
                            }
                        }
                    }
                }

                return "";
            }
            catch (Exception e)
            {
                builder.Clear();
                return "";
            }
            finally
            {
                builder.Clear();
            }
        }
        string session;
        // 点击登录后进行密码SH256加密转换
        private void login_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("点击登录后进行密码SH256加密转换\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    try
                    {
                        string username = username_textbox.Text;
                        string password = password_textbox.Text;
                        string password_sha256 = SHA256EncryptString(password);
                        output_rich_textbox.AppendText($"登录成功，可以进行固件升级检测：{password_sha256}\n");
                        // 登录操作获取session - 后续IP以后续输入为主，当前暂定
                        string loginCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"login\\\", \\\"username\\\": \\\"{username}\\\",\\\"password\\\": \\\"{password_sha256}\\\"}}\"";
                        output_string = executeCMDCommand(loginCommand);
                        MatchCollection results = Regex.Matches(output_string, "\"session\" : (.*)");
                        session = results[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                        output_rich_textbox.AppendText("当前固件校验Session是：" + session + "\n");
                        check_current_firmware_button.Enabled = true;
                        upgrade_button.Enabled = true;
                        getCurrentSN_button.Enabled = true;
                        writeIn_button.Enabled = true;
                        getCurrentPCBA_button.Enabled = true;
                        writeInPCBA_button.Enabled = true;
                        start_array_mic_audio_level_test_button.Enabled = true;
                        stop_array_mic_audio_level_test_button.Enabled = true;
                        gain_array_mic_audio_level_button.Enabled = true;
                        // 增加记住username和password功能
                        if (rememberCheckBox.Checked == true)
                        {
                            FileStream fileStream = new FileStream("data1.bin", FileMode.OpenOrCreate);
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            User user = new User();
                            user.Username = username;
                            user.Password = password;
                            if (users.ContainsKey(user.Username))
                            {
                                // 如果存在就清除掉
                                users.Clear();
                            }
                            users.Add(user.Username, user);
                            binaryFormatter.Serialize(fileStream, users);
                            fileStream.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        output_rich_textbox.AppendText($"登录失败，密码或者用户名错误，请重新输入检查：\n excetion:{ex.ToString()}\n");
                    }
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"点击登录后进行密码SH256加密转换失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                radioButton_80.Enabled = true;
                radioButton_8080.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
            }
        }

        // 获取当前设备PCBA号
        private void getCurrentPCBA_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("获取当前设备PCBA号:\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // 获取当前设备PCBA号
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"PCBA\" : (.*)");
                    string currentPCBA = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                    output_rich_textbox.AppendText("获取当前设备PCBA号：" + currentPCBA + "\n");
                    currentPCBA_textbox.Text = currentPCBA;
                }
                else
                {
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    device_status_label.Text = "已断开";
                    output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
                }

            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"获取当前设备PCBA号失败，当前未连接设备：\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        // 写入指定PCBA号
        private void writeInPCBA_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("写入指定PCBA号:\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {

                    // 写入指定PCBA号
                    string writeINPCBA = writeINPCBA_textbox.Text;
                    if (string.IsNullOrEmpty(writeINPCBA) || writeINPCBA.Length != 19 || !new Regex("^[A-Z|0-9]+$").IsMatch(writeINPCBA))
                    {
                        writeINPCBA_textbox.Text = "请写入正确的PCBA号再进行刷入，示例：ABCDEFGHI0123456789，只能包含大写字母和数字，且长度为19位！\n";
                        output_rich_textbox.AppendText("请写入正确的PCBA号再进行刷入，示例：ABCDEFGHI0123456789，只能包含大写字母和数字，且长度为19位！\n");
                    }
                    else
                    {
                        output_rich_textbox.AppendText(session);
                        string writeDeviceInfoCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\",\\\"value\\\": {{\\\"PCBA\\\": \\\"{writeINPCBA}\\\"}}}}\"";
                        output_string = executeCMDCommand(writeDeviceInfoCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                        string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                        // 获取PCBA号
                        string fetchDeviceInfoCommand = $"curl -X POST \"http://10.66.30.69/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                        output_string = executeCMDCommand(fetchDeviceInfoCommand);
                        MatchCollection results_2 = Regex.Matches(output_string, "\"PCBA\" : (.*)");
                        string currentPCBA = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                        output_rich_textbox.AppendText("获取当前设备的PCBA号：" + currentPCBA + "\n");
                        if (currentPCBA == writeINPCBA && Int32.Parse(backCode) == 0)
                        {
                            output_rich_textbox.AppendText($"当前写入结果为：PASS，当前PCBA号：{currentPCBA}\n");
                            writeINPCBA_textbox.Text = currentPCBA;
                        }
                        else
                        {
                            output_rich_textbox.AppendText($"当前写入结果为：FAIL，当前SN号，超过3次写入也会失败，硬件只能写三次：{currentPCBA}\n");
                            writeINPCBA_textbox.Text = currentPCBA;
                        }
                    }
                }
                else
                {
                    device_ip_textbox.Enabled = true;
                    radioButton_80.Enabled = true;
                    radioButton_8080.Enabled = true;
                    device_status_label.Text = "已断开";
                    output_rich_textbox.AppendText("设备连接已断开，请先连接设备！\n");
                }
            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"写入指定PCBA号失败，当前未连接设备：\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        
    }
}