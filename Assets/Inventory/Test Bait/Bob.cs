using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour {
	public float rate = 1.0f;
	public float amplitude = 0.2f;

	private Vector3 originalPosition;
	private float angle = 0.0f;


	void Start()
	{
		this.originalPosition = transform.localPosition;
	}

	void Update ()
	{
		this.angle += Time.deltaTime * this.rate;
		Vector3 bobPosition = new Vector3 (0, Mathf.Sin (angle) * this.amplitude, 0);
		transform.localPosition = this.originalPosition + bobPosition;
	}
}
