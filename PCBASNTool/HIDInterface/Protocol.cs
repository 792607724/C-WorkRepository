using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIDInterface
{
    public class Protocol
    {

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_CAM_VERSION = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x06, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_CAM_VERSION = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x07, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_REBOOT_BOOTLOADER = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x08, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_REBOOT_BOOTLOADER = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x09, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_CAMERA_MODEL = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x0A, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_CAMERA_MODEL = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x0B, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_UPGRADE_METHOD = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x0C, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_UPGRADE_METHOD = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x0D, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_OTA_KEY = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x0E, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_OTA_KEY = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x0F, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_EPTZ_SUPPORTED = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x90, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_EPTZ_SUPPORTED = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x91, 0x01, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_EPTZ_ENABLE = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x94, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SET_EPTZ_ENABLE = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x95, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_EPTZ_ENABLE = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x92, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_EPTZ_ENABLE = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x93, 0x01, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_DEVICE_NAME = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x84, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_DEVICE_NAME = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x85, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_CPU_TEMP = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x88, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_CPU_TEMP = new byte[] { 0xAA, 0xBB, 0xCC, 0x20, 0x89, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_SOUND_LOCATION = new byte[] { 0xAA, 0xBB, 0xCC, 0x21, 0x00, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SOUND_LOCATION = new byte[] { 0xAA, 0xBB, 0xCC, 0x21, 0x01, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_APP_VERSION = new byte[] { 0xAA, 0xBB, 0xCC, 0x22, 0x00, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_APP_VERSION = new byte[] { 0xAA, 0xBB, 0xCC, 0x22, 0x01, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_CAPTURE_SUPPORTED = new byte[] { 0xAA, 0xBB, 0xCC, 0x23, 0x00, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_CAPTURE_SUPPORTED = new byte[] { 0xAA, 0xBB, 0xCC, 0x23, 0x01, 0x01, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_HDR_SETTING = new byte[] { 0xAA, 0xBB, 0xCC, 0x24, 0x00, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_HDR_SETTING = new byte[] { 0xAA, 0xBB, 0xCC, 0x24, 0x01, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_CAPTURE_PREVIEW_ENABLE = new byte[] { 0xAA, 0xBB, 0xCC, 0x25, 0x00, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_CAPTURE_PREVIEW_ENABLE = new byte[] { 0xAA, 0xBB, 0xCC, 0x25, 0x01, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_AUTO_REBOOT = new byte[] { 0xAA, 0xBB, 0xCC, 0x30, 0x00, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_AUTO_REBOOT = new byte[] { 0xAA, 0xBB, 0xCC, 0x30, 0x01, 0x00, 0x00 };
    }
}
