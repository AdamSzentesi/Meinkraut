using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshArchitect
{
	private ACubeData cube = new CubeData();
	private Vector2[] atlasData = new Vector2[]
	{
		new Vector2(0.000f, 0.000f),
		new Vector2(0.000f, 0.125f),
		new Vector2(0.000f, 0.250f),
		new Vector2(0.000f, 0.375f),
		new Vector2(0.000f, 0.500f),
		new Vector2(0.000f, 0.625f),
	};

	public List<Vector3> vertices;
	public List<int> triangles;
	public List<Vector3> normals;
	public List<Vector2> uv;
	//public List<Vector3i> colliderPositions;

	private int size;
	private Block[,,] blocks;

	public MeshArchitect(int size, Block[,,] blocks)
	{
		this.size = size;
		this.blocks = blocks;

		this.vertices = new List<Vector3>();
		this.triangles = new List<int>();
		this.uv = new List<Vector2> ();
		//this.colliderPositions = new List<Vector3i> ();

		for (int x = 0; x < this.size; x++)
		{
			for (int z = 0; z < this.size; z++)
			{
				for (int y = 0; y < this.size; y++)
				{
					if (!this.blocks [x, y, z].transparent)
					{
						createVoxel(new Vector3i(x, y, z), this.blocks [x, y, z].type);
					}
				}
			}
		}
	}

	//decide which sides are needed
	//TODO: check neighbor chunks also
	private void createVoxel(Vector3i position, int atlasPosition)
	{
		bool isInside = true;
		if (!hasNeighbor (position, cube.UnitDirections [cube.DIR_U])) {createFace (cube.DIR_U, position, atlasPosition); isInside = false;}
		if (!hasNeighbor (position, cube.UnitDirections [cube.DIR_D])) {createFace (cube.DIR_D, position, atlasPosition); isInside = false;}
		if (!hasNeighbor (position, cube.UnitDirections [cube.DIR_L])) {createFace (cube.DIR_L, position, atlasPosition); isInside = false;}
		if (!hasNeighbor (position, cube.UnitDirections [cube.DIR_R])) {createFace (cube.DIR_R, position, atlasPosition); isInside = false;}
		if (!hasNeighbor (position, cube.UnitDirections [cube.DIR_F])) {createFace (cube.DIR_F, position, atlasPosition); isInside = false;}
		if (!hasNeighbor (position, cube.UnitDirections [cube.DIR_B])) {createFace (cube.DIR_B, position, atlasPosition); isInside = false;}
		//if (!isInside) {this.colliderPositions.Add(position);}
		this.blocks[position.x, position.y, position.z].isInside = isInside;
	}

	//add a new face to geometry
	void createFace(int direction, Vector3i position, int atlasPosition)
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
		this.uv.Add(cube.getUVs(direction, 0) + this.atlasData[atlasPosition]);
		this.uv.Add(cube.getUVs(direction, 1) + this.atlasData[atlasPosition]);
		this.uv.Add(cube.getUVs(direction, 2) + this.atlasData[atlasPosition]);
		this.uv.Add(cube.getUVs(direction, 3) + this.atlasData[atlasPosition]);
	}

	private Vector3[] getFaceVertices(int direction, Vector3i position)
	{
		Vector3[] result = new Vector3[4];
		for(int i = 0; i < 4; i++)
		{
			result [i] = position.add(cube.CubeVertices[cube.CubeIndices[direction][i]]);
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
