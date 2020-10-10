#include "Checkpoint.h"

void Checkpoint::ResetLogger()
{
	m_TRC = 0.0f;

	std::vector<float> tmp;

	m_RTBC.swap(tmp);
	tmp.clear();
}

void Checkpoint::SaveCheckpointTime(float RTBC)
{
	m_RTBC.push_back(RTBC);
	m_TRC += RTBC;
}

float Checkpoint::GetTotalTime()
{
	return m_TRC;
}

float Checkpoint::GetCheckpointTime(int index)
{
	if (index >= 0 && index < GetNumCheckpoints())
		return m_RTBC.at(index);
	else
		return -1.0f;
}

int Checkpoint::GetNumCheckpoints()
{
	return m_RTBC.size();
}
