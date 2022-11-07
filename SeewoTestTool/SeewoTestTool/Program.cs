using Microsoft.Win32;
using Microsoft.Win32;
using System;
using System.Net;
using Application = System.Windows.Forms.Application;
namespace SeewoTestTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Regist());
        }
        /**
         * 
        // 获取本地时间
        string dateString = "20221109";
        DateTime date = DateTime.ParseExact(dateString, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
        if (DateTime.Now > date)
        {
            MessageBox.Show("使用时间到期！请与开发者协商", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Application.Exit();
        }
        else
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        */


        /**
        // 限制使用次数激活代码
        RegistryKey RootKey, RegKey;
        RootKey = Registry.CurrentUser.OpenSubKey("Software", true);
        if ((RegKey = RootKey.OpenSubKey("MyRegDataApp", true)) == null)
        {
            RootKey.CreateSubKey("MyRegDataApp");
            RegKey = RootKey.OpenSubKey("MyRegDataApp", true);
            RegKey.SetValue("UseTime", (object)9);
            MessageBox.Show("您可以免费使用本软件10次！", "感谢您首次使用");
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
            return;
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
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }
        }
        catch
        {
            RegKey.SetValue("UseTime", (object)10);
            MessageBox.Show("您可以免费使用本软件10次！", "感谢您首次使用");
            return;
        }
        */
    }

}
