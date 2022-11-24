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
using Application = System.Windows.Forms.Application;

namespace SeewoTestTool
{
    public partial class Form1 : Form
    {
        Socket clientSocket;
        Dictionary<string, InternetPort> internetPorts = new Dictionary<string, InternetPort>();
        Dictionary<string, User> users = new Dictionary<string, User>();
        /**
         * 窗口构造方法：
         * 在这里进行：
         *  1、控件实例化
         *  2、数据读写操作
         *      A、"data.bin"：IP和端口号的记忆功能写入
         *      B、"data1.bin"：Web登录校验username和password功能写入
         */



        public Form1()
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            output_rich_textbox.AppendText("欢迎使用视熙测试标定软件！");
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
            //device_connect_button_Click(null, null);
            //login_button_Click(null, null);
        }

        private readonly ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        private void CallBackMethod(IAsyncResult asyncresult)
        {
            //使阻塞的线程继续 
            TimeoutObject.Set();
            
        }

        string ip_users;
        // 保证设备在连接状态 网口通信
    
        /**
         * 连接设备点击事件
         */
        private void device_connect_button_Click(object sender, EventArgs e)
        {
            output_rich_textbox.AppendText("【执行操作】进行设备绑定连接……\n");
            string host = device_ip_textbox.Text;

            // 检测如果ping不通直接FAIL无需往下继续执行
            string temp_check_ping_ip_exists = executeCMDCommand($"ping {host} -n 1");
            if (temp_check_ping_ip_exists.Contains("请求超时"))
            {
                MessageBox.Show("设备IP查找失败，请确认设备网口连接情况以及网络环境配置是否正常后重试！","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            // 判断如果是：219.198.235.11，需要在连接设备前加上ping操作打通路由，如果不是，则不需要ping 219.198.235.11 -t -S 219.198.235.17
            if (host == "219.198.235.11")
            {
                string localPCHost = null;
                foreach (NetworkInterface netItem in NetworkInterface.GetAllNetworkInterfaces())
                {
                    foreach (UnicastIPAddressInformation ipIntProp in netItem.GetIPProperties().UnicastAddresses.ToArray<UnicastIPAddressInformation>())
                    {
                        string inetName = netItem.Name;
                        string inetAddress = ipIntProp.Address.ToString();
                        string inetType = ipIntProp.Address.AddressFamily.ToString();
                        //output_rich_textbox.AppendText($"   接口名：{inetName}，IP：{inetAddress}，IP类型：{inetType}\n");
                        if (inetName == "以太网" && inetType == "InterNetwork")
                        {
                            localPCHost = inetAddress;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(localPCHost))
                {
                    output_rich_textbox.AppendText($"当前连接为局域网直连，本地PC的IP地址为：{localPCHost}\n");
                    string back_temp = executeCMDCommand($"ping {host} -t -S {localPCHost} -n 1");
                    output_rich_textbox.AppendText($"当前连接为局域网直连：IP：219.198.235.11\nPING 打通默认路由结果为：{back_temp}\n");
                }
                else
                {
                    MessageBox.Show("请检查设备网络连接！","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            ip_users = host;
            string port;
            if (radioButton_80.Checked)
            {
                port = radioButton_80.Text;
            }
            else 
            {
                port = radioButton_8080.Text;
            }
            ip_users = host + ":" + port;
            if (string.IsNullOrEmpty(host))
            {
                output_rich_textbox.AppendText("设备网口IP地址和端口号不能为空！\n");
            }
            else
            {
                // Socket Connection Build
                try
                {
                    TimeoutObject.Reset();
                    
                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint ipe = new IPEndPoint(ip, int.Parse(port));

                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    clientSocket.BeginConnect(ipe, new AsyncCallback(CallBackMethod), clientSocket);
                    if (TimeoutObject.WaitOne(2000, false))
                    {
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
                            device_reset_button.Enabled = true;
                            rebootDevice_button.Enabled = true;
                            button1.Enabled = true;
                            button2.Enabled = true;
                            upgrade_progressbar.Value = 0;
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
                            Thread.Sleep(1);
                            login_button_Click(null, null);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"连接超时，请检查！\nIP:{host}， PORT:{port}\n如果多次连接尝试无果，可尝试重启设备后再做连接操作！","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                }
            }
        }

        // 在cmd中执行命令操作
        /**
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

        // Socket发送命令函数
        /**
         *  提供了Socket端发送命令函数，当前需求未用到，保留该函数，后续项目有使用可以拿来用
         */
        private void send_Str(string sendStr)
        {
            output_rich_textbox.AppendText($"【执行操作】Socket发送指定命令{sendStr}……\n");
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                // Socket发送命令
                try
                {
                    byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);
                    clientSocket.Send(sendBytes);
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"Socket发送命令执行失败：\n{ex.ToString()}\n");
                    device_disconnect_button_Click(null, null);
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
        /**
         *  提供了Socket端接收设备终端返回值函数，当前需求未用到，保留该函数，后续项目有使用可以拿来用
         */
        private string receive_Str()
        {
            output_rich_textbox.AppendText("【执行操作】Socket接收返回内容……\n");
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
                        device_disconnect_button_Click(null, null);
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
        /**
         * 断开设备按钮点击事件
         */
        private void device_disconnect_button_Click(object sender, EventArgs e)
        {
            output_rich_textbox.AppendText("【执行操作】断开当前已连接设备……\n");
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    output_rich_textbox.AppendText("断开当前已连接设备\n");
                    stop_array_mic_audio_level_test_button_Click(null, null);
                    device_disconnect_button.Enabled = false;
                    device_connect_button.Enabled = true;
                    clientSocket.Close();
                    clientSocket = null;
                    upgrade_progressbar.Value = 0;
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
                    gainCurrentVersion_button.Enabled = false;
                    login_button.Text = "登录";
                    login_button.Enabled = true;
                    device_reset_button.Enabled = false;
                    rebootDevice_button.Enabled = false;
                    stop_rg_flicker_button.Enabled = false;
                    start_rg_flicker_button.Enabled = false;
                    get_poe_mic_info_button.Enabled = false;
                    login_button.Enabled = false;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    audioIn1_test_button.Enabled = false;
                    audioIn2_test_button.Enabled = false;
                    login_button.Text = "设备连接后\n可自动登录";
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
        /**
         * 选择升级固件包按钮点击事件
         */
        private void choose_upgrade_firmware_button_Click(object sender, EventArgs e)
        {
            output_rich_textbox.AppendText("【执行操作】选择升级固件……\n");
            //if(true)
            if (clientSocket != null && clientSocket.Connected)
            {
                OpenFileDialog dialog = new OpenFileDialog();
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
        /**
         * 升级按钮点击事件
         */
        private void upgrade_button_Click(object sender, EventArgs e)
        {
            output_rich_textbox.AppendText("【执行操作】固件升级……\n");
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
                        upgrade_progressbar.Value = 0;
                        device_disconnect_button_Click(null, null);
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
        /**
         * 检验当前固件包版本按钮点击事件
         */
        private void check_current_firmware_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】校验当前设备固件版本与固件包版本是否一致……\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                output_rich_textbox.AppendText("校验固件测试！\n");
                string currentFirmware = null;
                try
                {
                    // 校验固件 - 从session中获取固件当前版本
                    string checkVersionCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\":\\\"getParam\\\",\\\"session\\\":\\\"{session}\\\",\\\"name\\\":\\\"DevInfo\\\"}}\"";
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
        /**
         * 打开红绿指示灯交替闪烁按钮点击事件
         */
        private void start_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】打开红绿灯交替闪烁……\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // 打开红绿灯交替闪烁
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"ledTest\\\",\\\"open\\\": true}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    string result = "打开红绿灯交替闪烁操作未执行成功";
                    if (back_code == "0")
                    {
                        result = "成功";
                        stop_rg_flicker_button.Enabled = true;
                        start_rg_flicker_button.Enabled = false;
                    }
                    else
                    {
                        result = "失败";
                    }
                    output_rich_textbox.AppendText("打开红绿灯交替闪烁结果：" + result + "【back_code】："+ back_code + "\n");

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
        /**
         * 关闭红绿指示灯交替闪烁按钮点击事件
         */
        private void stop_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】关闭红绿灯交替闪烁……\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // 关闭红绿灯交替闪烁
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"ledTest\\\",\\\"open\\\": false}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    string result = "关闭红绿灯交替闪烁操作未执行成功";
                    if (back_code == "0")
                    {
                        result = "成功";
                        stop_rg_flicker_button.Enabled = false;
                        start_rg_flicker_button.Enabled = true;
                    }
                    else
                    {
                        result = "失败";
                    }
                    output_rich_textbox.AppendText("关闭红绿灯交替闪烁结果：" + result + "【back_code】：" + back_code + "\n");

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
        /**
         * 获取吊麦信息按钮点击事件
         */
        private void get_poe_mic_info_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("该功能目前未实现，预留位后续有需求再实现！","提示", MessageBoxButtons.OK,MessageBoxIcon.Information);
            //if (true)
            output_rich_textbox.AppendText("【执行操作】获取吊麦信息……\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // 获取吊麦信息
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"microphoneInfo\\\"}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    string result = "获取吊麦信息操作未执行成功";
                    if (back_code == "0")
                    {
                        result = "成功";
                        MatchCollection results_2 = Regex.Matches(output_string, "\"micHardwareVersion\" : (.*)");
                        string micHardwareVersion = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",","");
                        MatchCollection results_3 = Regex.Matches(output_string, "\"micSoftwareVersion\" : (.*)");
                        string micSoftwareVersion = results_3[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                        poe_mic_hardware_info_label.Text = micHardwareVersion;
                        poe_mic_firmware_info_label.Text = micSoftwareVersion;
                    }
                    else
                    {
                        result = "失败";
                    }
                    output_rich_textbox.AppendText("获取吊麦信息结果：" + result + "【back_code】：" + back_code + "\n");
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
        /**
         * Audio IN 1口测试点击事件，暂时用不上，保留该方法
         */
        private void audioin1_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】Audio IN 1口测试……\n");
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
        /**
         * Audio IN 2口测试点击事件，暂时用不上，保留该方法
         */
        private void audioin2_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】Audio IN 2口测试……\n");
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
        /**
         * 开启阵列麦克风音量值测试
         */
        private void array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】开启阵列MIC音量值测试……\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // 开启阵列MIC音量值测试
                    output_rich_textbox.AppendText(session);
                    string beginDeviceMICVolumeTestCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"startAudioTest\\\",\\\"format\\\": 0,\\\"soundmode\\\": 8,\\\"samplerate\\\": 16000,\\\"periodsize\\\": 1024}}\"";
                    output_string = executeCMDCommand(beginDeviceMICVolumeTestCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    
                    if (Int32.Parse(backCode) == 0)
                    {
                        output_rich_textbox.AppendText($"执行结果为：PASS，已开启阵列MIC音量值测试，backCode:[{backCode}]\n");
                        start_array_mic_audio_level_test_button.Enabled = false;
                        stop_array_mic_audio_level_test_button.Enabled = true;
                        gain_array_mic_audio_level_button.Enabled = true;
                        audioIn1_test_button.Enabled = true;
                        audioIn2_test_button.Enabled = true;
                    }
                    else if(Int32.Parse(backCode) == -1)
                    {
                        output_rich_textbox.AppendText($"执行结果为：FAIL，未开启阵列MIC音量值测试，backCode:[{backCode}]\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"开启阵列MIC音量值测试失败，请检查是否以开启了音频测试未正确关闭！\n可尝试重启机器恢复正常：\n{ex.ToString()}\n");
                    output_rich_textbox.AppendText($"开启阵列MIC音量值测试失败，请检查是否以开启了音频测试未正确关闭！\n可尝试重启机器恢复正常：\n{ex.ToString()}\n");
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
        /**
         * 停止阵列麦克风音量值测试
         */
        private void stop_array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】停止阵列MIC音量值测试……\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // 停止阵列MIC音量值测试
                    output_rich_textbox.AppendText(session);
                    string stopDeviceMICVolumeTestCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"stopAudioTest\\\"}}\"";
                    output_string = executeCMDCommand(stopDeviceMICVolumeTestCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                    if (Int32.Parse(backCode) == 0)
                    {
                        start_array_mic_audio_level_test_button.Enabled = true;
                        stop_array_mic_audio_level_test_button.Enabled = false;
                        gain_array_mic_audio_level_button.Enabled = false;
                        audioIn1_test_button.Enabled = false;
                        audioIn2_test_button.Enabled = false;
                        output_rich_textbox.AppendText($"执行结果为：PASS，已停止阵列MIC音量值测试，backCode:[{backCode}]\n");
                    }
                    else if (Int32.Parse(backCode) == -1)
                    {
                        output_rich_textbox.AppendText($"执行结果为：FAIL，无法关闭阵列MIC音量值测试，backCode:[{backCode}]\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"停止阵列MIC音量值测试失败，请检查是否以开启了音频测试未正确关闭！\n可尝试重启机器恢复正常：\n{ex.ToString()}\n");
                    output_rich_textbox.AppendText($"停止阵列MIC音量值测试失败，请检查是否以开启了音频测试未正确关闭！\n可尝试重启机器恢复正常：\n{ex.ToString()}\n");
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
        /**
         * 开启阵列麦克风音量值测试后，对8路麦克风的值进行获取
         */
        private void gain_array_mic_audio_level_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】获取各路MIC音频音量值……\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // 获取各路MIC音频音量值
                    output_rich_textbox.AppendText(session);
                    string gainDeviceMICVolumeCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getAudioVolume\\\"}}\"";
                    output_string = executeCMDCommand(gainDeviceMICVolumeCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    output_rich_textbox.AppendText(results_1.ToString());
                    string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",","");
                    if (Int32.Parse(backCode) == 0)
                    {
                        MatchCollection results_2 = Regex.Matches(output_string, "\\\"rmsdb\\\" : \\[\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*)\\n   ]");
                        string[] temp = results_2[0].ToString().Replace("\"rmsdb\" : [", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Replace(" ", "").Split(",");
                        string volume1 = temp[0].ToString();
                        string volume2 = temp[1].ToString();
                        string volume3 = temp[2].ToString();
                        string volume4 = temp[3].ToString();
                        string volume5 = temp[4].ToString();
                        string volume6 = temp[5].ToString();
                        string volume7 = temp[6].ToString();
                        string volume8 = temp[7].ToString();
                        output_rich_textbox.AppendText($"执行结果为：PASS，获取各路MIC音频音量值，backCode:[{backCode}]\n");
                        volume1_value_label.Text = volume1;
                        volume2_value_label.Text = volume2;
                        volume3_value_label.Text = volume3;
                        volume4_value_label.Text = volume4;
                        volume5_value_label.Text = volume5;
                        volume6_value_label.Text = volume6;
                        volume7_value_label.Text = volume7;
                        volume8_value_label.Text = volume8;
                        output_rich_textbox.AppendText($"rmsdb1：{volume1}\nrmsdb2：{volume2}\nrmsdb3：{volume3}\nrmsdb4：{volume4}\nrmsdb5：{volume5}\nrmsdb6：{volume6}\nrmsdb7：{volume7}\nrmsdb8：{volume8}\n");

                        float volume_1_f = float.Parse(volume1);
                        float volume_2_f = float.Parse(volume2);
                        float volume_3_f = float.Parse(volume3);
                        float volume_4_f = float.Parse(volume4);
                        float volume_5_f = float.Parse(volume5);
                        float volume_6_f = float.Parse(volume6);
                        float volume_7_f = float.Parse(volume7);
                        float volume_8_f = float.Parse(volume8);

                        float[] volumes_f = { volume_1_f, volume_2_f, volume_4_f, volume_5_f, volume_6_f, volume_8_f};
                        float maxINArray = volumes_f.Max();
                        float minINArray = volumes_f.Min();
                        if (Math.Abs(maxINArray) - Math.Abs(minINArray) <= 3 && (Math.Abs(volume_1_f) > 70 && Math.Abs(volume_2_f) > 70 && Math.Abs(volume_4_f) > 70 && Math.Abs(volume_5_f) > 70
                            && Math.Abs(volume_6_f) > 70 && Math.Abs(volume_8_f) > 70))
                        {
                            arrayMICTestResult_label.Text = "PASS";
                        }
                        else
                        {
                            arrayMICTestResult_label.Text = "FAIL";
                        }
                    }
                    else if (Int32.Parse(backCode) == -1)
                    {
                        output_rich_textbox.AppendText($"执行结果为：FAIL，无法获取各路MIC音频音量值，backCode:[{backCode}]\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"获取各路MIC音频音量值失败，请检查是否以开启了音频测试未正确关闭！\n可尝试重启机器恢复正常：\n{ex.ToString()}\n");
                    output_rich_textbox.AppendText($"获取各路MIC音频音量值失败，请检查是否以开启了音频测试未正确关闭！\n可尝试重启机器恢复正常：\n{ex.ToString()}\n");
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
        /**
         * 设备复位按钮点击事件
         */
                        private void device_reset_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】设备复位……\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // 设备复位
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"control\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"restoreSettings\\\"}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    string result = "复位操作未执行成功";
                    if (back_code == "0")
                    {
                        result = "成功";
                        device_disconnect_button_Click(null, null);
                    }
                    else
                    {
                        result = "失败";
                    }
                    output_rich_textbox.AppendText("设备复位结果：" + result + "\n");
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
                output_rich_textbox.AppendText($"设备复位失败，当前未连接设备：\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        // 窗体关闭事件，将设备socket连接释放掉
        /**
         * 重写窗体关闭事件，关闭时自动释放一些连接
         */
        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            output_rich_textbox.AppendText("【执行操作】窗体关闭事件，将设备socket连接释放掉……\n");
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // 释放掉音频测试的内容
                    stop_array_mic_audio_level_test_button_Click(null, null);
                    clientSocket.Close();
                    // 关闭SXW0301_Production_line.exe
                    executeCMDCommand("taskkill /f /t /im SeevisionTestTool.exe");
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"窗体关闭将设备socket连接释放掉失败：\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
            else
            {
                //executeCMDCommand("taskkill /f /t /im SeevisionTestTool.exe");
                new Regist().Show();
            }
        }

        // 输出列表始终显示最新内容Log
        /**
         * 定义输出框内容光标定位情况
         */
        private void richTextChanged_to(object sender, EventArgs e)
        {
            // 将光标位置设置到当前内容的末尾
            output_rich_textbox.SelectionStart = output_rich_textbox.Text.Length;
            // 滚动到光标位置
            output_rich_textbox.ScrollToCaret();
        }

        // 清空输出内容
        /**
         * 清空按钮点击事件
         */
        private void clear_output_button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(output_rich_textbox.Text))
            {
                output_rich_textbox.Text = "";
                output_rich_textbox.ForeColor = Color.Black;
                //Font font2 = new Font(FontFamily., 9, FontStyle.Regular);
                //output_rich_textbox.Font = font2;
            }
            output_rich_textbox.AppendText("【执行操作】清空输出内容……\n");
        }

        string output_string = "";
        // 新建backgroundworker给固件升级操作，防止UI阻塞交互卡死
        /**
         *  BackgroundWorker后台事件，DoWork重写，此处只能数据耗时操作，不能有UI操作
         */
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
                                        string ftp_ip_user = ip_users.Split(":")[0];
                                        output_string = executeCMDCommand($"curl -T {filePath} \"ftp://{ftp_ip_user}/\"");
                                        progress_i += 30;
                                        backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Pushing\n");
                                        System.Threading.Thread.Sleep(1000);

                                        // 升级操作 - 创建文件夹 need_upgrade
                                        if (!System.IO.Directory.Exists("need_upgrade"))
                                        {
                                            output_string = executeCMDCommand("type nul> need_upgrade");
                                        }
                                        progress_i += 20;
                                        backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Createing\n");
                                        System.Threading.Thread.Sleep(1000);

                                        // 升级操作 - 开始升级
                                        output_string = executeCMDCommand($"curl -T need_upgrade \"ftp://{ftp_ip_user}/\"");
                                        progress_i += 5;
                                        backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Upgrading\n");
                                        System.Threading.Thread.Sleep(1000);

                                        // 升级操作 - 检测升级状态
                                        bool upgradeDone = false;
                                        if (System.IO.File.Exists("upgrade_result"))
                                        {
                                            output_string = executeCMDCommand("del upgrade_result");
                                        }
                                        while (!upgradeDone)
                                        {
                                            output_string = executeCMDCommand($"curl \"ftp://{ftp_ip_user}/upgrade_result\" -o upgrade_result");
                                            if (!System.IO.File.Exists("upgrade_result"))
                                            {
                                                // 如果2s内没有获取到这个文件直接报升级失败
                                                backgroundworker_firmwareupgrade.CancelAsync();
                                                output_string = "未检测到upgrade_result文件，升级失败！\n";
                                                break;
                                            }
                                            // 检测到该文件，进行cat读取内容
                                            output_string = executeCMDCommand("type upgrade_result");
                                            if (output_string.Contains("start"))
                                            {
                                                continue;
                                            }
                                            if (output_string.Contains("success") || output_string.Contains("fail"))
                                            {
                                                //progress_i = 100;
                                                backgroundworker_firmwareupgrade.ReportProgress(progress_i, "Upgrading\n");
                                                backgroundworker_firmwareupgrade.CancelAsync();
                                                break;
                                            }
                                            progress_i += 5;
                                            if (progress_i >= 100)
                                            {
                                                //progress_i = 100;
                                            }
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
                device_disconnect_button_Click(null, null);
            }
            
        }

        /**
         *  BackgroundWorker的进度改变操作，实时上报耗时任务进展情况，此处进行UI操作
         */
        private void backgroundworker_firmwareupgrade_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            upgrade_progressbar.Value = e.ProgressPercentage;
            output_rich_textbox.AppendText($"升级进度：{Convert.ToString(e.ProgressPercentage)}%\n");


            // 临时增加执行cmd命令验证
            output_rich_textbox.AppendText(output_string);
        }

        int lastValue;
        private void waitForReboot()
        {
            //MessageBox.Show("请稍等设备正在重启中……\n", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            while (true)
            {
                Font font = new Font(FontFamily.GenericMonospace, 25, FontStyle.Bold);
                output_rich_textbox.ForeColor = Color.MediumVioletRed;
                output_rich_textbox.SelectionFont = font;
                output_rich_textbox.AppendText("请稍等设备正在重启中……\n");
                Thread.Sleep(5000);
                string temp_check_ping_ip_exists = executeCMDCommand($"ping {device_ip_textbox.Text} -n 1");
                if (upgrade_progressbar.Value != 0)
                {
                    lastValue = 100 - upgrade_progressbar.Value;
                    if (lastValue != 0)
                    {
                        upgrade_progressbar.Value += 10;
                    }
                }

                if (temp_check_ping_ip_exists.Contains("TTL"))
                {
                    Thread.Sleep(8000);
                    thread_reboot1.Interrupt();
                    device_disconnect_button_Click(null, null);
                    output_rich_textbox.AppendText("设备重启完成！\n");
                    upgrade_progressbar.Value = 100;
                    check_current_firmware_button.Enabled = false;
                    upgrade_button.Enabled = false;
                    getCurrentSN_button.Enabled = false;
                    writeIn_button.Enabled = false;
                    getCurrentPCBA_button.Enabled = false;
                    writeInPCBA_button.Enabled = false;
                    login_button.Enabled = false;
                    login_button.Text = "登录";
                    login_button.Enabled = true;
                    start_array_mic_audio_level_test_button.Enabled = false;
                    stop_array_mic_audio_level_test_button.Enabled = false;
                    gain_array_mic_audio_level_button.Enabled = false;
                    gainCurrentVersion_button.Enabled = false;
                    device_reset_button.Enabled = false;
                    rebootDevice_button.Enabled = false;
                    stop_rg_flicker_button.Enabled = false;
                    start_rg_flicker_button.Enabled = false;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    get_poe_mic_info_button.Enabled = false;
                    return;
                }
            }
            
            
        }
        Thread thread_reboot1;

        /**
         *  BackgroundWorker后台事件，耗时操作完成后的操作，此处多为进行UI操作，结束收尾，数据回收，结果反映
         */
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
                    upgrade_button.Enabled = true;
                }
                else if (output_string.Contains("fail"))
                {
                    output_rich_textbox.ForeColor = Color.Red;
                    output_rich_textbox.SelectionFont = font;
                }
                output_rich_textbox.AppendText(output_string);
                if (output_string.Contains("Upgrade finish"))
                {
                    thread_reboot1 = new Thread(waitForReboot);
                    thread_reboot1.IsBackground = true;
                    thread_reboot1.Start();
                    /**
                    output_rich_textbox.AppendText("等待20秒设备正在重启中，期间无法操作工具……\n");
                    System.Threading.Thread.Sleep(20000);
                    */
                    // 这里升级完重启，需要重新连接设备，设备状态那边需要同步更新
                }
            }
            else
            {
                output_rich_textbox.AppendText("升级终止！\n");
            }
        }
        
        // 获取当前电脑连接的所有设备的网口并筛选出Seewo的设备
        /**
         *  刷新网口点击事件，获取当前设备所关联的所有IP地址
         */
        private void getSeewoDevice_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】获取当前电脑连接的所有通过以太网口连接的设备……\n");
            try
            {
                foreach (NetworkInterface netItem in NetworkInterface.GetAllNetworkInterfaces())
                {
                    /*
                    output_rich_textbox.AppendText($"接口名：{netItem.Name}，接口类型：{netItem.NetworkInterfaceType}，" +
                        $"接口MAC：{netItem.GetPhysicalAddress().ToString()}\n");
                    */
                    foreach (UnicastIPAddressInformation ipIntProp in netItem.GetIPProperties().UnicastAddresses.ToArray<UnicastIPAddressInformation>())
                    {
                        string inetName = netItem.Name;
                        string inetAddress = ipIntProp.Address.ToString();
                        string inetType = ipIntProp.Address.AddressFamily.ToString();
                        //output_rich_textbox.AppendText($"   接口名：{inetName}，IP：{inetAddress}，IP类型：{inetType}\n");
                        if (inetName == "以太网" && inetType == "InterNetwork")
                        {
                            output_rich_textbox.AppendText($"   接口名：{inetName}，IP：{inetAddress}，IP类型：{inetType}\n");
                        }
                    }
                }
                /*
                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] endPoints = ipGlobalProperties.GetActiveTcpListeners();
                foreach (IPEndPoint endPoint in endPoints)
                {
                    output_rich_textbox.AppendText($"端口：{endPoint.Port}，IP：{endPoint.Address.ToString()}，IP类型：{endPoint.Address.AddressFamily.ToString()}\n");
                }
                */
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
        /**
         *  获取当前设备序列号按钮点击事件
         */
        private void getCurrentSN_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】获取当前设备的SN号……\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // 获取SN号
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"SN\" : (.*)");
                    string currentSN = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    output_rich_textbox.AppendText("当前设备的SN号为：" + currentSN + "\n");
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
        /**
         * 写入序列号按钮点击事件
         */
        private void writeIn_button_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("功能暂时关闭以防止误操作，每块板子只有三次写入机会。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //if (true)
            output_rich_textbox.AppendText("【执行操作】写入指定的设备SN号……\n");
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
                        string writeDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\",\\\"value\\\": {{\\\"SN\\\": \\\"{writeINSN}\\\"}}}}\"";
                        output_string = executeCMDCommand(writeDeviceInfoCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                        string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                        // 获取SN号
                        string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
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
        /**
         * Web端登录校验函数，主要是对密码进行SHA256加密转换
         */
        private void login_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】点击登录后进行密码SH256加密转换……\n");
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
                        string loginCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"login\\\", \\\"username\\\": \\\"{username}\\\",\\\"password\\\": \\\"{password_sha256}\\\"}}\"";
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
                        stop_array_mic_audio_level_test_button.Enabled = false;
                        gain_array_mic_audio_level_button.Enabled = false;
                        gainCurrentVersion_button.Enabled = true;
                        login_button.Text = "已登录";
                        login_button.Enabled = false;
                        stop_rg_flicker_button.Enabled = false;
                        start_rg_flicker_button.Enabled = true;
                        get_poe_mic_info_button.Enabled = true;
                        
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
        /**
         * 获取当前设备PCBA号按钮点击事件
         */
        private void getCurrentPCBA_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】获取当前设备PCBA号……\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // 获取当前设备PCBA号
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
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
        /**
         * 写入PCBA号点击事件
         */
        private void writeInPCBA_button_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("功能暂时关闭以防止误操作，每块板子只有三次写入机会。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //if (true)
            output_rich_textbox.AppendText("【执行操作】写入指定PCBA号……\n");
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
                        string writeDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\",\\\"value\\\": {{\\\"PCBA\\\": \\\"{writeINPCBA}\\\"}}}}\"";
                        output_string = executeCMDCommand(writeDeviceInfoCommand);
                        MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                        string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");

                        // 获取PCBA号
                        string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"SerialNumber\\\"}}\"";
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

        // 获取当前设备版本
        /**
         *  获取当前设备的固件版本号按钮点击事件
         */
        private void gainCurrentVersion_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】获取当前设备版本……\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // 校验固件 - 从session中获取固件当前版本
                    string checkVersionCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\":\\\"getParam\\\",\\\"session\\\":\\\"{session}\\\",\\\"name\\\":\\\"DevInfo\\\"}}\"";
                    output_string = executeCMDCommand(checkVersionCommand);
                    MatchCollection results_2 = Regex.Matches(output_string, "\"SoftwaveVersion\" : (.*)");
                    string currentVersion = results_2[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    output_rich_textbox.AppendText("当前版本是：" + currentVersion + "\n");
                    currentVersion_label.Text = currentVersion;
                }
                catch (Exception ex)
                {
                    currentVersion_label.Text = "获取当前设备版本失败";
                    output_rich_textbox.AppendText($"获取当前设备版本失败：\n{ex.ToString()}\n");
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

        // 重启设备操作
        /**
         * 重启设备按钮点击事件
         */
        private void rebootDevice_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】重启设备……\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // 重启设备操作
                    string fetchDeviceInfoCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"control\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"reboot\\\"}}\"";
                    output_string = executeCMDCommand(fetchDeviceInfoCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    string back_code = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                    string result = "重启设备操作未执行成功";
                    if (back_code == "0")
                    {
                        result = "成功";
                        thread_reboot1 = new Thread(waitForReboot);
                        thread_reboot1.IsBackground = true;
                        thread_reboot1.Start();
                        //device_disconnect_button_Click(null, null);
                        /**
                        output_rich_textbox.AppendText("等待20秒设备正在重启中，期间无法操作工具……\n");
                        System.Threading.Thread.Sleep(20000);
                        */
                    }
                    else
                    {
                        result = "失败";
                    }
                    output_rich_textbox.AppendText("重启设备结果：" + result + "\n");
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
                output_rich_textbox.AppendText($"重启设备失败，当前未连接设备：\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }
        public static byte[] ConvertToBinary(string Path)
        {
            FileStream stream = new FileInfo(Path).OpenRead();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
            return buffer;
        }

        // 标定数据写入操作
        private void calibrationDataWriteIn_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】标定数据写入操作……\n");
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    // 标定数据写入操作
                    string filePath = "";
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Multiselect = false;
                    dialog.Title = "请选择标定数据文件";
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        filePath = dialog.FileName;
                        output_rich_textbox.AppendText($"当前写入的标定数据为：【{filePath}】\n");
                        //byte[] br2Binary = ConvertToBinary(filePath);
                        string fetchDeviceInfoCommand = $"curl -s -X POST --data-binary @{filePath} \"http://{ip_users}/writeLdcCalib_api\" -H \"Content-Type: text/plain\"";
                        Font font = new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold);
                        output_rich_textbox.ForeColor = Color.Green;
                        output_rich_textbox.SelectionFont = font;
                        output_rich_textbox.AppendText("请等待一段时间写入完成，此间工具无法使用\n");
                        output_string = executeCMDCommand(fetchDeviceInfoCommand);
                        output_rich_textbox.AppendText($"【{fetchDeviceInfoCommand}】标定数据写入操作结果：" + output_string + "\n");
                        
                        string result = "标定数据写入操作未执行成功";
                        if (output_string.Contains("OK"))
                        {
                            result = "成功";
                        }
                        else
                        {
                            result = "失败";
                        }
                        output_rich_textbox.AppendText("标定数据写入操作结果：" + result + "\n");
                        
                    }
                    else
                    {
                        output_rich_textbox.AppendText("未选择标定数据，选择后才能进行标定数据写入！\n");
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
                output_rich_textbox.AppendText($"标定数据写入操作失败，当前未连接设备：\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        private void openByCMD_CameraProcess()
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                // 打开三摄模组测试工具
                Process process_cmd = new Process();
                string output_string = null;
                try
                {
                    process_cmd.StartInfo.FileName = "SXW0301_Production_line.exe";
                    process_cmd.StartInfo.RedirectStandardInput = true;
                    //process_cmd.StartInfo.RedirectStandardOutput = true;
                    process_cmd.StartInfo.CreateNoWindow = false;
                    process_cmd.StartInfo.UseShellExecute = false;
                    process_cmd.Start();
                    process_cmd.WaitForExit();
                    //process_cmd.StandardInput.AutoFlush = true;
                }
                catch (Exception ex)
                {
                    output_string = ex.ToString();
                }
                finally
                {

                    process_cmd.Close();
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

        // 打开三摄模组测试工具
        Thread thread = null;
        private void open3CameraTest_button_Click(object sender, EventArgs e)
        {
            //if (true)
            try
            {
                if (thread == null)
                {
                    thread = new Thread(openByCMD_CameraProcess);
                    output_rich_textbox.AppendText("【执行操作】打开三摄模组测试工具……\n");
                    thread.IsBackground = true;
                    thread.Start();
                }
                else
                {
                    if (thread.IsAlive)
                    {
                        output_rich_textbox.AppendText("已打开三摄模组测试工具，请勿重复打开哦\n");
                    }
                    else
                    {
                        thread = new Thread(openByCMD_CameraProcess);
                        output_rich_textbox.AppendText("【执行操作】打开三摄模组测试工具……\n");
                        thread.IsBackground = true;
                        thread.Start();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"打开三摄模组测试工具失败，当前未连接设备：\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }

        SXW0301_Production_line.Fom1 fom1;
        private void button1_Click(object sender, EventArgs e)
        {
            output_rich_textbox.AppendText("【执行操作】打开三摄标定工具操作……\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                if (fom1 == null)
                {
                    fom1 = new SXW0301_Production_line.Fom1();
                    fom1.Show();
                }
                else if (fom1.IsDisposed)
                {
                    fom1 = new SXW0301_Production_line.Fom1();
                    fom1.Activate();
                    fom1.Show();
                }
                else
                {
                    output_rich_textbox.AppendText("已打开三摄标定工具，请勿重复打开哦\n");
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

        SXW0301_Production_line.Form3 form3;
        private void button2_Click(object sender, EventArgs e)
        {
            output_rich_textbox.AppendText("【执行操作】打开三摄拼接图检测工具操作……\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                // 打开拼接图码流前将 Merge码率 curl 修改码流至61440
                int rate = 35000;
                string updateBitRateCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"setParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"Camera0Chn0\\\",\\\"value\\\": {{\\\"BitRate\\\": {rate}}}}}\"";
                output_string = executeCMDCommand(updateBitRateCommand);
                MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "");
                if (backCode == "0")
                {
                    string getBitRateCommand = $"curl -X POST \"http://{ip_users}/json_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getParam\\\",\\\"session\\\": \\\"{session}\\\",\\\"name\\\": \\\"Camera0Chn0\\\"}}\"";
                    output_string = executeCMDCommand(getBitRateCommand);
                    if (output_string.Contains(rate.ToString()))
                    {
                        output_rich_textbox.AppendText($"当前拼接流视频码率为：【{rate}】\n");
                        if (form3 == null)
                        {
                            form3 = new SXW0301_Production_line.Form3();
                            form3.Show();
                        }
                        else if (form3.IsDisposed)
                        {
                            form3 = new SXW0301_Production_line.Form3();
                            form3.Activate();
                            form3.Show();
                        }
                        else
                        {
                            output_rich_textbox.AppendText("已打开三摄拼接图检测工具，请勿重复打开哦\n");
                        }
                    }
                    else
                    {
                        output_rich_textbox.AppendText($"拼接流码率【{rate}】设置失败,返回值\n{output_string}\n，请检查！\n");
                    }
                }
                else
                {
                    output_rich_textbox.AppendText($"拼接流码率【{rate}】设置失败,返回值【{backCode}】，请检查！");
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

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void uiGroupBox3_Click(object sender, EventArgs e)
        {

        }

        // Audio IN 1 测试
        private void audioIn1_test_button_Click_1(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】Audio IN 1 测试……\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // 获取各路MIC音频音量值
                    output_rich_textbox.AppendText(session);
                    string gainDeviceMICVolumeCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getAudioVolume\\\"}}\"";
                    output_string = executeCMDCommand(gainDeviceMICVolumeCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    output_rich_textbox.AppendText(results_1.ToString());
                    string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                    if (Int32.Parse(backCode) == 0)
                    {
                        MatchCollection results_2 = Regex.Matches(output_string, "\\\"rmsdb\\\" : \\[\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*)\\n   ]");
                        string[] temp = results_2[0].ToString().Replace("\"rmsdb\" : [", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Replace(" ", "").Split(",");
                        string volume1 = temp[0].ToString();
                        string volume2 = temp[1].ToString();
                        string volume3 = temp[2].ToString();
                        string volume4 = temp[3].ToString();
                        string volume5 = temp[4].ToString();
                        string volume6 = temp[5].ToString();
                        string volume7 = temp[6].ToString();
                        string volume8 = temp[7].ToString();
                        output_rich_textbox.AppendText($"执行结果为：PASS，获取各路MIC音频音量值，backCode:[{backCode}]\n");
                        volume1_value_label.Text = volume1;
                        volume2_value_label.Text = volume2;
                        volume3_value_label.Text = volume3;
                        volume4_value_label.Text = volume4;
                        volume5_value_label.Text = volume5;
                        volume6_value_label.Text = volume6;
                        volume7_value_label.Text = volume7;
                        volume8_value_label.Text = volume8;
                        output_rich_textbox.AppendText($"rmsdb1：{volume1}\nrmsdb2：{volume2}\nrmsdb3：{volume3}\nrmsdb4：{volume4}\nrmsdb5：{volume5}\nrmsdb6：{volume6}\nrmsdb7：{volume7}\nrmsdb8：{volume8}\n");

                        float volume_1_f = float.Parse(volume1);
                        float volume_2_f = float.Parse(volume2);
                        float volume_3_f = float.Parse(volume3);
                        float volume_4_f = float.Parse(volume4);
                        float volume_5_f = float.Parse(volume5);
                        float volume_6_f = float.Parse(volume6);
                        float volume_7_f = float.Parse(volume7);
                        float volume_8_f = float.Parse(volume8);

                        if (Math.Abs(Math.Abs(volume_3_f) - Math.Abs(volume_7_f)) <= 3 && (Math.Abs(volume_3_f) > 70 && Math.Abs(volume_7_f) > 70))
                        {
                            audioin1_result_label.Text = "PASS";
                        }
                        else
                        {
                            audioin1_result_label.Text = "FAIL";
                        }
                    }
                    else if (Int32.Parse(backCode) == -1)
                    {
                        output_rich_textbox.AppendText($"执行结果为：FAIL，无法获取各路MIC音频音量值，backCode:[{backCode}]\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Audio IN 1 测试失败失败，请检查是否以开启了音频测试未正确关闭！\n可尝试重启机器恢复正常：\n{ex.ToString()}\n");
                    output_rich_textbox.AppendText($"Audio IN 1 测试失败失败，请检查是否以开启了音频测试未正确关闭！\n可尝试重启机器恢复正常：\n{ex.ToString()}\n");
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

        // Audio IN 2 测试
        private void audioIn2_test_button_Click_1(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("【执行操作】Audio IN 2 测试……\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    // 获取各路MIC音频音量值
                    output_rich_textbox.AppendText(session);
                    string gainDeviceMICVolumeCommand = $"curl -X POST \"http://{ip_users}/testAudioJson_api\" -H \"Content-Type: application/json\" -d \"{{\\\"method\\\": \\\"getAudioVolume\\\"}}\"";
                    output_string = executeCMDCommand(gainDeviceMICVolumeCommand);
                    MatchCollection results_1 = Regex.Matches(output_string, "\"result\" : (.*)");
                    output_rich_textbox.AppendText(results_1.ToString());
                    string backCode = results_1[0].ToString().Split(":")[1].ToString().Replace('"', ' ').Replace(" ", "").Replace(",", "");
                    if (Int32.Parse(backCode) == 0)
                    {
                        MatchCollection results_2 = Regex.Matches(output_string, "\\\"rmsdb\\\" : \\[\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*),\\n(.*)\\n   ]");
                        string[] temp = results_2[0].ToString().Replace("\"rmsdb\" : [", "").Replace("]", "").Replace("\"", "").Replace("\n", "").Replace(" ", "").Split(",");
                        string volume1 = temp[0].ToString();
                        string volume2 = temp[1].ToString();
                        string volume3 = temp[2].ToString();
                        string volume4 = temp[3].ToString();
                        string volume5 = temp[4].ToString();
                        string volume6 = temp[5].ToString();
                        string volume7 = temp[6].ToString();
                        string volume8 = temp[7].ToString();
                        output_rich_textbox.AppendText($"执行结果为：PASS，获取各路MIC音频音量值，backCode:[{backCode}]\n");
                        volume1_value_label.Text = volume1;
                        volume2_value_label.Text = volume2;
                        volume3_value_label.Text = volume3;
                        volume4_value_label.Text = volume4;
                        volume5_value_label.Text = volume5;
                        volume6_value_label.Text = volume6;
                        volume7_value_label.Text = volume7;
                        volume8_value_label.Text = volume8;
                        output_rich_textbox.AppendText($"rmsdb1：{volume1}\nrmsdb2：{volume2}\nrmsdb3：{volume3}\nrmsdb4：{volume4}\nrmsdb5：{volume5}\nrmsdb6：{volume6}\nrmsdb7：{volume7}\nrmsdb8：{volume8}\n");

                        float volume_1_f = float.Parse(volume1);
                        float volume_2_f = float.Parse(volume2);
                        float volume_3_f = float.Parse(volume3);
                        float volume_4_f = float.Parse(volume4);
                        float volume_5_f = float.Parse(volume5);
                        float volume_6_f = float.Parse(volume6);
                        float volume_7_f = float.Parse(volume7);
                        float volume_8_f = float.Parse(volume8);

                        if (Math.Abs(Math.Abs(volume_3_f) - Math.Abs(volume_7_f)) <= 3 && (Math.Abs(volume_3_f) > 70 && Math.Abs(volume_7_f) > 70))
                        {
                            audioin2_result_label.Text = "PASS";
                        }
                        else
                        {
                            audioin2_result_label.Text = "FAIL";
                        }
                    }
                    else if (Int32.Parse(backCode) == -1)
                    {
                        output_rich_textbox.AppendText($"执行结果为：FAIL，无法获取各路MIC音频音量值，backCode:[{backCode}]\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Audio IN 2 测试失败失败，请检查是否以开启了音频测试未正确关闭！\n可尝试重启机器恢复正常：\n{ex.ToString()}\n");
                    output_rich_textbox.AppendText($"Audio IN 2 测试失败失败，请检查是否以开启了音频测试未正确关闭！\n可尝试重启机器恢复正常：\n{ex.ToString()}\n");
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
    }
}