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

        // ��֤�豸������״̬
        private void device_connect_button_Click(object sender, EventArgs e)
        {
            string host = device_ip_textbox.Text;
            string port = device_port_textbox.Text;
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
            {
                MessageBox.Show("�豸����IP��ַ�Ͷ˿ںŲ���Ϊ�գ�");
            }
            else
            {
                // Socket Connection Build
                try 
                {
                    int port_int = Convert.ToInt32(port);
                    MessageBox.Show($"IP��{host} PORT��{port_int}");
                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint ipe = new IPEndPoint(ip, port_int);
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    clientSocket.Connect(ipe);
                    device_connect_button.Enabled = false;
                    device_disconnect_button.Enabled = true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("�豸����IP��ַ�Ͷ˿ںŴ��������Ƿ�������ȷ��");
                }

            }
        }

        // Socket���������
        private void send_Str(string sendStr)
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);
                clientSocket.Send(sendBytes);
            }
            else 
            {
                MessageBox.Show("�豸�����ѶϿ������������豸��"); 
            }
        }

        // Socket���շ������ݺ���
        private string receive_Str()
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                string recStr = "";
                byte[] recBytes = new byte[4096];
                int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
                recStr += Encoding.ASCII.GetString(recBytes, 0, bytes);

                return recStr;
            }
            else
            {
                MessageBox.Show("�豸�����ѶϿ������������豸��");
                return "";
            }
        }

        // �Ͽ��豸����
        private void device_disconnect_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                device_disconnect_button.Enabled = false;
                device_connect_button.Enabled = true;
                clientSocket.Close();
                MessageBox.Show("�豸�����ѹرգ�");
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
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = dialog.SelectedPath;
                    if (string.IsNullOrEmpty(filePath))
                    {
                        upgrade_firmware_textbox.Text = filePath;
                    }
                    else
                    {
                        upgrade_firmware_textbox.Text = "δѡ����ȷ�Ĺ̼�·��";
                    }
                }
            }
            else
            {
                MessageBox.Show("�豸�����ѶϿ������������豸��");
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
                    
                }
            }
            else
            {
                MessageBox.Show("�豸�����ѶϿ������������豸��");
            }
        }

        // У�鵱ǰ�̼�
        private void check_current_firmware_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                string currentFirmware = null;
            }
            else
            {
                MessageBox.Show("�豸�����ѶϿ������������豸��");
            }
        }

        // �򿪺��̵ƽ�����˸
        private void start_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                stop_rg_flicker_button.Enabled = true;
                start_rg_flicker_button.Enabled = false;
            }
            else
            {
                MessageBox.Show("�豸�����ѶϿ������������豸��");
            }
        }

        // �رպ��̵ƽ�����˸
        private void stop_rg_flicker_button_Click(object sender, EventArgs e)
        {
            //if (true)
            if (clientSocket != null && clientSocket.Connected)
            {
                stop_rg_flicker_button.Enabled = false;
                start_rg_flicker_button.Enabled = true;
            }
            else
            {
                MessageBox.Show("�豸�����ѶϿ������������豸��");
            }
        }
    }
}