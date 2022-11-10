#ifndef __MI_CV_HEADER_H__
#define __MI_CV_HEADER_H__

#include <stdint.h>

/* 支持的最大的blending层数 */
#define MAX_BLENDING_LAYER 6
#define MI_CV_FALSE 0
#define MI_CV_TRUE  1

// mapgen info
#ifndef MAX_CAMERA_NUM
#define MAX_CAMERA_NUM 10
#endif

#ifndef MAX_CAMERA_PAIR_NUM
#define MAX_CAMERA_PAIR_NUM 10
#endif

typedef public void *MI_CV_Handle_t;
typedef public unsigned long long int MI_CV_PhyAddr_t;
typedef public unsigned char MI_CV_BOOL;
typedef public unsigned int MI_CV_U32;

typedef public struct MI_CV_Intr_t
{
    float K[3][3];
    float D[14];
    float P[3][3];
} MI_CV_Intr; /*DO NOT EDIT*/

typedef public struct MI_CV_Extr_t
{
    float R[3][3];
    float T[3];
    float depth;
} MI_CV_Extr; /*DO NOT EDIT*/

typedef public struct MI_CV_Map_Gen_Conf_t
{
    int moduleType;  /*DO NOT EDIT*/
    int camNum;      /*DO NOT EDIT*/
    int camPairNum;  /*DO NOT EDIT*/
    int imgWidth;    /*DO NOT EDIT*/
    int imgHeight;   /*DO NOT EDIT*/
    int inputType;   /*DO NOT EDIT*/
    int mapProjType; /*DO NOT EDIT*/
    int mapCropType; /*DO NOT EDIT*/
    int gridSize;    /*DO NOT EDIT*/

    // DPU
    int edge_width;  /*DO NOT EDIT*/
    int edge_height; /*DO NOT EDIT*/
    int tile_width;  /*DO NOT EDIT*/
    int tile_StepX;  /*DO NOT EDIT*/
    int min_d;       /*DO NOT EDIT*/
    int num_d;       /*DO NOT EDIT*/

    // map-gen
    int scaleWidth;        /*DO NOT EDIT*/
    int scaleHeight;       /*DO NOT EDIT*/
    int in_seq_mode;       /*DO NOT EDIT*/
    int out_seq_mode;      /*DO NOT EDIT*/
    int src_mode;          /*DO NOT EDIT*/
    int hw_mode;           /*DO NOT EDIT*/
    int seprate_bin_mode;  /*DO NOT EDIT*/
    int preCropType;       /*DO NOT EDIT*/
    char camRotation[200]; /*DO NOT EDIT*/

    //char moduleName[200];
    char inputPath[200];       /*DO NOT EDIT*/
    char outputPath[200];      /*DO NOT EDIT*/
    char configPath[200];      /*DO NOT EDIT*/
    char camList[200];         /*DO NOT EDIT*/
    char camPairList[2][200];  /*DO NOT EDIT*/
    char camOffsetX[200];      /*DO NOT EDIT*/
    char camOffsetY[200];      /*DO NOT EDIT*/
    char imgEdgeTop[200];      /*DO NOT EDIT*/
    char imgEdgeBottom[200];   /*DO NOT EDIT*/
    char imgEdgeLeft[200];     /*DO NOT EDIT*/
    char imgEdgeRight[200];    /*DO NOT EDIT*/
    // char imgDepth[200];        /*DO NOT EDIT*/
    char rotateAngleList[200]; /*DO NOT EDIT*/

    float arrOptimalFocal;         /*DO NOT EDIT*/
    char layerNumList[200];         /*DO NOT EDIT*/
    int arrLayerEnable[MAX_CAMERA_PAIR_NUM][6];         /*DO NOT EDIT*/
    float arrLayerWidthPercent[MAX_CAMERA_PAIR_NUM][6]; /*DO NOT EDIT*/
    int seamFunnel;
    int debug_blend;
    int ovp_max_width;
    int ovp_capacity;

    int autoCalcMode;
    int FOVX;
    int FOVY;
    int prjCenterX;
    int prjCenterY;
    int yaw;
    int pitch;
    int roll;

    MI_CV_Intr arrIntr[MAX_CAMERA_NUM]; /*DO NOT EDIT*/
    MI_CV_Extr arrExtr[MAX_CAMERA_NUM]; /*DO NOT EDIT*/

    int s32_arrimgDepth[10];            /*DO NOT EDIT*/
    int VerticalNum;                    /*DO NOT EDIT*/
} MI_CV_Map_Gen_Conf;

