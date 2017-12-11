using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public GameData gameData;
	public MainMenuSeed mainMenuSeed;

	public void Play()
	{
		this.gameData.seed = int.Parse(mainMenuSeed.GetComponent<InputField> ().text);
		SceneManager.LoadScene ("Game");
	}

	public void Load()
	{
		
	}

	public void Quit()
	{
		Application.Quit();
	}
}
