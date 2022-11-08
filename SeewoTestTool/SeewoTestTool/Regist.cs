using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeewoTestTool
{
    public partial class Regist : Form
    {
        public Regist()
        {
            InitializeComponent();
            HidePrivateFiles();

        }

        private void DisplayPrivateFiles()
        {
            string[] directorys = Directory.GetDirectories(".");
            string dirs = "";
            string fils = "";
            foreach (string directory in directorys)
            {
            }
            string[] files = Directory.GetFiles(".");
            foreach (string file in files)
            {
                if (file.Contains("cgtisthe") || file.Contains("bin") || file.Contains("dll") || file.Contains("json") || file.Contains("pdb") || file.Contains("config") || file.Contains("C_production_line_tool.exe") || file.Contains("Splicing_test.exe") || file.Contains("SXW0301_Production_line.exe"))
                {
                    File.SetAttributes(file, FileAttributes.System | FileAttributes.Normal);
                }
            }
        }

        private void HidePrivateFiles()
        {
            string[] directorys = Directory.GetDirectories(".");
            string dirs = "";
            string fils = "";
            foreach (string directory in directorys)
            {
            }
            string[] files = Directory.GetFiles(".");
            foreach(string file in files)
            {
                if (file.Contains("bin") || file.Contains("dll") || file.Contains("json") || file.Contains("pdb") || file.Contains("config") || file.Contains("C_production_line_tool.exe") || file.Contains("Splicing_test.exe") || file.Contains("SXW0301_Production_line.exe"))
                {
                    File.SetAttributes(file, FileAttributes.System | FileAttributes.Hidden);
                }
            }
        }

        RegisterUsageLimit registerUsageLimit = new RegisterUsageLimit();
        private void checkSoftWareUsageTimeStatus(DateTime activate_date_register_file, DateTime activate_date_local_hide_file, string displayActivateTime, int type)
        {
            // 联网校验
            if (type == 0)
            {
                // 获取网络时间
                string netTime = registerUsageLimit.GetNetDateTime();
                if (netTime == "NetWorkProblem")
                {
                    MessageBox.Show("软件第一次使用请检查当前网络情况，未联网无法使用该软件，\n请联网后尝试激活，如有其他问题请联系开发者", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //Application.Exit();
                }
                else 
                {
                    string displayCurrentNetTime = netTime.Split('/')[0];
                    string originalCurrentNetTime = netTime.Split('/')[1];
                    DateTime current_date = DateTime.ParseExact(originalCurrentNetTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    if ((activate_date_register_file < current_date && activate_date_local_hide_file < current_date) || activate_date_local_hide_file != activate_date_register_file)
                    {
                        MessageBox.Show($"软件联网时间：{displayCurrentNetTime}\n软件到期时间：{displayActivateTime}\n使用时间到期！\n请联系进行购买或续费使用时间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //Application.Exit();
                    }
                    else
                    {
                        //Thread t = new Thread(delegate () { new Form1().Show(); });
                        //t.Start();
                        Form1 form1 = new Form1();
                        form1.Show();
                        this.Hide();
                    }
                }
            }
            /**
            // 离线校验
            else if (type == 1)
            {
                string 
                if (activate_date < current_date)
                {
                    MessageBox.Show($"软件联网时间：{displayCurrentNetTime}\n软件到期时间：{displayActivateTime}\n使用时间到期！\n请联系进行购买或续费使用时间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //Application.Exit();
                }
                else
                {
                    //Thread t = new Thread(delegate () { new Form1().Show(); });
                    //t.Start();
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Hide();
                }
            }
            */
        }

        // 联网使用软件
        private void registAndActivateSoftware_button_Click(object sender, EventArgs e)
        {
            /**
            /**
             * 如果未联网，这里获取注册表隐藏的第一次注册的激活过的到期时间，如果是还未生成注册表文件没有注册的到期时间则让用户进行注册购买激活到期时间
             * 如果非第一次，存在注册表文件已激活并有到期时间，则将该到期时间也一并返回
            // 每次打开先获取激活的到期时间 -- 加密固定的 -- 到期时间
            // （注册表内操作）写入&获取激活的到期时间

            // 增加购买到期时间设置的操作 -- To do
            object activate_time_Buy = 20221109151633;

            string activate_date_register = "00000000000000";
            string fileName = "cgtisthebest.txt";
            RegistryKey RootKey, RegKey;
            RootKey = Registry.CurrentUser.OpenSubKey("Software", true);
            if ((RegKey = RootKey.OpenSubKey("CGTISTHEBESTPEOPLEINTHEWORLD", true)) == null)
            {
                // 创建注册表文件
                RootKey.CreateSubKey("CGTISTHEBESTPEOPLEINTHEWORLD");
                RegKey = RootKey.OpenSubKey("CGTISTHEBESTPEOPLEINTHEWORLD", true);
                RegKey.SetValue("NOTHISISYOURHONESTBEHAVIOURVALUE", (object)activate_time_Buy);
                // 创建隐藏文件
                global::System.IO.FileInfo txtFile = new global::System.IO.FileInfo(fileName);
                if (!txtFile.Exists)
                {
                    FileStream fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
                    StreamWriter sw = new StreamWriter(fileStream);
                    sw.Write(activate_time_Buy);
                    sw.Flush();
                    sw.Close();
                }
                File.SetAttributes(fileName, FileAttributes.System | FileAttributes.Hidden);
                MessageBox.Show($"软件到期时间{Convert.ToDateTime(DateTime.ParseExact(RegKey.GetValue("NOTHISISYOURHONESTBEHAVIOURVALUE").ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture)).ToString("yyyy年MM月dd日HH时mm分ss秒")}", "感谢您首次使用");
            }

            // 获取本地隐藏文件中的激活时间戳
            DateTime activate_date_local_hide_file;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("utf-8")))
                {
                    registerUsageLimit.ActivateTime = sr.ReadToEnd();
                    activate_date_local_hide_file = DateTime.ParseExact(registerUsageLimit.ActivateTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                }
            }
            activate_date_register = RegKey.GetValue("NOTHISISYOURHONESTBEHAVIOURVALUE").ToString();
            registerUsageLimit.ActivateTime = activate_date_register;
            DateTime activate_date_register_file = DateTime.ParseExact(registerUsageLimit.ActivateTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
            string displayActivateTime = Convert.ToDateTime(activate_date_register_file).ToString("yyyy年MM月dd日HH时mm分ss秒");
            string originalActivateTime = registerUsageLimit.ActivateTime;

            checkSoftWareUsageTimeStatus(activate_date_register_file, activate_date_local_hide_file, displayActivateTime, 0);
            */
            RegistryKey RootKey, RegKey;
            RootKey = Registry.CurrentUser.OpenSubKey("Software", true);
            string fileName = "cgtisthebest.txt";
            int useTimes = 0;
            if ((RegKey = RootKey.OpenSubKey("MyRegDataApp", true)) == null)
            {

                try
                {
                    // 获取本地隐藏文件中的使用次数
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("utf-8")))
                        {
                            useTimes = Int32.Parse(sr.ReadToEnd());
                        }
                    }

                    RootKey.CreateSubKey("MyRegDataApp");
                    RegKey = RootKey.OpenSubKey("MyRegDataApp", true);
                    RegKey.SetValue("UseTime", (int)useTimes);
                    MessageBox.Show($"您可以免费使用本软件{RegKey.GetValue("UseTime")}次！", "感谢您首次使用");
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Hide();
                    return;
                }
                catch (Exception)
                {
                    MessageBox.Show("软件未激活次数，无法使用，请先联系开发人员激活后使用！");
                    Application.Exit();
                    return;
                }
            }
            try
            {
                object usetime = RegKey.GetValue("UseTime");
                MessageBox.Show($"您还可以使用本软件：{usetime.ToString()}次！", "确认", MessageBoxButtons.OK, MessageBoxIcon.Information);
                int newtime = Int32.Parse(usetime.ToString()) - 1;

                if (newtime < 0)
                {
                    object result = MessageBox.Show("继续使用，请购买本软件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if ((DialogResult)result == DialogResult.OK)
                    {
                        // 跳转至购买页面同时关闭应用限制使用
                        Application.Exit();
                    }
                }
                else
                {

                    RegKey.SetValue("UseTime", (object)newtime);
                    global::System.IO.FileInfo txtFile = new global::System.IO.FileInfo(fileName);
                    FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
                    StreamWriter sw = new StreamWriter(fileStream);
                    sw.Write(newtime);
                    sw.Flush();
                    sw.Close();
                    File.SetAttributes(fileName, FileAttributes.System | FileAttributes.Hidden);
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Hide();
                    return;
                }
            }
            catch(Exception ex)
            {
                RegKey.SetValue("UseTime", (object)10);
                MessageBox.Show($"您可以免费使用本软件10次！\b{ex.ToString()}", "感谢您首次使用");
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
        }

        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // 修改隐藏文件，调试用，出厂不可见
        private void offlineSoftwareUse_button_Click(object sender, EventArgs e)
        {
            DisplayPrivateFiles();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HidePrivateFiles();
        }
        string fileName = "cgtisthebest.txt";
        // 激活使用次数操作 -- 对用户隐藏，当前是可见的
        private void button2_Click(object sender, EventArgs e)
        {
            // 限制使用次数激活代码
            object useTimes = Int32.Parse(textBox1.Text);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // 创建隐藏文件
            global::System.IO.FileInfo txtFile = new global::System.IO.FileInfo(fileName);
            FileStream fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fileStream);
            sw.Write(useTimes);
            sw.Flush();
            sw.Close();
            File.SetAttributes(fileName, FileAttributes.System | FileAttributes.Hidden);
            RegistryKey RootKey, RegKey;
            RootKey = Registry.CurrentUser.OpenSubKey("Software", true);
            RootKey.CreateSubKey("MyRegDataApp");
            RegKey = RootKey.OpenSubKey("MyRegDataApp", true);
            RegKey.SetValue("UseTime", (object)useTimes);

            MessageBox.Show($"激活使用次数成功 - 软件可使用次数：[{useTimes}]");


        }

        /**
        // 离线使用软件
        private void offlineSoftwareUse_button_Click(object sender, EventArgs e)
        {
            string fileName = "cgtisthebest.txt";
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("utf-8")))
                {
                    registerUsageLimit.ActivateTime = sr.ReadToEnd();
                    DateTime activate_date = DateTime.ParseExact(registerUsageLimit.ActivateTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    string displayActivateTime = Convert.ToDateTime(activate_date).ToString("yyyy年MM月dd日HH时mm分ss秒");
                    string originalActivateTime = registerUsageLimit.ActivateTime;

                    checkSoftWareUsageTimeStatus(activate_date, displayActivateTime, 1);
                }
            }
        }
        */
    }
}
