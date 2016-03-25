using UnityEngine;
using System.Collections;

public class TerrainSegment
{

	public GameObject gameObject;
	TerrainGenerator TerrainGenerator;

	public readonly float From;
	public readonly float To;

	public readonly float SegmentLength;
	public readonly float Interval;

	public float Height
	{
		get { return TerrainGenerator.Height; }
	}

	//note(hassel): Height function
	public float GetHeightForX(float x)
	{
		return Mathf.Sin(x*0.5f) * 2 + Mathf.Sin(x/5)*3;
	}

	public Vector3 GetTangentAtX(float x1)
	{
		var x2 = x1 + Interval;
		var y1 = GetHeightForX(x1);
		var y2 = GetHeightForX(x2);
		return (new Vector3(x2,y2,0) - new Vector3(x1,y1,0)).normalized;
	}

	public TerrainSegment(float from,
						  float segmentLength,
						  float interval,
						  int index)
	{
		From = from;
		To = from + segmentLength;
		SegmentLength = segmentLength;
		Interval = interval;
		TerrainGenerator = new TerrainGenerator(From, segmentLength, interval);

		gameObject = new GameObject("segment" + index);

		var meshFilter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		var meshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

		TerrainGenerator.InitGeo(meshFilter, GetHeightForX);
		TerrainGenerator.InitTexture(meshRenderer);

	}
}
