using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {

	float Speed;
	Vector3 Direction = new Vector3(0,1,0);

	Player player;
	PlayerBody playerBody;

	// Use this for initialization
	void Start () {
		var playerGO = GameObject.Find("Player");
		if (playerGO != null)
		{
			player = playerGO.GetComponent("Player") as Player;
			playerBody = playerGO.GetComponent("Player") as PlayerBody;
		}

	}

	// Update is called once per frame
	void Update () {
		var tilt = player.Tilt;

		//var speedAgainstWindDirection;
		var angleAgainstWindDirection =
			Vector3.AngleBetween(Direction, player.Normal);
		Debug.Log("AngleAgainstWind: " + angleAgainstWindDirection);

	}
}
