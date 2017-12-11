using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSeed : MonoBehaviour
{
	void Start ()
	{
		//GetComponent<InputField>().text = ((int)Random.Range (0, int.MaxValue)).ToString();
		GetComponent<InputField>().text = ((int)Random.Range (0, 999999)).ToString();
	}

}
