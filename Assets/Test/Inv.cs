using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inv : MonoBehaviour
{
	public InvRenderer invRenderer;

	private List<InvItem> items = new List<InvItem>();
	private int slotCount = 4;
	public InvSlot[] slots;

	void Awake()
	{
		this.invRenderer.init (this.slotCount);
	}

	void Start()
	{
		this.slots = new InvSlot[this.slotCount];
		for (int i = 0; i < this.slots.Length; i++)
		{
			this.slots[i] = new InvSlot ();
		}
	}

	public bool addItem(InvItem inventoryItem)
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

	private bool addToStack(InvItem inventoryItem)
	{
		for(int i = 0; i < this.slots.Length; i++)
		{
			InvSlot inventorySlot = this.slots [i];
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

	private bool addNew(InvItem inventoryItem)
	{
		for(int i = 0; i < this.slots.Length; i++)
		{
			InvSlot inventorySlot = this.slots [i];
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

}
