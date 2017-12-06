using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeData : ACubeData
{
	override public int DIR_L {get{return 0;}}
	override public int DIR_R {get{return 1;}}
	override public int DIR_F {get{return 2;}}
	override public int DIR_B {get{return 3;}}
	override public int DIR_U {get{return 4;}}
	override public int DIR_D {get{return 5;}}

	private Vector3[] cubeVertices =
	{
		new Vector3(0.5f, 0.5f, 0.5f),
		new Vector3(-0.5f, 0.5f, 0.5f),
		new Vector3(-0.5f, -0.5f, 0.5f),
		new Vector3(0.5f, -0.5f, 0.5f),
		new Vector3(-0.5f, 0.5f, -0.5f),
		new Vector3(0.5f, 0.5f, -0.5f),
		new Vector3(0.5f, -0.5f, -0.5f),
		new Vector3(-0.5f, -0.5f, -0.5f),
	};

	private int[][] cubeIndices =
	{
		new int[] {1, 4, 7, 2},		
		new int[] {5, 0, 3, 6},
		new int[] {0, 1, 2, 3},
		new int[] {4, 5, 6, 7},
		new int[] {5, 4, 1, 0},
		new int[] {3, 2, 7, 6},
	};

	//TODO: side data
	private Vector2[][] cubeUVs =
	{
		new Vector2[] {new Vector2(0.250f, 0.125f), new Vector2(0.375f, 0.125f), new Vector2(0.375f, 0.000f), new Vector2(0.250f, 0.000f)},
		new Vector2[] {new Vector2(0.500f, 0.125f), new Vector2(0.625f, 0.125f), new Vector2(0.625f, 0.000f), new Vector2(0.500f, 0.000f)},
		new Vector2[] {new Vector2(0.625f, 0.125f), new Vector2(0.750f, 0.125f), new Vector2(0.750f, 0.000f), new Vector2(0.625f, 0.000f)},
		new Vector2[] {new Vector2(0.375f, 0.125f), new Vector2(0.500f, 0.125f), new Vector2(0.500f, 0.000f), new Vector2(0.375f, 0.000f)},
		new Vector2[] {new Vector2(0.000f, 0.125f), new Vector2(0.125f, 0.125f), new Vector2(0.125f, 0.000f), new Vector2(0.000f, 0.000f)},
		new Vector2[] {new Vector2(0.125f, 0.125f), new Vector2(0.250f, 0.125f), new Vector2(0.250f, 0.000f), new Vector2(0.125f, 0.000f)},
	};

	private Vector3i[] unitDirections =
	{
		new Vector3i(-1, 0, 0),
		new Vector3i(1, 0, 0),
		new Vector3i(0, 0, 1),
		new Vector3i(0, 0, -1),
		new Vector3i(0, 1, 0),
		new Vector3i(0, -1, 0),
	};

	override public Vector3[] CubeVertices {get {return this.cubeVertices;} }
	override public int[][] CubeIndices {get {return this.cubeIndices;} }
	override public Vector2 getUVs(int direction, int corner)
	{
		return this.cubeUVs [direction][corner];
	}
	override public Vector3i[] UnitDirections {get {return this.unitDirections;} }
}
