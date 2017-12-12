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
		Vector3i target = GetTarget(false);
		if (target != null) {
			this.debugObject.transform.position = target.Floatify ();

			//TODO: OPTIMIZE!!!!
			Vector3i target2 = GetTarget(true);
			Vector3 finalTarget = target2.Subtract (target).Floatify();
			//this.debugObject.transform.rotation = Quaternion.LookRotation(finalTarget);
			//this.debugObject.transform.rotation = Quaternion.Euler(target.floatify());
			//Vector3 finalTarget2 = new Vector3(finalTarget.z, finalTarget.y, -finalTarget.x);
			//Vector3 finalTarget2 = new Vector3(0.2f, 0, 0);
			this.debugObject.transform.rotation = Quaternion.LookRotation(finalTarget);
			this.debugObject.SetActive (true);
		}
		else
		{
			this.debugObject.SetActive (false);
		}

	}

	public Vector3i GetTarget(bool adding)
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
