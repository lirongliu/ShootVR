using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utility : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static float dist2(Vector2 a, Vector2 b) {
		return Mathf.Sqrt (Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
	}

	public static HashSet<GameObject> getAllPlayers() {
		GameObject[] AIs = GameObject.FindGameObjectsWithTag("AI");
		GameObject gc = GameObject.FindGameObjectWithTag("Player");

		HashSet<GameObject> hs = new HashSet<GameObject> ();
		for (int i = 0; i < AIs.Length; i++) {
			hs.Add(AIs[i]);
		}
		hs.Add (gc);
		return hs;
	}
	
	public static HashSet<GameObject> getAllPlayersInTeam(int team) {
		GameObject[] AIs = GameObject.FindGameObjectsWithTag("AI");
		GameObject gc = GameObject.FindGameObjectWithTag("Player");
		
		HashSet<GameObject> hs = new HashSet<GameObject> ();
		for (int i = 0; i < AIs.Length; i++) {
			AIController aiController = AIs [i].GetComponent<AIController> ();
			if (aiController.getTeam() == team) {
				hs.Add(AIs[i]);
			}
		}
		if (gc != null) {
			GCController gcController = gc.GetComponent<GCController> ();
			if (gcController != null) {
				if (gcController.getTeam () == team) {
					hs.Add (gc);
				}
			}
		}
		return hs;
	}
}
