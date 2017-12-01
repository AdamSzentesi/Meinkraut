using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshArchitect
{
	//direction keys
	private const int DIR_L = 0;
	private const int DIR_R = 1;
	private const int DIR_F = 2;
	private const int DIR_B = 3;
	private const int DIR_U = 4;
	private const int DIR_D = 5;

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

	private Vector3i[] unitDirections =
	{
		new Vector3i(-1, 0, 0),
		new Vector3i(1, 0, 0),
		new Vector3i(0, 0, 1),
		new Vector3i(0, 0, -1),
		new Vector3i(0, 1, 0),
		new Vector3i(0, -1, 0),
	};

	public List<Vector3> vertices;
	public List<int> triangles;
	public List<Vector3> normals;
	public List<Vector2> uv;

	private int size;
	private Block[,,] blocks;

	public MeshArchitect(int size, Block[,,] blocks)
	{
		this.size = size;
		this.blocks = blocks;

		this.vertices = new List<Vector3>();
		this.triangles = new List<int>();

		for (int x = 0; x < this.size; x++)
		{
			for (int z = 0; z < this.size; z++)
			{
				for (int y = 0; y < this.size; y++)
				{
					if (this.blocks [x, y, z].type != 0)
					{
						createVoxel(new Vector3i(x, y, z));
					}
				}
			}
		}
	}

	//decide which sides are needed
	void createVoxel(Vector3i position)
	{
		if (!hasNeighbor (position, this.unitDirections [DIR_U])) {createFace (DIR_U, position);}
		if (!hasNeighbor (position, this.unitDirections [DIR_D])) {createFace (DIR_D, position);}
		if (!hasNeighbor (position, this.unitDirections [DIR_L])) {createFace (DIR_L, position);}
		if (!hasNeighbor (position, this.unitDirections [DIR_R])) {createFace (DIR_R, position);}
		if (!hasNeighbor (position, this.unitDirections [DIR_F])) {createFace (DIR_F, position);}
		if (!hasNeighbor (position, this.unitDirections [DIR_B])) {createFace (DIR_B, position);}
	}

	//add a new face to geometry
	void createFace(int direction, Vector3i position)
	{
		//vertices
		this.vertices.AddRange (getFaceVertices(direction, position));
		int vertexCount = this.vertices.Count;

		//triangles
		this.triangles.Add(vertexCount - 4);
		this.triangles.Add(vertexCount - 3);
		this.triangles.Add(vertexCount - 2);
		this.triangles.Add(vertexCount - 4);
		this.triangles.Add(vertexCount - 2);
		this.triangles.Add(vertexCount - 1);

		//normals
		//uv
	}

	private Vector3[] getFaceVertices(int direction, Vector3i position)
	{
		Vector3[] result = new Vector3[4];
		for(int i = 0; i < 4; i++)
		{
			result [i] = position.add(cubeVertices[this.cubeIndices[direction][i]]);
		}
		return result;
	}

//	private Vector3[] getFaceVertices(int direction)
//	{
//		return getFaceVertices(direction, new Vector3i(0, 0, 0));
//	}

	//returns true if block has a neighbor
	public bool hasNeighbor(Vector3i position, Vector3i direction)
	{
		bool result = false;

		Vector3i requestedBlock = position.add (direction);
		if 	(requestedBlock.x < 0 || requestedBlock.x >= this.size
			|| requestedBlock.y < 0 || requestedBlock.y >= this.size
			|| requestedBlock.z < 0 || requestedBlock.z >= this.size)
		{
			return false;
		}
		if (this.blocks [requestedBlock.x, requestedBlock.y, requestedBlock.z].type != 0)
		{
			result = true;
		}
		return result;
	}

}
