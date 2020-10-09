using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CheckpointTimerScript : MonoBehaviour
{
	const string PLUGIN_DLL = "Engines_DLL_Midterm_F2020_DANIEL_FINDLETON";

	[DllImport(PLUGIN_DLL)]
	private static extern void ResetLogger();

	[DllImport(PLUGIN_DLL)]
	private static extern void SaveCheckpointTime(float RTBC);

	[DllImport(PLUGIN_DLL)]
	private static extern float GetTotalTime();

	[DllImport(PLUGIN_DLL)]
	private static extern float GetCheckpointTime(int index);

	[DllImport(PLUGIN_DLL)]
	private static extern int GetNumCheckpoints();

	private float last_time = 0.0f;

	public void _SaveCheckpointTime()
	{
		float new_time = Time.time;
		float delta = new_time - last_time;

		last_time = new_time;

		SaveCheckpointTime(delta);
	}

	public List<float> _GetCheckpointTimes()
	{
		List<float> ret = new List<float>();
		int num_checkpoints = GetNumCheckpoints();

		for (int c = 0; c < num_checkpoints; c++) {
			float t = GetCheckpointTime(c);

			if (t == -1.0f)
				break;

			ret.Add(t);
		}

		return ret;
	}

	public float _GetTotalTime()
	{
		return GetTotalTime();
	}

	public void _Reset()
	{
		ResetLogger();
	}

	void Start()
    {
		last_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	//private void OnDestroy()
	//{
	//	_Reset();
	//}
}
