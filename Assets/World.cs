using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	public GameObject player;
	private Transform playerTransform;
	private Vector3i playerChunkPosition; //players chunk position (chunk units)

	private int seed = 666; //default world seed
	private int viewDistance = 5; //Moore neighborhood for chunk generation (chunk units)
	private int chunkSize = 16; //default chunk size (block units)
	private GameObject[,] chunks;
	public Dictionary<Vector3i, Dictionary<Vector3i, byte>> changedBlocks = new Dictionary<Vector3i, Dictionary<Vector3i, byte>>(); //DB of changed blocks: saving, loading and redrawing
	public BlockDatabase blockDatabase; //DB of block templates

	private int colliderDistance = 4; //Moor neigh. for collider spawning (block units)
	private GameObject terrainCollider;
	private List<GameObject> blockColliders;

	void Start()
	{
		//player setup
		this.playerTransform = this.player.GetComponent<Transform> ();
		this.playerChunkPosition = new Vector3i(0, 0, 0);
		this.player.GetComponent<PlayerAction> ().setWorld (this);

		//start from menu setup
		GameObject gameData = GameObject.Find("GameData");
		if(gameData != null)
		{
			SaveData saveData = gameData.GetComponent<GameData>().saveData;
			this.seed = saveData.worldSeed;
			this.changedBlocks = saveData.getChangedBlocks();
			this.playerTransform.position.Set (saveData.playerPositionX, saveData.playerPositionY, saveData.playerPositionZ);
		}

		//collider setup
		this.terrainCollider = new GameObject();
		this.terrainCollider.name = "terrainCollider";
		this.blockColliders = new List<GameObject>();

		//chunks setup
		this.chunks = new GameObject[this.viewDistance * 2 + 1, this.viewDistance * 2 + 1];
		for (int x = 0; x < this.chunks.GetLength(0); x++)
		{
			for (int z = 0; z < this.chunks.GetLength(1); z++)
			{
				int chunkPositionX = x - this.viewDistance + playerChunkPosition.x;
				int chunkPositionZ = z - this.viewDistance + playerChunkPosition.z;

				this.chunks[x, z] = createChunk(chunkPositionX, chunkPositionZ);
				updateColliders(x, z);
			}
		}
	}

	void Update()
	{
		updateAllColliders();

		Vector3i currentChunkPosition = getChunkPosition(this.playerTransform.position);

		if(currentChunkPosition.x != this.playerChunkPosition.x)
		{
			this.playerChunkPosition.set(currentChunkPosition);
			//scrollChunks (this.playerChunkPosition.x - currentChunkPosition.x);
		}

//		if (!currentChunkPosition.Equals(this.playerChunkPosition))
//		{
//			print ("POS CHANGE");
//			this.playerChunkPosition.set (currentChunkPosition);
//		}
//
	}

	//create chunk at chunk coordinates
	private GameObject createChunk(int x, int z)
	{
		//print("CREATE " + x + ",0," + z);
		Vector3i chunkPosition = new Vector3i(x, 0, z);

		GameObject newChunk = new GameObject("Chunk_" + chunkPosition.x + "_" + chunkPosition.y + "_" + chunkPosition.z);
		newChunk.AddComponent<MeshFilter>();
		newChunk.AddComponent<MeshRenderer>();
		newChunk.AddComponent<Chunk>();

		//chunk init
		newChunk.GetComponent<Chunk>().initialize (chunkPosition, this);

		//place chunk in the Unity world
		newChunk.transform.SetPositionAndRotation(new Vector3 (x * chunkSize, 0, z * chunkSize), Quaternion.identity);

		return newChunk;
	}

	//TODO: optimize - no need to clear all and reload!
	private void updateAllColliders()
	{
		//clear colliders
		for (int i = 0; i < this.blockColliders.Count; i++)
		{
			Destroy(this.blockColliders[i]);
		}
		this.blockColliders.Clear();
		//update colliders
		for (int x = 0; x < this.chunks.GetLength(0); x++)
		{
			for (int z = 0; z < this.chunks.GetLength(1); z++)
			{
				updateColliders(x, z);
			}
		}
	}

	//update colliders of certain chunk
	private void updateColliders(int x, int z)
	{
		//add colliders
		List<Vector3i> colliderVoxels = this.chunks[x, z].GetComponent<Chunk>().getColliderVoxels(playerTransform.position, this.colliderDistance);
		Vector3i chunkWorldPosition = this.chunks[x, z].GetComponent<Chunk>().worldPosition;
		for (int i = 0; i < colliderVoxels.Count; i++)
		{
			Vector3i blockPosition = colliderVoxels[i];
			Vector3i worldPosition = blockPosition.add (chunkWorldPosition);
			GameObject newCollider = new GameObject();
			this.blockColliders.Add (newCollider);
			newCollider.name = (x.ToString() + "_" + z.ToString() + "_" + i.ToString());
			newCollider.AddComponent<BoxCollider>();
			newCollider.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
			newCollider.transform.SetParent (this.terrainCollider.transform);
		}
	}


	private Vector3i getChunkPosition(Vector3 position)
	{
		Vector3i result = new Vector3i ();
		result.x = Mathf.RoundToInt(position.x / this.chunkSize);
		result.y = Mathf.RoundToInt(position.y / this.chunkSize);
		result.z = Mathf.RoundToInt(position.z / this.chunkSize);
		return result;
	}

	private void scrollChunks(int amount)
	{
		int max = this.viewDistance * 2 + 1;

		for (int z = 0; z < max; z++)
		{
			Destroy (this.chunks[0, z]);
		}

		for (int x = 1; x < max; x++)
		{
			for (int z = 0; z < max; z++)
			{
				this.chunks [x - 1, z] = this.chunks[x, z];
			}
		}

		for (int z = 0; z < max; z++)
		{
			//createChunk (max - 1, z);
		}

	}

	public byte dig(Vector3i position, int damage)
	{
		byte diggedBlock = 0;
		for(int x = 0; x < this.chunks.GetLength(0); x++)
		{
			for(int z = 0; z < this.chunks.GetLength(1); z++)
			{
				Chunk chunk = this.chunks[x, z].GetComponent<Chunk>();
				bool isInside = chunk.isInside(position);
				if (isInside)
				{
					DigData result = chunk.dig(position, damage);
					if(result.success)
					{
						Vector3i chunkPosition = this.chunks [x, z].GetComponent<Chunk> ().position;
						addChangedBlock(chunkPosition, result.localPosition, 0);
					}
					return result.diggedBlockType;
				}
			}
		}
		return diggedBlock;
	}

	public void place(Vector3i position, byte blockType)
	{
		for (int x = 0; x < this.chunks.GetLength(0); x++)
		{
			for (int z = 0; z < this.chunks.GetLength(1); z++)
			{
				Chunk chunk = this.chunks[x, z].GetComponent<Chunk>();
				bool isInside = chunk.isInside(position);
				if(isInside)
				{
					DigData result = chunk.place(position, blockType);
					Vector3i chunkPosition = this.chunks [x, z].GetComponent<Chunk> ().position;
					addChangedBlock(chunkPosition, result.localPosition, blockType);
				}
			}
		}
	}

	private void addChangedBlock(Vector3i chunkPosition, Vector3i blockPosition, byte blockType)
	{
//		print("C: " + chunkPosition.x + "," + chunkPosition.y + "," + chunkPosition.z);
//		print(" B: " + blockPosition.x + "," + blockPosition.y + "," + blockPosition.z);
//		print("  T: " + blockType);

		Dictionary<Vector3i, byte> savedChunk;
		this.changedBlocks.TryGetValue(chunkPosition, out savedChunk);
		if(savedChunk == null)
		{
			this.changedBlocks.Add(chunkPosition, new Dictionary<Vector3i, byte>());
		}

		byte savedBlock;
		this.changedBlocks[chunkPosition].TryGetValue(blockPosition, out savedBlock);
		if(savedBlock == null)
		{
			this.changedBlocks[chunkPosition].Add(blockPosition, blockType);
		}
		else
		{
			this.changedBlocks[chunkPosition][blockPosition] = blockType;
		}

//		foreach (Vector3i key in this.changedBlocks.Keys)
//		{
//			Dictionary<Vector3i, byte> chunk = this.changedBlocks [key];
//			print (key);
//			foreach (Vector3i blockKey in chunk.Keys)
//			{
//				print (" - " + blockKey + ": " +  chunk[blockKey]);
//			}
//		}

	}

	public bool hasChunkChanges(Vector3i chunkPosition, out Dictionary<Vector3i, byte> blocks)
	{
		//print (chunkPosition.x + "," + chunkPosition.y + "," + chunkPosition.z);
		if (this.changedBlocks.TryGetValue(chunkPosition, out blocks))
		{
			//print ("OK");
			return true;
		}
		return false;
	}

	public int getChunkSize()
	{
		return this.chunkSize;
	}
	public int getWorldSeed()
	{
		return this.seed;
	}

}
