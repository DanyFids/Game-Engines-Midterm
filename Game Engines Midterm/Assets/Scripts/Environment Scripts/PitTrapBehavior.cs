using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrapBehavior : MonoBehaviour
{
	Vector3 velocity;

	bool active = false;

	private void OnTriggerEnter(Collider other)
	{
		active = true;
	}

	// Update is called once per frame
	void Update()
    {
		float dt = Time.deltaTime;

		if (active)
		{
			velocity += Physics.gravity * dt;

			transform.position += velocity * dt;
		}
    }
}
