using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MainMenu : MonoBehaviour
{
	public GameData gameData;
	public MainMenuSeed mainMenuSeed;

	private string savePath;

	void Awake()
	{
		this.savePath = Application.dataPath + "/saves";
	}

	public void Play()
	{
		this.gameData.clean();
		this.gameData.saveData.worldSeed = int.Parse(mainMenuSeed.GetComponent<InputField> ().text);
		this.gameData.saveData.playerPositionX = 0;
		this.gameData.saveData.playerPositionY = 20;
		this.gameData.saveData.playerPositionZ = 0;
		SceneManager.LoadScene ("Game");
	}

	public void Load()
	{
		if (!Directory.Exists (this.savePath))
		{
			return;
		}

		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream file = File.Open (this.savePath + "/ahoj.kraut", FileMode.Open);

		SaveData saveData = (SaveData)formatter.Deserialize(file);
		file.Close ();

		this.gameData.saveData = saveData;
		//this.gameData.GetComponent<GameData>().saveData = saveData;
		SceneManager.LoadScene ("Game");
	}

	public void Quit()
	{
		Application.Quit();
	}
}
