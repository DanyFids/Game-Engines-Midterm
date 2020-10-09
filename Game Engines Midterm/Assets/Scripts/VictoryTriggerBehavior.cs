using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTriggerBehavior : MonoBehaviour
{
	public SceneManagerScript SceneManager;
	public GameObject Player;
	public CheckpointTimerScript checkpoint_manager;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == Player)
		{
			checkpoint_manager._SaveCheckpointTime();

			SceneManager.MoveToResults();
		}
	}
}
