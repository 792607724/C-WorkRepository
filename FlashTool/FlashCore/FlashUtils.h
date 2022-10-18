#ifndef __FLASH_UTILS_H__
#define __FLASH_UTILS_H__

#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>
#include <tchar.h>
#include <Windows.h>
#include "usbscsicmd.h"
#include "burnImage.h"

HANDLE findScsiDevice(TCHAR *diskname, int *dev_state);


#endif /* __FLASH_UTILS_H__ */