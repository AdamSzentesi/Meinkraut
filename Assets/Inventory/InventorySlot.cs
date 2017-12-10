using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
	private Sprite sprite;
	public byte itemType;
	private InventoryItem inventoryItem;
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
				Debug.Log ("ZERO");
				this.sprite = null; //TODO: original sprite
			}
		}
		return result;
	}

}
