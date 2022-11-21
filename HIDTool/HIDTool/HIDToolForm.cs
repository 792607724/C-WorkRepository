using HIDInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            InitializeComponent();
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

        private void UpdatePreviewModeList()
        {
            if (comboBoxPreviewMode.Items.Count == 0)
            {
                comboBoxPreviewMode.Items.Add("普通预览模式");
                comboBoxPreviewMode.Items.Add("电子云台：手动模式");
                comboBoxPreviewMode.Items.Add("电子云台：AutoFraming");
                comboBoxPreviewMode.Items.Add("电子云台：主讲者模式");
            }

            int eptzIndex = GetEPTZMode();
            m_EPTZModeType = ConvertEPTZIndexToMode(eptzIndex);
            
            m_EPTZEnabled = GetEPTZEnable();
            m_ModeType = m_EPTZEnabled ? m_EPTZModeType : ModeType.Normal;
            comboBoxPreviewMode.SelectedIndex = ConvertModeTypeToPreviewIndex(m_ModeType);

            UpdateTabControlEPTZ(m_ModeType);
        }

        private void UpdateTabControlEPTZ(ModeType mode)
        {
            if (mode == ModeType.EPTZManual)
            {
                tabControlEPTZ.SelectedIndex = 0;
            }
            else if (mode == ModeType.EPTZSpeaker)
            {
                tabControlEPTZ.SelectedIndex = 1;
            }
        }

        private void HIDToolForm_Load(object sender, EventArgs e)
        {
            bool ret = UpdateDeviceList();
            if (!ret)
            {
                MessageBox.Show("未发现可用设备！！！");
                Application.Exit();
            }
            
            UpdatePreviewModeList();

            const int kMinAngle = 35;
            const int kMaxAngle = 145;
            for (int i = kMinAngle; i <= kMaxAngle; i += 5)
            {
                combBoxAngles.Items.Add(i.ToString());
            }
            combBoxAngles.SelectedIndex = combBoxAngles.Items.IndexOf("90");
            chkBoxCheckAck.Checked = true;

            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Filter = "Log文件 (*.tar.gz)|*.tar.gz";
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
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_SN, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_SN.Length);

                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string sn = "";
                for (int i = 0; i < 16; i++)
                {
                    sn += (char)responseBuffer[7 + i];
                }

                MessageBox.Show("SN号：" + sn);
            }
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

        private void comboBoxPreviewMode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int index = this.comboBoxPreviewMode.SelectedIndex;
            ModeType modeType = ConvertPreviewIndexToMode(index);
            UpdateTabControlEPTZ(modeType);
            SetPreviewMode(modeType);
        }

        private void btnUpdateDeviceList_Click(object sender, EventArgs e)
        {
            UpdateDeviceList();
        }

        private void GetEPTZModeBtn_Click(object sender, EventArgs e)
        {
            UpdatePreviewModeList();

            string msg = "";
            switch (m_EPTZModeType)
            {
                case ModeType.EPTZManual:
                    msg = "电子云台：手动模式";
                    break;
                case ModeType.EPTZAutoFraming:
                    msg = "电子云台：AutoFraming";
                    break;
                case ModeType.EPTZSpeaker:
                    msg = "电子云台：主讲者模式";
                    break;
                default:
                    msg = "电子云台模式：未支持的模式";
                    break;
            }
            MessageBox.Show(msg);
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
            
        private void EPTZ_ENLARGE_Click(object sender, EventArgs e)
        {
            if (!CheckEPTZManualModeOpened())
            {
                return;
            }

            // MessageBox.Show("电子云台比例放大");
            string step_size = eptz_size_textbox_length.Text;
            // MessageBox.Show("当前设置步长为：" + step_size);
            if (int.Parse(step_size) > 255)
            {
                MessageBox.Show("输入步长：" + step_size + "，超出最大值，按最大值已设置：255");
                step_size = "255";
                eptz_size_textbox_length.Text = step_size;
            }
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_ENLARGE_NARROW_EPTZ_SIZE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_ENLARGE_NARROW_EPTZ_SIZE.Length);
                // 放大
                requestBuffer[7] = 0x00;
                // 步数为1 =64 pixel，通过用户传值进入，默认取值1
                requestBuffer[8] = byte.Parse(step_size);

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

        private void EPTZ_NARROW_Click(object sender, EventArgs e)
        {
            if (!CheckEPTZManualModeOpened())
            {
                return;
            }

            //MessageBox.Show("电子云台比例缩小");
            string step_size = eptz_size_textbox_length.Text;
            //MessageBox.Show("当前设置步长为：" + step_size);
            if (int.Parse(step_size) > 255)
            {
                MessageBox.Show("输入步长：" + step_size + "，超出最大值，按最大值已设置：255");
                step_size = "255";
                eptz_size_textbox_length.Text = step_size;
            }
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_ENLARGE_NARROW_EPTZ_SIZE, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_ENLARGE_NARROW_EPTZ_SIZE.Length);
                // 缩小
                requestBuffer[7] = 0x01;
                // 步数为1 =64 pixel，通过用户传值进入，默认取值1
                requestBuffer[8] = byte.Parse(step_size);

                device.Send(requestBuffer, 0, requestBuffer.Length);
                //device.Receive(responseBuffer, 0, responseBuffer.Length);
                bool ret = ReceiveCommandSafely(device, responseBuffer, 0, responseBuffer.Length);
                // 比例变化后，进行检测，获取当前操作状态结果
                //MessageBox.Show("操作状态：" + responseBuffer);
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

        private void EPTZ_MOVE_UP_Click(object sender, EventArgs e)
        {
            if (!CheckEPTZManualModeOpened())
            {
                return;
            }

            //MessageBox.Show("电子云台上移");
            string step_size = eptz_move_textbox_length.Text;
            //MessageBox.Show("当前设置步长为：" + step_size);
            if (int.Parse(step_size) > 255)
            {
                MessageBox.Show("输入步长：" + step_size + "，超出最大值，按最大值已设置：255");
                step_size = "255";
                eptz_move_textbox_length.Text = step_size;
            }
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_MOVE_EPTZ, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_MOVE_EPTZ.Length);
                // 上移
                requestBuffer[7] = 0x00;
                // 步数为1 =32 pixel，通过用户传值进入，默认取值1
                requestBuffer[8] = byte.Parse(step_size);

                device.Send(requestBuffer, 0, requestBuffer.Length);
                //device.Receive(responseBuffer, 0, responseBuffer.Length);
                bool ret = ReceiveCommandSafely(device, responseBuffer, 0, responseBuffer.Length);
                // 移动变化后，进行检测，获取当前操作状态结果
                //MessageBox.Show("操作状态：" + responseBuffer);
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

        private void EPTZ_MOVE_LEFT_Click(object sender, EventArgs e)
        {
            if (!CheckEPTZManualModeOpened())
            {
                return;
            }

            //MessageBox.Show("电子云台左移");
            string step_size = eptz_move_textbox_length.Text;
            //MessageBox.Show("当前设置步长为：" + step_size);
            if (int.Parse(step_size) > 255)
            {
                MessageBox.Show("输入步长：" + step_size + "，超出最大值，按最大值已设置：255");
                step_size = "255";
                eptz_move_textbox_length.Text = step_size;
            }
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_MOVE_EPTZ, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_MOVE_EPTZ.Length);
                // 左移
                requestBuffer[7] = 0x02;
                // 步数为1 =32 pixel，通过用户传值进入，默认取值1
                requestBuffer[8] = byte.Parse(step_size);

                device.Send(requestBuffer, 0, requestBuffer.Length);
                //device.Receive(responseBuffer, 0, responseBuffer.Length);
                bool ret = ReceiveCommandSafely(device, responseBuffer, 0, responseBuffer.Length);
                // 移动变化后，进行检测，获取当前操作状态结果
                //MessageBox.Show("操作状态：" + responseBuffer);
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

        private void EPTZ_MOVE_RIGHT_Click(object sender, EventArgs e)
        {
            if (!CheckEPTZManualModeOpened())
            {
                return;
            }

            //MessageBox.Show("电子云台右移");
            string step_size = eptz_move_textbox_length.Text;
            //MessageBox.Show("当前设置步长为：" + step_size);
            if (int.Parse(step_size) > 255)
            {
                MessageBox.Show("输入步长：" + step_size + "，超出最大值，按最大值已设置：255");
                step_size = "255";
                eptz_move_textbox_length.Text = step_size;
            }
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_MOVE_EPTZ, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_MOVE_EPTZ.Length);
                // 左移
                requestBuffer[7] = 0x03;
                // 步数为1 =32 pixel，通过用户传值进入，默认取值1
                requestBuffer[8] = byte.Parse(step_size);

                device.Send(requestBuffer, 0, requestBuffer.Length);
                //device.Receive(responseBuffer, 0, responseBuffer.Length);
                bool ret = ReceiveCommandSafely(device, responseBuffer, 0, responseBuffer.Length);
                // 移动变化后，进行检测，获取当前操作状态结果
                //MessageBox.Show("操作状态：" + responseBuffer);
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
        private void EPTZ_MOVE_DOWN_Click(object sender, EventArgs e)
        {
            if (!CheckEPTZManualModeOpened())
            {
                return;
            }

            //MessageBox.Show("电子云台下移");
            string step_size = eptz_move_textbox_length.Text;
            //MessageBox.Show("当前设置步长为：" + step_size);
            if (int.Parse(step_size) > 255)
            {
                MessageBox.Show("输入步长：" + step_size + "，超出最大值，按最大值已设置：255");
                step_size = "255";
                eptz_move_textbox_length.Text = step_size;
            }
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_MOVE_EPTZ, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_MOVE_EPTZ.Length);
                // 下移
                requestBuffer[7] = 0x01;
                // 步数为1 =32 pixel，通过用户传值进入，默认取值1
                requestBuffer[8] = byte.Parse(step_size);

                device.Send(requestBuffer, 0, requestBuffer.Length);
                //device.Receive(responseBuffer, 0, responseBuffer.Length);
                bool ret = ReceiveCommandSafely(device, responseBuffer, 0, responseBuffer.Length);
                // 移动变化后，进行检测，获取当前操作状态结果
                //MessageBox.Show("操作状态：" + responseBuffer);
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

        private void GetRecordLogBtn_Click(object sender, EventArgs e)
        {
            if (!this.backgroundWorker1.IsBusy)
            {
                this.toolStripProgressBar1.Value = 0;
                this.saveFileDialog1.FileName = "SmartCameraLog_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.backgroundWorker1.RunWorkerAsync();
                    this.GetRecordLogBtn.Enabled = false;
                }
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

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                this.toolStripProgressBar1.Value = 0;
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

        private void btnSimulateSound_Click(object sender, EventArgs e)
        {
            if (!m_SimulateSoundStarted)
            {
                if (!m_EPTZEnabled || m_ModeType != ModeType.EPTZSpeaker)
                {
                    MessageBox.Show("无效操作，请先打开主讲者模式!!!");
                    return;
                }
                m_Device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);
                if (m_Device != null)
                {
                    DisableOtherButtons();
                    m_CheckAckEnabled = chkBoxCheckAck.Checked;
                    chkBoxCheckAck.Enabled = false;
                    combBoxAngles.Enabled = false;

                    if (m_RequestBuffer == null)
                    {
                        m_RequestBuffer = new byte[m_Device.OutputReportByteLength];
                        m_ResponseBuffer = new byte[m_Device.OutputReportByteLength];
                        Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SOUND_LOCATION, m_RequestBuffer, Protocol.COMMAND_REQUEST_TYPE_SOUND_LOCATION.Length);
                    }
                    short angle = Convert.ToInt16(combBoxAngles.SelectedItem.ToString());
                    m_RequestBuffer[7] = (byte)(angle & 0xFF);
                    m_RequestBuffer[8] = (byte)((angle >> 8) & 0xFF);

                    m_SimulateSoundStarted = true;
                    btnSimulateSound.Text = "停止";

                    m_SecndCnt = 0;
                    m_ReceiveCnt = 0;
                    lblSendCnt.Text = m_SecndCnt.ToString();
                    lblReceiveCnt.Text = m_ReceiveCnt.ToString();

                    timer1.Enabled = true;
                }
            }
            else
            {
                m_SimulateSoundStarted = false;
                btnSimulateSound.Text = "开始";
                timer1.Enabled = false;
                closeHIDDevice(m_Device);

                chkBoxCheckAck.Enabled = true;
                combBoxAngles.Enabled = true;
                EnableOtherButtons();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendSoundLocation();
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
                if ((deviceInfos[ki].PID == TARGET_HID_HONGHE_PID && deviceInfos[ki].VID == TARGET_HID_HONGHE_VID) || 
                    (deviceInfos[ki].VID == TARGET_HID_SEEVISION_VID) || (deviceInfos[ki].VID == TARGET_HID_HONGHE03_VID && deviceInfos[ki].PID == TARGET_HID_HONGHE03_PID)
                    || (deviceInfos[ki].VID == TARGET_HID_HONGHE04_VID && deviceInfos[ki].PID == TARGET_HID_HONGHE04_PID)
                    || (deviceInfos[ki].VID == TARGET_HID_HONGHE05_VID && deviceInfos[ki].PID == TARGET_HID_HONGHE05_PID))
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

        private void DisableOtherButtons()
        {
            comboBoxDevices.Enabled = false;
            btnUpdateDeviceList.Enabled = false;
            GetVersionBtn.Enabled = false;
            GetCameraModelBtn.Enabled = false;
            GetUpgradeMethodBtn.Enabled = false;
            GetOTAKeyBtn.Enabled = false;
            RebootBootloaderBtn.Enabled = false;
            StartRecordLogBtn.Enabled = false;
            StopRecordBtn.Enabled = false;
            GetRecordLogBtn.Enabled = false;
            GetAppVersionBtn.Enabled = false;
            GetDeviceNameBtn.Enabled = false;
            GetIQVersionBtn.Enabled = false;
            GetSNBtn.Enabled = false;
            IsCaptureSupportedBtn.Enabled = false;
            GetCPUTempBtn.Enabled = false;
            GetCPUFreqBtn.Enabled = false;
            IsCaptureSupportedBtn.Enabled = false;
            IsEPTZSupportedBtn.Enabled = false;
            GetEPTZModeBtn.Enabled = false;
            GetEPTZEnableBtn.Enabled = false;
            comboBoxPreviewMode.Enabled = false;
            OpenTencentModeBtn.Enabled = false;
            CloseTencentModeBtn.Enabled = false;
            SetFOVBtn72.Enabled = false;
            SetFOVBtn90.Enabled = false;
            SetFOVBtn114.Enabled = false;
            GetFOVBtn.Enabled = false;
            OpenAFMacroBtn.Enabled = false;
            CloseAFMacroBtn.Enabled = false;
        }
        private void EnableOtherButtons()
        {
            comboBoxDevices.Enabled = true;
            btnUpdateDeviceList.Enabled = true;
            GetVersionBtn.Enabled = true;
            GetCameraModelBtn.Enabled = true;
            GetUpgradeMethodBtn.Enabled = true;
            GetOTAKeyBtn.Enabled = true;
            RebootBootloaderBtn.Enabled = true;
            StartRecordLogBtn.Enabled = true;
            StopRecordBtn.Enabled = true;
            GetRecordLogBtn.Enabled = true;
            GetAppVersionBtn.Enabled = true;
            GetDeviceNameBtn.Enabled = true;
            GetIQVersionBtn.Enabled = true;
            GetSNBtn.Enabled = true;
            IsCaptureSupportedBtn.Enabled = true;
            GetCPUTempBtn.Enabled = true;
            GetCPUFreqBtn.Enabled = true;
            IsCaptureSupportedBtn.Enabled = true;
            IsEPTZSupportedBtn.Enabled = true;
            GetEPTZModeBtn.Enabled = true;
            GetEPTZEnableBtn.Enabled = true;
            comboBoxPreviewMode.Enabled = true;
            OpenTencentModeBtn.Enabled = true;
            CloseTencentModeBtn.Enabled = true;
            SetFOVBtn72.Enabled = true;
            SetFOVBtn90.Enabled = true;
            SetFOVBtn114.Enabled = true;
            GetFOVBtn.Enabled = true;
            OpenAFMacroBtn.Enabled = true;
            CloseAFMacroBtn.Enabled = true;
        }

        private void SendSoundLocation()
        {
            m_Device.Send(m_RequestBuffer, 0, m_RequestBuffer.Length);
            m_Device.Receive(m_ResponseBuffer, 0, m_ResponseBuffer.Length);

            m_SecndCnt++;
            lblSendCnt.Text = m_SecndCnt.ToString();
            if (m_CheckAckEnabled && equal(m_ResponseBuffer, Protocol.COMMAND_RESPONSE_TYPE_SOUND_LOCATION, 7))
            {
                m_ReceiveCnt++;
                lblReceiveCnt.Text = m_ReceiveCnt.ToString();
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

        private void setFocusButton_Click(object sender, EventArgs e)
        {
            string step_size = focusLocationTextBox.Text;
            // MessageBox.Show("当前设置步长为：" + step_size);
            //if (int.Parse(step_size) > 255)
            //{
            //    MessageBox.Show("输入步长：" + step_size + "，超出最大值，按最大值已设置：255");
            //    step_size = "255";
            //    eptz_size_textbox_length.Text = step_size;
            //}
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_FOCUS_LOCATION, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_FOCUS_LOCATION.Length);
                requestBuffer[7] = (byte)(int.Parse(step_size) >> 8);
                requestBuffer[8] = (byte)(int.Parse(step_size) & 0xff);

                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();
            }
        }

        private void getCurrentFocusLocationButton_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_FOCUS_LOCATION, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_FOCUS_LOCATION.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                int currentFocusLocation = (int.Parse(responseBuffer[7].ToString()) << 8) + int.Parse(responseBuffer[8].ToString());
                currentFocusLocationLabel.Text = "当前对焦位置：" + currentFocusLocation.ToString();
                //MessageBox.Show($"当前responseBuffer[7]：{int.Parse(responseBuffer[7].ToString()) << 8}，responseBuffer[8]：{responseBuffer[8].ToString()}");
            }
        }


        // 获取摄像头隐私黑屏状态
        private void getCameraPrivacyStatus()
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_GET_OUTPUT_BLACK_PIC, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_GET_OUTPUT_BLACK_PIC.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                string backConect = "";
                int strLen = responseBuffer[5];
                for (int i = 0; i < strLen; i++)
                {
                    backConect += (char)responseBuffer[7 + i];
                }
                MessageBox.Show("返回内容: " + backConect);

                if (backConect == "0")
                {
                    privacy_camera_output_status_label.Text = "摄像头隐私黑屏状态：已关闭";
                }
                else if (backConect == "1")
                {
                    privacy_camera_output_status_label.Text = "摄像头隐私黑屏状态：已打开";
                }

                bool isValidCommand = true;
                for (int i = 0; i < 7; i++)
                {
                    if (Protocol.COMMAND_RESPONSE_TYPE_GET_OUTPUT_BLACK_PIC[i] != responseBuffer[i])
                    {
                        isValidCommand = false;
                        break;
                    }
                }
                if (isValidCommand)
                {
                    MessageBox.Show("有效返回值！ 获取摄像头隐私黑屏状态");
                }
                else
                {
                    MessageBox.Show("无效返回值！");
                }
            }
        }

        // 开启摄像头隐私黑屏
        private void open_camera_privacy_button_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_OUTPUT_BLACK_PIC, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_OUTPUT_BLACK_PIC.Length);
                requestBuffer[7] = 0x01;
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                bool isValidCommand = true;
                for (int i = 0; i < 7; i++)
                {
                    if (Protocol.COMMAND_RESPONSE_TYPE_GET_OUTPUT_BLACK_PIC[i] != responseBuffer[i])
                    {
                        isValidCommand = false;
                        break;
                    }
                }
                if (isValidCommand)
                {
                    //MessageBox.Show("开启摄像头隐私黑屏");
                    privacy_camera_output_status_label.Text = "已开启";
                    getCameraPrivacyStatus();
                }
                else
                {
                    MessageBox.Show("无效返回值！");
                }
            }
        }

        // 关闭摄像头隐私黑屏
        private void close_camera_privacy_button_Click(object sender, EventArgs e)
        {
            HIDDevice device = OpenHIDDevice(this.comboBoxDevices.SelectedIndex);

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(Protocol.COMMAND_REQUEST_TYPE_SET_OUTPUT_BLACK_PIC, requestBuffer, Protocol.COMMAND_REQUEST_TYPE_SET_OUTPUT_BLACK_PIC.Length);
                requestBuffer[7] = 0x00;
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Receive(responseBuffer, 0, responseBuffer.Length);
                device.Close();

                bool isValidCommand = true;
                for (int i = 0; i < 7; i++)
                {
                    if (Protocol.COMMAND_RESPONSE_TYPE_GET_OUTPUT_BLACK_PIC[i] != responseBuffer[i])
                    {
                        isValidCommand = false;
                        break;
                    }
                }
                if (isValidCommand)
                {
                    privacy_camera_output_status_label.Text = "已关闭";
                    //MessageBox.Show("关闭摄像头隐私黑屏");
                    getCameraPrivacyStatus();
                }
                else
                {
                    MessageBox.Show("无效返回值！");
                }
            }
        }
    }
}
