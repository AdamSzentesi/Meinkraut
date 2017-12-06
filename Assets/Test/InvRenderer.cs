using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvRenderer : MonoBehaviour
{
	public GameObject slotImageTemplate;

	private bool initialized = false;
	private GameObject[] slotImages;

	public void init(int slotCount)
	{
		if (!this.initialized)
		{
			this.slotImages = new GameObject[slotCount];
			for (int i = 0; i < slotCount; i++)
			{
				GameObject newSlot = GameObject.Instantiate (this.slotImageTemplate);
				newSlot.transform.SetParent (this.transform);
				this.slotImages [i] = newSlot;
			}
		}
	}

	public void setSprite(int slotId, Sprite sprite)
	{
		print ("sprite: " + this.slotImages[slotId].GetComponent<Image>().sprite);
		this.slotImages[slotId].GetComponent<Image>().sprite = sprite;
	}

	public void updateCounter(int slotId, int count)
	{
		this.slotImages[slotId].GetComponent<InvSlotUpdateText>().updateText(count.ToString());
	}

//	public void redraw2(InvSlot[] inventorySlots)
//	{
//		for (int i = 0; i < inventorySlots.Length; i++)
//		{
//			GameObject newSlot = GameObject.Instantiate (this.slotImageTemplate);
//			newSlot.transform.SetParent (this.transform);
//		}
//	}
}
