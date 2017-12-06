using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public InventoryRenderer inventoryRenderer;
	private int slotCapacity = 20;
	private int activeSlot = 0;
	private InventorySlot[] slots = new InventorySlot[4];

	public void nextItem()
	{
		this.activeSlot++;
		clamp ();
		print (this.activeSlot);
	}

	public void previousItem()
	{
		this.activeSlot--;
		clamp ();
		print (this.activeSlot);
	}

	public void selectItem(int item)
	{
		this.activeSlot = item;
		clamp ();
		print (this.activeSlot);
	}

	private void clamp()
	{
		if (this.activeSlot >= this.slots.Length)
		{
			this.activeSlot = 0;
		}
		if (this.activeSlot < 0)
		{
			this.activeSlot = this.slots.Length - 1;
		}
	}

	public void addItem()
	{
		//print ("Inventory: addItem");
	}

	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}
}
