using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDatabase : MonoBehaviour
{
	public Material materialAtlas;
	public List<BlockMaterial> blockMaterials = new List<BlockMaterial>();

	public int getDiggable()
	{
		int result = 0;
		foreach (BlockMaterial blockMaterial in this.blockMaterials)
		{
			if (blockMaterial.collider == true && blockMaterial.inventorySprite != null)
			{
				result++;
			}
		}
		return result;
	}
}
