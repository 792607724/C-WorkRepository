#include "FlashUtils.h"

extern "C" __declspec(dllexport) int __stdcall nativeInit(OnFlashProgress pCallback, onFlashError eCallback)
{
	SetCallback(pCallback, eCallback);
	return 0;
}

extern "C" __declspec(dllexport) void* __stdcall nativeScsiFindDevice(int* deviceState)
{
	return USB_ScsiFindDevice(deviceState);
}

extern "C" __declspec(dllexport) int __stdcall nativeFlashUpdater(char* path)
{
	return loadUpdater(path);
}

extern "C" __declspec(dllexport) int __stdcall nativeFlashUBoot(char* path)
{
	return loadUboot(path);
}

extern "C" __declspec(dllexport) int __stdcall nativeFlashSystem(char* path)
{
	USB_ScsiRunCmd("setenv ota_upgrade_status 0", strlen("setenv ota_upgrade_status 0"));
	return RunScript(path);
}