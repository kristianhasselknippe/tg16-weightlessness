using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class Camera : MonoBehaviour {

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


	public float Speed = 3;

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
	}

	void UpdateHandlers()
	{
		foreach (var h in keys.Keys)
			keys[h].Update();

	}

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
		var newPos =  transform.position + (dir.normalized * Time.deltaTime * Speed);
		transform.position = newPos;
	}
}
