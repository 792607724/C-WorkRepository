
using HIDInterface;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VideoCapture;
using System.Threading;
using System.Diagnostics;
using System.Linq;

namespace SwitchResolutionTest
{
    public partial class TestForm : Form
    {
        //private static readonly string TARGET_CAMERA = "Smart Camera";
        private static readonly string[] TARGET_CAMERA = { "Smart Camera", "newline AI Camera(TC)" };
        private static readonly int TARGET_HID_HONGHE_VID = 0x2757;
        private static readonly int TARGET_HID_HONGHE_PID = 0x3007;

        private static readonly int TARGET_HID_SHIYUAN_VID = 9546;
        private static readonly int TARGET_HID_SHIYUAN_PID = 3074;

        private static readonly int[] VIDLIST = { 0x2757, 9546 };
        private static readonly int TARGET_REPORT_BUFFER_SIZE = 64;
        private static readonly int ERROR_COUNT_STOP = 5;

        public static readonly byte[] COMMAND_REQUEST_TYPE_REBOOT_BOOTLOADER = { 0xAA, 0xBB, 0xCC, 0x20, 0x08, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_REBOOT_BOOTLOADER = { 0xAA, 0xBB, 0xCC, 0x20, 0x09, 0x00, 0x00 };

        private static readonly PreviewSize[] m_PreviewSizes = new PreviewSize[] {
            new PreviewSize(3840, 2160),
            new PreviewSize(2560, 1440),
            new PreviewSize(1920, 1080),
            new PreviewSize(1280, 720),
            new PreviewSize(1024, 576),
            new PreviewSize(960, 540),
            new PreviewSize(800, 600),
            new PreviewSize(640, 480),
            new PreviewSize(640, 360),
            new PreviewSize(480, 270),
            new PreviewSize(352, 288),
            new PreviewSize(320, 240),
            new PreviewSize(320, 180),
            new PreviewSize(176, 144),
            new PreviewSize(160, 120),
            new PreviewSize(160, 90)
        };

        private List<CameraInfo> m_Cameras;
        private bool m_IsRunning = false;
        private int m_FlastCount = 0;
        private int m_failcount = 0;
        private int m_previewCount = 0;

        private IntPtr[] m_DisplayWindows = new IntPtr[4];
        private int[] m_DisplayWindowWidths = new int[4];
        private int[] m_DisplayWindowHeights = new int[4];

        private Label[] m_Labels = new Label[4];

        private VideoCapturer[] m_VideoCapturers = null;
        private int[] m_PreviewSizeCheckedIndices;
        private int[] m_CameraOpenCounts;
        private int m_CameraIndex = 0, m_CameraCount = 0;
        private int m_ActionType = 0;

        private enum SwitchIntervalMode
        {
            Fixed,
            Random
        }
        private enum SwitchMode
        {
            Sequential,
            Random
        }

        private static readonly int SWITCH_INTERVAL_MS_MIN = 2500;
        private static readonly int SWITCH_INTERVAL_MS_MAX = 5000;
        private static readonly int SWITCH_INTERVAL_MS_DEFAULT = 3000;
        private SwitchIntervalMode m_SwitchIntervalMode = SwitchIntervalMode.Fixed;

        private SwitchMode m_SwitchMode = SwitchMode.Sequential;

        private Random m_Random = new Random();

        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            m_DisplayWindows[0] = this.pictureBox1.Handle;
            m_DisplayWindowWidths[0] = this.pictureBox1.Width;
            m_DisplayWindowHeights[0] = this.pictureBox1.Height;

            m_Labels[0] = this.label1;
            m_Labels[1] = this.label2;
            m_Labels[2] = this.label3;
            m_Labels[3] = this.label4;

            this.comboBoxSwitchMode.SelectedIndex = 0;
            this.comboBoxSwitchInterval.SelectedIndex = 0;

            this.checkedListBoxPreviewSizes.Items.Clear();
            foreach (PreviewSize previewSize in m_PreviewSizes)
            {
                this.checkedListBoxPreviewSizes.Items.Add(previewSize.toString());
            }
            selectDefaultPreviewSizes();
        }

