using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
	public int worldSeed;
	public float playerPositionX;
	public float playerPositionY;
	public float playerPositionZ;
	public int[] inventoryItemCounts;
	public byte[] inventoryItems;
	public List<Vector3i> chunkPositions = new List<Vector3i>();
	public List<List<Vector3i>> blockPositions = new List<List<Vector3i>>();
	public List<List<byte>> blockTypes = new List<List<byte>>();


	//transform from Dictionary to List data: Dictionaries are not serializable
	public void SetChangedBlocks(Dictionary<Vector3i, Dictionary<Vector3i, byte>> changedBlocks)
	{
		foreach (Vector3i chunkPosition in changedBlocks.Keys)
		{
			//add new chunk position
			this.chunkPositions.Add (chunkPosition);
			int chunkId = this.chunkPositions.Count - 1;
			this.blockPositions.Add (new List<Vector3i> ());
			this.blockTypes.Add (new List<byte> ());
			Dictionary<Vector3i, byte> blocks = changedBlocks [chunkPosition];

			foreach (Vector3i blockPosition in blocks.Keys)
			{
				//add next block position
				this.blockPositions [chunkId].Add (blockPosition);
				//add next block type
				byte blockType = blocks [blockPosition];
				this.blockTypes [chunkId].Add (blockType);
			}
		}
	}

	//transform from List to Dictionary data: Dictionaries are not serializable
	public Dictionary<Vector3i, Dictionary<Vector3i, byte>> GetChangedBlocks()
	{
		Dictionary<Vector3i, Dictionary<Vector3i, byte>> result = new Dictionary<Vector3i, Dictionary<Vector3i, byte>> ();

		for (int c = 0; c < this.chunkPositions.Count; c++)
		{
			Vector3i chunk = this.chunkPositions [c];
			result.Add (chunk, new Dictionary<Vector3i, byte>());

			for (int b = 0; b < this.blockPositions[c].Count; b++)
			{
				Vector3i block = this.blockPositions[c][b];
				byte type = this.blockTypes[c][b];
				result [chunk].Add (block, type);
			}
		}
		return result;
	}

}
