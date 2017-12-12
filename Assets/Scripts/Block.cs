using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
	public byte type = 0;
	public int health = 0;

	public BlockMaterial blockMaterial;

	public bool isInside = false;
	public bool isChanged = false;
}
