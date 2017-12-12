using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameMenu : MonoBehaviour
{
	public GameObject menu;
	public World world;

	private float timescale;
	private string savePath;

	void Awake()
	{
		this.savePath = Application.dataPath + "/saves";
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			Toggle ();
		}

	}

	private void Toggle()
	{
		if (this.menu.activeInHierarchy == false)
		{
			this.menu.SetActive(true);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			this.timescale = Time.timeScale;
			Time.timeScale = 0;
		}
		else
		{
			this.menu.SetActive(false);
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			Time.timeScale = this.timescale;
		}
	}

	public void Save()
	{
		if (!Directory.Exists (this.savePath))
		{
			Directory.CreateDirectory (this.savePath);
		}

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Create (this.savePath + "/save.kraut");

		SaveData saveData = new SaveData ();
		saveData.worldSeed = this.world.GetWorldSeed();
		saveData.playerPositionX = this.world.player.transform.position.x;
		saveData.playerPositionY = this.world.player.transform.position.y;
		saveData.playerPositionZ = this.world.player.transform.position.z;
		saveData.inventoryItemCounts = this.world.player.GetComponent<Inventory> ().GetItemCounts ();
		saveData.inventoryItems = this.world.player.GetComponent<Inventory> ().GetItems ();
		saveData.SetChangedBlocks (this.world.changedBlocks);

		//print ("SAVING");
		formatter.Serialize (file, saveData);
		file.Close ();
		Toggle ();
	}

	public void Quit()
	{
		Application.Quit ();
	}

}
