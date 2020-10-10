#pragma once

#include "Checkpoint.h"

#ifdef __cplusplus
extern "C"{
#endif
	//Put your functions here

	/**********************************
	*		Checkpoint Timer
	**********************************/
	PLUGIN_API void ResetLogger();
	
	PLUGIN_API void SaveCheckpointTime(float RTBC);

	PLUGIN_API float GetTotalTime();

	PLUGIN_API float GetCheckpointTime(int index);

	PLUGIN_API float GetNumCheckpoints();

#ifdef __cplusplus
}
#endif

