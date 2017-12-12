using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

	public Vector3i() : this (0, 0, 0) {}

	public void Set(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public void Set(Vector3i v)
	{
		Set (v.x, v.y, v.z);
	}

	public Vector3 Add(Vector3 v)
	{
		return new Vector3(v.x + this.x, v.y + this.y, v.z + this.z);
	}

	public Vector3i Add(Vector3i v)
	{
		return new Vector3i(v.x + this.x, v.y + this.y, v.z + this.z);
	}

	public Vector3i Subtract(Vector3i v)
	{
		return new Vector3i(this.x - v.x, this.y - v.y, this.z - v.z);
	}

	public Vector3i Multiply(int i)
	{
		return new Vector3i(this.x * i, this.y * i, this.z * i);
	}

	public override bool Equals(object obj)
	{
		Vector3i vector3i = (Vector3i)obj;
		return vector3i.x == this.x && vector3i.y == this.y && vector3i.z == this.z;
	}

	public override int GetHashCode()
	{
		int result = 17;

		const int MULTIPLIER = 31;

		result = MULTIPLIER * result + this.x;
		result = MULTIPLIER * result + this.y;
		result = MULTIPLIER * result + this.z;

		return result;
	}

	public Vector3i Add(int number)
	{
		Vector3i result = new Vector3i (this.x + number, this.y + number, this.z + number);
		return result;
	}

	public Vector3 Floatify()
	{
		return new Vector3 (this.x, this.y, this.z);
	}
}
