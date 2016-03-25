using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class MyCamera : MonoBehaviour {
	float Acceleration = 8f;

	GameObject Target;

	void Start()
	{
		Target = GameObject.Find("Player");
	}

	void Update () {
		var targetPos = new Vector3(Target.transform.position.x,
									Target.transform.position.y,
									transform.position.z);
		transform.position += (targetPos - transform.position) * Acceleration * Time.deltaTime;
	}
}
