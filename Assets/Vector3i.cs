using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3i
{
	public int x;
	public int y;
	public int z;

	public Vector3i(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public void set(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vector3 add(Vector3 v)
	{
		return new Vector3(v.x + this.x, v.y + this.y, v.z + this.z);
	}

	public Vector3i add(Vector3i v)
	{
		return new Vector3i(v.x + this.x, v.y + this.y, v.z + this.z);
	}
}
