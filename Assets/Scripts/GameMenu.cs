using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
	public GameObject menu;
	private float timescale;

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

	public void quit()
	{
		Application.Quit ();
	}

}
