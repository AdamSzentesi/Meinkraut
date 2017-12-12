using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
	public SaveData saveData;

	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}

	public void Clean()
	{
		this.saveData = new SaveData ();
	}

}
