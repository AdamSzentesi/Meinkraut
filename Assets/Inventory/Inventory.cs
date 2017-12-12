using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public InventoryRenderer invRenderer;
	public BlockDatabase blockDatabase;
	public int activeSlot = 0;

	private int slotCount;
	private List<InventoryItem> items = new List<InventoryItem>();
	private InventorySlot[] slots;


	void Awake()
	{
		this.slotCount = this.blockDatabase.GetDiggable();
		this.invRenderer.Init (this.slotCount);
	}

	void Start()
	{
		this.slots = new InventorySlot[this.slotCount];
		for (int i = 0; i < this.slots.Length; i++)
		{
			this.slots[i] = new InventorySlot ();
		}
	}

	public bool AddItem(InventoryItem inventoryItem)
	{
		bool result = true;
		if (!AddToStack (inventoryItem))
		{
			if (!AddNew (inventoryItem))
			{
				result = false;
			}
		}
		return result;
	}

	public InventoryItem GetItem()
	{
		InventoryItem result = this.slots[this.activeSlot].GetItem();
		this.invRenderer.UpdateCounter(this.activeSlot, this.slots[this.activeSlot].count);
		this.invRenderer.SetSprite(this.activeSlot, this.slots[this.activeSlot].sprite);
		return result;
	}

	private bool AddToStack(InventoryItem inventoryItem)
	{
		for(int i = 0; i < this.slots.Length; i++)
		{
			InventorySlot inventorySlot = this.slots [i];
			if (inventorySlot.count != 0 && inventorySlot.inventoryItem.type == inventoryItem.type)
			{
				inventorySlot.AddItem (inventoryItem);
				this.invRenderer.UpdateCounter(i, inventorySlot.count);
				return true;
			}
		}
		return false;
	}

	private bool AddNew(InventoryItem inventoryItem)
	{
		for(int i = 0; i < this.slots.Length; i++)
		{
			InventorySlot inventorySlot = this.slots [i];
			if (inventorySlot.count == 0)
			{
				inventorySlot.AddItem (inventoryItem);
				this.invRenderer.UpdateCounter(i, inventorySlot.count);
				this.invRenderer.SetSprite (i, inventoryItem.sprite);
				return true;
			}
		}
		return false;
	}

	public void NextItem()
	{
		this.activeSlot++;
		Clamp ();
		UpdateSelected ();
	}

	public void PreviousItem()
	{
		this.activeSlot--;
		Clamp ();
		UpdateSelected ();
	}

	public void SelectItem(int item)
	{
		this.activeSlot = item;
		Clamp ();
		UpdateSelected ();
	}

	private void UpdateSelected()
	{
		this.invRenderer.UpdateSelected (this.activeSlot);
	}

	private void Clamp()
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
