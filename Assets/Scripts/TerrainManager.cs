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

		var height = 0.0f;
		foreach (var segment in segments)
			height += segment.Height;

		Debug.Log("extending to: " + height);
		var pos = new Vector3(currentX, 0, 0);
		ts.gameObject.transform.position = pos;


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