/* 算法库的返回码，需要算法库自己补充 */
typedef public enum
{
    E_MI_CV_SUCCESS = 0,
    E_MI_CV_INVALID_PARA = 1,
    E_MI_CV_ERROR = 2
} MI_CV_RetCode_e;

/* 内存块的类型 */
typedef public enum
{
    E_MI_CV_BUFTYPE_NONCONTINUED = 0, /* 非连续内存，离线算法库下只支持非连续内存 */
    E_MI_CV_BUFTYPE_CONTINUED = 1     /* 连续内存 */
} MI_CV_BufType_e;

/* 算法库使用的内存块结构体 */
typedef public struct
{
    void *pBufAddr;           /* 内存块的虚拟地址 */
    MI_CV_PhyAddr_t phyAddr;  /* 内存块地址物理地址，在离线库中该参数无效 */
    MI_CV_U32 u32BufSz;       /* 内存块的大小 */
    MI_CV_BufType_e eBufType; /* 内存块类型，在离线库中该参数无效 */
} MI_CV_BufBlock_t;

/* 申请和释放内存的函数指针，会由算法库外部实现，然后将函数指针传给算法库 */
typedef MI_CV_RetCode_e (*MI_CV_Malloc_t)(MI_CV_BufBlock_t *pstBufBlock);
typedef MI_CV_RetCode_e (*MI_CV_Free_t)(MI_CV_BufBlock_t *pstBufBlock);
typedef MI_CV_RetCode_e (*MI_CV_Flush_t)(MI_CV_BufBlock_t *pstBufBlock);

/* 回调函数接口 */
typedef public struct
{
    MI_CV_Malloc_t CVMalloc; /* 分配内存块的回调函数 */
    MI_CV_Free_t CVFree;     /* 释放内存块的回调函数 */
    MI_CV_Flush_t CVFlush;
} MI_CV_CallBack_t;

/* 算法库的初始化参数设置 */
typedef public struct
{
    MI_CV_CallBack_t stCVCb;
} MI_CV_InitPara_t;

/* 矩形框的描述信息 */
typedef public struct
{
    MI_CV_U32 x;              /* 矩形的起始点x坐标 */
    MI_CV_U32 y;              /* 矩形的起始点y坐标 */
    MI_CV_U32 w;              /* 矩形的宽度 */
    MI_CV_U32 h;              /* 矩形的高度 */
}MI_CV_Box_t;

/* blending 的层选信息 */
typedef public struct
{
    MI_CV_BOOL bUsed;
    MI_CV_U32 u32blendWidth;
}MI_CV_BlendLayerInfo_t;


/* mask table的描述信息 */
typedef public struct
{
    MI_CV_U32 w;                                              /* mask table的宽度 */
    MI_CV_U32 h;                                              /* mask table的高度 */
    MI_CV_U32 offset;                                         /* mask table在输出的bin中的偏移地址 */
    MI_CV_U32 size;                                           /* mask table的大小 */
    MI_CV_U32 blendedLayers[MAX_BLENDING_LAYER];              /* 层选开关， alpha blending只会用第0层， multi band blending可以使用多层 */
}MI_CV_MaskTableInfo_t;


/* blend region的描述信息 */
typedef public struct
{
    MI_CV_Box_t stDstRegion;              /* 目标blending区域 */
    MI_CV_Box_t stSrcRegion;              /* 重叠blending区域 */
    MI_CV_MaskTableInfo_t stMaskTableInfo;/* mask table */
}MI_CV_BlendRegionPair_t;


