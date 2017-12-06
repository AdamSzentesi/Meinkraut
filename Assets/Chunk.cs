using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	//Random random = new Random();

	public Vector3i position = new Vector3i(0, 0, 0);

	public int size = 4;
	public float noiseSize = 30.0f;
	public float biomeHeight = 10f;
	public Block[,,] blocks;

	public void Initiate()
	{
		this.blocks = new Block[this.size, this.size, this.size];
		for (int x = 0; x < this.size; x++)
		{
			for (int z = 0; z < this.size; z++)
			{
				int terrainHeight = getHeight(x + this.position.x, z + this.position.z);
//				terrainHeight = 16;
				for (int y = 0; y < this.size; y++)
				{
					this.blocks [x, y, z] = new Block();
					if (y < terrainHeight)
					{
						this.blocks [x, y, z].type = getType(x + this.position.x, y + this.position.y, z + this.position.z);
						this.blocks [x, y, z].transparent = false;
					}
				}
			}
		}

		MeshArchitect meshArchitect = new MeshArchitect(this.size, this.blocks);
		updateMesh (meshArchitect);
	}

	//TODO: better noise - assymatrical
	private int getHeight(int x, int y)
	{
		//int result = (int)(Mathf.PerlinNoise (x / this.noiseSize, y / this.noiseSize) * biomeHeight + size / 2);
		int result = (int)(Mathf.PerlinNoise (x / this.noiseSize, y / this.noiseSize) * 16 + 2);
		return result;
	}

	private int getType(int x, int y, int z)
	{
		//int result = (Random.Range (0, 5) + 1);

		float noise = Mathf.PerlinNoise (x / this.noiseSize * 2 + 987654, z / this.noiseSize * 2 + 123456);
		noise += Mathf.PerlinNoise (x / this.noiseSize + 123456, y / this.noiseSize + 987654);
		//noise += Mathf.PerlinNoise (y / this.noiseSize + 987654, z / this.noiseSize + 123456);

		return (int)(noise/2 * 5) + 1;
	}

	private void updateMesh(MeshArchitect meshArchitect)
	{
		Mesh mesh = new Mesh();
		mesh.vertices = meshArchitect.vertices.ToArray();
		mesh.triangles = meshArchitect.triangles.ToArray();
		mesh.RecalculateNormals ();
		mesh.uv = meshArchitect.uv.ToArray();

		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		meshFilter.mesh = mesh;

		GetComponent<MeshRenderer> ().material = Resources.Load ("Materials/block") as Material;
	}

	public List<Vector3i> getColliderVoxels(Vector3 playerPosition, int colliderDistance)
	{
		List<Vector3i> result = new List<Vector3i>();

		Vector3i intPlayerPosition = new Vector3i ((int)playerPosition.x, (int)playerPosition.y, (int)playerPosition.z);
		intPlayerPosition = intPlayerPosition.subtract (this.position);

		Vector3i maxDistance = new Vector3i ();
		Vector3i minDistance = new Vector3i ();

		maxDistance.x = intPlayerPosition.x + colliderDistance;
		minDistance.x = intPlayerPosition.x - colliderDistance;
		if(maxDistance.x >= 0 && minDistance.x <= this.size)
		{
			maxDistance.z = intPlayerPosition.z + colliderDistance;
			minDistance.z = intPlayerPosition.z - colliderDistance;
			if(maxDistance.z >= 0 && minDistance.z <= this.size)
			{
				maxDistance.y = intPlayerPosition.y + colliderDistance;
				minDistance.y = intPlayerPosition.y - colliderDistance;
				if(maxDistance.y >= 0 && minDistance.y <= this.size)
				{
					int startX = Mathf.Max (minDistance.x, 0);
					int endX = Mathf.Min ((maxDistance.x + 1), this.size);
					int startY = Mathf.Max (minDistance.y, 0);
					int endY = Mathf.Min ((maxDistance.y + 1), this.size);
					int startZ = Mathf.Max (minDistance.z, 0);
					int endZ = Mathf.Min ((maxDistance.z + 1), this.size);

					for (int x = 0; x < (endX - startX); x++)
					{
						for (int z = 0; z < (endZ - startZ); z++)
						{
							for (int y = 0; y < (endY - startY); y++)
							{
								Vector3i finalPosition = new Vector3i(x + startX, y + startY, z + startZ);
								if(this.blocks[finalPosition.x, finalPosition.y, finalPosition.z].type > 0 && !this.blocks[finalPosition.x, finalPosition.y, finalPosition.z].isInside)
								{
									result.Add (finalPosition);
								}

							}
						}
					}
				}
			}
		}
		return result;
	}
}
