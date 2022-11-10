#pragma once
#include "header/ldc_verify_tool.h"
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <vector>


namespace ldc_verify_tool {
public ref class ldc_verify_tool
{
public:
	LdcVerifyConfig config;
	bool photo_test(cv::String path);
};
}


