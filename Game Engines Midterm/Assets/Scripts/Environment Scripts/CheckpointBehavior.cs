using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour
{
	private static bool PLUGIN_CLEARED = false;

	public Material active_mat;
	public CheckpointTimerScript checkpoint_timer_control;
	public GameObject Player;

	public ScoreBehavior score_controller;

	private GameObject o_light;
	private GameObject o_spawn;
	private bool active = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == Player) {

			if (!active)
			{
				o_light.SetActive(true);
				transform.Find("Indicator").GetComponent<Renderer>().material = active_mat;
				active = true;

				checkpoint_timer_control._SaveCheckpointTime();
				float cur_time = checkpoint_timer_control._GetTotalTime();
				Debug.Log("Checkpoint Saved at: " + cur_time.ToString());

				score_controller.BoostScore();
			}

			Player.GetComponent<PlayerScript>().SetSpawnPoint(o_spawn.transform);
		}
	}

	// Start is called before the first frame update
	void Start()
    {
		if (!PLUGIN_CLEARED)
		{
			checkpoint_timer_control._Reset();
			PLUGIN_CLEARED = true;
		}

		o_light = transform.Find("Light").gameObject;
		o_spawn = transform.Find("Spawn").gameObject;
		o_light.SetActive(false);
		o_spawn.GetComponent<Renderer>().enabled = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnDestroy()
	{
		PLUGIN_CLEARED = false;
	}
}
