using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VideoCapture
{
    public class VideoCapturer : IDisposable
    {
        private const int WM_GRAPHNOTIFY = 0x8000 + 1;

        private CameraInfo m_CameraInfo;
        private IVideoWindow m_VideoWindow = null;
        private IMediaControl m_MediaControl = null;
        private IMediaEventEx m_MediaEventEx = null;
        private IGraphBuilder m_GraphBuilder = null;
        private ICaptureGraphBuilder2 m_CaptureGraphBuilder = null;
        private DsROTEntry m_Rot = null;
        private int m_PreviewWidth = 0;
        private int m_PreviewHeight = 0;
        private IntPtr m_DisplayWindow = IntPtr.Zero;
        private int m_DisplayWidth = 0;
        private int m_DisplayHeight = 0;

        public VideoCapturer()
        {
        }

        public void SetPreviewSize(int width, int height)
        {
            m_PreviewWidth = width;
            m_PreviewHeight = height;
        }

        public void SetDisplayWindow(IntPtr window)
        {
            m_DisplayWindow = window;
        }

        public void SetDisplaySize(int width, int height)
        {
            m_DisplayWidth = width;
            m_DisplayHeight = height;
        }

        public bool StartupCapture(CameraInfo cameraInfo, int cameraId)
        {
            int hr = 0;
            if (0 == m_PreviewWidth || 0 == m_PreviewHeight || IntPtr.Zero == m_DisplayWindow || 0 == m_DisplayWidth || 0 == m_DisplayHeight)
            {
                return false;
            }
            m_CameraInfo = cameraInfo;

            OpenInterfaces();

            hr = m_CaptureGraphBuilder.SetFiltergraph(m_GraphBuilder);
            DsError.ThrowExceptionForHR(hr);

            IBaseFilter sourceFilter = GetCaptureDevice(cameraInfo, cameraId);

            hr = m_GraphBuilder.AddFilter(sourceFilter, "Video Capture");
            DsError.ThrowExceptionForHR(hr);

            SetConfigParams(m_CaptureGraphBuilder, sourceFilter, 30, m_PreviewWidth, m_PreviewHeight);

            hr = m_CaptureGraphBuilder.RenderStream(PinCategory.Preview, MediaType.Video, sourceFilter, null, null);
            DsError.ThrowExceptionForHR(hr);

            Marshal.ReleaseComObject(sourceFilter);

            SetupVideoWindow();

            m_Rot = new DsROTEntry(m_GraphBuilder);

            hr = m_MediaControl.Run();
            DsError.ThrowExceptionForHR(hr);
            return true;
        }

        public void StopCapture()
        {
            CloseInterfaces();
        }

        public void Dispose()
        {
            StopCapture();
        }

        public static List<CameraInfo> GetCameraInfos()
        {
            List<CameraInfo> cameraInfos = new List<CameraInfo>();

            DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            for (int i = 0; i < devices.Length; i++)
            {
                cameraInfos.Add(new CameraInfo(devices[i].ClassID, devices[i].Name, devices[i].DevicePath));
            }
            return cameraInfos;
        }

        private void OpenInterfaces()
        {
            int hr = 0;

            m_GraphBuilder = (IGraphBuilder)new FilterGraph();
            m_CaptureGraphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            m_MediaControl = (IMediaControl)m_GraphBuilder;
            m_VideoWindow = (IVideoWindow)m_GraphBuilder;
            m_MediaEventEx = (IMediaEventEx)m_GraphBuilder;

            hr = m_MediaEventEx.SetNotifyWindow(m_DisplayWindow, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);
        }

        private IBaseFilter GetCaptureDevice(CameraInfo cameraInfo, int cameraId)
        {
            object source;

            DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            List<DsDevice> allTargetDevices = new List<DsDevice>();
            DsDevice device = null;
            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i].Name == cameraInfo.Name)
                {
                    //device = devices[i];
                    //break;
                    allTargetDevices.Add(devices[i]);
                }
            }
            if (cameraId >= 0 && cameraId < allTargetDevices.Count)
            {
                device = allTargetDevices[cameraId];
            }

            Guid iid = typeof(IBaseFilter).GUID;
            device.Mon.BindToObject(null, null, ref iid, out source);

            return (IBaseFilter)source;
        }

        private void SetupVideoWindow()
        {
            int hr = 0;

            hr = m_VideoWindow.put_Owner(m_DisplayWindow);
            DsError.ThrowExceptionForHR(hr);

            hr = m_VideoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren);
            DsError.ThrowExceptionForHR(hr);

            ResizeVideoWindow();

            hr = m_VideoWindow.put_Visible(OABool.True);
            DsError.ThrowExceptionForHR(hr);
        }

        public void ResizeVideoWindow()
        {
            if (m_VideoWindow != null)
            {
                m_VideoWindow.SetWindowPosition(0, 0, m_DisplayWidth, m_DisplayHeight + 1);
            }
        }

        private void SetConfigParams(ICaptureGraphBuilder2 capGraph, IBaseFilter capFilter, int iFrameRate, int iWidth, int iHeight)
        {
            int hr;
            object config;
            AMMediaType media_type = null;
            IntPtr pSCC = IntPtr.Zero;

            hr = capGraph.FindInterface(PinCategory.Capture, MediaType.Video, capFilter, typeof(IAMStreamConfig).GUID, out config);

            IAMStreamConfig videoStreamConfig = config as IAMStreamConfig;
            if (videoStreamConfig == null)
            {
                throw new Exception("Failed to get IAMStreamConfig");
            }

            int piCount = 0;
            int piSize = 0;

            hr = videoStreamConfig.GetNumberOfCapabilities(out piCount, out piSize);
            DsError.ThrowExceptionForHR(hr);

            for (int i = 0; i < piCount; i++)
            {
                pSCC = Marshal.AllocCoTaskMem(piSize);
                videoStreamConfig.GetStreamCaps(i, out media_type, pSCC);
                if (IntPtr.Zero != pSCC)
                {
                    Marshal.FreeCoTaskMem(pSCC);
                    pSCC = IntPtr.Zero;
                }
                VideoInfoHeader videoInfoHeader = new VideoInfoHeader();
                Marshal.PtrToStructure(media_type.formatPtr, videoInfoHeader);

                if (videoInfoHeader.BmiHeader.Width == iWidth && videoInfoHeader.BmiHeader.Height == iHeight && 
                    media_type.subType == MediaSubType.MJPG)
                {
                    hr = videoStreamConfig.SetFormat(media_type);
                    DsError.ThrowExceptionForHR(hr);
                    break;
                }
                DsUtils.FreeAMMediaType(media_type);
            }
            DsUtils.FreeAMMediaType(media_type);

        }

        private void CloseInterfaces()
        {
            if (m_MediaControl != null)
                m_MediaControl.StopWhenReady();

            if (m_MediaEventEx != null)
                m_MediaEventEx.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero);

            if (m_VideoWindow != null)
            {
                m_VideoWindow.put_Visible(OABool.False);
                m_VideoWindow.put_Owner(IntPtr.Zero);
            }

            if (m_Rot != null)
            {
                m_Rot.Dispose();
                m_Rot = null;
            }

            if (m_MediaControl != null)
            {
                Marshal.ReleaseComObject(m_MediaControl);
                m_MediaControl = null;
            }

            if (m_MediaEventEx != null)
            {
                Marshal.ReleaseComObject(m_MediaEventEx);
                m_MediaEventEx = null;
            }
            
            if (m_VideoWindow != null)
            {
                Marshal.ReleaseComObject(m_VideoWindow);
                m_VideoWindow = null;
            }
            
            if (m_GraphBuilder != null)
            {
                Marshal.ReleaseComObject(m_GraphBuilder);
                m_GraphBuilder = null;
            }
            
            if (m_CaptureGraphBuilder != null)
            {
                Marshal.ReleaseComObject(m_CaptureGraphBuilder);
                m_CaptureGraphBuilder = null;
            }
        }
    }
}
