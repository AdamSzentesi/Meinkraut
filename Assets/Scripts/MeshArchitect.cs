﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshArchitect
{
	private ACubeData cube = new CubeData();
	public List<Vector3> vertices;
	public List<int> triangles;
	public List<Vector3> normals;
	public List<Vector2> uv;
	//public List<Vector3i> colliderPositions;

	private int size;
	private Block[,,] blocks;
	private BlockDatabase blockDatabase;

	public MeshArchitect(int size, Block[,,] blocks, BlockDatabase blockDatabase)
	{
		this.size = size;
		this.blocks = blocks;

		this.vertices = new List<Vector3>();
		this.triangles = new List<int>();
		this.uv = new List<Vector2> ();
		this.blockDatabase = blockDatabase;
		//this.colliderPositions = new List<Vector3i> ();

		for (int x = 0; x < this.size; x++)
		{
			for (int z = 0; z < this.size; z++)
			{
				for (int y = 0; y < this.size; y++)
				{
					byte blockType = this.blocks [x, y, z].type;
					if (!this.blockDatabase.blockMaterials[blockType].transparent)
					{
						CreateVoxel(new Vector3i(x, y, z), blockType);
					}
				}
			}
		}
	}

	//decide which sides are needed
	//TODO: check neighbor chunks also
	private void CreateVoxel(Vector3i position, int atlasPosition)
	{
		bool isInside = true;
		if (!HasNeighbor (position, cube.UnitDirections [cube.DIR_U])) {CreateFace (cube.DIR_U, position, atlasPosition); isInside = false;}
		if (!HasNeighbor (position, cube.UnitDirections [cube.DIR_D])) {CreateFace (cube.DIR_D, position, atlasPosition); isInside = false;}
		if (!HasNeighbor (position, cube.UnitDirections [cube.DIR_L])) {CreateFace (cube.DIR_L, position, atlasPosition); isInside = false;}
		if (!HasNeighbor (position, cube.UnitDirections [cube.DIR_R])) {CreateFace (cube.DIR_R, position, atlasPosition); isInside = false;}
		if (!HasNeighbor (position, cube.UnitDirections [cube.DIR_F])) {CreateFace (cube.DIR_F, position, atlasPosition); isInside = false;}
		if (!HasNeighbor (position, cube.UnitDirections [cube.DIR_B])) {CreateFace (cube.DIR_B, position, atlasPosition); isInside = false;}
		//if (!isInside) {this.colliderPositions.Add(position);}
		this.blocks[position.x, position.y, position.z].isInside = isInside;
	}

	//add a new face to geometry
	void CreateFace(int direction, Vector3i position, int atlasPosition)
	{
		//vertices
		this.vertices.AddRange (GetFaceVertices(direction, position));
		int vertexCount = this.vertices.Count;

		//triangles
		this.triangles.Add(vertexCount - 4);
		this.triangles.Add(vertexCount - 3);
		this.triangles.Add(vertexCount - 2);
		this.triangles.Add(vertexCount - 4);
		this.triangles.Add(vertexCount - 2);
		this.triangles.Add(vertexCount - 1);

		//uv
		this.uv.Add(cube.GetUVs(direction, 0) + this.blockDatabase.blockMaterials[atlasPosition].atlasUVs);
		this.uv.Add(cube.GetUVs(direction, 1) + this.blockDatabase.blockMaterials[atlasPosition].atlasUVs);
		this.uv.Add(cube.GetUVs(direction, 2) + this.blockDatabase.blockMaterials[atlasPosition].atlasUVs);
		this.uv.Add(cube.GetUVs(direction, 3) + this.blockDatabase.blockMaterials[atlasPosition].atlasUVs);
	}

	private Vector3[] GetFaceVertices(int direction, Vector3i position)
	{
		Vector3[] result = new Vector3[4];
		for(int i = 0; i < 4; i++)
		{
			result [i] = position.Add(cube.CubeVertices[cube.CubeIndices[direction][i]]);
		}
		return result;
	}

	//returns true if block has a neighbor
	public bool HasNeighbor(Vector3i position, Vector3i direction)
	{
		bool result = false;

		Vector3i requestedBlock = position.Add (direction);
		if 	(requestedBlock.x < 0 || requestedBlock.x >= this.size
			|| requestedBlock.y < 0 || requestedBlock.y >= this.size
			|| requestedBlock.z < 0 || requestedBlock.z >= this.size)
		{
			return false;
		}

		byte blockType = this.blocks [requestedBlock.x, requestedBlock.y, requestedBlock.z].type;
		if (!this.blockDatabase.blockMaterials[blockType].transparent)
		{
			result = true;
		}
		return result;
	}

}
