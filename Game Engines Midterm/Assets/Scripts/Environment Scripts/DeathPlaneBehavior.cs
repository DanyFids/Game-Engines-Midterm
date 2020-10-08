using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlaneBehavior : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		PlayerScript p;
		//CharacterController c;

		if (p = other.gameObject.GetComponent<PlayerScript>())
		{
			p.Respawn();
		}

		//Debug.Log("HOI!");
	}
}
