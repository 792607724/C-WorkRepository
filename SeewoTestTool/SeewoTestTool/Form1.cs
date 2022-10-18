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
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Close();
                MessageBox.Show("�豸�����ѹرգ�");
            }
            else
            {
                MessageBox.Show("�豸δ���ӣ�����رգ���Ϥ֪��");
            }

        }

        // ѡ�������Ĺ̼�·��
        private void choose_upgrade_firmware_button_Click(object sender, EventArgs e)
        {
            if(true)
            //if (clientSocket != null && clientSocket.Connected)
            {
                string filePath = null;
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = dialog.SelectedPath;
                    if (filePath != null)
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
    }
}