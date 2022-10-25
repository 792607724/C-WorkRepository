using HIDInterface;
using System;
using System.IO;
using System.Windows.Forms;

namespace HIDDemo
{
    public partial class Form1 : Form
    {
        private static readonly int TARGET_HID_VID = 9546;
        private static readonly int TARGET_SHH0105_HID_VID = 10071;
        //private static readonly int TARGET_HID_PID = 3074;
        private static readonly int TARGET_REPORT_BUFFER_SIZE = 64;

        private static readonly string LOG_PASSWORD = "seevision_log_123";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Filter = "Log文件 (*.tar.gz)|*.tar.gz";
        }

        private HIDDevice OpenHIDDevice()
        {
            HIDDeviceInfo[] deviceInfos = HIDDevice.GetHIDDeviceInfos();
#if falst
            Console.WriteLine("---------------- HIDDevice Infos ---------------- ");
            for (int i = 0; i < deviceInfos.Length; i++)
            {
                Console.WriteLine("HIDDevice[{0}] --> VID: {1}, PID: {2}, reportByteLength: {3}, devicePath: {4}",
                    i, deviceInfos[i].VID, deviceInfos[i].PID, deviceInfos[i].OUT_reportByteLength, deviceInfos[i].devicePath);
            }
            Console.WriteLine("---------------- HIDDevice Infos ---------------- \n");
#endif

            HIDDeviceInfo targetDeviceInfo = null;
            for (int ki = 0; ki < deviceInfos.Length; ki++)
            {
                Console.WriteLine(deviceInfos[ki].VID);
                if (/*deviceInfos[ki].PID == TARGET_HID_PID &&*/ (deviceInfos[ki].VID == TARGET_HID_VID || deviceInfos[ki].VID == TARGET_SHH0105_HID_VID)
                        && deviceInfos[ki].OUT_reportByteLength == TARGET_REPORT_BUFFER_SIZE)
                {
                    targetDeviceInfo = deviceInfos[ki];
                    Console.WriteLine("Select HIDDevice[{0}]", ki);
                    break;
                }
            }

            if (targetDeviceInfo == null)
            {
                MessageBox.Show("设备未找到！", "HIDDemo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return HIDDevice.Open(targetDeviceInfo);
        }

        private void GetVersionBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_GET_CAM_VERSION, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_GET_CAM_VERSION.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string version = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    version += (char)responseBuffer[7 + i];
                }
                MessageBox.Show("版本号: " + version);
            }
        }

        private void GetCameraModelBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_GET_CAMERA_MODEL, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_GET_CAMERA_MODEL.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string model = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    model += (char)responseBuffer[7 + i];
                }
                MessageBox.Show("模组型号: " + model);
            }
        }

        private void GetUpgradeMethodBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_GET_UPGRADE_METHOD, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_GET_UPGRADE_METHOD.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string method = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    method += (char)responseBuffer[7 + i];
                }
                MessageBox.Show("升级方式: " + method);
            }
        }

        private void GetOTAKeyBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_GET_OTA_KEY, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_GET_OTA_KEY.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string key = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    key += (char)responseBuffer[7 + i];
                }
                MessageBox.Show("OTA Key: " + key);
            }
        }

        private void OpenEPTZBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_SET_EPTZ_ENABLE, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_SET_EPTZ_ENABLE.Length);
                requestBuffer[7] = 0x01;

                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("电子云台已打开");
            }
        }

        private void CloseEPTZBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_SET_EPTZ_ENABLE, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_SET_EPTZ_ENABLE.Length);
                requestBuffer[7] = 0x00;

                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("电子云台已关闭");
            }
        }

        private void GetDeviceNameBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_GET_DEVICE_NAME, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_GET_DEVICE_NAME.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string name = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    name += (char)responseBuffer[7 + i];
                }
                MessageBox.Show("设备名: " + name);
            }
        }

        private void GetCPUTempBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_GET_CPU_TEMP, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_GET_CPU_TEMP.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string temp = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    temp += (char)responseBuffer[7 + i];
                }
                MessageBox.Show("CPU温度: " + temp);
            }
        }

        private void RebootBootloaderBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_REBOOT_BOOTLOADER, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_REBOOT_BOOTLOADER.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                bool isValidCommand = true;
                for (int i = 0; i < 7; i++)
                {
                    if (HIDProtocol.COMMAND_RESPONSE_TYPE_REBOOT_BOOTLOADER[i] != responseBuffer[i])
                    {
                        isValidCommand = false;
                        break;
                    }
                }
                if (isValidCommand)
                {
                    MessageBox.Show("有效返回值！ 目标正在进入bootloader模式....");
                }
                else
                {
                    MessageBox.Show("无效返回值！");
                }
            }
        }

        private void GetAppVersionBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_APP_VERSION, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_APP_VERSION.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string version = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    version += (char)responseBuffer[7 + i];
                }
                MessageBox.Show("App版本号: " + version);
            }
        }

        private void IsEPTZSupportedBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_EPTZ_SUPPORTED, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_EPTZ_SUPPORTED.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                bool isEPTZSupported = responseBuffer[7] == 0x01 ? true : false;
                if (isEPTZSupported)
                {
                    MessageBox.Show("支持电子云台功能");
                } else
                {
                    MessageBox.Show("不支持电子云台功能");
                }

            }
        }

        private void GetEPTZEnableBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_GET_EPTZ_ENABLE, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_GET_EPTZ_ENABLE.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                bool isEPTZSupported = responseBuffer[7] == 0x01 ? true : false;
                if (isEPTZSupported)
                {
                    MessageBox.Show("当前电子云台开关状态：已打开");
                }
                else
                {
                    MessageBox.Show("当前电子云台开关状态：已关闭");
                }

            }
        }

        private void IsCaptureSupportedBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_CAPTURE_SUPPORTED, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_CAPTURE_SUPPORTED.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                bool isEPTZSupported = responseBuffer[7] == 0x01 ? true : false;
                if (isEPTZSupported)
                {
                    MessageBox.Show("支持48M拍照功能");
                }
                else
                {
                    MessageBox.Show("不支持48M拍照功能");
                }

            }
        }

        private void StartRecordLogBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_START_RECORD_LOG, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_START_RECORD_LOG.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("已开始录制log", "Log抓取", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void StopRecordLogBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_STOP_RECORD_LOG, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_STOP_RECORD_LOG.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("已停止录制log", "Log抓取", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void GetRecordLogBtn_Click(object sender, EventArgs e)
        {
            if (!this.backgroundWorker1.IsBusy)
            {
                this.progressBar1.Value = 0;
                this.saveFileDialog1.FileName = "SmartCameraLog_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.backgroundWorker1.RunWorkerAsync();
                    this.GetRecordLogBtn.Enabled = false;
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            HIDDevice device = OpenHIDDevice();

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_GET_RECORD_LOG_SIZE, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_GET_RECORD_LOG_SIZE.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);

                int logSize = (responseBuffer[7] << 24) | (responseBuffer[8] << 16) | (responseBuffer[9] << 8) | (responseBuffer[10] << 0);
                int packetCount = logSize / 59 + (logSize % 59 == 0 ? 0 : 1);
                byte[] logBuffer = new byte[packetCount * 59];
                for (int i = 0; i < packetCount; i++)
                {
                    Array.Copy(HIDProtocol.COMMAND_REQUEST_TYPE_GET_RECORD_LOG, requestBuffer, HIDProtocol.COMMAND_REQUEST_TYPE_GET_RECORD_LOG.Length);
                    requestBuffer[7] = (byte)((i >> 24) & 0xff);
                    requestBuffer[8] = (byte)((i >> 16) & 0xff);
                    requestBuffer[9] = (byte)((i >> 8) & 0xff);
                    requestBuffer[10] = (byte)((i >> 0) & 0xff);
                    device.Send(requestBuffer, 0, requestBuffer.Length);
                    device.Receive(responseBuffer, 0, responseBuffer.Length);

                    Array.Copy(responseBuffer, 5, logBuffer, i * 59, 59);
                    this.backgroundWorker1.ReportProgress((i + 1) * 80 / packetCount);
                }
                device.Close();

                byte[] encryptedLogBuffer = EncDecUtils.Encrypt(logBuffer, LOG_PASSWORD);
                this.backgroundWorker1.ReportProgress(90);

                FileStream logStream = new FileStream(this.saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                if (logStream != null)
                {
                    logStream.Write(encryptedLogBuffer, 0, encryptedLogBuffer.Length);
                    logStream.Close();
                    this.backgroundWorker1.ReportProgress(100);
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Log抓取失败！\n" + e.Error.ToString(), "Log抓取", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!e.Cancelled)
            {
                MessageBox.Show("log已保存到:" + this.saveFileDialog1.FileName, "Log抓取", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.GetRecordLogBtn.Enabled = true;
                this.progressBar1.Value = 0;
            }
        }
    }
}
