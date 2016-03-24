using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	float TiltSpeed = 3f;
	public float Tilt = 0f;
	public Vector3 Normal = new Vector3(0,-1,0);

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
		Tilt = transform.rotation.eulerAngles.z;

		Normal = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (180-Tilt)),
							 Mathf.Cos(Mathf.Deg2Rad * (180-Tilt)),
							 0);
		Debug.DrawLine(transform.position, transform.position + Normal, Color.red);

	}
}
