using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class GridScript : MonoBehaviour {

	public int xSize, ySize;

	private Vector3[] vertices;

	float fixW;
	// Use this for initialization
	void Start () {
		Debug.Log (Camera.main.transform.position);
		Debug.Log (Camera.main.transform.forward);
		gameObject.transform.position += Camera.main.transform.forward * 200;
		gameObject.transform.forward = Camera.main.transform.forward;

		var mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		// 网格自动计算法线向量
		mesh.RecalculateNormals();

		fixW = 300.0f;
		StartCoroutine(Timer());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Awake ()
	{
		
	}
	IEnumerator Timer() {
		while (true) {
			yield return new WaitForSeconds(0.05f);
			//Debug.Log(string.Format("Timer2 is up !!! time=${0}", Time.time));
			fixW -= 2.0f;
			if (fixW <= 0.0f)fixW = 300.0f;
			Generate();
		}
	}
	private void Generate () 
	{
		var mat = gameObject.GetComponent<MeshRenderer>().material;
		
		float tilingW = fixW/(float)mat.mainTexture.width;
		mat.SetTextureScale("_MainTex", new Vector2(tilingW, 1));
		
		
		vertices = new Vector3[4];
		vertices [0] = new Vector3 (0.0f, 0.0f, 0.0f);
		vertices [1] = new Vector3 (fixW, 0.0f, 0.0f);
		vertices [2] = new Vector3 (0.0f, (float)mat.mainTexture.height, 0.0f);
		vertices [3] = new Vector3 (fixW, (float)mat.mainTexture.height, 0.0f);

		Vector2[] uv = new Vector2[vertices.Length];
		uv [0] = new Vector2 (0, 0);
		uv [1] = new Vector2 (1, 0);
		uv [2] = new Vector2 (0, 1);
		uv [3] = new Vector2 (1, 1);
	
		var mesh = GetComponent<MeshFilter>().mesh;
		if(null != mesh)
		{
			mesh.Clear();
		}
		
		//mesh.name = "Procedural Grid";

		mesh.vertices = vertices;
		mesh.uv = uv;

		int[] triangles = new int[6];
		triangles [0] = 0;
		triangles [1] = 2;
		triangles [2] = 1;

		triangles [3] = 2;
		triangles [4] = 3;
		triangles [5] = 1;

		mesh.triangles = triangles;

		

		
	}

	private void Generate3 () 
	{
		int repeatCntW = 3;

		var mat = gameObject.GetComponent<MeshRenderer>().material;
		Debug.Log("mat");
		Debug.Log( mat.mainTexture.width);
		Debug.Log( mat.mainTexture.height);
		Debug.Log(mat.GetTextureScale("_MainTex"));
		Debug.Log(mat.GetTextureOffset("_MainTex"));
		Debug.Log("mat");
		
		mat.SetTextureScale("_MainTex", new Vector2(repeatCntW, 1));
		//Debug.Log( mat.mainTexture.width);
		//Debug.Log( mat.mainTexture.height);
		
		vertices = new Vector3[4];
		vertices [0] = new Vector3 (0.0f, 0.0f, 0.0f);
		vertices [1] = new Vector3 (repeatCntW * mat.mainTexture.width, 0.0f, 0.0f);
		vertices [2] = new Vector3 (0.0f, mat.mainTexture.height, 0.0f);
		vertices [3] = new Vector3 (repeatCntW * mat.mainTexture.width, mat.mainTexture.height, 0.0f);

		Vector2[] uv = new Vector2[vertices.Length];
		uv [0] = new Vector2 (0, 0);
		uv [1] = new Vector2 (1, 0);
		uv [2] = new Vector2 (0, 1);
		uv [3] = new Vector2 (1, 1);

		var mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		//mesh.name = "Procedural Grid";

		mesh.vertices = vertices;
		mesh.uv = uv;

		int[] triangles = new int[6];
		triangles [0] = 0;
		triangles [1] = 2;
		triangles [2] = 1;

		triangles [3] = 2;
		triangles [4] = 3;
		triangles [5] = 1;

		mesh.triangles = triangles;

		// 网格自动计算法线向量
		mesh.RecalculateNormals();

		Debug.Log (Camera.main.transform.position);
		Debug.Log (Camera.main.transform.forward);
		gameObject.transform.position += Camera.main.transform.forward * 200;
		gameObject.transform.forward = Camera.main.transform.forward;


		
	}

	private void Generate2 () 
	{
		vertices = new Vector3[(xSize + 1) * (ySize + 1)];
		Vector2[] uv = new Vector2[vertices.Length];


		for (int i = 0, y = 0; y <= ySize; y++)
		{
			for (int x = 0; x <= xSize; x++, i++)
			{
				vertices[i] = new Vector3(x, y);
				uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
			}
		}

		var mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		//mesh.name = "Procedural Grid";

		mesh.vertices = vertices;
		mesh.uv = uv;

		int[] triangles = new int[xSize * ySize * 6];
		for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
			for (int x = 0; x < xSize; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
				triangles[ti + 5] = vi + xSize + 2;
			}
		}
		mesh.triangles = triangles;

		// 网格自动计算法线向量
		mesh.RecalculateNormals();
	}

	private void OnDrawGizmos () 
	{
		if (vertices == null)
			return;
		
		Gizmos.color = Color.black;
		for (int i = 0; i < vertices.Length; i++) 
		{
			Gizmos.DrawSphere(vertices[i], 0.1f);
		}
	}
}
