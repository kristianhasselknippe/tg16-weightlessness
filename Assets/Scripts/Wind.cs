using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {

	float WindSpeed = 1f;
	Vector3 Direction = new Vector3(-1,1,0);

	Player player;
	PlayerBody playerBody;

	// Use this for initialization
	void Start () {
		var playerGO = GameObject.Find("Player");
		if (playerGO != null)
		{
			player = playerGO.GetComponent("Player") as Player;
			playerBody = playerGO.GetComponent("PlayerBody") as PlayerBody;
		}

	}

	// Update is called once per frame
	void Update () {
		var tilt = player.Tilt;

		//var speedAgainstWindDirection;
		var angleAgainstWindDirection =
			Vector3.AngleBetween(Direction, player.Normal);
//		Debug.Log("AngleAgainstWind: " + angleAgainstWindDirection);

		var windReflectionVector = Vector3.Reflect(Direction, player.Normal).normalized;
		var windReflectionForce = -windReflectionVector  * WindSpeed * Mathf.Clamp(playerBody.Velocity.magnitude,0,40) * 0.1f;

		Debug.DrawLine(player.transform.position,
					   player.transform.position + windReflectionForce,
					   Color.red);

		var windDirectForce = Direction * WindSpeed;

		playerBody.ApplyForce(windReflectionForce); //Reflection vector
		playerBody.ApplyForce(windDirectForce);

		//Debug.DrawLine(player.transform.position, player.transform.position + windReflectionVector, Color.white);
		//Debug.DrawLine(player.transform.position, player.transform.position - Direction*WindSpeed, Color.green);



	}
}
