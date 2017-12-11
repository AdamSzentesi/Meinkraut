using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
	public Sprite sprite;
	public int count;
	public InventoryItem inventoryItem;

	public InventorySlot()
	{
		this.count = 0;
	}

	public void addItem(InventoryItem inventoryItem)
	{
		if (this.count == 0)
		{
			this.sprite = inventoryItem.sprite;
			this.inventoryItem = inventoryItem;
		}
		this.count++;
	}

	public InventoryItem getItem()
	{
		InventoryItem result = null;
		if (this.count > 0)
		{
			result = this.inventoryItem;
			this.count--;
			if (this.count == 0)
			{
				this.sprite = null;
				this.inventoryItem = null;
			}
		}
		return result;
	}

}
