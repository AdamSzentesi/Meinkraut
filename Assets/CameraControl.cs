using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	private float pitchInput = 0;
	private Transform transform;

	public GameObject debugObject;

	void Start()
	{
		this.transform = GetComponent<Transform>();
	}

	void Update ()
	{
		this.pitchInput = Input.GetAxis("Mouse Y");
		float angularVelocity = this.transform.parent.GetComponent<PlayerController> ().angularVelocity;

		transform.rotation *= Quaternion.AngleAxis (pitchInput * Time.deltaTime * angularVelocity, Vector3.right);

		//Matrix4x4 worldMatrix = this.transform.localToWorldMatrix;
		Vector3 worldPosition = this.transform.position;
		Vector3 worldDirection = this.transform.forward;

		RaycastHit raycastHit;
		bool rayHit = Physics.Raycast (worldPosition, worldDirection, out raycastHit, 5.0f);
		if (rayHit)
		{
			Vector3 hitNormal = raycastHit.normal;
			Vector3 hitPoint = raycastHit.point - hitNormal * 0.2f;
			Vector3 intPosition = new Vector3 (Mathf.Round(hitPoint.x),Mathf.Round(hitPoint.y), Mathf.Round(hitPoint.z));
			this.debugObject.transform.position = intPosition;
		}

	}
}
