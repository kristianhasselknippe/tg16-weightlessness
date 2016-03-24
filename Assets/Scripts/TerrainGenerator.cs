using UnityEngine;
using System.Collections;

public delegate float HeightFunction(float x);

public class TerrainGenerator {
	Vector3[] verts;
    Vector2[] uvs;
    int[] tris;

	float Start;
	float Length;
	float Interval;

	float height;
	public float Height
	{
		get { return height; }
	}

	public TerrainGenerator(float start, float length, float interval)
	{
		Start = start;
		Length = length;
		Interval = interval;
	}

	public void InitGeo(MeshFilter filter, HeightFunction hf)
	{
		GenrateGeo(hf);
        Mesh mesh = new Mesh();

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
		mesh.RecalculateNormals();
		filter.mesh = mesh;
	}

	void GenrateGeo(HeightFunction heightFun)
	{
		var nSegments = Mathf.CeilToInt(Length / Interval);
		Debug.Log("NSegments: "  + nSegments);
		var nVerts = (int)((nSegments+1) * 2);

		verts = new Vector3[nVerts];
		uvs = new Vector2[nVerts];
		var lowestY = float.MaxValue;
		var highestY = float.MinValue;

		for (var i = 0; i < nVerts; i+=2)
		{
			var x = (i/2f) * Interval;
			var yLow = heightFun(x + Start);
			var yHigh = yLow;

			verts[i] = new Vector3(x, yHigh, 0);
			verts[i+1] = new Vector3(x, yLow, 0);

			uvs[i] = new Vector2(x,0);
			uvs[i+1] = new Vector2(x,1);

			if (yLow < lowestY)
				lowestY = yLow;
			if (yHigh > highestY)
				highestY = yHigh;

		}

		height = highestY - lowestY;
		for (int i = 1; i < nVerts; i+=2)
		{
			verts[i].y = lowestY - 30;

		}

		var nTris = (nSegments * 2);
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
		Debug.Log("CurrentTEnd: " + currentT);
	}

	//todo(hassel): this sucks
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
				t.SetPixel(x,y, new Color(1,1,1,1));
					/*246/255f,
										  182/255f,
										  30/255f,
										  1));*/
			}
		}
		t.Apply();
		texture = t;
	}

	public void InitTexture(MeshRenderer meshRenderer)
	{
		RenderTexture();
		var mat = new Material(Shader.Find("Sprites/Default"));
		mat.mainTexture = texture;
		meshRenderer.material = mat;
	}

}
