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

		//target display
		Vector3i target = getTarget(true);
		if (target != null)
		{
			this.debugObject.transform.position = target.floatify();
		}

	}

	public Vector3i getTarget(bool adding)
	{
		Vector3i result = null;

		RaycastHit raycastHit;
		bool rayHit = Physics.Raycast (this.transform.position, this.transform.forward, out raycastHit, 5.0f); //set distance
		if (rayHit)
		{
			Vector3 hitNormal = raycastHit.normal;
			Vector3 hitPoint;
			if (adding)
			{
				hitPoint = raycastHit.point + hitNormal * 0.2f;
			}
			else
			{
				hitPoint = raycastHit.point - hitNormal * 0.2f;
			}

			result = new Vector3i ((int)Mathf.Round(hitPoint.x),(int)Mathf.Round(hitPoint.y), (int)Mathf.Round(hitPoint.z));
		}

		return result;
	}

}
