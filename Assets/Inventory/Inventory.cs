using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public InventoryRenderer invRenderer;
	public BlockDatabase blockDatabase;
	public int activeSlot = 0;

	private int slotCount;
	private InventorySlot[] slots;


	void Awake()
	{
		this.slotCount = this.blockDatabase.GetDiggable();
		this.invRenderer.Init (this.slotCount);
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
		UpdateSelected ();
	}

	public void PreviousItem()
	{
		this.activeSlot--;
		UpdateSelected ();
	}

	public void SelectItem(int item)
	{
		this.activeSlot = item;
		UpdateSelected ();
	}

	private void UpdateSelected()
	{
		Clamp ();
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

	public int[] GetItemCounts()
	{
		int[] result = new int[this.slotCount];
		for(int i = 0; i < this.slots.Length; i++)
		{
			result [i] = slots [i].count;
		}
		return result;
	}

	public byte[] GetItems()
	{
		byte[] result = new byte[this.slotCount];
		for(int i = 0; i < this.slots.Length; i++)
		{
			result [i] = 0;
			if (slots [i].inventoryItem != null)
			{
				result [i] = slots [i].inventoryItem.type;
			}
		}
		return result;
	}

	public void Populate(int[] inventoryItemCounts, byte[] inventoryItems)
	{
		for (int i = 0; i < inventoryItemCounts.Length; i++)
		{
			this.slots [i].Clean ();

			if (inventoryItemCounts [i] > 0)
			{
				InventoryItem newItem = new InventoryItem();
				newItem.type = inventoryItems[i];
				newItem.sprite = this.blockDatabase.blockMaterials[inventoryItems[i]].inventorySprite;
				this.slots [i].AddItem (newItem, inventoryItemCounts [i]);
				this.invRenderer.UpdateCounter(i, this.slots [i].count);
				this.invRenderer.SetSprite (i, this.slots [i].sprite);
			}
		}
	}

}
