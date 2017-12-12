using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ACubeData
{
	//direction keys
	abstract public int DIR_L {get;}
	abstract public int DIR_R {get;}
	abstract public int DIR_F {get;}
	abstract public int DIR_B {get;}
	abstract public int DIR_U {get;}
	abstract public int DIR_D {get;}

	abstract public Vector3[] CubeVertices {get;}
	abstract public int[][] CubeIndices {get;}
	abstract public Vector2 GetUVs(int direction, int corner);
	abstract public Vector3i[] UnitDirections {get;}
}
