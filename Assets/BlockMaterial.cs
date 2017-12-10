using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMaterial : MonoBehaviour
{
	//public int id;

	public bool transparent;
	public bool collider;
	public byte hardness;
	public Vector2 atlasUVs;
	public Sprite inventorySprite;
	private ACubeData cubeData = new CubeData();

	void Awake()
	{
		List<Vector2> UVs = new List<Vector2>();
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		for(int i = 0; i < 4; i++){UVs.Add (this.cubeData.getUVs(this.cubeData.DIR_U, i) + this.atlasUVs);}
		for(int i = 0; i < 4; i++){UVs.Add (this.cubeData.getUVs(this.cubeData.DIR_D, i) + this.atlasUVs);}
		for(int i = 0; i < 4; i++){UVs.Add (this.cubeData.getUVs(this.cubeData.DIR_L, i) + this.atlasUVs);}
		for(int i = 0; i < 4; i++){UVs.Add (this.cubeData.getUVs(this.cubeData.DIR_R, i) + this.atlasUVs);}
		for(int i = 0; i < 4; i++){UVs.Add (this.cubeData.getUVs(this.cubeData.DIR_F, i) + this.atlasUVs);}
		for(int i = 0; i < 4; i++){UVs.Add (this.cubeData.getUVs(this.cubeData.DIR_B, i) + this.atlasUVs);}
		mesh.SetUVs (0, UVs);
	}
}
