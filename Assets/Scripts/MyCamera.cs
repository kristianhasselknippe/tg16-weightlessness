using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class MyCamera : MonoBehaviour {

	class KeyHandler
	{
		KeyCode keyCode;
		public bool IsDown { get; private set; }

		public KeyHandler(KeyCode keyCode)
		{
			this.keyCode = keyCode;
			IsDown = Input.GetKey(keyCode);
		}

		public void Update()
		{
			IsDown = Input.GetKey(keyCode);
		}

	}


	float Speed = 10;
	float Acceleration = 0.6f;

	Dictionary<KeyCode,KeyHandler> keys = new Dictionary<KeyCode,KeyHandler>();

	void AddKeyCode(KeyCode k)
	{
		keys.Add(k,new KeyHandler(k));
	}

	// Use this for initialization
	void Start () {
		AddKeyCode(KeyCode.W);
		AddKeyCode(KeyCode.A);
		AddKeyCode(KeyCode.S);
		AddKeyCode(KeyCode.D);
		AddKeyCode(KeyCode.Q);
		AddKeyCode(KeyCode.E);
		AddKeyCode(KeyCode.F);
		AddKeyCode(KeyCode.Space);

		Target = transform.position;
	}

	void UpdateHandlers()
	{
		foreach (var h in keys.Keys)
			keys[h].Update();

	}

	TerrainManager terrainManager;

	Vector3 Target;

	// Update is called once per frame
	void Update () {
		//UpdateHandlers();

		var dir = new Vector3();
		if (Input.GetKey(KeyCode.W))
		{
			dir.y = 1;
		}
		if (Input.GetKey(KeyCode.A))
		{
			dir.x = - 1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			dir.y = -1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			dir.x = 1;
		}

		if (terrainManager == null)
		{
			var tgo = GameObject.Find("TerrainManager");
			if (tgo != null)
			{
				terrainManager = tgo.GetComponent(typeof(TerrainManager)) as TerrainManager;
			}
		}

		var newPos =  Target + (dir.normalized * Time.deltaTime * Speed);
		newPos.y = terrainManager.GetHeightForX(newPos.x);

		Target = newPos;

		transform.position += (Target - transform.position) * Acceleration * Time.deltaTime;

		DebugExtension.DebugPoint(Target, Color.red, 2);
		Debug.DrawLine(Target, Target + terrainManager.GetTangentAtX(Target.x) * 3, Color.blue);

		terrainManager.GetSegmentAtX(Target.x);
	}
}