/* blend的类型 */
typedef public enum
{
    E_BLEND_TYPE_ALPHA = 0,            /* alpha blending */
    E_BLEND_TYPE_MULTIBAND = 1,        /* multiband blending */
    E_BLEND_TYPE_MAX
}MI_CV_BlendType_e;

/* blend的描述信息 */
typedef public struct
{
    MI_CV_U32 blendInfoSize;                 /* 这个blend info bin的总大小 */
    MI_CV_BlendType_e blendType;             /* blend类型 */
    MI_CV_Box_t stTargetRegion;              /* blending之后，保留的目标区域的尺寸 */
    MI_CV_Box_t stOverlapRegion;			 /* overlap图像的尺寸 */
    MI_CV_U32 blendRegionPairNum;            /* 需要blend的区域的个数 */
    MI_CV_BlendRegionPair_t stBlendRegions[0];/* blending的区域对，根据blengding的区域个数长度可变 */
}MI_CV_BlendInfo_t;


/* CV的动态参数 */
typedef public struct
{
    MI_CV_U32 u32CVDistance; /* CV的拼接距离 */
} MI_CV_DynamicPara_t;

typedef public struct MI_CV_Img_Scale_Q15_t
{
    int width;
    int height;
}MI_CV_Img_Scale_Q15;

typedef public struct MI_CV_Camera_Q15_t
{
    int camNum;
    MI_CV_Img_Scale_Q15 *inputScale;
    MI_CV_Img_Scale_Q15 *outputScale;
}MI_CV_Camera_Q15;

#ifdef __cplusplus
extern "C"
{
#endif

MI_CV_RetCode_e MI_CV_Init(MI_CV_Handle_t *pAlgHandle, MI_CV_InitPara_t *pstInitPara);
MI_CV_RetCode_e MI_CV_Deinit(MI_CV_Handle_t algHandle);

// map gen
MI_CV_RetCode_e MI_CV_ParseStaticPara(MI_CV_BufBlock_t *pstInJsonBuf, MI_CV_Map_Gen_Conf *pMI_conf);
MI_CV_RetCode_e MI_CV_GenMap(MI_CV_Handle_t algHandle, MI_CV_Map_Gen_Conf *pMI_conf);
MI_CV_RetCode_e MI_CV_GenBIN(MI_CV_Handle_t algHandle, MI_CV_BufBlock_t *Bin_1, MI_CV_BufBlock_t *Bin_2, MI_CV_BufBlock_t *Bin_3);
MI_CV_RetCode_e MI_CV_GenMapBin(MI_CV_Handle_t algHandle, MI_CV_Map_Gen_Conf *pMI_conf, MI_CV_BufBlock_t *Bin_1, MI_CV_BufBlock_t *Bin_2, MI_CV_BufBlock_t *Bin_3);

MI_CV_RetCode_e MI_CV_GenStitchInfo(MI_CV_Handle_t algHandle, MI_CV_BufBlock_t *StitchBin, MI_CV_BufBlock_t *OverLapBin);
MI_CV_RetCode_e MI_CV_GenDPUInfo(MI_CV_Handle_t algHandle,MI_CV_BufBlock_t *pstDPUBinLeft, MI_CV_BufBlock_t *pstDPUBinRight);
MI_CV_RetCode_e MI_CV_GenNirInfo(MI_CV_Handle_t algHandle, MI_CV_BufBlock_t *pstNirBinLeft, MI_CV_BufBlock_t *pstNirBinRight);

// blending
MI_CV_RetCode_e MI_CV_BlendingTable(MI_CV_Handle_t algHandle, MI_CV_BufBlock_t *pstBlendInfo);
MI_CV_RetCode_e MI_CV_PrintStitchInfo(MI_CV_BufBlock_t *pstBlendInfo);

// dump message
MI_CV_RetCode_e MI_CV_GetCameraInfo(MI_CV_Handle_t algHandle, MI_CV_Camera_Q15 *CameraPara);
MI_CV_RetCode_e MI_CV_CameraFree(MI_CV_Camera_Q15 *CameraPara);

void MI_CV_FreeBufBlock(MI_CV_BufBlock_t *pstInJsonBuf);

#ifdef __cplusplus
}
#endif

#endif
