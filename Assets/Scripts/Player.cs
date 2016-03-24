using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	float TiltSpeed = 3f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		var tilt = 0f;

		if (Input.GetKey(KeyCode.A))
		{
			tilt += 1f;
		}
		if (Input.GetKey(KeyCode.D))
		{
			tilt -= 1f;
		}

		transform.RotateAround(Vector3.forward, tilt * Time.deltaTime * TiltSpeed);

	}
}
