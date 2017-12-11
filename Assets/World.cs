using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	public GameObject player;
	private Transform playerTransform;
	private Vector3i playerChunkPosition;

	private int viewDistance = 1;
	private int chunkSize = 16;
	private GameObject[,] chunks;
	private Dictionary<Vector3i, Dictionary<Vector3i, byte>> changedBlocks;

	private int colliderDistance = 4;
	private GameObject terrainCollider;
	private List<GameObject> blockColliders;
	public BlockDatabase blockDatabase;

	void Start ()
	{
		//player setup
		this.playerTransform = this.player.GetComponent<Transform> ();
		this.playerChunkPosition = getChunkPosition (playerTransform.position);
		this.player.GetComponent<PlayerAction> ().setWorld (this);

		this.chunks = new GameObject[this.viewDistance * 2 + 1, this.viewDistance * 2 + 1];
		this.terrainCollider = new GameObject ();
		this.terrainCollider.name = "terrainCollider";
		this.blockColliders = new List<GameObject> ();
		this.changedBlocks = new Dictionary<Vector3i, Dictionary<Vector3i, byte>> ();

		for (int x = 0; x < this.chunks.GetLength(0); x++)
		{
			for (int z = 0; z < this.chunks.GetLength(1); z++)
			{
				createChunk(x, z);
				updateColliders (x, z);
			}
		}

		//DDA.getPoints (new Vector3 (-1.32849399f,1.4215488f, 0.0f), new Vector3 (4.13839787f,-2.50394962f, 0.0f));
		//drawChunks ();
	}

	void Update()
	{
		updateAllColliders ();

		Vector3i currentChunkPosition = getChunkPosition (this.playerTransform.position);

		if (currentChunkPosition.x != this.playerChunkPosition.x)
		{
			this.playerChunkPosition.set (currentChunkPosition);
			//scrollChunks (this.playerChunkPosition.x - currentChunkPosition.x);
		}

//		if (!currentChunkPosition.Equals(this.playerChunkPosition))
//		{
//			print ("POS CHANGE");
//			this.playerChunkPosition.set (currentChunkPosition);
//		}
//
	}

	//TODO: optimize - no need to clear all and reload!
	private void updateAllColliders()
	{
		clearColliders ();

		for (int x = 0; x < this.chunks.GetLength(0); x++)
		{
			for (int z = 0; z < this.chunks.GetLength(1); z++)
			{
				updateColliders (x, z);
			}
		}
	}

	private void clearColliders()
	{
		for (int i = 0; i < this.blockColliders.Count; i++)
		{
			Destroy(this.blockColliders[i]);
		}
		this.blockColliders.Clear ();
	}

	private void updateColliders(int x, int z)
	{
		//add colliders
		List<Vector3i> colliderVoxels = this.chunks [x, z].GetComponent<Chunk> ().getColliderVoxels(playerTransform.position, this.colliderDistance);
		Vector3i chunkPosition = this.chunks [x, z].GetComponent<Chunk> ().position;
		for (int i = 0; i < colliderVoxels.Count; i++)
		{
			Vector3i blockPosition = colliderVoxels [i];
			Vector3i worldPosition = blockPosition.add (chunkPosition);
			GameObject newCollider = new GameObject ();
			this.blockColliders.Add (newCollider);
			newCollider.name = (x.ToString() + "_" + z.ToString() + "_" + i.ToString());
			newCollider.AddComponent<BoxCollider> ();
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
			Destroy (this.chunks [0, z]);
		}

		for (int x = 1; x < max; x++)
		{
			for (int z = 0; z < max; z++)
			{
				this.chunks [x - 1, z] = this.chunks [x, z];
			}
		}

		for (int z = 0; z < max; z++)
		{
			createChunk (max - 1, z);
		}

	}

	private void createChunk(int x, int z)
	{
//		print ("CREATE");
		Vector3i chunkPosition = new Vector3i (x - this.viewDistance, 0, z - this.viewDistance).add (playerChunkPosition);
		GameObject newChunk = new GameObject ("Chunk_" + chunkPosition.x + "_" + chunkPosition.z);
		this.chunks[x, z] = newChunk;
		this.chunks[x, z].AddComponent<MeshFilter> ();
		this.chunks[x, z].AddComponent<MeshRenderer> ();
		this.chunks[x, z].AddComponent<Chunk> ();
		this.chunks[x, z].GetComponent<Chunk> ().size = this.chunkSize;
		this.chunks[x, z].GetComponent<Chunk> ().position.set(chunkPosition.x * chunkSize, 0, chunkPosition.z * chunkSize);
		this.chunks[x, z].GetComponent<Chunk> ().blockDatabase = this.blockDatabase;
		Vector3 worldPosition = new Vector3 (chunkPosition.x * chunkSize, 0, chunkPosition.z * chunkSize);
		this.chunks[x, z].transform.SetPositionAndRotation (worldPosition, Quaternion.identity);
		this.chunks [x, z].GetComponent<Chunk> ().initialize ();
	}

	public byte dig(Vector3i position, int damage)
	{
		byte diggedBlock = 0;
		for (int x = 0; x < this.chunks.GetLength(0); x++)
		{
			for (int z = 0; z < this.chunks.GetLength(1); z++)
			{
				Chunk chunk = this.chunks[x, z].GetComponent<Chunk> ();
				bool isInside = chunk.isInside (position);
				if (isInside)
				{
					DigData result = chunk.dig (position, damage);
					if (result.digSuccess)
					{
						addChangedBlock (new Vector3i (x, 0, z), position, result.diggedBlockType);
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
				Chunk chunk = this.chunks[x, z].GetComponent<Chunk> ();
				bool isInside = chunk.isInside (position);
				if (isInside)
				{
					chunk.place(position, blockType);
					addChangedBlock (new Vector3i (x, 0, z), position, blockType);
				}
			}
		}
	}

	private void addChangedBlock(Vector3i chunkPosition, Vector3i blockPosition, byte blockType)
	{
		Dictionary<Vector3i, byte> savedChunk;
		this.changedBlocks.TryGetValue (chunkPosition, out savedChunk);
		if (savedChunk == null)
		{
			this.changedBlocks.Add (chunkPosition, new Dictionary<Vector3i, byte> ());
		}

		byte savedBlock;
		this.changedBlocks[chunkPosition].TryGetValue (blockPosition, out savedBlock);
		if (savedBlock == null)
		{
			this.changedBlocks [chunkPosition].Add (blockPosition, blockType);
		}
		else
		{
			this.changedBlocks [chunkPosition] [blockPosition] = blockType;
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

}
