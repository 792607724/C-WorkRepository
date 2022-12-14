using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VideoCapture;

namespace SwitchResolutionTest
{
    public partial class TestForm : Form
    {
        private const int MAX_CAMERA_COUNT = 6;
// private static readonly string TARGET_CAMERA = "Smart Camera";
        private static readonly string[] TARGET_CAMERA = { "Smart Camera", "newline AI Camera(TC)"};

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

        private static readonly int[] m_SwitchIntervals = new int[] {
            3000,
            5000
        };

        private List<CameraInfo> m_Cameras;
        private bool m_IsRunning = false;

        private IntPtr[] m_DisplayWindows = new IntPtr[MAX_CAMERA_COUNT];
        private int[] m_DisplayWindowWidths = new int[MAX_CAMERA_COUNT];
        private int[] m_DisplayWindowHeights = new int[MAX_CAMERA_COUNT];

        private Label[] m_Labels = new Label[MAX_CAMERA_COUNT];

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

            m_DisplayWindows[1] = this.pictureBox2.Handle;
            m_DisplayWindowWidths[1] = this.pictureBox2.Width;
            m_DisplayWindowHeights[1] = this.pictureBox2.Height;

            m_DisplayWindows[2] = this.pictureBox3.Handle;
            m_DisplayWindowWidths[2] = this.pictureBox3.Width;
            m_DisplayWindowHeights[2] = this.pictureBox3.Height;

            m_DisplayWindows[3] = this.pictureBox4.Handle;
            m_DisplayWindowWidths[3] = this.pictureBox4.Width;
            m_DisplayWindowHeights[3] = this.pictureBox4.Height;

            m_DisplayWindows[4] = this.pictureBox5.Handle;
            m_DisplayWindowWidths[4] = this.pictureBox5.Width;
            m_DisplayWindowHeights[4] = this.pictureBox5.Height;

            m_DisplayWindows[5] = this.pictureBox6.Handle;
            m_DisplayWindowWidths[5] = this.pictureBox6.Width;
            m_DisplayWindowHeights[5] = this.pictureBox6.Height;

            m_Labels[0] = this.lblCamera1;
            m_Labels[1] = this.lblCamera2;
            m_Labels[2] = this.lblCamera3;
            m_Labels[3] = this.lblCamera4;
            m_Labels[4] = this.lblCamera5;
            m_Labels[5] = this.lblCamera6;

            foreach (int i in m_SwitchIntervals)
            {
                this.comboBoxSwitchInterval.Items.Add(i.ToString() + " ms");
            }
            this.comboBoxSwitchInterval.SelectedIndex = 0;
            this.comboBoxSwitchMode.SelectedIndex = 0;

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
                m_CameraCount = m_Cameras.Count > MAX_CAMERA_COUNT ? MAX_CAMERA_COUNT : m_Cameras.Count;
                if (m_CameraCount == 0)
                {
                    MessageBox.Show("未找到可以测试的摄像头设备!!!");
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

                this.timer1.Interval = m_SwitchIntervals[this.comboBoxSwitchInterval.SelectedIndex];
                this.timer1.Enabled = true;
                m_IsRunning = true;
                this.TestBtn.Text = "停止";
            }
            else
            {
                m_IsRunning = false;
                this.timer1.Enabled = false;
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
                        return;
                    }
                    m_CameraOpenCounts[m_CameraIndex]++;

                    m_Labels[m_CameraIndex].Text = string.Format("Camera{0}：第{1}次，{2}", m_CameraIndex + 1, m_CameraOpenCounts[m_CameraIndex], previewSize.toString());
                }
                catch (Exception)
                {
                }
            }
            else
            {
                m_VideoCapturers[m_CameraIndex].StopCapture();

                m_CameraIndex = (m_CameraIndex + 1) % m_CameraCount;
            }
            m_ActionType = (m_ActionType + 1) % 2;
            if (m_SwitchIntervalMode == SwitchIntervalMode.Random)
            {
                this.timer1.Interval = m_Random.Next(SWITCH_INTERVAL_MS_MIN, SWITCH_INTERVAL_MS_MAX);
            }
            //Console.WriteLine("timer interval: {0}", this.timer1.Interval);
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
            this.checkedListBoxPreviewSizes.SetItemChecked(indexOfPreviewSize(1280, 720), true);
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
    }
}
