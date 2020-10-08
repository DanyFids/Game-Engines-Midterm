using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
	public GameObject parent;

	private float x_rot = 0.0f;
	private float y_rot = 0.0f;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update()
    {
		Transform transform = GetComponent<Transform>();
		float dt = Time.deltaTime;

		float mouse_x = Input.GetAxis("Mouse X");
		float mouse_y = Input.GetAxis("Mouse Y");

		y_rot += mouse_x;
		x_rot -= mouse_y;

		x_rot = Mathf.Clamp(x_rot, -90.0f, 90.0f);

		if (y_rot < -360.0f)
			y_rot += 360.0f;
		else if (y_rot > 360.0f)
			y_rot -= 360.0f;

		transform.localRotation = Quaternion.Euler(x_rot, 0.0f, 0.0f);
		parent.transform.localRotation = Quaternion.Euler(0.0f, y_rot, 0.0f);
	}

	public void ClearRot()
	{
		x_rot = 0.0f;
		y_rot = 0.0f;
	}

	public void SetRot(float x, float y)
	{
		x_rot = x;
		y_rot = y;
	}
}