        private int indexOfPreviewSize(int width, int height)
        {
            int index = -1;
            for (int i = 0; i < m_PreviewSizes.Length; i++)
            {
                if (m_PreviewSizes[i].Width == width && m_PreviewSizes[i].Height == height)
                {
                    index = i;
                    break;
                }
            }
            System.Diagnostics.Debug.Assert(index != -1);
            return index;
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            if (!m_IsRunning)
             {
                 m_Cameras = GetCameras();
                 m_CameraCount = m_Cameras.Count > 4 ? 4 : m_Cameras.Count;
                 if (m_CameraCount == 0)
                 {
                     MessageBox.Show("未找到可以测试的摄像头设备");
                     return;
                 }
                 m_CameraIndex = 0;
                 m_ActionType = 0;


                 m_VideoCapturers = new VideoCapturer[m_CameraCount];
                 m_PreviewSizeCheckedIndices = new int[m_CameraCount];
                 m_CameraOpenCounts = new int[m_CameraCount];
                 for (int i = 0; i < m_Cameras.Count; i++)
                 {
                     m_VideoCapturers[i] = new VideoCapturer();
                     m_PreviewSizeCheckedIndices[i] = 0;
                     m_CameraOpenCounts[i] = 0;
                 }

                 this.comboBoxSwitchInterval.Enabled = false;
                 this.comboBoxSwitchMode.Enabled = false;
                 this.checkedListBoxPreviewSizes.Enabled = false;
                 this.btnSelectAll.Enabled = false;
                 this.btnSelectDefault.Enabled = false;

                 if (this.checkedListBoxPreviewSizes.CheckedIndices.Count == 0)
                 {
                     selectDefaultPreviewSizes();
                 }
                 m_SwitchIntervalMode = this.comboBoxSwitchInterval.SelectedIndex == 0 ? SwitchIntervalMode.Fixed : SwitchIntervalMode.Random;
                 m_SwitchMode = this.comboBoxSwitchMode.SelectedIndex == 0 ? SwitchMode.Sequential : SwitchMode.Random;

                 this.timer1.Interval = m_SwitchIntervalMode == SwitchIntervalMode.Fixed ? SWITCH_INTERVAL_MS_DEFAULT : m_Random.Next(SWITCH_INTERVAL_MS_MIN, SWITCH_INTERVAL_MS_MAX);
                 this.timer1.Enabled = true;
                 m_IsRunning = true;
                 this.TestBtn.Text = "停止";
                 this.label2.Text = string.Format("烧录：第{0}次", m_FlastCount);
                 m_FlastCount = 0;
                 m_previewCount = 0;
             }
             else
             {
                 m_IsRunning = false;
                 this.timer1.Enabled = false;
                 this.m_failcount = 0;
                 this.TestBtn.Text = "开始";

                 this.comboBoxSwitchInterval.Enabled = true;
                 this.comboBoxSwitchMode.Enabled = true;
                 this.checkedListBoxPreviewSizes.Enabled = true;
                 this.btnSelectAll.Enabled = true;
                 this.btnSelectDefault.Enabled = true;
             }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            HIDDevice device = OpenHIDDevice();

            if (device != null)
            {

                if (m_ActionType == 0)
                {
                    int index = this.checkedListBoxPreviewSizes.CheckedIndices[m_PreviewSizeCheckedIndices[m_CameraIndex]];
                    PreviewSize previewSize = m_PreviewSizes[index];
                    if (m_SwitchMode == SwitchMode.Sequential)
                    {
                        m_PreviewSizeCheckedIndices[m_CameraIndex] = (m_PreviewSizeCheckedIndices[m_CameraIndex] + 1) % this.checkedListBoxPreviewSizes.CheckedIndices.Count;
                    }
                    else
                    {
                        m_PreviewSizeCheckedIndices[m_CameraIndex] = m_Random.Next(0, this.checkedListBoxPreviewSizes.CheckedIndices.Count);
                    }

                    try
                    {
                        m_VideoCapturers[m_CameraIndex].SetPreviewSize(previewSize.Width, previewSize.Height);
                        m_VideoCapturers[m_CameraIndex].SetDisplayWindow(m_DisplayWindows[m_CameraIndex]);
                        m_VideoCapturers[m_CameraIndex].SetDisplaySize(m_DisplayWindowWidths[m_CameraIndex], m_DisplayWindowHeights[m_CameraIndex]);

                        if (!m_VideoCapturers[m_CameraIndex].StartupCapture(m_Cameras[m_CameraIndex], m_CameraIndex))
                        {

                            string Text = string.Format("第{0}次 Camera：第{1}次切换，{2} open error!",m_FlastCount, m_previewCount, previewSize.toString());
                            MessageBox.Show(Text);                            
                            m_failcount++;
                            if (m_failcount == ERROR_COUNT_STOP)
                            {
                                timer1.Enabled = false;
                            }
                            return;
                        }
                        m_CameraOpenCounts[m_CameraIndex]++;

                        m_Labels[m_CameraIndex].Text = string.Format("Camera：第{0}次切换，{1}", m_previewCount, previewSize.toString());

                        this.label8.Text = string.Format("过程描述：{0}阅览中", previewSize.toString());
                        ///m_Labels[m_CameraIndex].Text = string.Format("Camera{0}：第{1}次，{2}", m_CameraIndex + 1, m_CameraOpenCounts[m_CameraIndex], previewSize.toString());
                    }
                    catch (Exception)
                    {
                        m_failcount++;
                        if (m_failcount == ERROR_COUNT_STOP)
                        {
                            timer1.Enabled = false;
                        }
                        string Text = string.Format("第{0}次烧录Camera：第{1}次切换，{2} open error!", m_FlastCount, m_previewCount, previewSize.toString());
                        MessageBox.Show(Text);
                        return;

                    }
                }
                else
                {
                    m_VideoCapturers[m_CameraIndex].StopCapture();

                    m_CameraIndex = (m_CameraIndex + 1) % m_CameraCount;

                    m_previewCount++;

                    if (m_previewCount % (this.checkedListBoxPreviewSizes.CheckedIndices.Count) == 0)
                    {
                        m_FlastCount++;
                        this.label8.Text = string.Format("过程描述：flash 过程中");
                        this.label2.Text = string.Format("烧录：第{0}次", m_FlastCount);
                        this.Update();
                        timer1.Stop();

                        
                        Enterbootloader();
                        timer1.Stop();
                        Thread.Sleep(2000);//睡眠2秒
                        EnterFlashLK();
                        Thread.Sleep(2000);
                        EnterFlashBOOT();
                        Thread.Sleep(2000);
                        EnterFlashSystem();
                        Thread.Sleep(160000);//睡眠160秒 nor flash 擦除太久
                        EnterFlashFinish();
                        Thread.Sleep(2000);//睡眠2秒
                        EnterFlashReboot();
                        Thread.Sleep(20000);//睡眠25秒
                        Thread.Sleep(50000);
                        timer1.Start();
                        
                    }

                }
                m_ActionType = (m_ActionType + 1) % 2;
                if (m_SwitchIntervalMode == SwitchIntervalMode.Random)
                {
                    this.timer1.Interval = m_Random.Next(SWITCH_INTERVAL_MS_MIN, SWITCH_INTERVAL_MS_MAX);
                }
                device.Close();
                
            }
        }

