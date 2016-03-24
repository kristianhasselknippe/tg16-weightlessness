using UnityEngine;
using System.Collections;

public class PlayerBody : MonoBehaviour {

	public Vector3 Velocity = new Vector3(70,0,0);
	float InverseMass = 0.5f;

	float Drag = 2000f;
	float Bounciness = 0.7f;


	Vector3 Gravity = new Vector3(0,-3,0);

	float DEBUG_MULTIPLIER = 0.05f;

	TerrainManager terrainManager;

	// Use this for initialization
	void Start () {
		terrainManager = GameObject.Find("TerrainManager").GetComponent(typeof(TerrainManager)) as TerrainManager;
	}

	// Update is called once per frame
	void Update () {
		//Apply gravity
		ApplyForce(Gravity);

		//Apply drag
		ApplyForce(-Velocity.normalized *
				   Mathf.Lerp(0f,
							  Velocity.magnitude,
							  (Velocity.magnitude / Drag)*(Velocity.magnitude / Drag)));

		//Bounce against terrain
		var terrainPos = terrainManager.GetHeightForX(transform.position.x);
		if (transform.position.y <= terrainPos)//we have hit terrain
		{
			Debug.Log("Did collide");
			var terrainNormal = terrainManager.GetNormalAtX(transform.position.x);
			var reflectionVector = Vector3.Reflect(Velocity, terrainNormal);
			var posAfterImpulse = new Vector3(transform.position.x, terrainPos, 0);
			transform.position = posAfterImpulse;
			Velocity = reflectionVector * Bounciness;
		}


		transform.position +=
			Velocity * InverseMass * Time.deltaTime * DEBUG_MULTIPLIER;

		//Debug.Log("Velocity: " + Velocity.magnitude);
	}

	public void ApplyForce(Vector3 force)
	{
		//var f = new Vector3(force.x,0,0);
		Velocity += force;
	}
}
