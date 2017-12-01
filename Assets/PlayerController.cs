using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float translationSpeed;
	public float rotationSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 translation = new Vector3 ();
		float inputVertical = Input.GetAxis("Vertical");
		translation.z = inputVertical * Time.deltaTime * this.translationSpeed;

		Vector3 rotation = new Vector3 ();
		float inputMouseX = Input.GetAxis("Mouse X");
		rotation.y = inputMouseX * Time.deltaTime * this.rotationSpeed;

		Transform transform = GetComponent<Transform>();
		transform.Translate(translation);
		transform.Rotate(transform.up, rotation.y);
	}
}
