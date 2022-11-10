#ifndef __MI_CALIBRATION_H__
#define __MI_CALIBRATION_H__

#include <stdint.h>

typedef void *MI_CALIB_Handle_t;
typedef unsigned long long int MI_CALIB_PhyAddr_t;

typedef public struct MI_CALIB_Conf_t
{
    int camNum;
    int camPairNum;
    int camLensType;
    int imgWidth;
    int imgHeight;
    int inputType;
    int intrBoardWidth;
    int intrBoardHeight;
    int intrBoardSize;
    int extrBoardWidth;
    int extrBoardHeight;
    int extrBoardSize;

    int blendType;
    int mapProjType;
    int mapCropType;
    int checkCalibMode;
    int fastCalibMode;
    int productCalibMode;

    // DPU
    int edge_width;
    int edge_height;
    int tile_width;
    int tile_StepX;
    int min_d;
    int num_d;

    // map-gen
    int orgWidth;
    int orgHeight;
    int scaleWidth;
    int scaleHeight;
    int in_seq_mode;
    int out_seq_mode;
    int src_mode;
    int hw_mode;
	int seprate_bin_mode;
    char camRotation[200];
	int gridSize;
    int preCropType;

    char ptguiPath[200];
    char imgPath[200];
    char paraPath[200];
    char outputPath[200];
    char moduleName[200];
    char imgFormat[200];
    char imgPrefix[200];
    char imgDepthList[200];
    char camIDList[200];
    char camPairIDList[2][200];
    char camCenterID[200];
    char rotateAngleList[200];
    char blendWidthPercentList[200];

    char camOffsetX[200];
    char camOffsetY[200];
    char imgEdgeTop[200];
    char imgEdgeBottom[200];
    char imgEdgeLeft[200];
    char imgEdgeRight[200];
    char calibOutPath[300];
    char configPath[200];

    int VerticalNum;

    int autoCalcMode;
    int FOVX;
    int FOVY;
    int prjCenterX;
    int prjCenterY;
    int yaw;
    int pitch;
    int roll;
} MI_CALIB_Conf;

typedef public enum
{
    E_MI_CALIB_SUCCESS = 0,
    E_MI_CALIB_INVALID_PARA = 1,
    E_MI_CALIB_ERROR = 2
} MI_CALIB_RetCode_e;

typedef public enum
{
    E_MI_CALIB_BUFTYPE_NONCONTINUED = 0,
    E_MI_CALIB_BUFTYPE_CONTINUED = 1
} MI_CALIB_BufType_e;

typedef public struct
{
    void *pBufAddr;
    MI_CALIB_PhyAddr_t phyAddr;
    uint32_t u32BufSz;
    MI_CALIB_BufType_e eBufType;
}MI_CALIB_BufBlock_t;

typedef MI_CALIB_RetCode_e (*MI_CALIB_Malloc_t)(MI_CALIB_BufBlock_t *pstBufBlock);
typedef MI_CALIB_RetCode_e (*MI_CALIB_Free_t)(MI_CALIB_BufBlock_t *pstBufBlock);
typedef MI_CALIB_RetCode_e (*MI_CALIB_Flush_t)(MI_CALIB_BufBlock_t *pstBufBlock);

typedef public struct
{
    MI_CALIB_Malloc_t CVMalloc;
    MI_CALIB_Free_t CVFree;
    MI_CALIB_Flush_t CVFlush;
}MI_CALIB_CallBack_t;

typedef public struct
{
    MI_CALIB_CallBack_t stCVCb;
}MI_CALIB_InitPara_t;

MI_CALIB_RetCode_e MI_CALIB_Init(MI_CALIB_Handle_t *pAlgHandle,MI_CALIB_InitPara_t *pstInitPara);
MI_CALIB_RetCode_e MI_CALIB_Deinit(MI_CALIB_Handle_t algHandle);

MI_CALIB_RetCode_e MI_CALIB_Calibration(MI_CALIB_Handle_t algHandle, MI_CALIB_Conf *pMI_Conf, MI_CALIB_BufBlock_t *pstInJsonBuf, MI_CALIB_BufBlock_t *pstOutJsonBuf, MI_CALIB_BufBlock_t *pcalibErrBuf);
MI_CALIB_RetCode_e MI_CALIB_FineTune(MI_CALIB_Handle_t algHandle, const char *confPath, MI_CALIB_BufBlock_t *pstInJsonBuf, MI_CALIB_BufBlock_t *pstCalibJsonBuf, int *stateflag);

#endif