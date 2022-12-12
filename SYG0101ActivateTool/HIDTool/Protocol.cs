﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIDTool
{
    public class Protocol
    {
        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_CAM_VERSION = { 0xAA, 0xBB, 0xCC, 0x20, 0x06, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_CAM_VERSION = { 0xAA, 0xBB, 0xCC, 0x20, 0x07, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_REBOOT_BOOTLOADER = { 0xAA, 0xBB, 0xCC, 0x20, 0x08, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_REBOOT_BOOTLOADER = { 0xAA, 0xBB, 0xCC, 0x20, 0x09, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_CAMERA_MODEL = { 0xAA, 0xBB, 0xCC, 0x20, 0x0A, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_CAMERA_MODEL = { 0xAA, 0xBB, 0xCC, 0x20, 0x0B, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_UPGRADE_METHOD = { 0xAA, 0xBB, 0xCC, 0x20, 0x0C, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_UPGRADE_METHOD = { 0xAA, 0xBB, 0xCC, 0x20, 0x0D, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_OTA_KEY = { 0xAA, 0xBB, 0xCC, 0x20, 0x0E, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_OTA_KEY = { 0xAA, 0xBB, 0xCC, 0x20, 0x0F, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_EPTZ_SUPPORTED = { 0xAA, 0xBB, 0xCC, 0x20, 0x90, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_EPTZ_SUPPORTED = { 0xAA, 0xBB, 0xCC, 0x20, 0x91, 0x01, 0x00 };
 
        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_EPTZ_ENABLE =  { 0xAA, 0xBB, 0xCC, 0x20, 0x92, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_EPTZ_ENABLE = { 0xAA, 0xBB, 0xCC, 0x20, 0x93, 0x01, 0x00 };
  
        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_EPTZ_ENABLE =  { 0xAA, 0xBB, 0xCC, 0x20, 0x94, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SET_EPTZ_ENABLE = { 0xAA, 0xBB, 0xCC, 0x20, 0x95, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_EPTZ_MODE =  { 0xAA, 0xBB, 0xCC, 0x20, 0x96, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SET_EPTZ_MODE = { 0xAA, 0xBB, 0xCC, 0x20, 0x97, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_EPTZ_MODE =  { 0xAA, 0xBB, 0xCC, 0x20, 0xB0, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_EPTZ_MODE = { 0xAA, 0xBB, 0xCC, 0x20, 0xB1, 0x01, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_ENLARGE_NARROW_EPTZ_SIZE = { 0xAA, 0xBB, 0xCC, 0x20, 0x98, 0x02, 0x00 };
        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_MOVE_EPTZ =                { 0xAA, 0xBB, 0xCC, 0x20, 0x9A, 0x02, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_DEVICE_NAME = { 0xAA, 0xBB, 0xCC, 0x20, 0x84, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_DEVICE_NAME = { 0xAA, 0xBB, 0xCC, 0x20, 0x85, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_CPU_TEMP = { 0xAA, 0xBB, 0xCC, 0x20, 0x88, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_CPU_TEMP = { 0xAA, 0xBB, 0xCC, 0x20, 0x89, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_SOUND_LOCATION = { 0xAA, 0xBB, 0xCC, 0x21, 0x00, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SOUND_LOCATION = { 0xAA, 0xBB, 0xCC, 0x21, 0x01, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_APP_VERSION = { 0xAA, 0xBB, 0xCC, 0x22, 0x00, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_APP_VERSION = { 0xAA, 0xBB, 0xCC, 0x22, 0x01, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_CAPTURE_SUPPORTED = { 0xAA, 0xBB, 0xCC, 0x23, 0x00, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_CAPTURE_SUPPORTED = { 0xAA, 0xBB, 0xCC, 0x23, 0x01, 0x01, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_HDR_SETTING = { 0xAA, 0xBB, 0xCC, 0x24, 0x00, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_HDR_SETTING = { 0xAA, 0xBB, 0xCC, 0x24, 0x01, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_CAPTURE_PREVIEW_ENABLE = { 0xAA, 0xBB, 0xCC, 0x25, 0x00, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_CAPTURE_PREVIEW_ENABLE = { 0xAA, 0xBB, 0xCC, 0x25, 0x01, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_AUTO_REBOOT = { 0xAA, 0xBB, 0xCC, 0x30, 0x00, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_AUTO_REBOOT = { 0xAA, 0xBB, 0xCC, 0x30, 0x01, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_START_RECORD_LOG = { 0xAA, 0xBB, 0xCC, 0x60, 0x00, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_START_RECORD_LOG = { 0xAA, 0xBB, 0xCC, 0x60, 0x01, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_STOP_RECORD_LOG = { 0xAA, 0xBB, 0xCC, 0x61, 0x00, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_STOP_RECORD_LOG = { 0xAA, 0xBB, 0xCC, 0x61, 0x01, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_RECORD_LOG_SIZE = { 0xAA, 0xBB, 0xCC, 0x62, 0x00, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_RECORD_LOG_SIZE = { 0xAA, 0xBB, 0xCC, 0x62, 0x01, 0x04, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_RECORD_LOG = { 0xAA, 0xBB, 0xCC, 0x63, 0x01, 0x04, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_RECORD_LOG = { 0xAA };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_CPU_FREQ = { 0xAA, 0xBB, 0xCC, 0x88, 0x00, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_CPU_FREQ = { 0xAA, 0xBB, 0xCC, 0x88, 0x01, 0x01, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_IQ_VERSION = { 0xAA, 0xBB, 0xCC, 0x89, 0x00, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_IQ_VERSION = { 0xAA, 0xBB, 0xCC, 0x89, 0x01, 0x01, 0x00 };


        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_AFMACRO_ENABLE = { 0xAA, 0xBB, 0xCC, 0x90, 0x00, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SET_AFMACRO_ENABLE = { 0xAA, 0xBB, 0xCC, 0x90, 0x01, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_AFMACRO_ENABLE = { 0xAA, 0xBB, 0xCC, 0x90, 0x02, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_AFMACRO_ENABLE = { 0xAA, 0xBB, 0xCC, 0x90, 0x03, 0x01, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_FOV_MODE = { 0xAA, 0xBB, 0xCC, 0x90, 0x0E, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SET_FOV_MODE  = { 0xAA, 0xBB, 0xCC, 0x90, 0x0F, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_FOV_MODE  = { 0xAA, 0xBB, 0xCC, 0x90, 0x10, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_FOV_MODE = { 0xAA, 0xBB, 0xCC, 0x90, 0x11, 0x01, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_TENCENT_CERTIFI_MODE = { 0xAA, 0xBB, 0xCC, 0x90, 0x12, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SET_TENCENT_CERTIFI_MODE = { 0xAA, 0xBB, 0xCC, 0x90, 0x13, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_AUDIO_VERSION = { 0xAA, 0xBB, 0xCC, 0x90, 0x60, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_AUDIO_VERSION = { 0xAA, 0xBB, 0xCC, 0x90, 0x61, 0x05, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_AUDIO_RMS = { 0xAA, 0xBB, 0xCC, 0x90, 0x68, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_AUDIO_RMS = { 0xAA, 0xBB, 0xCC, 0x90, 0x69, 0x0a, 0x00 };




        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_FOCUS_MODE = { 0xAA, 0xBB, 0xCC, 0x90, 0x70, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SET_FOCUS_MODE = { 0xAA, 0xBB, 0xCC, 0x90, 0x71, 0x01, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_FOCUS_LOCATION = { 0xAA, 0xBB, 0xCC, 0x90, 0x72, 0x02, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SET_FOCUS_LOCATION = { 0xAA, 0xBB, 0xCC, 0x90, 0x73, 0x02, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_FOCUS_LOCATION = { 0xAA, 0xBB, 0xCC, 0x90, 0x74, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_FOCUS_LOCATION = { 0xAA, 0xBB, 0xCC, 0x90, 0x75, 0x02, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_OUTPUT_BLACK_PIC = { 0xAA, 0xBB, 0xCC, 0x20, 0xB6, 0x01, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_OUTPUT_BLACK_PIC = { 0xAA, 0xBB, 0xCC, 0x20, 0xB7, 0x01, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_OUTPUT_BLACK_PIC = { 0xAA, 0xBB, 0xCC, 0x20, 0xB8, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_OUTPUT_BLACK_PIC_ACK = { 0xAA, 0xBB, 0xCC, 0x20, 0xB9, 0x01, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_FUNC_LIST = { 0xAA, 0xBB, 0xCC, 0x31, 0xfe, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_FUNC_LIST_ACK = { 0xAA, 0xBB, 0xCC, 0x31, 0xff, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_SN = { 0xAA, 0xBB, 0xCC, 0x90, 0x18, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_SN = { 0xAA, 0xBB, 0xCC, 0x90, 0x19, 0x11, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_SN = { 0xAA, 0xBB, 0xCC, 0x90, 0x16, 0x12, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SET_SN = { 0xAA, 0xBB, 0xCC, 0x90, 0x17, 0x0, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_PCBASN = { 0xAA, 0xBB, 0xCC, 0x90, 0x1C, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_PCBASN = { 0xAA, 0xBB, 0xCC, 0x90, 0x1D, 0x13, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_PCBASN = { 0xAA, 0xBB, 0xCC, 0x90, 0x1A, 0x14, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SET_PCBASN = { 0xAA, 0xBB, 0xCC, 0x86, 0x1B, 0x0, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_GET_UUID = { 0xAA, 0xBB, 0xCC, 0x90, 0x76, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_GET_UUID_ACK = { 0xAA, 0xBB, 0xCC, 0x90, 0x77, 0x00, 0x00 };

        public static readonly byte[] COMMAND_REQUEST_TYPE_SET_UUID_CIPHERTEXT = { 0xAA, 0xBB, 0xCC, 0x90, 0x78, 0x00, 0x00 };
        public static readonly byte[] COMMAND_RESPONSE_TYPE_SET_UUID_CIPHERTEXT_ACK = { 0xAA, 0xBB, 0xCC, 0x90, 0x79, 0x01, 0x00 };

    }

}
