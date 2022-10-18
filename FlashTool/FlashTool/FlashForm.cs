using HIDInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FlashTool.FlashUtils;


namespace FlashTool
{
    public partial class FlashForm : Form
    {
        private Thread m_FlashThread;
        private Mutex m_FlashMutex;
        private bool m_IsFlashing = false;
        private Graphics canvas;
        private string fireWarePath = null;

        private static readonly int TARGET_HID_VID = 9546;

        public static readonly byte[] COMMAND_REQUEST_TYPE_REBOOT_BOOTLOADER = { 0xAA, 0xBB, 0xCC, 0x20, 0x08, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_REBOOT_BOOTLOADER = { 0xAA, 0xBB, 0xCC, 0x20, 0x09, 0x00, 0x00 };

        public FlashForm()
        {
            InitializeComponent();
        }

        private void FlashForm_Load(object sender, EventArgs e)
        {
            canvas = this.CreateGraphics();
        }

        private void FlashBtn_Click(object sender, EventArgs e)
        {
            if (fireWarePath == null)
            {
                MessageBox.Show("请选择固件路径");
            }
            else if (!File.Exists(fireWarePath + "\\auto_update.txt"))
            {
                MessageBox.Show("请选择正确的固件路径");
            }
            else
            {
                if (!m_IsFlashing)
                {
                    CopyDirectory(fireWarePath, System.IO.Directory.GetCurrentDirectory());
                    this.FlashBtn.Text = "正在下载，请勿断电或停止";
                    this.FlashBtn.ForeColor = Color.Red;
                    this.FlashBtn.ForeColor = Color.Red;
                    m_IsFlashing = true;
                    m_FlashMutex = new Mutex();
                    m_FlashThread = new Thread(DownloadEvent);
                    m_FlashThread.Start();
                }

                else
                {
                    this.FlashBtn.Text = "开始下载";
                    m_FlashMutex.WaitOne();
                    m_IsFlashing = false;
                    m_FlashMutex.ReleaseMutex();

                    canvas.Clear(this.BackColor);
                    this.progressBar1.Invoke(new Action(() =>
                    {
                        this.progressBar1.Value = 0;
                    }));
                    m_FlashThread.Join();                    
                    
                }
            }
        }

        private void DownloadEvent()
        {

            string invalidString = "3%";
            using (Font font = new Font("Arial", 20))
            {
                Rectangle region = new Rectangle(0, 140, 804, 100);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                canvas.Clear(this.BackColor);
                canvas.DrawString(invalidString, font, Brushes.Green, region, stringFormat);
            }

            
            FlashUtils flashUtils = new FlashUtils();

            flashUtils.setProgressCallback(onFlashProgressCallback);
            flashUtils.setErrorCallback(onFlashErrorCallback);
            flashUtils.init();
       
            m_FlashMutex.WaitOne();
       
            m_FlashMutex.ReleaseMutex();
            PnPEntityInfo[] entityInfo = USB.AllUsbDevices;

            if (flashUtils.ScanScsiDevice())
            {
                bool flashResult = true;
                Stopwatch sw = new Stopwatch();
                sw.Start();

                this.progressBar1.Invoke(new Action(() =>
                {
                    this.progressBar1.Value = 2;
                }));

                invalidString = "8%";
                using (Font font = new Font("Arial", 20))
                {
                    Rectangle region = new Rectangle(0, 140, 804, 100);

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    canvas.Clear(this.BackColor);
                    canvas.DrawString(invalidString, font, Brushes.Green, region, stringFormat);
                }

                if (flashUtils.GetDeviceState() == FlashUtils.DeviceStateRom)
                {
                    // flash updater
                    flashResult = flashUtils.flashUpdater("usb_updater.bin");
                    this.progressBar1.Invoke(new Action(() =>
                    {
                        this.progressBar1.Value = 10;
                    }));
                    string progressString = "15%";
                    using (Font font = new Font("Arial", 20))
                    {
                        Rectangle region = new Rectangle(0, 140, 804, 100);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        canvas.Clear(this.BackColor);
                        canvas.DrawString(progressString, font, Brushes.Green, region, stringFormat);
                    }
                }
                if (flashResult && flashUtils.GetDeviceState() == FlashUtils.DeviceStateUpdater)
                {
                    // flash uboot
                    if (File.Exists(fireWarePath + "\\customer.jffs2"))
                    {
                        flashResult = flashUtils.flashUboot("u-boot-nor.bin");
                    }
                    else if (File.Exists(fireWarePath + "\\customer.ubifs"))
                    {
                        flashResult = flashUtils.flashUboot("u-boot-nand.bin");
                    }
                    this.progressBar1.Invoke(new Action(() =>
                    {
                        this.progressBar1.Value = 20;
                    }));
                    string progressString = "25%";
                    using (Font font = new Font("Arial", 20))
                    {
                        Rectangle region = new Rectangle(0, 140, 804, 100);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        canvas.Clear(this.BackColor);
                        canvas.DrawString(progressString, font, Brushes.Green, region, stringFormat);
                    }
                }
                if (flashResult && flashUtils.GetDeviceState() == FlashUtils.DeviceStateUBoot)
                {
                    // flash system
                    flashResult = flashUtils.flashSystem(fireWarePath + "\\auto_update.txt");
                }
                sw.Stop();
                double costTime = sw.ElapsedMilliseconds / 1000.0;
                if (flashResult)
                {
                    string passString = "PASS Times: " + costTime + "s";
                    using (Font font = new Font("Arial", 20))
                    {
                        Rectangle region = new Rectangle(0, 140, 804, 100);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        canvas.Clear(this.BackColor);
                        canvas.DrawString(passString, font, Brushes.Green, region, stringFormat);
                    }
                    this.FlashBtn.Invoke(new Action(() =>
                    {
                        this.FlashBtn.Text = "下载完成";
                    }));
                    DeleteFireWareFiles(fireWarePath);

                }
                else
                {
                    string ngString = "NG";
                    using (Font font = new Font("Arial", 20))
                    {
                        Rectangle region = new Rectangle(0, 140, 804, 100);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        canvas.Clear(this.BackColor);
                        canvas.DrawString(ngString, font, Brushes.Red, region, stringFormat);
                    }
                    DeleteFireWareFiles(fireWarePath);

                }
            } else if (OpenHIDDevice() != null || hasBootloadDevice(entityInfo))//设备在fastboot 和usb camera状态都接入升级
            {

                Enterbootloader();
                // flash all images 等设备启动
                Thread.Sleep(2000);
                //更新 设备接入fastboot之后更新 usb列表
                entityInfo = USB.AllUsbDevices;
                if (hasBootloadDevice(entityInfo))
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    this.progressBar1.Invoke(new Action(() =>
                    {
                        this.progressBar1.Value = 2;
                    }));

                    invalidString = "8%";
                    using (Font font = new Font("Arial", 20))
                    {
                        Rectangle region = new Rectangle(0, 140, 804, 100);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        canvas.Clear(this.BackColor);
                        canvas.DrawString(invalidString, font, Brushes.Green, region, stringFormat);
                    }
                    if (!m_isTest)
                    {
                        doFastbootEraseAll();
                        doFastbootReboot();
                    }

                    Thread.Sleep(2000);
                 
                    entityInfo = USB.AllUsbDevices;
                    bool flashResult = true;
                    if (flashUtils.ScanScsiDevice())
                    {

                        if (flashUtils.GetDeviceState() == FlashUtils.DeviceStateRom)
                        {
                            // flash updater
                            flashResult = flashUtils.flashUpdater("usb_updater.bin");
                            this.progressBar1.Invoke(new Action(() =>
                            {
                                this.progressBar1.Value = 10;
                            }));
                            string progressString = "15%";
                            using (Font font = new Font("Arial", 20))
                            {
                                Rectangle region = new Rectangle(0, 140, 804, 100);

                                StringFormat stringFormat = new StringFormat();
                                stringFormat.Alignment = StringAlignment.Center;
                                stringFormat.LineAlignment = StringAlignment.Center;

                                canvas.Clear(this.BackColor);
                                canvas.DrawString(progressString, font, Brushes.Green, region, stringFormat);
                            }
                        }
                        if (flashResult && flashUtils.GetDeviceState() == FlashUtils.DeviceStateUpdater)
                        {
                            // flash uboot
                            if (File.Exists(fireWarePath + "\\customer.jffs2"))
                            {
                                flashResult = flashUtils.flashUboot("u-boot-nor.bin");
                            }
                            else if (File.Exists(fireWarePath + "\\customer.ubifs"))
                            {
                                flashResult = flashUtils.flashUboot("u-boot-nand.bin");
                            }
                            this.progressBar1.Invoke(new Action(() =>
                            {
                                this.progressBar1.Value = 20;
                            }));
                            string progressString = "25%";
                            using (Font font = new Font("Arial", 20))
                            {
                                Rectangle region = new Rectangle(0, 140, 804, 100);

                                StringFormat stringFormat = new StringFormat();
                                stringFormat.Alignment = StringAlignment.Center;
                                stringFormat.LineAlignment = StringAlignment.Center;

                                canvas.Clear(this.BackColor);
                                canvas.DrawString(progressString, font, Brushes.Green, region, stringFormat);
                            }
                        }
                        if (flashResult && flashUtils.GetDeviceState() == FlashUtils.DeviceStateUBoot)
                        {
                            // flash system
                            flashResult = flashUtils.flashSystem(fireWarePath + "\\auto_update.txt");
                        }
                        sw.Stop();
                        double costTime = sw.ElapsedMilliseconds / 1000.0;
                        if (flashResult)
                        {
                            string passString = "PASS Times: " + costTime + "s";
                            using (Font font = new Font("Arial", 20))
                            {
                                Rectangle region = new Rectangle(0, 140, 804, 100);

                                StringFormat stringFormat = new StringFormat();
                                stringFormat.Alignment = StringAlignment.Center;
                                stringFormat.LineAlignment = StringAlignment.Center;

                                canvas.Clear(this.BackColor);
                                canvas.DrawString(passString, font, Brushes.Green, region, stringFormat);
                            }
                            this.FlashBtn.Invoke(new Action(() =>
                            {
                                this.FlashBtn.Text = "下载完成";
                            }));
                            DeleteFireWareFiles(fireWarePath);
                        }
                        else
                        {
                            string ngString = "NG";
                            using (Font font = new Font("Arial", 20))
                            {
                                Rectangle region = new Rectangle(0, 140, 804, 100);

                                StringFormat stringFormat = new StringFormat();
                                stringFormat.Alignment = StringAlignment.Center;
                                stringFormat.LineAlignment = StringAlignment.Center;

                                canvas.Clear(this.BackColor);
                                canvas.DrawString(ngString, font, Brushes.Red, region, stringFormat);
                            }
                            DeleteFireWareFiles(fireWarePath);

                        }
                    }

                }
            } else
            {
                this.FlashBtn.Invoke(new Action(() =>
                {
                    this.FlashBtn.Text = "开始下载";
                }));
                m_FlashMutex.WaitOne();
                m_IsFlashing = false;
                m_FlashMutex.ReleaseMutex();

                canvas.Clear(this.BackColor);
                this.progressBar1.Invoke(new Action(() =>
                {
                    this.progressBar1.Value = 0;
                }));
                m_FlashThread.Join();
                MessageBox.Show("未检出设备请重新插入设备！");
            }

           // }
            //MessageBox.Show("OK");
        }


