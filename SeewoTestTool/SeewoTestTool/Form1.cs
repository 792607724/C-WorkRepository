using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace SeewoTestTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Socket clientSocket;

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
                    }
                    device_connect_button.Enabled = false;
                    device_disconnect_button.Enabled = true;
                    device_status_label.Text = "已连接";
                    device_ip_textbox.Enabled = false;
                    device_port_textbox.Enabled = false;
                    send_Str("am start com.android.browser");
                    string rec_Str = receive_Str();
                }
                catch (Exception ex)
                {
                    device_status_label.Text = "已断开";
                    device_ip_textbox.Enabled = true;
                    device_port_textbox.Enabled = true;
                    output_rich_textbox.AppendText($"设备网口IP地址和端口号错误，请检查是否输入正确！\n问题Log如下：{ex.ToString()}\n");
                }

            }
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

        // 选择升级的固件路径
        private void choose_upgrade_firmware_button_Click(object sender, EventArgs e)
        {
            //if(true)
            if (clientSocket != null && clientSocket.Connected)
            {
                string filePath = null;
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                output_rich_textbox.AppendText("选择升级固件！\n");
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = dialog.SelectedPath;
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
                output_rich_textbox.AppendText("校验固件测试！");
                string currentFirmware = null;
                try
                {
                    // 校验固件

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

        // 新建backgroundworker给固件升级操作，防止UI阻塞交互卡死
        private void backgroundworker_firmwareupgrade_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            for (int i = 0; i < 101; i++)
            {
                if (backgroundworker_firmwareupgrade.CancellationPending)
                { 
                    e.Cancel = true;
                    return;
                }
                else
                {
                    backgroundworker_firmwareupgrade.ReportProgress(i, "Working\n");
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
        private void backgroundworker_firmwareupgrade_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            upgrade_progressbar.Value = e.ProgressPercentage;
            output_rich_textbox.AppendText($"升级进度：{Convert.ToString(e.ProgressPercentage)}%\n");
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