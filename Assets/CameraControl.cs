using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	private float pitchInput = 0;
	private Transform transform;

	void Start()
	{
		this.transform = GetComponent<Transform>();
	}

	void Update ()
	{
		this.pitchInput = Input.GetAxis("Mouse Y");
		float angularVelocity = this.transform.parent.GetComponent<PlayerController> ().angularVelocity;

		transform.rotation = Quaternion.AngleAxis (pitchInput * Time.deltaTime * angularVelocity, Vector3.right);
	}
}
