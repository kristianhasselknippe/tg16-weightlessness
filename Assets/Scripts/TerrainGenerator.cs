using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour {
	public Vector3[] verts;
    public Vector2[] uvs;
    public int[] tris;


	void GenrateGeo()
	{
		var length = 10;
		var height = 2;

		var interval = 1;

		var nSegments = length / interval;
		var nVerts = nSegments * 2;


		verts = new Vector3[nVerts];
		uvs = new Vector2[nVerts];
		for (var i = 0; i < nVerts; i+=2)
		{
			var x = i * interval;
			var y = height;
			verts[i] = new Vector3(x, y, 0);
			verts[i+1] = new Vector3(x, 0, 0);

			uvs[i] = new Vector2(x,0);
			uvs[i+1] = new Vector2(x,1);
		}


		var nTris = (nSegments * 2) - 2;
		tris = new int[nTris * 3];

		Debug.Log("Ntris: " + nTris);
		var currentT = 0;
		for (var i = 0; i < nTris*3; i+=6)
		{
			tris[i] = currentT;
			tris[i+1] = currentT + 1;
			tris[i+2] = currentT + 2;

			tris[i+3] = currentT + 1;
			tris[i+4] = currentT + 3;
			tris[i+5] = currentT + 2;
			currentT+=2;
		}
	}

	Texture2D texture;
	void RenderTexture()
	{
		var w = 10;
		var h = 10;
		var t = new Texture2D(w,h, TextureFormat.ARGB32, false);


		for (var x = 0; x < w; x++)
		{
			for (var y = 0; y < h; y++)
			{
				t.SetPixel(x,y, new Color(
										  246/255f,
										  182/255f,
										  30/255f,
										  1));
			}
		}
		t.Apply();
		texture = t;
	}

    void Start()
	{
		GenrateGeo();
		RenderTexture();
        Mesh mesh = new Mesh();

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
		mesh.RecalculateNormals();


		var tex = Resources.Load("stonetexture") as Texture2D;



		var mat = new Material(Shader.Find("Sprites/Default"));
		mat.mainTexture = texture;


		GetComponent<MeshRenderer>().material = mat;


		GetComponent<MeshFilter>().mesh = mesh;
    }

	// Update is called once per frame
	void Update ()
	{

	}
}
