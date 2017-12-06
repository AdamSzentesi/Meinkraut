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
	private CapsuleCollider collider;

	void Start()
	{
		this.transform = GetComponent<Transform>();
		this.rigidBody = GetComponent<Rigidbody>();
		this.collider = GetComponent<CapsuleCollider>();
		this.targetRotation = this.transform.rotation;
		this.targetVelocity = new Vector3();
	}
	
	void Update()
	{
		this.walkInput = Input.GetAxis("Vertical");
		this.strafeInput = Input.GetAxis("Horizontal");
		this.turnInput = Input.GetAxis("Mouse X");
		this.jumpInput = Input.GetAxis("Jump");
		turn();
		inventory();
	}

	void FixedUpdate()
	{
		walk();
		jump();

		this.rigidBody.velocity = this.rigidBody.transform.TransformDirection(this.targetVelocity);
	}

	void turn()
	{
		this.targetRotation *= Quaternion.AngleAxis (turnInput * Time.deltaTime * this.angularVelocity, Vector3.up);
		transform.rotation = this.targetRotation;
	}

	void inventory()
	{
//		if (Input.GetKeyDown (KeyCode.Q)){this.GetComponent<Inventory> ().previousItem();}
//		if (Input.GetKeyDown (KeyCode.E)){this.GetComponent<Inventory> ().nextItem();}
//		if (Input.GetKeyDown (KeyCode.Keypad1)){this.GetComponent<Inventory> ().selectItem (0);}
//		if (Input.GetKeyDown (KeyCode.Keypad2)){this.GetComponent<Inventory> ().selectItem (1);}
//		if (Input.GetKeyDown (KeyCode.Keypad3)){this.GetComponent<Inventory> ().selectItem (2);}
//		if (Input.GetKeyDown (KeyCode.Keypad4)){this.GetComponent<Inventory> ().selectItem (3);}
	}

	void walk()
	{
		if (this.walkInput != 0 || this.strafeInput != 0)
		{
			if (isGrounded ())
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

	private void jump()
	{
		if (isGrounded())
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
	private bool isGrounded()
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
