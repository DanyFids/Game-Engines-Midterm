#include "Wrapper.h"

Checkpoint cp_timer;

PLUGIN_API void ResetLogger()
{
	return cp_timer.ResetLogger();
}

PLUGIN_API void SaveCheckpointTime(float RTBC)
{
	return cp_timer.SaveCheckpointTime(RTBC);
}

PLUGIN_API float GetTotalTime()
{
	return cp_timer.GetTotalTime();
}

PLUGIN_API float GetCheckpointTime(int index)
{
	return cp_timer.GetCheckpointTime(index);
}

PLUGIN_API float GetNumCheckpoints()
{
	return cp_timer.GetNumCheckpoints();
}
