using Microsoft.Win32;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
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
            Process currentProcess = Process.GetCurrentProcess();
            Process[] Processes = Process.GetProcessesByName(currentProcess.ProcessName);
            bool currentSoftwareExists = false;
            //�����뵱ǰ����������ͬ�Ľ����б�
            foreach (Process process in Processes)
            {
                //���ʵ���Ѿ�����,����Ե�ǰ����
                if (process.Id != currentProcess.Id)
                {
                    currentSoftwareExists = true;
                }
            }
            if (currentSoftwareExists)
            {
                MessageBox.Show($"��ǰ���{currentProcess.ProcessName}�Ѵ򿪣������ظ��򿪣�");
            }
            else
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new Regist());
            }
        }


        /**
         * 
        // ��ȡ����ʱ��
        string dateString = "20221109";
        DateTime date = DateTime.ParseExact(dateString, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
        if (DateTime.Now > date)
        {
            MessageBox.Show("ʹ��ʱ�䵽�ڣ����뿪����Э��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Application.Exit();
        }
        else
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        */


        /**
        // ����ʹ�ô����������
        RegistryKey RootKey, RegKey;
        RootKey = Registry.CurrentUser.OpenSubKey("Software", true);
        if ((RegKey = RootKey.OpenSubKey("MyRegDataApp", true)) == null)
        {
            RootKey.CreateSubKey("MyRegDataApp");
            RegKey = RootKey.OpenSubKey("MyRegDataApp", true);
            RegKey.SetValue("UseTime", (object)9);
            MessageBox.Show("���������ʹ�ñ����10�Σ�", "��л���״�ʹ��");
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
            return;
        }
        try
        {
            object usetime = RegKey.GetValue("UseTime");
            MessageBox.Show($"��������ʹ�ñ������{usetime.ToString()}�Σ�", "ȷ��", MessageBoxButtons.OK, MessageBoxIcon.Information);
            int newtime = Int32.Parse(usetime.ToString()) - 1;
            if (newtime < 0)
            {
                object result = MessageBox.Show("����ʹ�ã��빺�������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if ((DialogResult)result == DialogResult.OK)
                {
                    // ��ת������ҳ��ͬʱ�ر�Ӧ������ʹ��
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
            MessageBox.Show("���������ʹ�ñ����10�Σ�", "��л���״�ʹ��");
            return;
        }
        */
    }

}
