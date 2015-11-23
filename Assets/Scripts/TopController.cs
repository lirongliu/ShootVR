using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class TopController : MonoBehaviour {
	
	public int numOfPlayersOnEachSide;
	
	private int[] curNumOfPlayersOnTeam;
	public int numOfTeams = 2;		//	max: 5
	
	public float initialMinDistance = 15;
	
	bool isEnd;

	bool resettingPlayer;

	static float totalTime = 40.0f;
	private float startTime;
	private float elapsedTime;

	Stopwatch timer;

	int killed;
	int kills;

//	GameObject frontCanvas;
	
	// Use this for initialization
	void Start () {
		timer = new Stopwatch ();
		killed = 0;
		kills = 0;
		reset ();

	}
	
	// Update is called once per frame
	void Update () {
		if (isEnd) {
			return;
		}

//		if (frontCanvas == null) {
//			frontCanvas = GameObject.FindGameObjectWithTag ("FrontCanvas");
//		}

//		frontCanvas.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 100f;
//		frontCanvas.transform.forward = Camera.main.transform.forward;
//		print ("frontCanvas.transform.position " + frontCanvas.transform.position);
//		print ("frontCanvas.transform.forward " + frontCanvas.transform.forward);

		// check whether the gc has dead
		GameObject gc = GameObject.FindGameObjectWithTag ("Player");
		if (gc == null) {
			print ("die....");
//			end();
			if (resettingPlayer == false) {
				killed++;
				StartCoroutine(PrepareToResetPlayer(0));
			}
//			StartCoroutine(PrepareToRestart());
			return;
		}

		GCController gcController = gc.GetComponent<GCController>();
		
		GameObject powerIndicator;
		powerIndicator = GameObject.FindGameObjectWithTag ("PowerIndicator");
		Text text = powerIndicator.GetComponent<Text> ();

		text.text = "";
		text.text += "Power: " + gcController.getCurrentPower();

		float timeLeft = totalTime - timer.ElapsedMilliseconds / 1000f;
		if (timeLeft <= 0) {
			end ();
		}
			
		text.text += "\n";
//		text.text += "Time: " + timeLeft;
		text.text += "Life: " + (int) gcController.getLife() + "/100";

		text.text += "\n";
		text.text += "Kills: " + kills;
		text.text += "\n";
		text.text += "Times Killed: " + killed;

//		elapsedTime = Time.time - startTime;

		// TODO: make it more reliable
		for (int i = 0; i < numOfTeams; i++) {
			if (curNumOfPlayersOnTeam[i] < numOfPlayersOnEachSide) {

				StartCoroutine(PrepareToAddAI(i));
				curNumOfPlayersOnTeam[i]++;
			}
		}
	}
	
	IEnumerator PrepareToAddAI(int team) {
		yield return new WaitForSeconds(2);
		
		GameObject ai = Instantiate(Resources.Load("AI", typeof(GameObject))) as GameObject;
		ai.transform.position = new Vector3 (Random.Range(-numOfTeams, numOfTeams) * initialMinDistance, 2, Random.Range(-numOfPlayersOnEachSide, numOfPlayersOnEachSide) * initialMinDistance);
		AIController aiController = ai.GetComponent<AIController>();
		aiController.setTeam(team);
	}
	
	IEnumerator PrepareToResetPlayer(int team) {
		resettingPlayer = true;
		yield return new WaitForSeconds(2);
		
		GameObject gc = Instantiate(Resources.Load("Character", typeof(GameObject))) as GameObject;
		gc.transform.position = new Vector3 (Random.Range(-numOfTeams, numOfTeams) * initialMinDistance, 2, Random.Range(-numOfPlayersOnEachSide, numOfPlayersOnEachSide) * initialMinDistance);
		GCController gcController = gc.GetComponent<GCController>();
		gcController.setTeam(team);
		resettingPlayer = false;
	}
	
	IEnumerator PrepareToRestart() {
		yield return new WaitForSeconds(2);
		reset ();
	}
	
	
	public void deathNotification(int team, int owner) {
		curNumOfPlayersOnTeam[team]--;
		if (owner == 0) {
			kills++;
		}
	}
	
	void end() {
		isEnd = true;
		reset ();
	}
	
	void reset() {
		
		Destroy (GameObject.FindGameObjectWithTag ("Player"));
		GameObject[] AIs = GameObject.FindGameObjectsWithTag ("AI");
		for (int i = 0; i < AIs.Length; i++) {
			Destroy(AIs[i]);
		}

		
		numOfPlayersOnEachSide = 3;
		curNumOfPlayersOnTeam = new int[5];
		
		for (int i = 0; i < numOfTeams; i++) {
			curNumOfPlayersOnTeam [i] = numOfPlayersOnEachSide;
		}
		
		for (int i = 0; i < numOfPlayersOnEachSide; i++) {
			for (int j = 0;j < numOfTeams;j++) {
				if (i == 0 && j == 0) {
					GameObject gc = Instantiate(Resources.Load("Character", typeof(GameObject))) as GameObject;
					gc.transform.position = new Vector3 (0, 2, 0);
					GCController gcController = gc.GetComponent<GCController>();
					gcController.setTeam(j);
				} else {
					GameObject ai = Instantiate(Resources.Load("AI", typeof(GameObject))) as GameObject;
					ai.transform.position = new Vector3 (i * initialMinDistance, 2, j * initialMinDistance);
					AIController aiController = ai.GetComponent<AIController>();
					aiController.setTeam(j);
				}
			}
		}
		isEnd = false;
		resettingPlayer = false;

		startTime = 0;

		timer.Reset ();
		timer.Start ();
	}
	
}
