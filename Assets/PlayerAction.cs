using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
	public Camera camera;
	private CameraControl cameraControl;

	private Inventory inventory;
	private World world;
	private float actionInput = 0;

	void Start()
	{
		this.inventory = GetComponent<Inventory> ();
		this.cameraControl = this.camera.GetComponent<CameraControl> ();
	}

	void Update()
	{
		this.actionInput = Input.GetAxis("Fire1");

		if (this.actionInput > 0)
		{
			Vector3i target = this.cameraControl.getTarget ();
			if (target != null)
			{
				int diggedblock = this.world.dig (target);
				if (diggedblock > 0)
				{
					GameObject gameObject = new GameObject ();
					gameObject.AddComponent<InventoryItem> ().type = diggedblock;
					InventoryItem newItem = gameObject.GetComponent<InventoryItem> ();
					newItem.type = diggedblock;
					//newItem.sprite
					addItem (newItem);
				}
			}

		}

		if (Input.GetKeyDown (KeyCode.Q)){this.inventory.previousItem();}
		if (Input.GetKeyDown (KeyCode.E)){this.inventory.nextItem();}
		//		if (Input.GetKeyDown (KeyCode.Keypad1)){this.GetComponent<Inventory> ().selectItem (0);}
		//		if (Input.GetKeyDown (KeyCode.Keypad2)){this.GetComponent<Inventory> ().selectItem (1);}
		//		if (Input.GetKeyDown (KeyCode.Keypad3)){this.GetComponent<Inventory> ().selectItem (2);}
		//		if (Input.GetKeyDown (KeyCode.Keypad4)){this.GetComponent<Inventory> ().selectItem (3);}
	}

	public void OnTriggerEnter(Collider trigger)
	{
		if (trigger.gameObject.CompareTag ("Item"))
		{
			InventoryItem inventoryItem = trigger.gameObject.GetComponent<InventoryItem> ();
			if (addItem (inventoryItem))
			{
				trigger.gameObject.SetActive (false);
			}
		}
	}

	private bool addItem(InventoryItem inventoryItem)
	{
		return this.inventory.addItem (inventoryItem);
	}

	public void setWorld(World world)
	{
		this.world = world;
	}
}
