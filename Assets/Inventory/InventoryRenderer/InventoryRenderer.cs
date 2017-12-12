using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryRenderer : MonoBehaviour
{
	public GameObject slotImageTemplate;
	public GameObject slotSelectedTemplate;
	public Sprite emptySprite;

	private bool initialized = false;
	private int initTimer = 2;

	private GameObject[] slotImages;
	private GameObject slotSelected;

	public void Init(int slotCount)
	{
		if (!this.initialized)
		{
			GameObject newSelected = GameObject.Instantiate (this.slotSelectedTemplate);
			this.slotSelected = newSelected;
			this.slotSelected.transform.SetParent (this.transform);

			this.slotImages = new GameObject[slotCount];
			for (int i = 0; i < slotCount; i++)
			{
				GameObject newSlot = GameObject.Instantiate (this.slotImageTemplate);
				newSlot.transform.SetParent (this.transform);
				this.slotImages [i] = newSlot;
			}
		}
	}

	//I know, it is terrible
	void Update()
	{
		if (this.initTimer > 0)
		{
			this.slotSelected.transform.position = this.slotImages [0].transform.position;
			this.initTimer--;
		}
	}

	public void SetSprite(int slotId, Sprite sprite)
	{
		if (sprite == null)
		{
			sprite = this.emptySprite;
		}
		this.slotImages[slotId].GetComponent<InventorySlotUpdateText>().image.sprite = sprite;
	}

	public void UpdateCounter(int slotId, int count)
	{
		this.slotImages[slotId].GetComponent<InventorySlotUpdateText>().updateText(count.ToString());
	}

	public void UpdateSelected(int slotId)
	{
		this.slotSelected.transform.position = this.slotImages [slotId].transform.position;
	}

}
