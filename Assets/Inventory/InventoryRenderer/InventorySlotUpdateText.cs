using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUpdateText : MonoBehaviour
{
	public Text text;

	void Start () {}

	public void updateText(string text)
	{
		this.text.text = text;
	}


	
	void Update () {}
}