        private void TestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Enabled = false;
            if (m_VideoCapturers != null)
            {
                for (int i = 0; i < m_VideoCapturers.Length; i++)
                {
                    m_VideoCapturers[i].StopCapture();
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBoxPreviewSizes.Items.Count; i++)
            {
                this.checkedListBoxPreviewSizes.SetItemChecked(i, true);
            }
        }

        private void btnSelectDefault_Click(object sender, EventArgs e)
        {
            selectDefaultPreviewSizes();
        }

        private void selectDefaultPreviewSizes()
        {
            for (int i = 0; i < this.checkedListBoxPreviewSizes.Items.Count; i++)
            {
                this.checkedListBoxPreviewSizes.SetItemChecked(i, false);
            }
            this.checkedListBoxPreviewSizes.SetItemChecked(indexOfPreviewSize(3840, 2160), true);
            this.checkedListBoxPreviewSizes.SetItemChecked(indexOfPreviewSize(1920, 1080), true);
        }

        private List<CameraInfo> GetCameras()
        {
            List<CameraInfo> cameras = new List<CameraInfo>();

            List<CameraInfo> cameraInfos = VideoCapturer.GetCameraInfos();
            for (int i = 0; i < cameraInfos.Count; i++)
            {
                if (TARGET_CAMERA.Contains(cameraInfos[i].Name))
                {
                    cameras.Add(cameraInfos[i]);
                }
            }
            return cameras;
        }

        private HIDDevice OpenHIDDevice()
        {
            HIDDeviceInfo[] deviceInfos = HIDDevice.GetHIDDeviceInfos();
            HIDDeviceInfo targetDeviceInfo = null;
            for (int ki = 0; ki < deviceInfos.Length; ki++)
            {
                if (VIDLIST.Contains(deviceInfos[ki].VID))
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
                m_failcount++;
                if(m_failcount == ERROR_COUNT_STOP)
                {
                    timer1.Enabled = false;
                }
                MessageBox.Show("设备未找到！ 第"+ m_FlastCount+"烧录");

            }
            return null;
        }

        private void EnterFlashLK()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Application.StartupPath + "/platform-tools/fastboot";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.Arguments = "flash lk lk.bin";
            proc.Start();
            proc.WaitForExit();
            proc.Close();

        }

        private void EnterFlashBOOT()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Application.StartupPath + "/platform-tools/fastboot";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.Arguments = "flash boot boot.img";
            proc.Start();
            proc.WaitForExit();
            proc.Close();

        }

        private void EnterFlashSystem()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Application.StartupPath + "/platform-tools/fastboot";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.Arguments = "flash system SY_images/system.img";
            proc.Start();
            proc.WaitForExit();
            proc.Close();

        }

        private void EnterFlashFinish()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Application.StartupPath + "/platform-tools/fastboot";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.Arguments = "oem finish_upgrade";
            proc.Start();
            proc.WaitForExit();
            proc.Close();   
        }

        private void EnterFlashReboot()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Application.StartupPath + "/platform-tools/fastboot";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.Arguments = "reboot";
            proc.Start();
            proc.WaitForExit();
            proc.Close();
        }

        private void Enterbootloader()
        {
            HIDDevice device = OpenHIDDevice();

            if (device != null)
            {
                byte[] requestBuffer = new byte[device.OutputReportByteLength];
                byte[] responseBuffer = new byte[device.InputReportByteLength];

                Array.Copy(COMMAND_REQUEST_TYPE_REBOOT_BOOTLOADER, requestBuffer, COMMAND_REQUEST_TYPE_REBOOT_BOOTLOADER.Length);
                device.Send(requestBuffer, 0, requestBuffer.Length);
                device.Close();
            }

            Thread.Sleep(5000);
        }


    }
}
