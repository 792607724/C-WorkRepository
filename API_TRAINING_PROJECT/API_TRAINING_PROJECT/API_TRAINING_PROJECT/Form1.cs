using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace API_TRAINING_PROJECT
{
    public partial class Form1 : Form
    {
        public static uint SND_ASYNC = 0x0001;
        public static uint SND_FILENAME = 0x00020000;
        [DllImport("winmm.dll")]
        public static extern uint mciSendString(string lpstrCommand,
        string lpstrReturnString, uint uReturnLength, uint hWndCallback);

        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        public void setVolumeToMaximum()
        {
            Process p = Process.GetCurrentProcess();
            Console.WriteLine(p.ProcessName);
            for (int i = 0; i < 50; i++)
            {
                SendMessageW(p.Handle, 0x319, p.Handle, (IntPtr)(0x0a0000));//加2
            }
            mciSendString("setaudio temp_alias volume to 10000", null, 0, 0);
        }

        public void setVolumeToMinimum()
        {
            Process p = Process.GetCurrentProcess();
            for (int i = 0; i < 50; i++)
            {
                SendMessageW(p.Handle, 0x319, p.Handle, (IntPtr)(0x090000));//加2
            }
            mciSendString("setaudio temp_alias volume to 1", null, 0, 0);
        }

        //string musicName = "./粉红噪音15s.mp3";
        string musicName = "./王靖雯 - 诗中的灯火.mp3";
        public void Play()
        {
            mciSendString(@"close temp_alias", null, 0, 0);
            mciSendString($@"open ""{musicName}"" alias temp_alias", null, 0, 0);
            mciSendString("play temp_alias repeat", null, 0, 0);
        }

        public void Stop()
        {
            mciSendString(@"close temp_alias", null, 0, 0);
            mciSendString($@"stop ""{musicName}"" alias temp_alias", null, 0, 0);
            mciSendString("play temp_alias repeat", null, 0, 0);
        }

        private void clickToPlayMusic_button_Click(object sender, EventArgs e)
        {
            setVolumeToMaximum();
            setVolumeToMaximum();
            setVolumeToMaximum();
            setVolumeToMaximum();
            setVolumeToMaximum();
            Play();
        }

        private void clickToStopMusic_button_Click(object sender, EventArgs e)
        {
            setVolumeToMinimum();
            setVolumeToMinimum();
            setVolumeToMinimum();
            setVolumeToMinimum();
            setVolumeToMinimum();
            Stop();
        }

    }
}
