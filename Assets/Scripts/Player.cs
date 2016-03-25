using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {



	TerrainManager TM;

	void Start()
	{
		TM = GameObject.Find("TerrainManager").GetComponent<TerrainManager>();
		lastTangent = TM.GetTangentAtX(transform.position.x);
	}

	bool InBallMode = true;
	bool InPlaneMode { get { return !InBallMode; } }

	void HandleInput()
	{
		var t = 0f;
		if (Input.GetKey(KeyCode.A))
			t += 1f;
		if (Input.GetKey(KeyCode.D))
			t -= 1f;
		if (Input.GetKeyDown(KeyCode.Space))
		{
			InBallMode = !InBallMode;
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
	float Damping = 0.4f;

	float lockinThreshold = 40f;
	bool lockedIn = false;
	Vector3 lastTangent;

	Vector3 WindDirection = new Vector3(-1,0,0);
	float WindSpeed = 1f;

	void Update () {
		HandleInput();
		//Gravity
		Velocity += new Vector3(0f,-0.3f,0f);

		var pos = transform.position;
		var groundHeight = TM.GetHeightForX(pos.x);
		var groundPos = new Vector3(pos.x,groundHeight,0);
		var groundNormal = TM.GetNormalAtX(pos.x);
		var groundTangent = TM.GetTangentAtX(pos.x);

		var closestPoint = EstimateClosestPoint(groundHeight); //move in when no debug is needed
		if (pos.y < groundHeight)
		{
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
			}
			else // bounce
			{
				var reflectionVector = Vector3.Reflect(Velocity, groundNormal);
				Velocity = reflectionVector * Damping;
			}
		}

		if (lockedIn)
		{
			if (groundTangent.y < lastTangent.y)
				lockedIn = false;
		}


		if (InPlaneMode)
		{

		}

		transform.position += Velocity * Time.deltaTime;
		lastTangent = groundTangent;

		DebugExtension.DebugPoint(closestPoint, Color.black);
		Debug.DrawLine(groundPos, groundPos + groundNormal, Color.red); //Draw normal
		Debug.DrawLine(groundPos, groundPos + groundTangent, Color.red); //Draw tangent
	}
}
