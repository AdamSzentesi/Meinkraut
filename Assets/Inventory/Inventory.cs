using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public InventoryRenderer invRenderer;

	private List<InventoryItem> items = new List<InventoryItem>();
	private int slotCount = 4;
	private InventorySlot[] slots;
	private int activeSlot = 0;

	void Awake()
	{
		this.invRenderer.init (this.slotCount);
	}

	void Start()
	{
		this.slots = new InventorySlot[this.slotCount];
		for (int i = 0; i < this.slots.Length; i++)
		{
			this.slots[i] = new InventorySlot ();
		}
	}

	public bool addItem(InventoryItem inventoryItem)
	{
		bool result = true;
		print ("inventory adding type: " + inventoryItem.type);
		if (!addToStack (inventoryItem))
		{
			if (!addNew (inventoryItem))
			{
				result = false;
			}
		}
		return result;
	}

	private bool addToStack(InventoryItem inventoryItem)
	{
		for(int i = 0; i < this.slots.Length; i++)
		{
			InventorySlot inventorySlot = this.slots [i];
			if (inventorySlot.count != 0 && inventorySlot.itemType == inventoryItem.type)
			{
				print ("inventory adding to stack: " + inventoryItem.type);
				inventorySlot.addItem (inventoryItem);
				this.invRenderer.updateCounter(i, inventorySlot.count);
				return true;
			}
		}
		return false;
	}

	private bool addNew(InventoryItem inventoryItem)
	{
		for(int i = 0; i < this.slots.Length; i++)
		{
			InventorySlot inventorySlot = this.slots [i];
			if (inventorySlot.count == 0)
			{
				print ("inventory adding as new: " + inventoryItem.type);
				inventorySlot.addItem (inventoryItem);
				this.invRenderer.updateCounter(i, inventorySlot.count);
				this.invRenderer.setSprite (i, inventoryItem.sprite);
				return true;
			}
		}
		return false;
	}

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

}
