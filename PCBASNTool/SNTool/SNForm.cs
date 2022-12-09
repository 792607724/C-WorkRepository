using HIDInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNTool
{
    public partial class SNForm : Form
    {
        private static readonly int TARGET_HID_SEEVISION_VID = 9546;
        private static readonly int TARGET_HID_HONGHE_VID = 0x2757;
        private static readonly int TARGET_HID_HONGHE_PID = 0x3007;
        private static readonly int TARGET_REPORT_BUFFER_SIZE = 64;
        private Graphics m_Canvas;

        public SNForm()
        {
            InitializeComponent();

        }

        private void SNForm_Load(object sender, EventArgs e)
        {
            m_Canvas = this.CreateGraphics();

           

        }

        private void SetSNBtn_Click(object sender, EventArgs e)
        {
            this.textBox1.Focus();
            if (this.textBox1.Text.Length < 17)
            {
                MessageBox.Show("无效整机序列号长度");
                return;
            }
            m_Canvas.Clear(this.BackColor);

            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_SN, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_SN.Length);
                byte[] sn = System.Text.Encoding.ASCII.GetBytes(this.textBox1.Text);
                if (sn1.Checked)
                {
                    requestBuffer[7] = 0;
                } else if (sn2.Checked)
                {
                    requestBuffer[7] = 1;
                }
                else if (sn3.Checked)
                {
                    requestBuffer[7] = 2;
                }
                
                for (int i = 0; i < 17; i++)
                {
                    requestBuffer[8 + i] = sn[i];
                }

                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_SN, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_SN.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);

                string snStr = "";
                for (int i = 0; i < 17; i++)
                {
                    snStr += (char)responseBuffer[7 + i];
                }

                device.Close();

                if (snStr == this.textBox1.Text)
                {
                    string passString = "成功写入整机序列号";
                    using (Font font = new Font("Arial", 24))
                    {
                        Rectangle region = new Rectangle(50, 200, 816, 100);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        m_Canvas.DrawString(passString, font, Brushes.Green, region, stringFormat);
                    }
                } else
                {
                    string ngString = "写入整机序列号失败";
                    using (Font font = new Font("Arial", 24))
                    {
                        Rectangle region = new Rectangle(50, 200, 816, 100);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        m_Canvas.DrawString(ngString, font, Brushes.Red, region, stringFormat);
                    }
                }
                this.textBox1.Text = "";
            }
        }

        private void GetSNBtn_Click(object sender, EventArgs e)
        {
            this.textBox1.Focus();
            HIDDevice device = OpenHIDDevice();

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];
                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_SN, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_SN.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string sn = "";
                for (int i = 0; i < 17; i++)
                {
                    sn += (char)responseBuffer[7 + i];
                }
                this.textBox1.Text = sn;

                //MessageBox.Show("SN已写入");
            }
        }


        private HIDDevice OpenHIDDevice()
        {
            HIDDeviceInfo[] deviceInfos = HIDDevice.GetHIDDeviceInfos();

            HIDDeviceInfo targetDeviceInfo = null;
            for (int ki = 0; ki < deviceInfos.Length; ki++)
            {
                if (deviceInfos[ki].VID == TARGET_HID_SEEVISION_VID || (deviceInfos[ki].VID == TARGET_HID_HONGHE_VID))
                {
                    targetDeviceInfo = deviceInfos[ki];
                    break;
                }
            }
            if (targetDeviceInfo != null)
            {
                HIDDevice device = HIDDevice.Open(targetDeviceInfo);
                return device;

            }
            else
            {
                MessageBox.Show("设备未找到！");
            }
            return null;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == 8))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void sn2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            writePCBAInResult_label.Text = "";
            this.textBox2.Focus();
            if (this.textBox2.Text.Length < 19)
            {
                MessageBox.Show("无效PCBA列号长度");
                return;
            }
            m_Canvas.Clear(this.BackColor);

            HIDDevice device = OpenHIDDevice();

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_PCBASN, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_PCBASN.Length);
                byte[] sn = System.Text.Encoding.ASCII.GetBytes(this.textBox2.Text);
                if (pcba1.Checked)
                {
                    requestBuffer[7] = 0;
                }
                else if (pcba2.Checked)
                {
                    requestBuffer[7] = 1;
                }
                else if (pcba3.Checked)
                {
                    requestBuffer[7] = 2;
                }
                for (int i = 0; i < 19; i++)
                {
                    requestBuffer[8 + i] = sn[i];
                }

                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_PCBASN, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_PCBASN.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);

                string snStr = "";
                for (int i = 0; i < 19; i++)
                {
                    snStr += (char)responseBuffer[7 + i];
                }

                device.Close();
                Font font_temp = new Font(FontFamily.GenericSansSerif, 15, FontStyle.Bold);
                if (snStr == this.textBox2.Text)
                {
                    writePCBAInResult_label.ForeColor = Color.Green;
                    writePCBAInResult_label.Font = font_temp;
                    writePCBAInResult_label.Text = "成功写入PCBA序列号";
                    //MessageBox.Show("成功写入PCBA序列号");
                    /**
                    string passString = "成功写入PCBA序列号";
                    using (Font font = new Font("Arial", 24))
                    {
                        Rectangle region = new Rectangle(50, 200, 816, 100);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        m_Canvas.DrawString(passString, font, Brushes.Green, region, stringFormat);
                    }*/
                }
                else
                {
                    writePCBAInResult_label.ForeColor = Color.Red;
                    writePCBAInResult_label.Font = font_temp;
                    writePCBAInResult_label.Text = "写入PCBA号失败";
                    /**
                    MessageBox.Show("写入PCBA号失败");
                    string ngString = "写入PCBA号失败";
                    using (Font font = new Font("Arial", 24))
                    {
                        Rectangle region = new Rectangle(50, 200, 816, 100);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        m_Canvas.DrawString(ngString, font, Brushes.Red, region, stringFormat);
                    }
                    */
                }
                this.textBox2.Text = "";
            }
        }

        private void pcbagroup_Enter(object sender, EventArgs e)
        {

        }

        private void pcba1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void sn1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void sn3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox2.Focus();
            HIDDevice device = OpenHIDDevice();

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_PCBASN, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_PCBASN.Length);

                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string sn = "";
                for (int i = 0; i < 19; i++)
                {
                    sn += (char)responseBuffer[7 + i];
                }
                this.textBox2.Text = sn;

                //MessageBox.Show("SN已写入");
            }
        }

        private void pcba2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pcba3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
