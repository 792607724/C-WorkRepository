﻿using HIDInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIDTool
{
    public partial class HIDToolForm : Form
    {
        private static readonly int TARGET_HID_SEEVISION_VID = 9546;
        private static readonly int TARGET_HID_HONGHE_VID = 0x2757;
        private static readonly int TARGET_HID_HONGHE_PID = 0x3007;
        private static readonly int TARGET_HID_HONGHE03_VID = 0x2757;
        private static readonly int TARGET_HID_HONGHE03_PID = 0x3008;
        private static readonly int TARGET_HID_HONGHE04_VID = 0x2757;
        private static readonly int TARGET_HID_HONGHE04_PID = 0x3014;
        private static readonly int TARGET_HID_HONGHE05_VID = 0x2757;
        private static readonly int TARGET_HID_HONGHE05_PID = 0x3015;
        private static readonly int VID_1 = 0x2757;
        private static readonly int VID_2 = 0x254a;
        //private static readonly int TARGET_HID_PID = 3074;
        private static readonly int TARGET_REPORT_BUFFER_SIZE = 64;

        private enum ModeType
        {
            Normal,
            EPTZManual,
            EPTZAutoFraming,
            EPTZSpeaker,
        }

        private struct ModeInfo
        {
            public ModeInfo(int index, ModeType mode)
            {
                this.index = index;
                this.mode = mode;
            }

            public int index { get; }
            public ModeType mode { get; }
        }

        private static readonly ModeInfo[] kPreviewModeList =
        {
            new ModeInfo(0, ModeType.Normal),
            new ModeInfo(1, ModeType.EPTZManual),
            new ModeInfo(2, ModeType.EPTZAutoFraming),
            new ModeInfo(3, ModeType.EPTZSpeaker),
        };

        private static readonly ModeInfo[] kEPTZModeList =
        {
            new ModeInfo(0, ModeType.EPTZManual),
            new ModeInfo(1, ModeType.EPTZAutoFraming),
            new ModeInfo(2, ModeType.EPTZSpeaker),
        };

        private List<HIDDeviceInfo> m_HIDDevices;
        private bool m_SimulateSoundStarted = false;
        private bool m_CheckAckEnabled = false;
        private long m_SecndCnt = 0;
        private long m_ReceiveCnt = 0;
        private HIDDevice m_Device;
        private byte[] m_RequestBuffer;
        private byte[] m_ResponseBuffer;

        private ModeType m_ModeType = ModeType.Normal;
        private ModeType m_EPTZModeType = ModeType.EPTZManual;
        private bool m_EPTZEnabled = false;

        private static readonly string LOG_PASSWORD = "seevision_log_123";

        public HIDToolForm()
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            btnUpdateDeviceList_Click(null, null);
        }

        private bool UpdateDeviceList()
        {
            m_HIDDevices = GetHIDDevices();
            comboBoxDevices.Items.Clear();
            for (int i = 0; i < m_HIDDevices.Count; i++)
            {
                comboBoxDevices.Items.Add("Smart Camera " + i);
            }
            if (m_HIDDevices.Count > 0)
            {
                comboBoxDevices.SelectedIndex = 0;
                return true;
            }
            return false;
        }




        private ModeType ConvertEPTZIndexToMode(int index)
        {
            foreach (ModeInfo info in kEPTZModeList)
            {
                if (index == info.index)
                {
                    return info.mode;
                }
            }
            MessageBox.Show("ConvertEPTZIndexToMode failed!!!");
            return ModeType.EPTZManual;
        }

        private ModeType ConvertPreviewIndexToMode(int index)
        {
            foreach (ModeInfo info in kPreviewModeList)
            {
                if (index == info.index)
                {
                    return info.mode;
                }
            }
            MessageBox.Show("ConvertPreviewIndexToMode failed!!!");
            return ModeType.Normal;
        }

        private int ConvertModeTypeToEPTZIndex(ModeType mode)
        {
            foreach (ModeInfo info in kEPTZModeList)
            {
                if (mode == info.mode)
                {
                    return info.index;
                }
            }
            MessageBox.Show("ConvertModeTypeToIndex failed!!!");
            return 0;
        }

        private int ConvertModeTypeToPreviewIndex(ModeType mode)
        {
            foreach (ModeInfo info in kPreviewModeList)
            {
                if (mode == info.mode)
                {
                    return info.index;
                }
            }
            MessageBox.Show("ConvertModeTypeToIndex failed!!!");
            return 0;
        }

        private void GetVersionBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_CAM_VERSION, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_CAM_VERSION.Length);
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
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_CAMERA_MODEL, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_CAMERA_MODEL.Length);
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
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_UPGRADE_METHOD, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_UPGRADE_METHOD.Length);
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
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_OTA_KEY, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_OTA_KEY.Length);
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

        private void RebootBootloaderBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_REBOOT_BOOTLOADER, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_REBOOT_BOOTLOADER.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                bool isValidCommand = true;
                for (int i = 0; i < 7; i++)
                {
                    if (Protocol.COMMAND_RESPONSE_TYPE_REBOOT_BOOTLOADER[i] != responseBuffer[i])
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
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_APP_VERSION, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_APP_VERSION.Length);
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

        private void GetDeviceNameBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_DEVICE_NAME, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_DEVICE_NAME.Length);
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
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_CPU_TEMP, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_CPU_TEMP.Length);
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

        private void GetCPUFreqBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_CPU_FREQ, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_CPU_FREQ.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string cpuFreq = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    cpuFreq += (char)responseBuffer[7 + i];
                }
                MessageBox.Show("CPU频率: " + cpuFreq);
            }
        }

        private void GetIQVersionBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_IQ_VERSION, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_IQ_VERSION.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                uint iqVersion = (uint)((responseBuffer[7] << 24) |
                    (responseBuffer[8] << 16) |
                    (responseBuffer[9] << 8) |
                    (responseBuffer[10] << 0));
                MessageBox.Show("IQ版本号: " + iqVersion);
            }
        }

        private void GetSNBtn_Click(object sender, EventArgs e)
        {

        }

        private void IsEPTZSupportedBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_EPTZ_SUPPORTED, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_EPTZ_SUPPORTED.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                bool isEPTZSupported = responseBuffer[7] == 0x01 ? true : false;
                if (isEPTZSupported)
                {
                    MessageBox.Show("支持电子云台功能");
                }
                else
                {
                    MessageBox.Show("不支持电子云台功能");
                }
            }
        }

        private bool GetEPTZEnable()
        {
            bool enabled = false;

            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_EPTZ_ENABLE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_EPTZ_ENABLE.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                enabled = responseBuffer[7] == 0x01 ? true : false;
            }

            return enabled;
        }

        private void SetEPTZEnable(bool enable)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_EPTZ_ENABLE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_EPTZ_ENABLE.Length);
                requestBuffer[7] = (byte)(enable ? 0x01 : 0x00);

                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();
            }
        }

        private void GetEPTZEnableBtn_Click(object sender, EventArgs e)
        {
            bool enabled = GetEPTZEnable();
            if (enabled)
            {
                MessageBox.Show("当前电子云台开关状态：已打开");
            }
            else
            {
                MessageBox.Show("当前电子云台开关状态：已关闭");
            }
        }

        private bool ReceiveCommandSafely(HIDDevice device, byte[] data, int offset, int size)
        {
            Task task = Task.Run(async () => {
                await device.ReceiveAsync(data, offset, size);
            });
            try
            {
                task.Wait(1000);
                if (!task.IsCompleted)
                {
                    device.Cancel(); // FIXME: Cancel() doesn't work as expected.
                    return false;
                }
                return true;
            }
            catch (AggregateException)
            {
                Console.WriteLine("ReceiveCommandSafely exception!!!");
            }
            return false;
        }

        private int GetEPTZMode()
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_EPTZ_MODE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_EPTZ_MODE.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                //device.Receive(responseBuffer, 0, responseBuffer.Length);
                bool ret = ReceiveCommandSafely(device, responseBuffer, 0, requestBuffer.Length);
                device.Close();

                if (ret)
                {
                    int index = responseBuffer[7];
                    if (index > kEPTZModeList[kEPTZModeList.Length - 1].index)
                    {
                        MessageBox.Show(string.Format("未支持的电子云台模式，ID：{0}", index));
                    }
                    return index;
                }
                else
                {
                    bool enable = GetEPTZEnable(); // Given that EPTZ is always supported
                    if (enable)
                    {
                        return ConvertModeTypeToEPTZIndex(ModeType.EPTZSpeaker);
                    }
                    else
                    {
                        Console.WriteLine("GetEPTZMode failed: not supported!!!");
                    }
                }
            }

            return 0;
        }

        private void SetPreviewMode(ModeType modeType)
        {
            m_ModeType = modeType;
            if (modeType == ModeType.Normal)
            {
                SetEPTZEnable(false);
                m_EPTZEnabled = false;
                MessageBox.Show("已切换至普通预览模式"); 
            }
            else
            {
                SetEPTZEnable(true);
                m_EPTZEnabled = true;

                SetEPTZMode(modeType);
                string msg;
                switch (modeType)
                {
                    case ModeType.EPTZManual:
                        msg = "已切换至电子云台：手动模式";
                        break;
                    case ModeType.EPTZAutoFraming:
                        msg = "已切换至电子云台：AutoFraming";
                        break;
                    case ModeType.EPTZSpeaker:
                        msg = "已切换至电子云台：主讲者模式";
                        break;
                    default:
                        msg = "无效操作！！！";
                        break;
                }
                MessageBox.Show(msg);
            }
        }

        private void SetEPTZMode(ModeType modeType)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_EPTZ_MODE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_EPTZ_MODE.Length);
                requestBuffer[7] = (byte)ConvertModeTypeToEPTZIndex(modeType);

                device.Send(requestBuffer, 0, requestBuffer.Length);
                //device.Receive(responseBuffer, 0, responseBuffer.Length);
                bool ret = ReceiveCommandSafely(device, responseBuffer, 0, responseBuffer.Length);
                device.Close();
                if (!ret)
                {
                    MessageBox.Show("操作失败：操作未支持！！！");
                }
            }
        }


        private void btnUpdateDeviceList_Click(object sender, EventArgs e)
        {
            UpdateDeviceList();
        }


        private bool CheckEPTZManualModeOpened()
        {
            if (!m_EPTZEnabled || m_ModeType != ModeType.EPTZManual)
            {
                MessageBox.Show("无效操作，请先打开手动模式!!!");
                return false;
            }
            return true;
        }

        private void GET_OPERATE_STATUS(byte back_code)
        {
            switch (back_code)
            {
                //case 0x00:
                    //MessageBox.Show("操作成功！");
                    //break;
                case 0x01:
                    MessageBox.Show("当前已到达边界！");
                    break;
                case 0xff:
                    MessageBox.Show("操作失败！您当前输入的步长已经超出承受范围了");
                    break;
            }
                
        }
            

        private void EPTZ_ENLARGE_NARROW_RESET_Click(object sender, EventArgs e)
        {
            if (!CheckEPTZManualModeOpened())
            {
                return;
            }

            //MessageBox.Show("电子云台比例复位");
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_ENLARGE_NARROW_EPTZ_SIZE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_ENLARGE_NARROW_EPTZ_SIZE.Length);
                // 复位
                requestBuffer[7] = 0xff;

                device.Send(requestBuffer, 0, requestBuffer.Length);
                //device.Receive(responseBuffer, 0, responseBuffer.Length);
                bool ret = ReceiveCommandSafely(device, responseBuffer, 0, responseBuffer.Length);
                // 比例变化后，进行检测，获取当前操作状态结果
                // MessageBox.Show("操作状态：" + responseBuffer);
                device.Close();
                if (ret)
                {
                    GET_OPERATE_STATUS(responseBuffer[7]);
                }
                else
                {
                    MessageBox.Show("操作失败：操作未支持！！！");
                }
            }
        }



        // Guangtao -- modified -- END

        private void StartRecordLogBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_START_RECORD_LOG, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_START_RECORD_LOG.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("已开始录制log", "Log抓取", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void StopRecordLogBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_STOP_RECORD_LOG, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_STOP_RECORD_LOG.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("已停止录制log", "Log抓取", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_RECORD_LOG_SIZE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_RECORD_LOG_SIZE.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);

                int logSize = (responseBuffer[7] << 24) | (responseBuffer[8] << 16) | (responseBuffer[9] << 8) | (responseBuffer[10] << 0);
                int packetCount = logSize / 59 + (logSize % 59 == 0 ? 0 : 1);
                byte[] logBuffer = new byte[packetCount * 59];
                for (int i = 0; i < packetCount; i++)
                {
                    Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_RECORD_LOG, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_RECORD_LOG.Length);
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



        private void IsCaptureSupportedBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_CAPTURE_SUPPORTED, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_CAPTURE_SUPPORTED.Length);
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
                closeHIDDevice(device);
            }
        }


        private void HIDToolForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer1.Enabled)
            {
                closeHIDDevice(m_Device);
            }
        }

        private List<HIDDeviceInfo> GetHIDDevices()
        {
            List<HIDDeviceInfo> devices = new List<HIDDeviceInfo>();
            HIDDeviceInfo[] deviceInfos = HIDDevice.GetHIDDeviceInfos();
            for (int ki = 0; ki < deviceInfos.Length; ki++)
            {
                HIDDeviceInfo deviceInfo = deviceInfos[ki];
                //Console.WriteLine("deviceInfos[{0}], PID: {1}, VID: {2} outlen: {3}, devicePath: {4}", ki, deviceInfo.PID, 
                //    deviceInfo.VID, deviceInfo.OUT_reportByteLength, deviceInfo.devicePath);
                // 下个版本直接改成识别VID就可以了，去掉PID
                if (deviceInfos[ki].VID == VID_1 || deviceInfos[ki].VID == VID_2)
                {
                    if (deviceInfos[ki].OUT_reportByteLength == TARGET_REPORT_BUFFER_SIZE)
                    {
                        devices.Add(deviceInfos[ki]);
                        //Console.WriteLine(" --> add");
                    }

                }
            }
            //Console.WriteLine("device count: {0}", devices.Count);
            return devices;
        }

        private HIDDevice OpenHIDDevice(int index)
        {
            List<HIDDeviceInfo> deviceInfos = GetHIDDevices();

            HIDDeviceInfo targetDeviceInfo = null;
            //for (int ki = 0; ki < deviceInfos.Length; ki++)
            //{
            //    if (deviceInfos[ki].PID == TARGET_HID_PID && deviceInfos[ki].VID == TARGET_HID_VID && deviceInfos[ki].OUT_reportByteLength == TARGET_REPORT_BUFFER_SIZE)
            //    {
            //        targetDeviceInfo = deviceInfos[ki];
            //        break;
            //    }
            //}
            if (index >= 0 && index < deviceInfos.Count)
            {
                targetDeviceInfo = deviceInfos[index];
            }
            if (targetDeviceInfo != null)
            {
                HIDDevice device = HIDDevice.Open(targetDeviceInfo, true);
                return device;

            }
            else
            {
                MessageBox.Show("设备未找到！");
            }
            return null;
        }

        private void closeHIDDevice(HIDDevice device)
        {
            if (device != null)
            {
                device.Close();
            }
        }



        private bool equal(byte[] array0, byte[] array1, int len)
        {
            for (int i = 0; i < len; i++)
            {
                if (array0[i] != array1[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void SetFOVBtn72_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_FOV_MODE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_FOV_MODE.Length);
                requestBuffer[7] = 0x02;
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("视场角已切换72°");
            }
        }

        private void SetFOVBtn90_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_FOV_MODE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_FOV_MODE.Length);
                requestBuffer[7] = 0x01;
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("视场角已切换90°");
            }
        }

        private void SetFOVBtn114_Click_1(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_FOV_MODE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_FOV_MODE.Length);
                requestBuffer[7] = 0x00;
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("视场角已切换114°");
            }
        }

        private void GetFOVBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_FOV_MODE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_FOV_MODE.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                int fovMode = responseBuffer[7];
                string fovStr = "";
                switch (fovMode)
                {
                    case 0:
                        fovStr = "114°";
                        break;
                    case 1:
                        fovStr = "90°";
                        break;
                    case 2:
                        fovStr = "72°";
                        break;
                    default:
                        fovStr = "Invalid Value";
                        break;
                }

                MessageBox.Show("当前视场角为：" + fovStr);
            }
        }

        private void OpenAFMacroBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_AFMACRO_ENABLE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_AFMACRO_ENABLE.Length);
                requestBuffer[7] = 0x01;
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("微距模式已开启");
            }

        }

        private void CloseAFMacroBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_AFMACRO_ENABLE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_AFMACRO_ENABLE.Length);
                requestBuffer[7] = 0x00;
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("微距模式已关闭");
            }

        }

        private void OpenTencentModeBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_TENCENT_CERTIFI_MODE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_TENCENT_CERTIFI_MODE.Length);
                requestBuffer[7] = 0x01;
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("腾讯认证已开启");
            }

        }

        private void CloseTencentModeBtn_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_TENCENT_CERTIFI_MODE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_TENCENT_CERTIFI_MODE.Length);
                requestBuffer[7] = 0x00;
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                MessageBox.Show("腾讯认证已关闭");
            }
        }

        private void getAudioVersion_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_CAM_VERSION, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_CAM_VERSION.Length);
                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_AUDIO_VERSION, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_AUDIO_VERSION.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string audio_version = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    audio_version += (char)responseBuffer[7 + i];
                }
                MessageBox.Show("音频版本号: " + audio_version);
            }
        }

        private void getRMSData_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_AUDIO_RMS, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_AUDIO_RMS.Length);
                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_AUDIO_RMS, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_AUDIO_RMS.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string rms_data = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    rms_data += (char)responseBuffer[7 + i];
                }
                string[] data_array = rms_data.Split('\n');
                string result_data = "";
                for (int j = 1; j <= 6; j++) {
                    result_data += "\n" + data_array[j];
                }
                result_data += "\n" + data_array[9];
                MessageBox.Show("RMS数据: " + result_data);
            }
        }

        private void manualFocusButton_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_FOCUS_MODE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_FOCUS_MODE.Length);
                requestBuffer[7] = 0x00;
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();
                
                string switch_result = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    switch_result += (char)responseBuffer[7 + i];
                }
                MessageBox.Show("切换为手动对焦模式: " + switch_result);
            }
        }

        private void autoFocusButton_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_FOCUS_MODE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_FOCUS_MODE.Length);
                requestBuffer[7] = 0x01;
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string switch_result = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    switch_result += (char)responseBuffer[7 + i];
                }
                MessageBox.Show("切换为自动对焦模式: " + switch_result);
            }
        }


        // 获取当前设备UUID
        private void getCurrentDeviceUUID_button_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice();

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_UUID, requestBuffer, Protocol.COMMAND_RESPONSE_TYPE_GET_UUID_ACK.Length);

                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string uuid = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    uuid += (char)responseBuffer[7 + i];
                }
                currentDeviceUUID_label.Text = uuid;
                MessageBox.Show($"当前设备UUID：{uuid}");
            }
        }

        // 获取当前设备激活状态
        private void getCurrentDeviceActivateStatus_button_Click(object sender, EventArgs e)
        {

        }

        // 激活当前连接的设备
        private void acticateCurrentDevice_button_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_UUID_CIPHERTEXT, requestBuffer, Protocol.COMMAND_RESPONSE_TYPE_SET_UUID_CIPHERTEXT_ACK.Length);

                string cipher_text = "";
                requestBuffer[5] = 0;

                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();
                //getCameraPrivacyStatus();
                bool isValidCommand = true;
                
                for (int i = 0; i < 7; i++)
                {
                    if (Protocol.COMMAND_RESPONSE_TYPE_SET_UUID_CIPHERTEXT_ACK[i] != responseBuffer[i])
                    {
                        isValidCommand = false;
                        break;
                    }
                }
                if (isValidCommand)
                {
                    MessageBox.Show("激活当前连接的设备");
                    int back_code = responseBuffer[7];
                    if (back_code == 0)
                    {
                        MessageBox.Show("激活成功！");
                        MessageBox.Show(back_code.ToString());
                    }
                    if (back_code == 1)
                    {
                        MessageBox.Show("激活失败！");
                        MessageBox.Show(back_code.ToString());
                    }
                    MessageBox.Show(back_code.ToString());
                }
                else
                {
                    MessageBox.Show("无效返回值！");
                    MessageBox.Show("激活失败！");
                }
            }
        }

        // 读取整机序列号
        private void GetSNBtn_Click_1(object sender, EventArgs e)
        {
            this.textBox1.Focus();
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
                this.textBox1.Text = sn;

                //MessageBox.Show("SN已写入");
            }
        }

        // 设置整机序列号
        private void SetSNBtn_Click(object sender, EventArgs e)
        {
            //this.textBox1.Focus();
            if (this.textBox1.Text.Length < 17)
            {
                MessageBox.Show("无效PCBA号长度");
                return;
            }

            HIDDevice device = OpenHIDDevice();
            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_PCBASN, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_PCBASN.Length);
                byte[] sn = System.Text.Encoding.ASCII.GetBytes(this.textBox1.Text);
                if (sn1.Checked)
                {
                    requestBuffer[7] = 0;
                }
                else if (sn2.Checked)
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

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_PCBASN, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_PCBASN.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);

                string snStr = "";
                for (int i = 0; i < 19; i++)
                {
                    snStr += (char)responseBuffer[7 + i];
                }

                device.Close();

                if (snStr == this.textBox1.Text)
                {
                    string passString = "成功写入PCBA号";
                    using (Font font = new Font("Arial", 24))
                    {
                        Rectangle region = new Rectangle(50, 200, 816, 100);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                    }
                    MessageBox.Show(passString);

                }
                else
                {
                    string ngString = "写入PCBA号失败";
                    using (Font font = new Font("Arial", 24))
                    {
                        Rectangle region = new Rectangle(50, 200, 816, 100);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                    }
                    MessageBox.Show(ngString);
                }
                this.textBox1.Text = "";
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
    }
}
