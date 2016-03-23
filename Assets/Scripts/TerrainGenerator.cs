using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour {
	Vector3[] verts;
    Vector2[] uvs;
    int[] tris;


	delegate float HeightFunction(float x);


	public float Length = 20;
	public float Height = 2;
	public float Interval = 0.1f;

	void GenrateGeo(HeightFunction heightFun, params HeightFunction[] funs)
	{
		var nSegments = (int)(Length / Interval);
		var nVerts = (int)(nSegments * 2);


		verts = new Vector3[nVerts];
		uvs = new Vector2[nVerts];
		var lowestY = float.MaxValue;
		for (var i = 0; i < nVerts; i+=2)
		{
			var x = i * Interval;
			var yLow = heightFun(x);
			if (funs.Length > 0)
			{
				foreach (var hf in funs)
					yLow += hf(x);
			}
			var yHigh = yLow + Height;

			verts[i] = new Vector3(x, yHigh, 0);
			verts[i+1] = new Vector3(x, yLow, 0);

			uvs[i] = new Vector2(x,0);
			uvs[i+1] = new Vector2(x,1);

			if (yLow < lowestY)
				lowestY = yLow;
		}

		for (int i = 1; i < nVerts - 2; i+=2)
		{
			verts[i].y = lowestY;
		}


		var nTris = (nSegments * 2) - 2;
		tris = new int[nTris * 3];


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

	void RegenerateGeo()
	{
		GenrateGeo((x) => {
				return Mathf.Sin(x) * Mathf.Cos(x / 10) * Mathf.Tan(x/4);
			},(x) => {
				return Length - x;
			});
		RenderTexture();
        Mesh mesh = new Mesh();

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
		mesh.RecalculateNormals();

		GetComponent<MeshFilter>().mesh = mesh;
	}

    void Start()
	{

		RegenerateGeo();

		//var tex = Resources.Load("stonetexture") as Texture2D;



		var mat = new Material(Shader.Find("Sprites/Default"));
		mat.mainTexture = texture;


		GetComponent<MeshRenderer>().material = mat;



    }

	// Update is called once per frame
	void Update ()
	{

	}
}
