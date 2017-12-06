using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
	public Inv inventory;

	void Start(){}

	public void OnTriggerEnter(Collider trigger)
	{
		if (trigger.gameObject.CompareTag ("Item"))
		{
			InvItem inventoryItem = trigger.gameObject.GetComponent<InvItem> ();
			if (addItem (inventoryItem))
			{
				trigger.gameObject.SetActive (false);
			}
		}
	}

	private bool addItem(InvItem inventoryItem)
	{
		return this.inventory.addItem (inventoryItem);
	}

}
