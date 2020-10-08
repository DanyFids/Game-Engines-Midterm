using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformBehavior : MonoBehaviour
{
	public Transform[] path;
	public float speed = 2.0f;

	private float cur_vel;

	private bool back;
	private int cur_node;
	private float t;
	private bool move_player = false;
	private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
		cur_node = 0;
		back = false;
		t = 0.0f;

		player = GameObject.Find("Player");
    }

	private void OnTriggerEnter(Collider other)
	{
		move_player = true;
	}

	private void OnTriggerExit(Collider other)
	{
		move_player = false;
	}

	// Update is called once per frame
	void Update()
    {
		if (path.Length > 1)
		{
			Vector3 start = path[cur_node].position;
			Vector3 end;

			bool lock_move = move_player;

			if (back)
			{
				end = path[cur_node - 1].position;
			}
			else
			{
				end = path[cur_node + 1].position;
			}

			t = t + (speed / Vector3.Distance(start, end)) * Time.deltaTime;

			Vector3 cur_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

			transform.position = Vector3.Lerp(start, end, Mathf.Clamp(t, 0.0f, 1.0f));

			Vector3 new_pos = transform.position;

			Vector3 vel = new_pos - cur_pos;

			if (lock_move)
			{
				CharacterController c = player.GetComponent<CharacterController>();

				//OnTriggerExit(c);
				//c.enabled = false;
				//Vector3 n_pos = player.transform.position + vel;

				c.Move(vel);
				c.Move(new Vector3(0.0f, -0.01f, 0.0f));
				//player.transform.position = new Vector3(n_pos.x, n_pos.y, n_pos.z);
				//c.enabled = true;
				//OnTriggerExit(player.gameObject.GetComponent<Collider>());
				//OnTriggerEnter(player.gameObject.GetComponent<Collider>());
			}

			if (t >= 1.0f)
			{
				t = 0.0f;
				if (back)
				{
					cur_node--;
					if (cur_node == 0)
						back = false;
				}
				else
				{
					cur_node++;
					if (cur_node == path.Length - 1)
						back = true;
				}
			}
		}
    }
}
