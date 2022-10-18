using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoCapture
{
    public class CameraInfo
    {
        private Guid m_ClassID;

        private string m_Name;

        private string m_DevicePath;

        public Guid ClassID
        {
            get
            {
                return m_ClassID;
            }

            set
            {
                m_ClassID = value;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }

            set
            {
                m_Name = value;
            }
        }

        public string DevicePath
        {
            get
            {
                return m_DevicePath;
            }

            set
            {
                m_DevicePath = value;
            }
        }

        public CameraInfo(Guid classID, string name, string devicePath)
        {
            m_ClassID = classID;
            m_Name = name;
            m_DevicePath = devicePath;
        }
    }
}
