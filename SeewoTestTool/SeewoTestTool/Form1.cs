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
                MessageBox.Show("设备网口IP地址和端口号不能为空！");
            }
            else
            {
                int port_int = Convert.ToInt32(port);
                MessageBox.Show($"IP：{ip_address} PORT：{port_int}");

            }
        }
    }
}