using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour
{
	public Material active_mat;
	public CheckpointTimerScript checkpoint_timer_control;
	public GameObject Player;

	private GameObject o_light;
	private GameObject o_spawn;
	private bool active = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == Player && !active) {
			o_light.SetActive(true);
			transform.Find("Indicator").GetComponent<Renderer>().material = active_mat;
			active = true;

			Player.GetComponent<PlayerScript>().SetSpawnPoint(o_spawn.transform);

			checkpoint_timer_control._SaveCheckpointTime();
			float cur_time = checkpoint_timer_control._GetTotalTime();
			Debug.Log("Checkpoint Saved at: " + cur_time.ToString());
		}
	}

	// Start is called before the first frame update
	void Start()
    {
		o_light = transform.Find("Light").gameObject;
		o_spawn = transform.Find("Spawn").gameObject;
		o_light.SetActive(false);
		o_spawn.GetComponent<Renderer>().enabled = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
