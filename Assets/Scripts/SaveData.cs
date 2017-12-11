using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
	public int worldSeed;
	public float playerPositionX;
	public float playerPositionY;
	public float playerPositionZ;
	public Dictionary<Vector3i, Dictionary<Vector3i, byte>> changedBlocks;

	public void clean()
	{
		this.worldSeed = 0;
		this.playerPositionX = 0;
		this.playerPositionY = 0;
		this.playerPositionZ = 0;
		this.changedBlocks = null;
	}
}
