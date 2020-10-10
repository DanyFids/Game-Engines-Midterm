using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemBehavior : MonoBehaviour
{
	Vector3 rotation = new Vector3();
    // Update is called once per frame
    void Update()
    {
		rotation += (new Vector3(20.5f, 20.5f, -10.5f)) * Time.deltaTime;

		transform.localRotation = Quaternion.Euler(rotation);
    }
}
