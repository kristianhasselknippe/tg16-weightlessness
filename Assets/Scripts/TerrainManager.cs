using UnityEngine;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour {


	float SegmentLength = 5;
	float Interval = 0.1f;

	List<TerrainSegment> segments = new List<TerrainSegment>();

	float currentX = 0;

	public void Extend()
	{
		var ts = new TerrainSegment(currentX,
									SegmentLength,
									Interval,
									segments.Count);

		segments.Add(ts);
		//ts.gameObject.transform.parent = transform;

		var height = 0.0f;
		foreach (var segment in segments)
			height += segment.Height;

		Debug.Log("extending to: " + height);
		var pos = new Vector3(currentX, 0, 0);
		ts.gameObject.transform.position = pos;

		//note(hassel): This is perhaps not the best idea
		/*var fillerPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
		var fps = new Vector3(SegmentLength,20,1);
		fillerPlane.transform.localScale = fps;
		fillerPlane.transform.position =

			new Vector3(currentX + (fps.x/2), (fps.y/2)-height);*/

		currentX = currentX + SegmentLength;
	}

	public void AddSegmentsUntilAtIndex(int i)
	{
		while (segments.Count <= i)
		{
			Extend();
		}
	}

	public TerrainSegment GetSegmentAtX(float x)
	{
		var index = Mathf.FloorToInt(x / SegmentLength);

		if (index >= segments.Count)
		{
			AddSegmentsUntilAtIndex(index);
			Debug.Log("adding segment");
		}
		//Debug.Log("SegmentsCoun: " + segments.Count);
		return segments[index];
	}

	public float GetHeightForX(float x)
	{
		return GetSegmentAtX(x).GetHeightForX(x);
	}

	public Vector3 GetTangentAtX(float x)
	{
		return GetSegmentAtX(x).GetTangentAtX(x);
	}
}
