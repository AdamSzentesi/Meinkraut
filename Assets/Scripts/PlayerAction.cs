using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
	public Camera camera;
	private CameraControl cameraControl;

	public AudioClip dig;
	public AudioClip place;
	private AudioSource audioSource;

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
		this.audioSource = GetComponent<AudioSource> ();
	}

	void Update()
	{
		this.digInput = Input.GetAxis("Dig");
		this.placeInput = Input.GetAxis("Place");

		if (Input.GetKeyDown (KeyCode.Q)){this.inventory.PreviousItem();}
		if (Input.GetKeyDown (KeyCode.E)){this.inventory.NextItem();}
		//		if (Input.GetKeyDown (KeyCode.Keypad1)){this.GetComponent<Inventory> ().selectItem (0);}
		//		if (Input.GetKeyDown (KeyCode.Keypad2)){this.GetComponent<Inventory> ().selectItem (1);}
		//		if (Input.GetKeyDown (KeyCode.Keypad3)){this.GetComponent<Inventory> ().selectItem (2);}
		//		if (Input.GetKeyDown (KeyCode.Keypad4)){this.GetComponent<Inventory> ().selectItem (3);}

		Dig ();
		Place ();

		this.lastCharge -= Time.deltaTime;
	}

	private void Dig()
	{
		if (this.digInput > 0)
		{
			if (this.lastCharge <= 0)
			{
				Vector3i target = this.cameraControl.GetTarget (false);
				if (target != null)
				{
					byte diggedBlockType = this.world.Dig (target, this.damage);
					this.audioSource.clip = this.dig;
					this.audioSource.Play();
					if (diggedBlockType > 0)
					{
						InventoryItem newItem = new InventoryItem();
						newItem.type = diggedBlockType;
						newItem.sprite = this.world.blockDatabase.blockMaterials[diggedBlockType].inventorySprite;
						AddItem (newItem);
					}
				}
				this.lastCharge = this.rechargeTime;
			}
		}		
	}

	private void Place()
	{
		if (this.placeInput > 0)
		{
			if (this.lastCharge <= 0)
			{
				Vector3i target = this.cameraControl.GetTarget (true);
				if (target != null)
				{
					InventoryItem inventoryItem = this.inventory.GetItem ();
					if (inventoryItem != null)
					{
						this.audioSource.clip = this.place;
						this.audioSource.Play();
						this.world.Place (target, inventoryItem.type);
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
			if (AddItem (inventoryItem))
			{
				trigger.gameObject.SetActive (false);
			}
		}
	}

	private bool AddItem(InventoryItem inventoryItem)
	{
		return this.inventory.AddItem (inventoryItem);
	}

	public void SetWorld(World world)
	{
		this.world = world;
	}

}
