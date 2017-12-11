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
			toggle ();
		}

	}

	private void toggle()
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

	public void save()
	{
		if (!Directory.Exists (this.savePath))
		{
			Directory.CreateDirectory (this.savePath);
		}

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Create (this.savePath + "/ahoj.kraut");

		SaveData saveData = new SaveData ();
		saveData.worldSeed = this.world.seed;
		saveData.playerPositionX = 1.0f;
		saveData.playerPositionY = 2.0f;
		saveData.playerPositionZ = 3.0f;
		//saveData.changedBlocks = this.world.changedBlocks;
		saveData.setChangedBlocks (this.world.changedBlocks);

		//print ("SAVING");
		formatter.Serialize (file, saveData);
		file.Close ();
		toggle ();
	}

	public void quit()
	{
		Application.Quit ();
	}

}
