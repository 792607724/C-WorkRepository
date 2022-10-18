using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlashTool
{
    public class FlashUtils
    {
        public static readonly int DeviceStateRom = 0;
        public static readonly int DeviceStateUpdater = 1;
        public static readonly int DeviceStateUBoot = 2;
        public static readonly int DeviceStateUnknown = 3;
        public static readonly int DeviceStateEnd = 4;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnFlashProgress(int progress);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void onFlashError(int error);

        [DllImport("FlashCore.dll")]
        private static extern int nativeInit(OnFlashProgress pCallback, onFlashError eCallback);

        [DllImport("FlashCore.dll")]
        private static extern IntPtr nativeScsiFindDevice(int[] deviceState);

        [DllImport("FlashCore.dll")]
        private static extern int nativeFlashUpdater(string binPath);

        [DllImport("FlashCore.dll")]
        private static extern int nativeFlashUBoot(string binPath);

        [DllImport("FlashCore.dll")]
        private static extern int nativeFlashSystem(string binPath);

        private int[] m_DeviceState = new int[1];

        private OnFlashProgress m_OnProgressCallback;

        private onFlashError m_OnErrorCallback;

        public FlashUtils()
        {
        }

        public void setProgressCallback(OnFlashProgress callback)
        {
            m_OnProgressCallback = callback;
        }

        public void setErrorCallback(onFlashError callback)
        {
            m_OnErrorCallback = callback;
        }

        public void init()
        {
            nativeInit(m_OnProgressCallback, m_OnErrorCallback);
        }

        public bool ScanScsiDevice()
        {
            if (IntPtr.Zero != nativeScsiFindDevice(m_DeviceState))
            {
                return true;
            }
            return false;
        }

        public bool flashUpdater(string path)
        {
            if (0 == nativeFlashUpdater(path))
            {
                m_DeviceState[0] = DeviceStateUpdater;
                return true;
            }
            return false;
        }

        public bool flashUboot(string path)
        {
            if (0 == nativeFlashUBoot(path))
            {
                Thread.Sleep(1000);

                while (true)
                {
                    Thread.Sleep(500);
                    if (IntPtr.Zero != nativeScsiFindDevice(m_DeviceState))
                    {
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        public bool flashSystem(string path)
        {
            if (0 == nativeFlashSystem(path))
            {
                m_DeviceState[0] = DeviceStateEnd;
                return true;
            }
            return false;
        }

        public int GetDeviceState()
        {
            return m_DeviceState[0];
        }

    }
}
