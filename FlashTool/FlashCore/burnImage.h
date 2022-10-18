
//#include "stdafx.h"

#define MD5_LENGTH 16

int loadUpdater(const char *filename);
int loadUboot(const char *filename);
int RunScript(const char *filename);

typedef void(*OnFlashProgress)(int progress);

typedef void(*onFlashError)(int error);

void SetCallback(OnFlashProgress pCallback, onFlashError eCallback);