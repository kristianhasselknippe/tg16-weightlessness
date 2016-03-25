using UnityEngine;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour {

	GameObject Player;

	float SegmentLength = 20;
	float Interval = 0.1f;

	List<TerrainSegment> segments = new List<TerrainSegment>();

	float currentX = 0;

	void Start()
	{
		Player = GameObject.Find("Player");
	}

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
		}
		return segments[index];
	}

	public float GetHeightForX(float x)
	{
		return GetSegmentAtX(x).GetHeightForX(x);
	}

	public Vector3 GetNormalAtX(float x)
	{
		var t = GetTangentAtX(x);
		return -(new Vector3(t.y,-t.x,0)).normalized;
	}

	public Vector3 GetTangentAtX(float x)
	{
		return GetSegmentAtX(x).GetTangentAtX(x).normalized;
	}


	void Update()
	{
		var pos = Player.transform.position;
		GetSegmentAtX(pos.x);
	}
}
