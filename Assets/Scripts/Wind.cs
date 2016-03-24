﻿using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {

	float WindSpeed = 0.3f;
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


	void UpdatePlaneMode()
	{
		var tilt = player.Tilt;

		//var speedAgainstWindDirection;
		var angleAgainstWindDirection =
			Vector3.AngleBetween(Direction, player.Normal);

		var windReflectionVector = Vector3.Reflect(Direction, player.Normal).normalized;
		var windReflectionForce = -windReflectionVector  * WindSpeed * Mathf.Clamp(playerBody.Velocity.magnitude,0,40) * 0.1f;

		Debug.DrawLine(player.transform.position,
					   player.transform.position + windReflectionForce,
					   Color.red);

		var windDirectForce = Direction * WindSpeed;

		playerBody.ApplyForce(windReflectionForce); //Reflection vector
		playerBody.ApplyForce(windDirectForce);
	}

	void UpdateBallMode()
	{
		var windDirectForce = Direction * WindSpeed;
		playerBody.ApplyForce(windDirectForce);
	}

	void Update ()
	{

		if (playerBody.InBallMode)
		{
			UpdateBallMode();
		}
		else
		{
			UpdatePlaneMode();
		}




	}
}
