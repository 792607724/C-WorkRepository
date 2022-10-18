using System;
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

        private void device_connect_button_Click(object sender, EventArgs e)
        {
            string host = device_ip_textbox.Text;
            string port = device_port_textbox.Text;
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
            {
                MessageBox.Show("设备网口IP地址和端口号不能为空！");
            }
            else
            {
                // Socket Connection Build
                int port_int = Convert.ToInt32(port);
                MessageBox.Show($"IP：{host} PORT：{port_int}");
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port_int);
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(ipe);

            }
        }

        private void send_command(Socket clientSocket, string command)
        { 

        }


    }
}