using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
	public Inventory inventory;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		addItem ();
	}

	private void addItem()
	{
		this.inventory.addItem ();
	}
}
