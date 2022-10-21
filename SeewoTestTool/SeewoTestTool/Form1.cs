using System.Net;
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

        Socket clientSocket = null;

        // 保证设备在连接状态 网口通信
        private void device_connect_button_Click(object sender, EventArgs e)
        {
            string host = device_ip_textbox.Text;
            string port = device_port_textbox.Text;
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
            {
                output_rich_textbox.Text = "设备网口IP地址和端口号不能为空！";
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
                        output_rich_textbox.Text = $"设备ip:{host}:{port}已连接上！";
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
                    output_rich_textbox.Text = $"设备网口IP地址和端口号错误，请检查是否输入正确！\n问题Log如下：{ex.ToString()}";
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
                    output_rich_textbox.Text = $"指令发送:{sendStr}";
                    byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);
                    clientSocket.Send(sendBytes);
                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"Socket发送命令执行失败：\n{ex.ToString()}";
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
                output_rich_textbox.Text = "设备连接已断开，请先连接设备！";
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
                        output_rich_textbox.Text = $"接收内容：{recStr}";
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
                        output_rich_textbox.Text = $"Socket接收命令执行失败：\n{ex.ToString()}";
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
                output_rich_textbox.Text = "设备连接已断开，请先连接设备！";
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
                    output_rich_textbox.Text = "断开当前已连接设备";
                    device_disconnect_button.Enabled = false;
                    device_connect_button.Enabled = true;
                    clientSocket.Close();
                    device_status_label.Text = "已断开";
                    device_ip_textbox.Enabled = true;
                    device_port_textbox.Enabled = true;
                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"断开设备连接失败：\n{ex.ToString()}";
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
                output_rich_textbox.Text = "选择升级固件！";
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
                    }
                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "已断开";
                output_rich_textbox.Text = "设备连接已断开，请先连接设备！";
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
                        output_rich_textbox.Text = "开始升级，请耐心等待完成！";

                    }
                    catch (Exception ex)
                    {
                        output_rich_textbox.Text = $"固件升级操作失败：\n{ex.ToString()}";
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
                output_rich_textbox.Text = "设备连接已断开，请先连接设备！";
            }
        }

        // 校验当前固件
        private void check_current_firmware_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                output_rich_textbox.Text = "校验固件测试！";
                string currentFirmware = null;
                try
                {
                    // 校验固件

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"校验固件测试失败：\n{ex.ToString()}";
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
                output_rich_textbox.Text = "设备连接已断开，请先连接设备！";
            }
        }

        // 打开红绿灯交替闪烁
        private void start_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "打开红绿灯交替闪烁";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    stop_rg_flicker_button.Enabled = true;
                    start_rg_flicker_button.Enabled = false;

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"打开红绿灯交替闪烁功能失败：\n{ex.ToString()}";
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
                output_rich_textbox.Text = "设备连接已断开，请先连接设备！";
            }
        }

        // 关闭红绿灯交替闪烁
        private void stop_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "关闭红绿灯交替闪烁";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    stop_rg_flicker_button.Enabled = false;
                    start_rg_flicker_button.Enabled = true;

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"关闭红绿灯交替闪烁功能失败：\n{ex.ToString()}";
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
                output_rich_textbox.Text = "设备连接已断开，请先连接设备！";
            }
        }

        // 获取吊麦信息
        private void get_poe_mic_info_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "获取吊麦信息";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"获取吊麦信息失败：\n{ex.ToString()}";
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
                output_rich_textbox.Text = "设备连接已断开，请先连接设备！";
            }
        }

        // Audio IN 1口测试
        private void audioin1_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "Audio IN 1口测试";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"Audio IN 1口测试失败：\n{ex.ToString()}";
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
                output_rich_textbox.Text = "设备连接已断开，请先连接设备！";
            }
        }

        // Audio IN 2口测试
        private void audioin2_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "Audio IN 2口测试";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"Audio IN 2口测试失败：\n{ex.ToString()}";
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
                output_rich_textbox.Text = "设备连接已断开，请先连接设备！";
            }
        }

        // 阵列MIC音量值测试
        private void array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "阵列MIC音量值测试";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"阵列MIC音量值测试失败：\n{ex.ToString()}";
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
                output_rich_textbox.Text = "设备连接已断开，请先连接设备！";
            }
        }

        // 设备复位
        private void device_reset_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "设备复位";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"设备复位失败：\n{ex.ToString()}";
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
                output_rich_textbox.Text = "设备连接已断开，请先连接设备！";
            }
        }

        // 窗体关闭事件，将设备socket连接释放掉
        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "窗体关闭事件，将设备socket连接释放掉";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    clientSocket.Close();
                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"窗体关闭将设备socket连接释放掉失败：\n{ex.ToString()}";
                }
                finally
                {

                }
            }
        }
    }
}