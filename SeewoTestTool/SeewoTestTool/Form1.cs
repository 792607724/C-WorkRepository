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

        // ��֤�豸������״̬ ����ͨ��
        private void device_connect_button_Click(object sender, EventArgs e)
        {
            string host = device_ip_textbox.Text;
            string port = device_port_textbox.Text;
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
            {
                output_rich_textbox.Text = "�豸����IP��ַ�Ͷ˿ںŲ���Ϊ�գ�";
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
                        output_rich_textbox.Text = $"�豸ip:{host}:{port}�������ϣ�";
                    }
                    device_connect_button.Enabled = false;
                    device_disconnect_button.Enabled = true;
                    device_status_label.Text = "������";
                    device_ip_textbox.Enabled = false;
                    device_port_textbox.Enabled = false;
                    send_Str("am start com.android.browser");
                    string rec_Str = receive_Str();
                }
                catch (Exception ex)
                {
                    device_status_label.Text = "�ѶϿ�";
                    device_ip_textbox.Enabled = true;
                    device_port_textbox.Enabled = true;
                    output_rich_textbox.Text = $"�豸����IP��ַ�Ͷ˿ںŴ��������Ƿ�������ȷ��\n����Log���£�{ex.ToString()}";
                }

            }
        }

        // Socket���������
        private void send_Str(string sendStr)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                // Socket��������
                try
                {
                    output_rich_textbox.Text = $"ָ���:{sendStr}";
                    byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);
                    clientSocket.Send(sendBytes);
                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"Socket��������ִ��ʧ�ܣ�\n{ex.ToString()}";
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.Text = "�豸�����ѶϿ������������豸��";
            }
        }

        // Socket���շ������ݺ���
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
                        //  �������ķ�ʽ���գ����յ�����ǰ����Ӧ��
                        int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
                        if (bytes == 0)
                        {
                            break;
                        }
                        recStr += Encoding.ASCII.GetString(recBytes, 0, bytes);
                        output_rich_textbox.Text = $"�������ݣ�{recStr}";
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
                        device_status_label.Text = "�ѶϿ�";
                        output_rich_textbox.Text = $"Socket��������ִ��ʧ�ܣ�\n{ex.ToString()}";
                        break;
                    }
                }
                return "";
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.Text = "�豸�����ѶϿ������������豸��";
                return "";
            }
        }

        // �Ͽ��豸����
        private void device_disconnect_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    output_rich_textbox.Text = "�Ͽ���ǰ�������豸";
                    device_disconnect_button.Enabled = false;
                    device_connect_button.Enabled = true;
                    clientSocket.Close();
                    device_status_label.Text = "�ѶϿ�";
                    device_ip_textbox.Enabled = true;
                    device_port_textbox.Enabled = true;
                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"�Ͽ��豸����ʧ�ܣ�\n{ex.ToString()}";
                }
                finally
                {

                }
            }
        }

        // ѡ�������Ĺ̼�·��
        private void choose_upgrade_firmware_button_Click(object sender, EventArgs e)
        {
            //if(true)
            if (clientSocket != null && clientSocket.Connected)
            {
                string filePath = null;
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                output_rich_textbox.Text = "ѡ�������̼���";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = dialog.SelectedPath;
                    if (string.IsNullOrEmpty(filePath))
                    {
                        upgrade_firmware_textbox.Text = "δѡ����ȷ�Ĺ̼�·��";
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
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.Text = "�豸�����ѶϿ������������豸��";
            }
        }

        // �̼���������
        private void upgrade_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                string filePath = upgrade_firmware_textbox.Text;
                if (string.IsNullOrEmpty(filePath))
                {
                    upgrade_firmware_textbox.Text = "δѡ����ȷ�Ĺ̼�·��";
                }
                else
                {
                    // �̼���ʼ����
                    try
                    {
                        output_rich_textbox.Text = "��ʼ�����������ĵȴ���ɣ�";

                    }
                    catch (Exception ex)
                    {
                        output_rich_textbox.Text = $"�̼���������ʧ�ܣ�\n{ex.ToString()}";
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
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.Text = "�豸�����ѶϿ������������豸��";
            }
        }

        // У�鵱ǰ�̼�
        private void check_current_firmware_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                output_rich_textbox.Text = "У��̼����ԣ�";
                string currentFirmware = null;
                try
                {
                    // У��̼�

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"У��̼�����ʧ�ܣ�\n{ex.ToString()}";
                }
                finally
                { 
                    
                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.Text = "�豸�����ѶϿ������������豸��";
            }
        }

        // �򿪺��̵ƽ�����˸
        private void start_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "�򿪺��̵ƽ�����˸";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    stop_rg_flicker_button.Enabled = true;
                    start_rg_flicker_button.Enabled = false;

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"�򿪺��̵ƽ�����˸����ʧ�ܣ�\n{ex.ToString()}";
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.Text = "�豸�����ѶϿ������������豸��";
            }
        }

        // �رպ��̵ƽ�����˸
        private void stop_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "�رպ��̵ƽ�����˸";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    stop_rg_flicker_button.Enabled = false;
                    start_rg_flicker_button.Enabled = true;

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"�رպ��̵ƽ�����˸����ʧ�ܣ�\n{ex.ToString()}";
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.Text = "�豸�����ѶϿ������������豸��";
            }
        }

        // ��ȡ������Ϣ
        private void get_poe_mic_info_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "��ȡ������Ϣ";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"��ȡ������Ϣʧ�ܣ�\n{ex.ToString()}";
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.Text = "�豸�����ѶϿ������������豸��";
            }
        }

        // Audio IN 1�ڲ���
        private void audioin1_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "Audio IN 1�ڲ���";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"Audio IN 1�ڲ���ʧ�ܣ�\n{ex.ToString()}";
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.Text = "�豸�����ѶϿ������������豸��";
            }
        }

        // Audio IN 2�ڲ���
        private void audioin2_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "Audio IN 2�ڲ���";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"Audio IN 2�ڲ���ʧ�ܣ�\n{ex.ToString()}";
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.Text = "�豸�����ѶϿ������������豸��";
            }
        }

        // ����MIC����ֵ����
        private void array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "����MIC����ֵ����";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"����MIC����ֵ����ʧ�ܣ�\n{ex.ToString()}";
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.Text = "�豸�����ѶϿ������������豸��";
            }
        }

        // �豸��λ
        private void device_reset_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "�豸��λ";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"�豸��λʧ�ܣ�\n{ex.ToString()}";
                }
                finally
                {

                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.Text = "�豸�����ѶϿ������������豸��";
            }
        }

        // ����ر��¼������豸socket�����ͷŵ�
        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            //if (true)
            output_rich_textbox.Text = "����ر��¼������豸socket�����ͷŵ�";
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    clientSocket.Close();
                }
                catch (Exception ex)
                {
                    output_rich_textbox.Text = $"����رս��豸socket�����ͷŵ�ʧ�ܣ�\n{ex.ToString()}";
                }
                finally
                {

                }
            }
        }
    }
}