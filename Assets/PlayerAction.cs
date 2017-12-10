using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
	public Camera camera;
	private CameraControl cameraControl;

	private Inventory inventory;
	private World world;
	private float digInput = 0;
	private float placeInput = 0;

	public float rechargeTime = 0.2f;
	public int damage = 1;
	private float lastCharge = 0.0f;


	void Start()
	{
		this.inventory = GetComponent<Inventory> ();
		this.cameraControl = this.camera.GetComponent<CameraControl> ();
	}

	void Update()
	{
		this.digInput = Input.GetAxis("Fire1");
		this.placeInput = Input.GetAxis("Fire2");

		if (Input.GetKeyDown (KeyCode.Q)){this.inventory.previousItem();}
		if (Input.GetKeyDown (KeyCode.E)){this.inventory.nextItem();}
		//		if (Input.GetKeyDown (KeyCode.Keypad1)){this.GetComponent<Inventory> ().selectItem (0);}
		//		if (Input.GetKeyDown (KeyCode.Keypad2)){this.GetComponent<Inventory> ().selectItem (1);}
		//		if (Input.GetKeyDown (KeyCode.Keypad3)){this.GetComponent<Inventory> ().selectItem (2);}
		//		if (Input.GetKeyDown (KeyCode.Keypad4)){this.GetComponent<Inventory> ().selectItem (3);}

		dig ();
		place ();

		this.lastCharge -= Time.deltaTime;
	}

	private void dig()
	{
		if (this.digInput > 0)
		{
			if (this.lastCharge <= 0)
			{
				Vector3i target = this.cameraControl.getTarget (false); //get tool info!!!§
				if (target != null)
				{
					byte diggedBlockType = this.world.dig (target, this.damage);
					this.GetComponent<AudioSource> ().Play();
					if (diggedBlockType > 0)
					{
						GameObject gameObject = new GameObject ();
						gameObject.AddComponent<InventoryItem> ().type = diggedBlockType;
						InventoryItem newItem = gameObject.GetComponent<InventoryItem> ();
						newItem.type = diggedBlockType;
						newItem.sprite = this.world.blockDatabase.blockMaterials[diggedBlockType].inventorySprite;
						addItem (newItem);
					}
				}
				this.lastCharge = this.rechargeTime;
			}
		}		
	}

	private void place()
	{
		if (this.placeInput > 0)
		{
			if (this.lastCharge <= 0)
			{
				Vector3i target = this.cameraControl.getTarget (true);
				if (target != null)
				{
					InventoryItem inventoryItem = this.inventory.getItem ();
					if (inventoryItem != null)
					{
						this.world.place (target, inventoryItem.type);
					}
				}
				this.lastCharge = this.rechargeTime;
			}
		}		
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
