using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	public float PLAYER_SPEED = 5.0f;
	public float PLAYER_JUMP_HEIGHT = 1.0f;

	private CharacterController controller;
	private CameraBehavior cam_behavior;
	private Vector3 player_velocity;
	private bool isOnGround = false;

	public Transform spawn_point;

	// Start is called before the first frame update
	void Start()
	{
		controller = GetComponent<CharacterController>();
		cam_behavior = transform.Find("Main Camera").GetComponent<CameraBehavior>();
		player_velocity = new Vector3();
		//Physics.gravity = new Vector3(0.0f, -1.0f, 0.0f);
	}

	// Update is called once per frame
	void Update()
	{
		float dt = Time.deltaTime;

		player_velocity.x = 0.0f;
		player_velocity.z = 0.0f;
		if (controller.isGrounded && player_velocity.y < 0.0f)
		{
			player_velocity.y = 0.0f;
		}


		if (Input.GetKey(KeyCode.W))
		{
			player_velocity += transform.forward * PLAYER_SPEED;
		}
		if (Input.GetKey(KeyCode.A))
		{
			player_velocity += -transform.right * PLAYER_SPEED;
		}
		if (Input.GetKey(KeyCode.S))
		{
			player_velocity += -transform.forward * PLAYER_SPEED;
		}
		if (Input.GetKey(KeyCode.D))
		{
			player_velocity += transform.right * PLAYER_SPEED;
		}
		if (isOnGround && Input.GetKeyDown(KeyCode.Space))
		{
			player_velocity.y += Mathf.Sqrt(PLAYER_JUMP_HEIGHT * -3.0f * Physics.gravity.y);
		}

		float cur_y = transform.position.y;

		player_velocity += Physics.gravity * dt;
		CollisionFlags hit = controller.Move(player_velocity * dt);

		float new_y = transform.position.y;

		if (isOnGround && (cur_y == new_y))
		{
			isOnGround = true;
		}
		else
		{
			isOnGround = controller.isGrounded;
		}
	}

	public void SetSpawnPoint(Transform new_spawn)
	{
		spawn_point = new_spawn;
	}

	public void Respawn() {
		controller.enabled = false;
		transform.position = spawn_point.position;
		cam_behavior.SetRot(0.0f, spawn_point.rotation.ToEuler().y);
		controller.enabled = true;
	}
}
