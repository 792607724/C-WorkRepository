#include "ldc_verify_tool.h"

bool ldc_verify_tool::ldc_verify_tool::photo_test(cv::String path) {
	ldc_verify_tool::config.num_board = 2;
	ldc_verify_tool::config.board_grid_width = 4;
	ldc_verify_tool::config.board_grid_height = 24;
	ldc_verify_tool::config.show_enabled = true;
	ldc_verify_tool::config.log_verbose = true;
	int32_t ret = 0;
	bool pass = false;
	LdcVerifyTool ^tool = LdcVerifyTool::create();
	if (tool == nullptr) {
		return false;
	}
	ret = tool->setConfig(ldc_verify_tool::config);
	ret = tool->verify(path.c_str(), pass);
	return pass;
}