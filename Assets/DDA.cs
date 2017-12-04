using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DDA
{

	public static void getPoints(Vector3i start, Vector3i end)
	{
		Vector3i distance = end.subtract (start);
		float dy = (float)distance.y / distance.x;

		float border0 = 0;
		float border1;
		for (int i = 0; i < distance.x; i++)
		{
			border1 = (i + 0.5f);
			helper (border0, border1, dy);
			border0 = border1;
		}

		border1 = distance.x;
		helper (border0, border1, dy);
	}

	public static void getPoints(Vector3 start, Vector3 end)
	{
		Vector3 distance = end - start;
		float dy = distance.y / distance.x;

		int intStart = (int)System.Math.Round (start.x, System.MidpointRounding.AwayFromZero);
		int intEnd = (int)System.Math.Round (end.x, System.MidpointRounding.AwayFromZero);
		int intDistance = intEnd - intStart;

		float border0 = start.x;
		float border1;

		for (int i = 0; i < intDistance; i++)
		{
			Debug.Log ("X: " + (intStart + i));
			border1 = intStart + 0.5f + i;
			getElevation (border0, border1, dy, start);
			border0 = border1;
		}
		Debug.Log ("X: " + intEnd);
		border1 = end.x;
		getElevation (border0, border1, dy, start);
	}

	private static void getElevation(float border0, float border1, float dy, Vector3 start)
	{
		float elevation0 = border0 * dy + start.y - start.x * dy + 0.5f;
		float elevation1 = border1 * dy + start.y - start.x * dy + 0.5f;
		int elevation = (int)elevation1 - (int)elevation0;

		Debug.Log (dy + " " + border0 + "/" + border1 + " :d " + elevation0 + "/" + elevation1 + ", " + elevation);		
		for (int i = 0; i < Mathf.Abs(elevation) + 1; i++)
		{
			Debug.Log (" Y: " + ((int)elevation0 + Mathf.Sign(elevation) * i));
		}
	}

	private static void helper(float border0, float border1, float dy)
	{
		float elevation0 = border0 * dy + 0.5f;
		float elevation1 = border1 * dy + 0.5f;
		int elevation = (int)elevation1 - (int)elevation0;

		Debug.Log (dy + " " + border0 + "/" + border1 + " :d " + elevation0 + "/" + elevation1 + ", " + elevation);		
		for (int i = 0; i < elevation + 1; i++)
		{
			Debug.Log ((int)elevation0 + i);

		}
	}

}
