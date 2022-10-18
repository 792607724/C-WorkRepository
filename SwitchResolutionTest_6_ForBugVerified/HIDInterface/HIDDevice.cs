using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HIDInterface
{
    public class HIDDeviceInfo
    {
        public string manufacturer;
        public string product;
        public string serialNumber;
        public ushort VID;
        public ushort PID;
        public string devicePath;
        public int IN_reportByteLength;
        public int OUT_reportByteLength;
        public ushort versionNumber;
    }

    public class HIDDevice
    {
        private const int DIGCF_DEFAULT = 0x1;
        private const int DIGCF_PRESENT = 0x2;
        private const int DIGCF_ALLCLASSES = 0x4;
        private const int DIGCF_PROFILE = 0x8;
        private const int DIGCF_DEVICEINTERFACE = 0x10;
        private const short FILE_ATTRIBUTE_NORMAL = 0x80;
        private const short INVALID_HANDLE_VALUE = -1;
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;
        private const uint CREATE_NEW = 1;
        private const uint CREATE_ALWAYS = 2;
        private const uint OPEN_EXISTING = 3;

        [StructLayout(LayoutKind.Sequential)]
        private struct HIDP_CAPS
        {
            public System.UInt16 Usage;
            public System.UInt16 UsagePage;
            public System.UInt16 InputReportByteLength;
            public System.UInt16 OutputReportByteLength;
            public System.UInt16 FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public System.UInt16[] Reserved;
            public System.UInt16 NumberLinkCollectionNodes;
            public System.UInt16 NumberInputButtonCaps;
            public System.UInt16 NumberInputValueCaps;
            public System.UInt16 NumberInputDataIndices;
            public System.UInt16 NumberOutputButtonCaps;
            public System.UInt16 NumberOutputValueCaps;
            public System.UInt16 NumberOutputDataIndices;
            public System.UInt16 NumberFeatureButtonCaps;
            public System.UInt16 NumberFeatureValueCaps;
            public System.UInt16 NumberFeatureDataIndices;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public Guid InterfaceClassGuid;
            public uint Flags;
            public IntPtr Reserved;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HIDD_ATTRIBUTES
        {
            public Int32 Size;
            public Int16 VendorID;
            public Int16 ProductID;
            public Int16 VersionNumber;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct COMMTIMEOUTS
        {
            public UInt32 ReadIntervalTimeout;
            public UInt32 ReadTotalTimeoutMultiplier;
            public UInt32 ReadTotalTimeoutConstant;
            public UInt32 WriteTotalTimeoutMultiplier;
            public UInt32 WriteTotalTimeoutConstant;
        }

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetupDiGetClassDevs(
            ref Guid ClassGuid,
            IntPtr Enumerator,
            IntPtr hwndParent,
            uint Flags);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern void HidD_GetHidGuid(ref Guid hidGuid);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiEnumDeviceInterfaces(
            IntPtr hDevInfo,
            IntPtr devInfo,
            ref Guid interfaceClassGuid,
            UInt32 memberIndex,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
           ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
           UInt32 deviceInterfaceDetailDataSize,
           out UInt32 requiredSize,
           ref SP_DEVINFO_DATA deviceInfoData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess,
            uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition,
            uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadFile(SafeFileHandle hFile, byte[] lpBuffer,
           uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteFile(SafeFileHandle hFile, byte[] lpBuffer,
           uint nNumberOfBytesToWrite, ref uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern bool HidD_GetPreparsedData(
            SafeFileHandle hObject,
            ref IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern Boolean HidD_FreePreparsedData(ref IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern int HidP_GetCaps(
            IntPtr pPHIDP_PREPARSED_DATA,
            ref HIDP_CAPS myPHIDP_CAPS);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern Boolean HidD_GetAttributes(SafeFileHandle hObject, ref HIDD_ATTRIBUTES Attributes);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool HidD_GetFeature(
           IntPtr hDevice,
           IntPtr hReportBuffer,
           uint ReportBufferLength);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool HidD_SetFeature(
           IntPtr hDevice,
           IntPtr ReportBuffer,
           uint ReportBufferLength);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool HidD_GetProductString(
           SafeFileHandle hDevice,
           IntPtr Buffer,
           uint BufferLength);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool HidD_GetSerialNumberString(
           SafeFileHandle hDevice,
           IntPtr Buffer,
           uint BufferLength);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean HidD_GetManufacturerString(
            SafeFileHandle hDevice,
            IntPtr Buffer,
            uint BufferLength);

        private SafeFileHandle m_Handle;
        private HIDDeviceInfo m_ProductInfo;
        private HIDP_CAPS m_Capabilities;
        private FileStream m_Stream;
        private int m_InputReportByteLength;
        private int m_OutputReportByteLength;

        public int InputReportByteLength
        {
            get
            {
                return m_InputReportByteLength;
            }

            set
            {
                m_InputReportByteLength = value;
            }
        }

        public int OutputReportByteLength
        {
            get
            {
                return m_OutputReportByteLength;
            }

            set
            {
                m_OutputReportByteLength = value;
            }
        }

        public static HIDDevice Open(HIDDeviceInfo deviceInfo)
        {
            HIDDevice device = new HIDDevice();

            device.m_Handle = CreateFile(deviceInfo.devicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE,
                IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

            IntPtr ptrToPreParsedData = new IntPtr();
            bool ppdSucsess = HidD_GetPreparsedData(device.m_Handle, ref ptrToPreParsedData);

            device.m_Capabilities = new HIDP_CAPS();
            int hidCapsSucsess = HidP_GetCaps(ptrToPreParsedData, ref device.m_Capabilities);

            HIDD_ATTRIBUTES attributes = new HIDD_ATTRIBUTES();
            bool hidAttribSucsess = HidD_GetAttributes(device.m_Handle, ref attributes);

            string productName = "";
            string SN = "";
            string manfString = "";
            IntPtr buffer = Marshal.AllocHGlobal(126);//max alloc for string; 
            if (HidD_GetProductString(device.m_Handle, buffer, 126)) productName = Marshal.PtrToStringAuto(buffer);
            if (HidD_GetSerialNumberString(device.m_Handle, buffer, 126)) SN = Marshal.PtrToStringAuto(buffer);
            if (HidD_GetManufacturerString(device.m_Handle, buffer, 126)) manfString = Marshal.PtrToStringAuto(buffer);
            Marshal.FreeHGlobal(buffer);

            HidD_FreePreparsedData(ref ptrToPreParsedData);
            if (device.m_Handle.IsInvalid)
                return null;

            device.m_ProductInfo = new HIDDeviceInfo();
            device.m_ProductInfo.devicePath = deviceInfo.devicePath;
            device.m_ProductInfo.manufacturer = manfString;
            device.m_ProductInfo.product = productName;
            device.m_ProductInfo.serialNumber = SN.Equals("") ? generateRandomSN() : SN;// Convert.ToInt32(SN);
            device.m_ProductInfo.PID = (ushort)attributes.ProductID;
            device.m_ProductInfo.VID = (ushort)attributes.VendorID;
            device.m_ProductInfo.versionNumber = (ushort)attributes.VersionNumber;
            device.m_ProductInfo.IN_reportByteLength = (int)device.m_Capabilities.InputReportByteLength;
            device.m_ProductInfo.OUT_reportByteLength = (int)device.m_Capabilities.OutputReportByteLength;
            device.m_InputReportByteLength = (int)device.m_Capabilities.InputReportByteLength;
            device.m_OutputReportByteLength = (int)device.m_Capabilities.OutputReportByteLength;

            device.m_Stream = new FileStream(device.m_Handle, FileAccess.ReadWrite, device.m_Capabilities.OutputReportByteLength, false);

            return device;
        }

        public void Send(byte[] data, int offset, int count)
        {
            if (data.Length > m_Capabilities.OutputReportByteLength)
            {
                throw new Exception("Output report must not exceed " + (m_Capabilities.OutputReportByteLength - 1).ToString() + " bytes");
            }

            if (m_Stream.CanWrite)
            {
                m_Stream.Write(data, 0, data.Length);
            }
            else
            {
                throw new Exception("Stream can not write");
            }
        }

        public void Receive(byte[] data, int offset, int size)
        {
            if (data.Length > m_Capabilities.InputReportByteLength)
            {
                throw new Exception("Input report must not exceed " + (m_Capabilities.InputReportByteLength - 1).ToString() + " bytes");
            }

            if (m_Stream.CanRead)
            {
                m_Stream.Read(data, 0, data.Length);
            }
            else
            {
                throw new Exception("Stream can not read");
            }
        }

        public void Close()
        {
            if (m_Stream != null)
            {
                m_Stream.Close();
            }
            if ((m_Handle != null) && (!(m_Handle.IsInvalid)))
            {
                m_Handle.Close();
            }
        }

        private static string generateRandomSN()
        {
            string sn = "";
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                sn += random.Next(10);
            }
            return sn;
        }

        //private static readonly string[] SYSTEM_SN =
        //{
        //    "abcdefghijk",
        //    "01234567890",
        //    "abc12345678",
        //    "test1234567",
        //    "kk112233445",
        //    "abcd1234565",
        //    "00000111111",
        //    "11111222222",
        //    "22222333333",
        //    "44444555555",
        //};

        public static HIDDeviceInfo[] GetHIDDeviceInfos()
        {
            HIDDeviceInfo[] devices = new HIDDeviceInfo[0];

            SP_DEVINFO_DATA devInfo = new SP_DEVINFO_DATA();
            SP_DEVICE_INTERFACE_DATA devIface = new SP_DEVICE_INTERFACE_DATA();
            devInfo.cbSize = (uint)Marshal.SizeOf(devInfo);
            devIface.cbSize = (uint)(Marshal.SizeOf(devIface));

            Guid G = new Guid();
            HidD_GetHidGuid(ref G);

            IntPtr i = SetupDiGetClassDevs(ref G, IntPtr.Zero, IntPtr.Zero, DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);

            SP_DEVICE_INTERFACE_DETAIL_DATA didd = new SP_DEVICE_INTERFACE_DETAIL_DATA();
            if (IntPtr.Size == 8)
            {
                didd.cbSize = 8;
            }
            else
            {
                didd.cbSize = 4 + Marshal.SystemDefaultCharSize;
            }

            int j = -1;
            bool b = true;
            int error;
            SafeFileHandle tempHandle;

            while (b)
            {
                j++;

                b = SetupDiEnumDeviceInterfaces(i, IntPtr.Zero, ref G, (uint)j, ref devIface);
                error = Marshal.GetLastWin32Error();
                if (b == false)
                    break;

                uint requiredSize = 0;
                bool b1 = SetupDiGetDeviceInterfaceDetail(i, ref devIface, ref didd, 256, out requiredSize, ref devInfo);
                string devicePath = didd.DevicePath;

                tempHandle = CreateFile(devicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE,
                    IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

                IntPtr ptrToPreParsedData = new IntPtr();
                bool ppdSucsess = HidD_GetPreparsedData(tempHandle, ref ptrToPreParsedData);
                if (ppdSucsess == false)
                    continue;

                HIDP_CAPS capabilities = new HIDP_CAPS();
                int hidCapsSucsess = HidP_GetCaps(ptrToPreParsedData, ref capabilities);

                HIDD_ATTRIBUTES attributes = new HIDD_ATTRIBUTES();
                bool hidAttribSucsess = HidD_GetAttributes(tempHandle, ref attributes);

                string productName = "";
                string SN = "";
                string manfString = "";
                IntPtr buffer = Marshal.AllocHGlobal(126);
                if (HidD_GetProductString(tempHandle, buffer, 126)) productName = Marshal.PtrToStringAuto(buffer);
                if (HidD_GetSerialNumberString(tempHandle, buffer, 126)) SN = Marshal.PtrToStringAuto(buffer);
                if (HidD_GetManufacturerString(tempHandle, buffer, 126)) manfString = Marshal.PtrToStringAuto(buffer);
                Marshal.FreeHGlobal(buffer);

                HidD_FreePreparsedData(ref ptrToPreParsedData);

                HIDDeviceInfo productInfo = new HIDDeviceInfo();
                productInfo.devicePath = devicePath;
                productInfo.manufacturer = manfString;
                productInfo.product = productName;
                productInfo.PID = (ushort)attributes.ProductID;
                productInfo.VID = (ushort)attributes.VendorID;
                productInfo.versionNumber = (ushort)attributes.VersionNumber;
                productInfo.IN_reportByteLength = (int)capabilities.InputReportByteLength;
                productInfo.OUT_reportByteLength = (int)capabilities.OutputReportByteLength;

                //if (ConvertStringToInteger(SN))
                //if (SN.Equals(""))
                //{
                //    SN = SYSTEM_SN[devices.Length];
                //}
                productInfo.serialNumber = SN;//.Equals("") ? generateRandomSN() : SN;// 0;//Convert.ToInt32(SN);

                int newSize = devices.Length + 1;
                Array.Resize(ref devices, newSize);
                devices[newSize - 1] = productInfo;
            }
            SetupDiDestroyDeviceInfoList(i);

            return devices;
        }

        private static bool ConvertStringToInteger(string val)
        {
            Double result;
            return Double.TryParse(val, System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }
    }
}
