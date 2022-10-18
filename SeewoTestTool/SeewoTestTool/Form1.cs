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
            string ip_address = device_ip_textbox.Text;
            string port = device_port_textbox.Text;
            if (string.IsNullOrEmpty(ip_address) || string.IsNullOrEmpty(port))
            {
                MessageBox.Show("�豸����IP��ַ�Ͷ˿ںŲ���Ϊ�գ�");
            }
            else
            {
                int port_int = Convert.ToInt32(port);
                MessageBox.Show($"IP��{ip_address} PORT��{port_int}");

            }
        }
    }
}