        private HIDDevice OpenHIDDevice()
        {
            HIDDeviceInfo[] deviceInfos = HIDDevice.GetHIDDeviceInfos();
            HIDDeviceInfo targetDeviceInfo = null;
            for (int ki = 0; ki < deviceInfos.Length; ki++)
            {
                if (deviceInfos[ki].VID == TARGET_HID_VID)
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
       
            return null;
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
                Thread.Sleep(3000);
            } 
            
        }


        private void doFastbootEraseAll()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Application.StartupPath + "/platform-tools/fastboot";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.Arguments = " erase all";
            proc.Start();
            proc.WaitForExit();
            proc.Close();
        }

        private void doFastbootReboot()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Application.StartupPath + "/platform-tools/fastboot";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.Arguments = " reboot";
            proc.Start();
            proc.WaitForExit();
            proc.Close();
        }

        private void onFlashProgressCallback(int progress)
        {
            if (progress <= 100)
            {
                this.progressBar1.Invoke(new Action(() =>
                {
                    this.progressBar1.Value = 20 + progress * 80 / 100;
                }));

                string progressString = "" + (20 + progress * 80 / 100) + "%";
                using (Font font = new Font("Arial", 20))
                {
                    Rectangle region = new Rectangle(0, 140, 804, 100);

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    canvas.Clear(this.BackColor);
                    canvas.DrawString(progressString, font, Brushes.Green, region, stringFormat);
                }
            }
        }

        private void onFlashErrorCallback(int error)
        {

        }

        private void FlashForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_IsFlashing)
            {
                m_FlashMutex.WaitOne();
                m_IsFlashing = false;
                m_FlashMutex.ReleaseMutex();

                m_FlashThread.Join();
            }
        }

        private bool m_isTest = false;
        private bool hasBootloadDevice(PnPEntityInfo[] entityInfo)
        {
            for (int i = 0; i < entityInfo.Length; i++)
            {
                Console.WriteLine("entityInfo[i].Name " + entityInfo[i].Name);
                if (entityInfo[i].Name.Contains("Bootloader") || entityInfo[i].Name.Contains("Android Sooner Single ADB Interface"))
                {
                    return true;
                }
            }
            if (m_isTest)
            {
                m_isTest = false;
                return true;
            } else
            {
                return false;
            }
            //return false;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                fireWarePath = fbd.SelectedPath;
                this.textBox1.Text =  fireWarePath;
            }
        }

        public void CopyDirectory(string sourceDirPath, string saveDirPath)
        {
            try
            {
                if (!Directory.Exists(saveDirPath))
                {
                    Directory.CreateDirectory(saveDirPath);
                }
                string[] files = Directory.GetFiles(sourceDirPath);
                foreach (string file in files)
                {
                    string pFilePath = saveDirPath + "\\" + Path.GetFileName(file);                  
                    File.Copy(file, pFilePath, true);
                }

                string[] dirs = Directory.GetDirectories(sourceDirPath);
                foreach (string dir in dirs)
                {
                    CopyDirectory(dir, saveDirPath + "\\" + Path.GetFileName(dir));
                }
            }
            catch (Exception ex)
            {

            }
        }

        //根据原来的路径将本地的删除
        public void DeleteFireWareFiles(string sourceDirPath)
        {
            try
            {
                string[] files = Directory.GetFiles(sourceDirPath);
                foreach (string file in files)
                {
                    string pFilePath = System.IO.Directory.GetCurrentDirectory() + "\\" + Path.GetFileName(file);
                    File.Delete(pFilePath);
                }

                string[] dirs = Directory.GetDirectories(sourceDirPath);
                foreach (string dir in dirs)
                {
                    DirectoryInfo director = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory() +"\\"+ Path.GetFileName(dir));
                    FileSystemInfo[] fileinfo = director.GetFileSystemInfos();  //返回目录中所有文件和子目录
                    foreach (FileSystemInfo i in fileinfo)
                    {
                        if (i is DirectoryInfo)            //判断是否文件夹
                        {
                            DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true);          //删除子目录和文件
                        }
                        else
                        {
                            File.Delete(i.FullName);      //删除指定文件
                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }




    }
}
