using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
	private Sprite sprite;
	public int itemType;
	public int count;

	public InventorySlot()
	{
		this.count = 0;
	}

	public void addItem(InventoryItem inventoryItem)
	{
		if (this.count == 0)
		{
			this.sprite = inventoryItem.sprite;
			this.itemType = inventoryItem.type;
		}
		this.count++;
	}

	public void removeItem()
	{
		if (this.count > 0)
		{
			this.count--;
			if (this.count == 0)
			{
				this.sprite = null;
			}
		}
	}

}
