using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	private int viewDistance = 5;
	private int chunkSize = 16;
	public GameObject[,] chunks;
	public GameObject player;
	private Transform playerTransform;
	private Vector3i playerChunkPosition;

	void Start ()
	{
		this.chunks = new GameObject[this.viewDistance * 2 + 1, this.viewDistance * 2 + 1];
		this.playerTransform = this.player.GetComponent<Transform> ();
		this.playerChunkPosition = getChunkPosition (playerTransform.position);

		for (int x = 0; x < this.viewDistance * 2 + 1; x++)
		{
			for (int z = 0; z < this.viewDistance * 2 + 1; z++)
			{
				Vector3i chunkPosition = new Vector3i (x - this.viewDistance, 0, z - this.viewDistance).add (playerChunkPosition);

				GameObject newChunk = new GameObject ("Chunk_" + chunkPosition.x + "_" + chunkPosition.z);
				this.chunks[x, z] = newChunk;
				this.chunks[x, z].AddComponent<MeshFilter> ();
				this.chunks[x, z].AddComponent<MeshRenderer> ();
				this.chunks[x, z].AddComponent<MeshCollider> ();
				this.chunks[x, z].AddComponent<Chunk> ();
				this.chunks[x, z].GetComponent<Chunk> ().size = this.chunkSize;
				this.chunks[x, z].GetComponent<Chunk> ().position.set(chunkPosition.x * chunkSize, 0, chunkPosition.z * chunkSize);

				Vector3 worldPosition = new Vector3 (chunkPosition.x * chunkSize - chunkSize / 2, 0, chunkPosition.z * chunkSize - chunkSize / 2);
				this.chunks[x, z].transform.SetPositionAndRotation (worldPosition, Quaternion.identity);
			}
		}

		//drawChunks ();
	}

	void Update()
	{
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

	private Vector3i getChunkPosition(Vector3 position)
	{
		Vector3i result = new Vector3i ();
		result.x = Mathf.RoundToInt(position.x / this.chunkSize);
		result.y = Mathf.RoundToInt(position.y / this.chunkSize);
		result.z = Mathf.RoundToInt(position.z / this.chunkSize);
		return result;
	}

	//private updateChunks()

//	private void drawChunks()
//	{
//		for (int x = -this.viewDistance; x < this.viewDistance + 1; x++)
//		{
//			for (int z = -this.viewDistance; z < this.viewDistance + 1; z++)
//			{
//				GameObject chunk = new GameObject("Chunk_" + x + "_" + z);
//				chunk.AddComponent<MeshFilter> ();
//				chunk.AddComponent<MeshRenderer> ();
//				chunk.AddComponent<MeshCollider> ();
//				chunk.AddComponent<Chunk> ();
//				chunk.GetComponent<Chunk> ().size = this.chunkSize;
//				chunk.GetComponent<Chunk> ().position.set(x * chunkSize, 0, z * chunkSize);
//				chunk.transform.SetPositionAndRotation (new Vector3(x * chunkSize - chunkSize / 2, 0, z * chunkSize - chunkSize / 2), Quaternion.identity);
//			}
//		}
//	}

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
		Vector3i chunkPosition = new Vector3i (x - this.viewDistance, 0, z - this.viewDistance).add (playerChunkPosition);

		GameObject newChunk = new GameObject ("Chunk_" + chunkPosition.x + "_" + chunkPosition.z);
		this.chunks[x, z] = newChunk;
		this.chunks[x, z].AddComponent<MeshFilter> ();
		this.chunks[x, z].AddComponent<MeshRenderer> ();
		this.chunks[x, z].AddComponent<MeshCollider> ();
		this.chunks[x, z].AddComponent<Chunk> ();
		this.chunks[x, z].GetComponent<Chunk> ().size = this.chunkSize;
		this.chunks[x, z].GetComponent<Chunk> ().position.set(chunkPosition.x * chunkSize, 0, chunkPosition.z * chunkSize);

		Vector3 worldPosition = new Vector3 (chunkPosition.x * chunkSize - chunkSize / 2, 0, chunkPosition.z * chunkSize - chunkSize / 2);
		this.chunks[x, z].transform.SetPositionAndRotation (worldPosition, Quaternion.identity);
	}

}
