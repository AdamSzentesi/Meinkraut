using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	public Vector3i position = new Vector3i(0, 0, 0);
	public int size = 16;
	public float noiseSize = 30.0f;
	public float biomeHeight = 10f;
	public Block[,,] blocks;

	void Start()
	{
		this.blocks = new Block[this.size, this.size, this.size];

		for (int x = 0; x < this.size; x++)
		{
			for (int z = 0; z < this.size; z++)
			{
				int terrainHeight = getHeight(x + this.position.x, z + this.position.z);
				for (int y = 0; y < this.size; y++)
				{
					this.blocks [x, y, z] = new Block();
					if (y < terrainHeight)
					{
						this.blocks [x, y, z].type = 1;
					}
				}
			}
		}

		MeshArchitect meshArchitect = new MeshArchitect(this.size, this.blocks);
		updateMesh (meshArchitect);
	}

	private int getHeight(int x, int y)
	{
		int result = (int)(Mathf.PerlinNoise (x / this.noiseSize, y / this.noiseSize) * biomeHeight + size / 2);
		//result = result + (int)(Mathf.PerlinNoise (x / this.noiseSize / 2, y / this.noiseSize / 2) * biomeHeight + gridSize / 4);
		return result;
	}

	private void updateMesh(MeshArchitect meshArchitect)
	{
		Mesh mesh = new Mesh();
		mesh.vertices = meshArchitect.vertices.ToArray();
		mesh.triangles = meshArchitect.triangles.ToArray();
		mesh.RecalculateNormals ();
		//		mesh.uv = uv;

		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		meshFilter.mesh = mesh;

		MeshCollider meshCollider = GetComponent<MeshCollider> ();
		meshCollider.sharedMesh = mesh;

		GetComponent<MeshRenderer> ().material = Resources.Load ("Materials/Material_1") as Material;
	}

}
