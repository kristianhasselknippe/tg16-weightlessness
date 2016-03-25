using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {



	TerrainManager TM;
	bool simulate = true;

	void Start()
	{
		TM = GameObject.Find("TerrainManager").GetComponent<TerrainManager>();
		lastTangent = TM.GetTangentAtX(transform.position.x);

		SetInBallMode(InBallMode);
	}

	bool InBallMode = false;
	bool InPlaneMode { get { return !InBallMode; } }

	void SetInBallMode(bool t)
	{
		InBallMode = t;
		var br = GameObject.Find("Ball").GetComponent<Renderer>();
		var pr = GameObject.Find("Plane").GetComponent<Renderer>();
		if (InBallMode)
		{
			br.enabled = true;
			pr.enabled = false;
		}
		else
		{
			br.enabled = false;
			pr.enabled = true;
		}
	}

	void HandleInput()
	{
		var t = 0f;
		if (Input.GetKey(KeyCode.A))
			t += 1f;
		if (Input.GetKey(KeyCode.D))
			t -= 1f;
		if (Input.GetKey(KeyCode.R))
		{
			Velocity = Vector3.zero;
			simulate = !simulate;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			SetInBallMode(!InBallMode);
		}

		Tilt += t * TiltSpeed * Time.deltaTime;
		Tilt = Mathf.Clamp(Tilt, -1.4f, 1.4f);
		transform.rotation = Quaternion.AxisAngle(new Vector3(0,0,1), Tilt);
	}

	Vector3 EstimateClosestPoint(float groundHeight)
	{
		var distanceFromGround = transform.position.y - groundHeight;
		var from = transform.position.x - distanceFromGround;
		var to = transform.position.x + distanceFromGround;
		var nSamples = 20;
		var lowestDist = float.MaxValue;
		Vector3 lowestDistPoint = Vector3.zero;
		for (int i = 0; i < nSamples; i++)
		{
			var x = Mathf.Lerp(from,to,i/(float)nSamples);
			var y = TM.GetHeightForX(x);
			var p = new Vector2(x,y);
			var dist = Vector2.Distance(p, new Vector2(transform.position.x,transform.position.y));
			if (dist < lowestDist)
			{
				lowestDist = dist;
				lowestDistPoint = p;
			}
		}
		return new Vector3(lowestDistPoint.x, lowestDistPoint.y,0f);
	}

	float TiltSpeed = 3f;
	float Tilt = 0f;
	Vector3 Normal = new Vector3(0,-1,0);

	Vector3 Velocity = Vector3.zero;
	Vector3 Acceleration = Vector3.zero;
	float Damping = 0.4f;
	float Drag = 1f;

	float lockinThreshold = 40f;
	bool lockedIn = false;
	Vector3 lastTangent;

	Vector3 WindDirection = new Vector3(-1,0,0);
	float WindSpeed = 1f;

	float SimulationFactor = 1f;

	void Update () {
		if (simulate)
		{
			Velocity += Acceleration * Time.deltaTime;
			transform.position += Velocity * Time.deltaTime * SimulationFactor;
		}

		Acceleration = Vector3.zero;
		HandleInput();
		//Gravity
		Acceleration += new Vector3(0f,-15f,0f);

		var pos = transform.position;
		var groundHeight = TM.GetHeightForX(pos.x);
		var groundPos = new Vector3(pos.x,groundHeight,0);
		var groundNormal = TM.GetNormalAtX(pos.x);
		var groundTangent = TM.GetTangentAtX(pos.x);
		var playerNormal = Quaternion.Euler(0,0,Tilt * Mathf.Rad2Deg) * Vector3.down;

		var closestPoint = EstimateClosestPoint(groundHeight); //move in when no debug is needed
		if (pos.y < groundHeight)
		{
			Debug.Log("Collided " + Time.deltaTime);
			transform.position = closestPoint; //impulse resolution

			var angle = Vector3.Angle(groundTangent, Velocity);

			if (angle < lockinThreshold)
			{
				lockedIn = true;
			}


			if (lockedIn) //slide
			{
				var magnitude = Velocity.magnitude;
				Velocity = groundTangent * magnitude;
				Acceleration = Vector3.zero;

			}
			else // bounce
			{
				var reflectionVector = Vector3.Reflect(Velocity, groundNormal);
				Velocity = reflectionVector * Damping;
				Acceleration = Vector3.zero;
			}
		}

		if (lockedIn)
		{
			if (groundTangent.y < lastTangent.y)
			{
				Debug.Log("Release lock in" + Time.deltaTime);
				lockedIn = false;
			}
		}



		if (InPlaneMode)
		{
			var windResistance = -Velocity.normalized;
			var windResistanceReflection = Vector3.Reflect(windResistance, playerNormal).normalized;
			var windResistanceAngle = Mathf.Abs(Vector3.Dot(playerNormal, Velocity.normalized));
			var windResistancePush = -windResistanceReflection;
			Debug.DrawLine(pos, pos + windResistancePush, Color.green);
			var windAcceleration = windResistancePush * Drag * windResistanceAngle * Velocity.magnitude;
			Acceleration += windAcceleration + windResistance * Drag * Velocity.magnitude;
		}


		lastTangent = groundTangent;

		//DebugExtension.DebugPoint(closestPoint, Color.black);
		//Debug.DrawLine(groundPos, groundPos + groundNormal, Color.red); //Draw normal
		Debug.DrawLine(groundPos, groundPos + groundTangent, Color.red); //Draw tangent
		Debug.DrawLine(pos, pos + playerNormal, Color.green); //Player normal
	}
}
