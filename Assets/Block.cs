using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
	//TODO: this should be a BlockMaterial class
	public int type = 0;
	public bool transparent = true;

	public BlockMaterial blockMaterial;

	public bool isInside = false;
	public bool isChanged = false;
}
