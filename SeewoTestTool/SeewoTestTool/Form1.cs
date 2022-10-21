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

        // ��֤�豸������״̬ ����ͨ��
        private void device_connect_button_Click(object sender, EventArgs e)
        {
            string host = device_ip_textbox.Text;
            string port = device_port_textbox.Text;
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
            {
                output_rich_textbox.AppendText("�豸����IP��ַ�Ͷ˿ںŲ���Ϊ�գ�\n");
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
                        output_rich_textbox.AppendText($"�豸ip:{host}:{port}�������ϣ�\n");
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
                    output_rich_textbox.AppendText($"�豸����IP��ַ�Ͷ˿ںŴ��������Ƿ�������ȷ��\n����Log���£�{ex.ToString()}\n");
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
                    output_rich_textbox.AppendText($"ָ���:{sendStr}\n");
                    byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);
                    clientSocket.Send(sendBytes);
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"Socket��������ִ��ʧ�ܣ�\n{ex.ToString()}\n");
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
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
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
                        output_rich_textbox.AppendText($"�������ݣ�{recStr}\n");
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
                        output_rich_textbox.AppendText($"Socket��������ִ��ʧ�ܣ�\n{ex.ToString()}\n");
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
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
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
                    output_rich_textbox.AppendText("�Ͽ���ǰ�������豸\n");
                    device_disconnect_button.Enabled = false;
                    device_connect_button.Enabled = true;
                    clientSocket.Close();
                    device_status_label.Text = "�ѶϿ�";
                    device_ip_textbox.Enabled = true;
                    device_port_textbox.Enabled = true;
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"�Ͽ��豸����ʧ�ܣ�\n{ex.ToString()}\n");
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
                output_rich_textbox.AppendText("ѡ�������̼���\n");
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
                        output_rich_textbox.AppendText($"��ǰѡ��Ĺ̼�·��Ϊ��\n{filePath}\n");

                    }
                }
            }
            else
            {
                device_ip_textbox.Enabled = true;
                device_port_textbox.Enabled = true;
                device_status_label.Text = "�ѶϿ�";
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
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
                        output_rich_textbox.AppendText("��ʼ�����������ĵȴ���ɣ�\n");
                        // ʹ�ú�̨�߳�ȥ������������ֹUI��������
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
                        output_rich_textbox.AppendText($"�̼���������ʧ�ܣ�\n{ex.ToString()}\n");
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
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // У�鵱ǰ�̼�
        private void check_current_firmware_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                output_rich_textbox.AppendText("У��̼����ԣ�");
                string currentFirmware = null;
                try
                {
                    // У��̼�

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"У��̼�����ʧ�ܣ�\n{ex.ToString()}\n");
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
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // �򿪺��̵ƽ�����˸
        private void start_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("�򿪺��̵ƽ�����˸\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    stop_rg_flicker_button.Enabled = true;
                    start_rg_flicker_button.Enabled = false;

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"�򿪺��̵ƽ�����˸����ʧ�ܣ�\n{ex.ToString()}\n");
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
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // �رպ��̵ƽ�����˸
        private void stop_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("�رպ��̵ƽ�����˸\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    stop_rg_flicker_button.Enabled = false;
                    start_rg_flicker_button.Enabled = true;

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"�رպ��̵ƽ�����˸����ʧ�ܣ�\n{ex.ToString()}\n");
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
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // ��ȡ������Ϣ
        private void get_poe_mic_info_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ȡ������Ϣ\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"��ȡ������Ϣʧ�ܣ�\n{ex.ToString()}\n");
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
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // Audio IN 1�ڲ���
        private void audioin1_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("Audio IN 1�ڲ���\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"Audio IN 1�ڲ���ʧ�ܣ�\n{ex.ToString()}\n");
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
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // Audio IN 2�ڲ���
        private void audioin2_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("Audio IN 2�ڲ���\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"Audio IN 2�ڲ���ʧ�ܣ�\n{ex.ToString()}\n");
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
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // ����MIC����ֵ����
        private void array_mic_audio_level_test_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("����MIC����ֵ����\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"����MIC����ֵ����ʧ�ܣ�\n{ex.ToString()}\n");
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
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // �豸��λ
        private void device_reset_button_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("�豸��λ\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"�豸��λʧ�ܣ�\n{ex.ToString()}\n");
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
                output_rich_textbox.AppendText("�豸�����ѶϿ������������豸��\n");
            }
        }

        // ����ر��¼������豸socket�����ͷŵ�
        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("����ر��¼������豸socket�����ͷŵ�\n");
            if (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    clientSocket.Close();
                }
                catch (Exception ex)
                {
                    output_rich_textbox.AppendText($"����رս��豸socket�����ͷŵ�ʧ�ܣ�\n{ex.ToString()}\n");
                }
                finally
                {

                }
            }
        }

        // ����б�ʼ����ʾ��������Log
        private void richTextChanged_to(object sender, EventArgs e)
        {
            // �����λ�����õ���ǰ���ݵ�ĩβ
            output_rich_textbox.SelectionStart = output_rich_textbox.Text.Length;
            // ���������λ��
            output_rich_textbox.ScrollToCaret();
        }

        // ����������
        private void clear_output_button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(output_rich_textbox.Text))
            {
                output_rich_textbox.Text = "";
            }
        }

        // �½�backgroundworker���̼�������������ֹUI������������
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
            output_rich_textbox.AppendText($"�������ȣ�{Convert.ToString(e.ProgressPercentage)}%\n");
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
                output_rich_textbox.AppendText("������ϣ�\n");
                upgrade_button.Enabled = true;

            }
            else
            {
                output_rich_textbox.AppendText("������ֹ��\n");
            }
        }
        
        // ��ȡ��ǰ�������ӵ������豸�����ڲ�ɸѡ��Seewo���豸
        private void getSeewoDevice_Click(object sender, EventArgs e)
        {
            //if (true)
            output_rich_textbox.AppendText("��ȡ��ǰ�������ӵ������豸�����ڲ�ɸѡ��Seewo���豸\n");
            try
            {
                
                foreach (NetworkInterface netItem in NetworkInterface.GetAllNetworkInterfaces())
                {
                    output_rich_textbox.AppendText($"�ӿ�����{netItem.Name}���ӿ����ͣ�{netItem.NetworkInterfaceType}��" +
                        $"�ӿ�MAC��{netItem.GetPhysicalAddress().ToString()}\n");
                    foreach (UnicastIPAddressInformation ipIntProp in netItem.GetIPProperties().UnicastAddresses.ToArray<UnicastIPAddressInformation>())
                    {
                        output_rich_textbox.AppendText($"   �ӿ�����{netItem.Name}��IP��{ipIntProp.Address.ToString()}��IP���ͣ�{ipIntProp.Address.AddressFamily}\n");
                    }
                }

                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] endPoints = ipGlobalProperties.GetActiveTcpListeners();
                foreach (IPEndPoint endPoint in endPoints)
                {
                    output_rich_textbox.AppendText($"�˿ڣ�{endPoint.Port}��IP��{endPoint.Address.ToString()}��IP���ͣ�{endPoint.Address.AddressFamily.ToString()}\n");
                }
            }
            catch (Exception ex)
            {
                output_rich_textbox.AppendText($"��ȡ�豸ʧ�ܣ���ǰδ�����豸��\n{ex.ToString()}\n");
            }
            finally
            {

            }
        }
    }
}