using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_ScreenBehavior : MonoBehaviour
{
	public CheckpointTimerScript ch_controller;

	float total_time;
	List<float> check_times;

    // Start is called before the first frame update
    void Start()
    {
		check_times = ch_controller._GetCheckpointTimes();
		total_time = ch_controller._GetTotalTime();

		Text result_display = GetComponent<Text>();

		string display_text = result_display.text;

		display_text = display_text.Replace("$tot_time", timeToStr(total_time));

		float time_at = 0.0f;
		if (check_times.Count < 5)
		{
			for (int c = 0; c < 5 - check_times.Count; c++)
			{
				display_text = display_text + "Checkpoint N/A   " + " | RTBC: --:--:-- | Found at: --:--:--\n";
			}
		}

		int start = 0;

		if (check_times.Count > 5)
		{
			start = check_times.Count - 5;
		}

		for (int c = 0; c < check_times.Count; c++) {
			float RTBC = check_times[c];

			time_at += RTBC;

			if (c >= start)
			{
				if (c < check_times.Count - 1)
					display_text = display_text + "Checkpoint " + (c + 1).ToString() + " | RTBC:" + timeToStr(RTBC) + " | Found at: " + timeToStr(time_at) + "\n";
				else
					display_text = display_text + "Victory    " + " | RTBC:" + timeToStr(RTBC) + " | Found at: " + timeToStr(time_at) + "\n";
			}
		}

		result_display.text = display_text;
    }

	public string timeToStr(float t) {
		int time_hr = (int)Mathf.Floor((t / 60) / 60);
		t -= (time_hr * 60) * 60;
		int time_min = (int)Mathf.Floor(t / 60);
		t -= (time_min * 60);
		int time_sec = (int)Mathf.Floor(t);
		t -= time_sec;
		int time_milli = (int)Mathf.Floor(t * 100);

		string ret = "";

		if (time_milli > 0)
			ret = "." + time_milli;

		ret = time_sec.ToString("D2") + ret;

		if (time_min > 0)
			ret = time_min.ToString("D2") + ":" + ret;
		if (time_hr > 0)
		{
			if (time_min <= 0)
			{
				ret = time_min.ToString("D2") + ":" + ret;
			}

			ret = time_hr.ToString("D2") + ":" + ret;
		}

		return ret;
	}
}
