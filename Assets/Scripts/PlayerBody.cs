using UnityEngine;
using System.Collections;

public class PlayerBody : MonoBehaviour {

	public Vector3 Velocity = new Vector3(70,0,0);
	float InverseMass = 0.5f;

	float Drag = 3000f;
	float Bounciness = 0.7f;


	Vector3 Gravity = new Vector3(0,-3,0);

	float DEBUG_MULTIPLIER = 0.05f;

	TerrainManager terrainManager;

	// Use this for initialization
	void Start () {
		terrainManager = GameObject.Find("TerrainManager").GetComponent(typeof(TerrainManager)) as TerrainManager;
	}



	float PlaneDrag = 3000f;
	float PlaneBounciness = 0.7f;
	float BallDrag = 10000f;
	float BallBounciness = 0.2f;
	public bool InBallMode = false;
	public void ToggleBallMode()
	{
		InBallMode = !InBallMode;
		if (InBallMode)
		{
			Drag = BallDrag;
			Bounciness = BallBounciness;

			var ball = GameObject.Find("Ball");
			ball.GetComponent<Renderer>().enabled = true;
			GameObject.Find("Plane").GetComponent<Renderer>().enabled = false;
		}
		else
		{
			Drag = PlaneDrag;
			Bounciness = PlaneBounciness;
			GameObject.Find("Ball").GetComponent<Renderer>().enabled = false;
			GameObject.Find("Plane").GetComponent<Renderer>().enabled = true;
		}
	}

	void BouncePlane()
	{
		var terrainPos = terrainManager.GetHeightForX(transform.position.x);
		if (transform.position.y <= terrainPos)//we have hit terrain
		{
			var terrainNormal = terrainManager.GetNormalAtX(transform.position.x);
			var reflectionVector = Vector3.Reflect(Velocity, terrainNormal);
			var posAfterImpulse = new Vector3(transform.position.x, terrainPos, 0);
			transform.position = posAfterImpulse;
			Velocity = reflectionVector * Bounciness;
		}
	}


	//void MoveTransformX(float x) { transform.position = new Vector3(x,transform.position.y,0); }
	//void MoveTransformY(float y) { transform.position = new Vector3(transform.position.x,y,0); }
	void MoveTransformBy(float x, float y)
	{
		transform.position =
			new Vector3(transform.position.x + x, transform.position.y + y,0);
	}


	float ballLockinThreshold = 0.2f;
	bool lockedIn = false;
	void BounceBall()
	{
		var terrainPos = terrainManager.GetHeightForX(transform.position.x);
		if (transform.position.y <= terrainPos)//we have hit terrain
		{
			var x = transform.position.x; var y = transform.position.y;
			var distanceInside = terrainPos - y;

			var terrainNormal = terrainManager.GetNormalAtX(x);
			var terrainTangent = terrainManager.GetTangentAtX(x);

			//Impulse resolution
			MoveTransformBy(terrainNormal.x * distanceInside,terrainNormal.y * distanceInside);

			var tangentVelocityAngle = Vector3.AngleBetween(Velocity, terrainTangent);
			Debug.Log("TangentVelAng: " + tangentVelocityAngle);
			if (tangentVelocityAngle < ballLockinThreshold)
			{
				Debug.Log("Locked in");
				lockedIn = true;
				var velocityMagnitute = Velocity.magnitude;
				Velocity = terrainTangent * velocityMagnitute;
			}
			else
			{
				lockedIn = false;
				BouncePlane();
				return;
			}

			var force = terrainTangent * Velocity.magnitude * BallBounciness * 0.1f;
			ApplyForce(force);
		}
	}

	// Update is called once per frame
	void Update () {
		//Toggle ballmode on space
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ToggleBallMode();
		}

		//Apply gravity
		if (!lockedIn)
		{
			ApplyForce(Gravity);
		}
		else
		{
			ApplyForce(new Vector3(Mathf.Clamp(Gravity.x, 0, float.MaxValue),
								   Mathf.Clamp(Gravity.y, 0, float.MaxValue),
								   0));
		}



		//Apply drag
		ApplyForce(-Velocity.normalized *
				   Mathf.Lerp(0f,
							  Velocity.magnitude,
							  (Velocity.magnitude / Drag)*(Velocity.magnitude / Drag)));

		//Bounce against terrain
		if (InBallMode)
		{
			BounceBall();
		}
		else
		{
			BouncePlane();
		}


		transform.position +=
			Velocity * InverseMass * Time.deltaTime * DEBUG_MULTIPLIER;

	}

	public void ApplyForce(Vector3 force)
	{
		//var f = new Vector3(force.x,0,0);
		Velocity += force;
	}
}
