using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateBlock : MonoBehaviour {


	static void CreateMesha (MenuCommand menuCommand)
	{
		GameObject gameObject = new GameObject ("Happy");
		GameObjectUtility.SetParentAndAlign (gameObject, menuCommand.context as GameObject);
		Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

		Block[,,] blocks = new Block[1,1,1];
		blocks[0, 0, 0] = new Block();
		blocks [0, 0, 0].type = 1;
		MeshArchitect meshArchitect = new MeshArchitect(1, blocks);

		Mesh mesh = new Mesh();
		mesh.vertices = meshArchitect.vertices.ToArray();
		mesh.triangles = meshArchitect.triangles.ToArray();
		mesh.RecalculateNormals ();
		mesh.uv = meshArchitect.uv.ToArray();
//
//		MeshFilter meshFilter = new MeshFilter();
//		meshFilter.mesh = mesh;

		gameObject.AddComponent<MeshFilter> ();
		gameObject.GetComponent<MeshFilter>().mesh = mesh;

//		GetComponent<MeshRenderer> ().material = Resources.Load ("Materials/block4") as Material;
//		AssetDatabase.CreateAsset (meshFilter, "Assets/666.asset");
	}

	[MenuItem("GameObject/Create Block Mesh")]
	static void CreateMesh()
	{
		string path = EditorUtility.SaveFilePanelInProject ("Save Procedural Mesh", "Procedural Mesh", "asset", "");
		if (path == "")
			return;
		Block[,,] blocks = new Block[1,1,1];
		blocks[0, 0, 0] = new Block();
		blocks [0, 0, 0].type = 1;
		MeshArchitect meshArchitect = new MeshArchitect(1, blocks);

		Mesh mesh = new Mesh();
		mesh.vertices = meshArchitect.vertices.ToArray();
		mesh.triangles = meshArchitect.triangles.ToArray();
		mesh.RecalculateNormals ();
		mesh.uv = meshArchitect.uv.ToArray();

		AssetDatabase.CreateAsset (mesh, path);
	}
}
