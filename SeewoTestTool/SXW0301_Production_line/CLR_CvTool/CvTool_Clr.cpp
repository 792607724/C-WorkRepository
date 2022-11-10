#include "CvTool_Clr.h"


NAME_LDC_VERIFY_TOOL::ldc_verify_tool::ldc_verify_tool() {};
NAME_LDC_VERIFY_TOOL::ldc_verify_tool::~ldc_verify_tool() {};

int NAME_LDC_VERIFY_TOOL::ldc_verify_tool::passorfail() {
	const cv::String imageDir = "data/board_2_4x24";
	std::vector<cv::String> imagePaths;
	cv::glob(imageDir + "/*.jpg", imagePaths,false);
	LdcVerifyConfig config;
	config.num_board = 2;
	config.board_grid_width = 4;
	config.board_grid_height = 24;
	config.show_enabled = true;
	config.log_verbose = true;

	if (imagePaths.empty()) {
		return 2;
	}
	int ret = 0;
	bool pass = false;
	if (ldc_object == NULL) {
		return 3;
	}
	ret = ldc_object->setConfig(config);
	if (ret == 0) {
		return 4;
	}

	for (int i = 0; i < (int)imagePaths.size(); i++) {
		const cv::String &imagePath = imagePaths[i];
		ret = ldc_object->verify(imagePath.c_str(), pass);
		if (ret == 0) {
			return 5;
		}
		if (config.show_enabled) {
			char key = cv::waitKey(0);
			if (key == 27) { // press 'ESC' to quit
				break;
			}
			//退出前一个窗口
			cv::destroyAllWindows();
		}
	}
	delete ldc_object;
	ldc_object = NULL;
	return 0;
}


