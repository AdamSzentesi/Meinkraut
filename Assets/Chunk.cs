using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	public Vector3i position;
	public Vector3i worldPosition;
	public int size;
	public float noiseSize = 30.0f;
	public float biomeHeight = 10f;
	public BlockDatabase blockDatabase;

	private Block[,,] blocks;

	public void initialize(Vector3i position, World world)
	{
		this.position = position;
		this.size = world.getChunkSize();
		this.blockDatabase = world.blockDatabase;

		this.worldPosition = position.multiply (this.size);
		this.blocks = new Block[this.size, this.size, this.size];

		Dictionary<Vector3i, byte> blocks;
		bool hasChanges = world.hasChunkChanges (this.position, out blocks);
		for (int x = 0; x < this.size; x++)
		{
			for (int z = 0; z < this.size; z++)
			{
				int terrainHeight = getHeight(x + this.worldPosition.x, z + this.worldPosition.z, world.getWorldSeed());
				for (int y = 0; y < this.size; y++)
				{
					byte blockType;
					this.blocks [x, y, z] = new Block();
					if (hasChanges && blocks.TryGetValue (new Vector3i (x, y, z), out blockType))
					{
						//print ("OK");
					}
					else
					{
						blockType = 0;
						if (y < terrainHeight)
						{
							blockType = getType (x + this.worldPosition.x, y + this.worldPosition.y, z + this.worldPosition.z, world.getWorldSeed ());
						}
					}
					this.blocks [x, y, z].type = blockType;
					this.blocks [x, y, z].health = this.blockDatabase.blockMaterials [blockType].hardness;
				}
			}
		}

		MeshArchitect meshArchitect = new MeshArchitect(this.size, this.blocks, this.blockDatabase);
		GetComponent<MeshRenderer> ().material = this.blockDatabase.materialAtlas;
		updateMesh (meshArchitect);
	}

	//TODO: better noise - assymatrical
	private int getHeight(int x, int y, int seed)
	{
		//int result = (int)(Mathf.PerlinNoise (x / this.noiseSize, y / this.noiseSize) * biomeHeight + size / 2);
		//int result = (int)(Mathf.PerlinNoise ((float)(seed + x) / (float)this.noiseSize, (float)(seed + y) / (float)this.noiseSize) * 16.0f + 2.0f);
		int result = (int)((Mathf.PerlinNoise ((float)(seed + x) / (float)this.noiseSize, (float)(seed + y) / (float)this.noiseSize)) * 16 + 2);
		return result;
	}

	private byte getType(int x, int y, int z, int seed)
	{
		switch (y)
		{
			case 0:
			{
				return 6;
			}
			default:
			{
				//int result = (Random.Range (0, 5) + 1);
				float noise = Mathf.PerlinNoise (seed + x / this.noiseSize * 2.0f, seed + z / this.noiseSize * 2.0f + 1000.0f);
				noise += Mathf.PerlinNoise (seed + x / this.noiseSize + 1000.0f, seed + y / this.noiseSize);
				//noise += Mathf.PerlinNoise (y / this.noiseSize + 987654, z / this.noiseSize + 123456);
				return (byte)((noise/2 * 5) + 1);
			}
		}
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
	}

	public List<Vector3i> getColliderVoxels(Vector3 playerPosition, int colliderDistance)
	{
		List<Vector3i> result = new List<Vector3i>();

		Vector3i intPlayerPosition = new Vector3i ((int)playerPosition.x, (int)playerPosition.y, (int)playerPosition.z);
		intPlayerPosition = intPlayerPosition.subtract (this.worldPosition);

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
								byte blockType = this.blocks [finalPosition.x, finalPosition.y, finalPosition.z].type;
								if(this.blockDatabase.blockMaterials[blockType].collider && !this.blocks[finalPosition.x, finalPosition.y, finalPosition.z].isInside)
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

	public bool isInside(Vector3i position)
	{
		if (position.x >= this.worldPosition.x && position.x < (this.worldPosition.x + this.size))
		{
			if (position.z >= this.worldPosition.z && position.z < (this.worldPosition.z + this.size))
			{
				if (position.y >= this.worldPosition.y && position.y < (this.worldPosition.y + this.size))
				{
					//print(this.position.x + "," + this.position.y + "," + this.position.z + " IS IN");
					return true;
				}
			}
		}
		return false;
	}

	public DigData dig(Vector3i worldPosition, int damage)
	{
		DigData result = new DigData ();
		Vector3i localPosition = getLocalPosition(worldPosition);
		Block diggedBlock = this.blocks [localPosition.x, localPosition.y, localPosition.z];
		diggedBlock.health -= damage;
		if (diggedBlock.health <= 0)
		{
			result = place(worldPosition, 0);
		}
		return result;
	}

	public DigData place(Vector3i worldPosition, byte placedBlockType)
	{
		DigData result = new DigData ();
		Vector3i localPosition = getLocalPosition(worldPosition);
		byte diggedBlockType = this.blocks[localPosition.x, localPosition.y, localPosition.z].type;
		this.blocks [localPosition.x, localPosition.y, localPosition.z].type = placedBlockType;
		this.blocks [localPosition.x, localPosition.y, localPosition.z].health = this.blockDatabase.blockMaterials[placedBlockType].hardness;
		MeshArchitect meshArchitect = new MeshArchitect(this.size, this.blocks, this.blockDatabase);
		updateMesh (meshArchitect);
		result.success = true;
		result.diggedBlockType = diggedBlockType;
		result.localPosition = localPosition;
		return result;
	}

	private Vector3i getLocalPosition(Vector3i worldPosition)
	{
		return worldPosition.subtract (this.worldPosition);
	}
	
}
