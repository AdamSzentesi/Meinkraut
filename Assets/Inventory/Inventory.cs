using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public InventoryRenderer invRenderer;
	public int slotCount = 4;

	private List<InventoryItem> items = new List<InventoryItem>();
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
		updateSelected ();
	}

	public void previousItem()
	{
		this.activeSlot--;
		clamp ();
		updateSelected ();
	}

	public void selectItem(int item)
	{
		this.activeSlot = item;
		clamp ();
		updateSelected ();
	}

	private void updateSelected()
	{
		this.invRenderer.updateSelected (this.activeSlot);
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
