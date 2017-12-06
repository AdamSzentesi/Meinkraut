using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
	public Inventory inventory;

	void Start(){}

	public void OnTriggerEnter(Collider trigger)
	{
		if (trigger.gameObject.CompareTag ("Item"))
		{
			InventoryItem inventoryItem = trigger.gameObject.GetComponent<InventoryItem> ();
			if (addItem (inventoryItem))
			{
				trigger.gameObject.SetActive (false);
			}
		}
	}

	private bool addItem(InventoryItem inventoryItem)
	{
		return this.inventory.addItem (inventoryItem);
	}

}
