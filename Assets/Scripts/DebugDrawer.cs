using UnityEngine;
using System.Collections;

public class DebugDrawer : MonoBehaviour {

	TerrainManager terrainManager;
	GameObject Player;

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		if (terrainManager == null)
		{
			var tgo = GameObject.Find("TerrainManager");
			if (tgo != null)
			{
				terrainManager = tgo.GetComponent(typeof(TerrainManager)) as TerrainManager;
			}
		}
	}

	// Update is called once per frame
	void Update () {

		var playerPos = Player.transform.position;
		var heightAtX = terrainManager.GetHeightForX(playerPos.x);

		var groundPos = new Vector3(playerPos.x,heightAtX,0);

		DebugExtension.DebugPoint(groundPos, Color.red, 2);
		Debug.DrawLine(groundPos, groundPos + terrainManager.GetTangentAtX(playerPos.x) * 3, Color.blue);
		Debug.DrawLine(groundPos, groundPos + terrainManager.GetNormalAtX(playerPos.x) * 3, Color.red);
	}
}
