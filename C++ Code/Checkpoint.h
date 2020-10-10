#pragma once

#include "PlugginSettings.h";

#include <vector>

class PLUGIN_API Checkpoint
{
private:
	float m_TRC;
	std::vector<float> m_RTBC;
public:
	void ResetLogger();

	void SaveCheckpointTime(float RTBC);

	float GetTotalTime();
	float GetCheckpointTime(int index);
	int GetNumCheckpoints();
};

