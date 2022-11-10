#pragma once
#include "header/ldc_verify_tool.h"
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/highgui/highgui.hpp>

namespace NAME_LDC_VERIFY_TOOL {
	public ref class ldc_verify_tool {
	public:
		ldc_verify_tool();
		~ldc_verify_tool();
		LdcVerifyTool * ldc_object;
		int passorfail();
	};
}





