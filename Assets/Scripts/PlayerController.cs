using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float translationSpeed;
	public float angularVelocity;
	public float jumpForce;
	public LayerMask ground;

	private Quaternion targetRotation;
	private Vector3 targetVelocity;

	private float walkInput = 0;
	private float strafeInput = 0;
	private float turnInput = 0;
	private float jumpInput = 0;
	private Vector3 gravity = Physics.gravity;

	private Transform transform;
	private Rigidbody rigidBody;
//	private CapsuleCollider collider;

	void Start()
	{
		this.transform = GetComponent<Transform>();
		this.rigidBody = GetComponent<Rigidbody>();
//		this.collider = GetComponent<CapsuleCollider>();
		this.targetRotation = this.transform.rotation;
		this.targetVelocity = new Vector3();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update()
	{
		this.walkInput = Input.GetAxis("Walk");
		this.strafeInput = Input.GetAxis("Strafe");
		this.turnInput = Input.GetAxis("Mouse X");
		this.jumpInput = Input.GetAxis("Jump");
		Turn();

	}

	void FixedUpdate()
	{
		Walk();
		Jump();

		this.rigidBody.velocity = this.rigidBody.transform.TransformDirection(this.targetVelocity);
	}

	private void Turn()
	{
		this.targetRotation *= Quaternion.AngleAxis (turnInput * Time.deltaTime * this.angularVelocity, Vector3.up);
		transform.rotation = this.targetRotation;
	}

	private void Walk()
	{
		if (this.walkInput != 0 || this.strafeInput != 0)
		{
			if (IsGrounded ())
			{
				this.targetVelocity.z = walkInput * this.translationSpeed;
				this.targetVelocity.x = strafeInput * this.translationSpeed;
			}
			else
			{
				this.targetVelocity.z = walkInput * this.translationSpeed;
				this.targetVelocity.x = strafeInput * this.translationSpeed;
			}
		}
		else
		{
			this.targetVelocity.z = 0;
			this.targetVelocity.x = 0;
		}
	}

	private void Jump()
	{
		if (IsGrounded())
		{
			if (this.jumpInput > 0)
			{
				//jump
				this.targetVelocity.y = this.jumpForce;
			}
			else
			{
				//added vel = 0
				this.targetVelocity.y = 0;
			}
		}
		else
		{
			//fall
			this.targetVelocity.y += this.gravity.y * Time.deltaTime;
		}
	}

	//TODO: smarter raycasting, this gets stuck ok walls
	private bool IsGrounded()
	{
		return Physics.Raycast(this.transform.position, Vector3.down, 1.1f, this.ground);
//		return Physics.CheckCapsule
//		(
//			this.collider.bounds.center,
//			new Vector3 (this.collider.bounds.center.x, this.collider.bounds.min.y, this.collider.bounds.center.z),
//			this.collider.radius * 0.9f,
//			this.ground
//		);
//		return Physics.CapsuleCast
//			(
//				this.collider.bounds.center,
//				new Vector3 (this.collider.bounds.center.x, this.collider.bounds.min.y, this.collider.bounds.center.z),
//				this.collider.radius * 1.1f,
//				Vector3.down,
//				0.5f,
//				this.ground
//			);
	}

}
