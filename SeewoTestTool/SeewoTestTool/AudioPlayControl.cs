using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SeevisionTestTool
{
    public class AudioPlayControl
    {
        [DllImport("user32.dll")]

        static extern void keybd_event(byte bVk, byte bScan, UInt32 dwFlags, UInt32 dwExtraInfo);


        [DllImport("user32.dll")]

        static extern Byte MapVirtualKey(UInt32 uCode, UInt32 uMapType);


        private const byte VK_VOLUME_MUTE = 0xAD;

        private const byte VK_VOLUME_DOWN = 0xAE;

        private const byte VK_VOLUME_UP = 0xAF;

        private const UInt32 KEYEVENTF_EXTENDEDKEY = 0x0001;

        private const UInt32 KEYEVENTF_KEYUP = 0x0002;

        /// <summary>
        /// 改变系统音量大小，增加
        /// </summary>

        public void VolumeUp()
        {

            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY, 0);

            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// 改变系统音量大小，减小
        /// </summary>

        public void VolumeDown()
        {

            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY, 0);

            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// 改变系统音量大小，静音
        /// </summary>

        public void Mute()
        {

            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY, 0);

            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        [DllImport("winmm.dll")]
        public static extern uint mciSendString(string lpstrCommand, string lpstrReturnString, uint uReturnLength, uint hWndCallback);

        string musicName = "./粉红噪音15s.mp3";
        //string musicName = "./王靖雯 - 诗中的灯火.mp3";
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

        public void PlayMusic()
        {
            for (int i = 0; i < 100; i++)
            {
                VolumeUp();
            }
            Play();
        }

        public void StopMusic()
        {
            /**
            setVolumeToMinimum();
            setVolumeToMinimum();
            setVolumeToMinimum();
            setVolumeToMinimum();
            setVolumeToMinimum();
            */
            Mute();
            Stop();
        }

    }
}
