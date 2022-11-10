#ifndef __LDC_VERIFY_TOOL_H__
#define __LDC_VERIFY_TOOL_H__

#include <cstdio>
#include <cstdint>

#if defined(LDC_VERIFY_TOOL_BUILD_SHARED_LIBRARY) && defined(_WIN32)
#define API_FUNC __declspec(dllexport)
#define API_ENTRY __cdecl
#else
#define API_FUNC
#define API_ENTRY
#endif

struct LdcVerifyConfig
{
    int32_t roi_width = 900;
    int32_t num_board = 2;
    int32_t board_grid_width = 4;
    int32_t board_grid_height = 24;

    double thresh_max_grid_width_error = 0.020;
    double thresh_max_horizontal_adjacent_angle = 5.0;

    bool show_enabled = false; // for debug: show ploted image
    bool log_verbose = false;  // for debug: enable verbose log
};

class API_FUNC LdcVerifyTool
{
public:
    LdcVerifyTool() { }

    virtual ~LdcVerifyTool() { }

    virtual int32_t setConfig(const LdcVerifyConfig &config) = 0;

    virtual int32_t verify(const char *imagePath, bool &pass) = 0;

    static LdcVerifyTool* create();
};

#endif /* __LDC_VERIFY_TOOL_H__ */