using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	private int size = 16;
	private int chunkSize = 16;
	public GameObject[] chunks;

	void Start ()
	{
		for (int x = 0; x < size; x++)
		{
			for (int z = 0; z < size; z++)
			{
				GameObject chunk = new GameObject("Chunk_" + x + "_" + z);
				chunk.AddComponent<MeshFilter> ();
				chunk.AddComponent<MeshRenderer> ();
				chunk.AddComponent<MeshCollider> ();
				chunk.AddComponent<Chunk> ();
				chunk.GetComponent<Chunk> ().position.set(x * chunkSize, 0, z * chunkSize);
				chunk.transform.SetPositionAndRotation (new Vector3(x * chunkSize - chunkSize / 2, 0, z * chunkSize - chunkSize / 2), Quaternion.identity);
			}
		}
	}

}
