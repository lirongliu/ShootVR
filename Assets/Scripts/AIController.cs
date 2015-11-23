using UnityEngine;
using System.Collections;
using System.Diagnostics; 
using System.Collections.Generic;

public class AIController : InteractiveObject {

	Stopwatch shootTimer;
	long shootTimeInterval;

	bool readyToChangeMovement;	//	1/4: stay still, 1/4: walk to their teammates, 1/4: walk to a specific target, 1/4: random direction
	int movementState;

	Vector3 randomeDirection;
	Vector3 moveDir;

	// Use this for initialization
	void Start () {
		setup ();

		shootTimer = new Stopwatch ();
		shootTimer.Start ();

		shootTimeInterval = Random.Range (6000, 12000);


		readyToChangeMovement = true;
		movementState = 0;

	}

	// Update is called once per frame
	void Update () {

		if (readyToChangeMovement) {
			StartCoroutine (randomWalk ());
		} else {
			move ();
		}
		if (shootTimer.ElapsedMilliseconds > shootTimeInterval) {
			// shoot...
			GameObject enemy = getRandomEnemy();
			if (enemy != null) {
				Vector2 myPlaneVec = new Vector2(transform.position.x, transform.position.z);
				Vector2 enemyPlaneVec = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
				float dist = Utility.dist2(myPlaneVec, enemyPlaneVec);
				Vector2 pointingVec = enemyPlaneVec - myPlaneVec;
				Vector3 shootDir = new Vector3(pointingVec.x * Random.Range(0.8f, 1.2f), Random.Range(dist / 1.3f, dist), pointingVec.y * Random.Range(0.8f, 1.2f));
				shootDir.Normalize();

				viewAngle = shootDir;

				moveDir = new Vector3(shootDir.x, 0, shootDir.z);

				shoot(Random.Range(60, 300));
				shootTimer.Reset ();
				shootTimer.Start ();
				shootTimeInterval = Random.Range (6000, 12000);
			}
		}
	}

	void updateMoveDir() {
		//		HashSet<GameObject> allPlayers = Utility.getAllPlayers ();
		
		if (movementState == 0) {
			return;
		} else if (movementState == 1) {		// walk to their teammates
			HashSet<GameObject> allTeammates = Utility.getAllPlayersInTeam (team);		//	assume there are only 2 teams
			Vector3 posSum = Vector3.zero;
			foreach (GameObject player in allTeammates) {
				posSum += player.transform.position;
			}
			moveDir = posSum / allTeammates.Count - transform.position;
		} else if (movementState == 2) {		//	walk to a specific target
			GameObject enemy = getRandomEnemy();
			if (enemy != null) {
				moveDir = enemy.transform.position - transform.position;

			}

		} else if (movementState == 3) {		//	random direction
			moveDir = randomeDirection;
		}
	}

	override public void move() {

		moveDir.Normalize();
		if (moveDir != Vector3.zero) {
			transform.forward = moveDir;
		}
		
		Vector3 oldPos = transform.position;
		Vector3 newPos = oldPos + moveDir * Constants.moveSpeedMultiplier;
		transform.position = newPos;

	}
	
	IEnumerator randomWalk() {
		readyToChangeMovement = false;

		movementState = Random.Range (0, 4);
		if (movementState == 3) {
			randomeDirection = new Vector3(Random.Range (-1, 1), 0, Random.Range (-1, 1));
		}
		updateMoveDir ();

		yield return new WaitForSeconds (Random.Range(2, 6));
		readyToChangeMovement = true;
	}

	GameObject getRandomEnemy() {
		HashSet<GameObject> allEnemies = Utility.getAllPlayersInTeam (1 - team);
		int idx = Random.Range(0, allEnemies.Count - 1);
//		print ("count: " + allEnemies.Count);
		int count = 0;
		GameObject defaultEnemy = null;
		foreach (GameObject player in allEnemies) {
			defaultEnemy = player;
			if (count >= idx) {
				return player;
			}
			count++;
		}
		return defaultEnemy;
	}
}
