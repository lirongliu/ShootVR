using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class InputManager : MonoBehaviour {
	
	Stopwatch touchTimer;
	GameObject gc;
	GCController gcController;

	bool beingClicked;

	public int delayToStartShooting = 1000;


	// Use this for initialization
	void Start () {
		beingClicked = false;
		touchTimer = new Stopwatch ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gcController == null) {
			gc = GameObject.FindGameObjectWithTag("Player");
			if (gc == null) return;
			gcController = gc.GetComponent<GCController>();
			if (gcController == null) return;
		}

		if (Input.touchCount > 0) {
			if (Input.GetTouch(0).phase == TouchPhase.Began) {
//				print ("touch Began");
				touchTimer.Reset();
				touchTimer.Start();
			} else if (Input.GetTouch(0).phase == TouchPhase.Stationary) {
				
				if (touchTimer.ElapsedMilliseconds > delayToStartShooting) {
					gcController.updatePower(touchTimer.ElapsedMilliseconds);
				}
//				print ("touch Stationary");

			} else if (Input.GetTouch(0).phase == TouchPhase.Ended) {
//				print ("time: " + touchTimer.ElapsedMilliseconds);
				if (touchTimer.ElapsedMilliseconds > delayToStartShooting) {
					gcController.shootWithPowerTime(touchTimer.ElapsedMilliseconds);
				} else {	//	changing movement state
					gcController.changeMovementState();
				}
//				print ("touch Ended");

			}
		}

		if (Input.GetMouseButton (0) || Input.GetKey("z")) {
			if (beingClicked == false) {
				touchTimer.Reset();
				touchTimer.Start();
				beingClicked = true;
			} else {
				if (touchTimer.ElapsedMilliseconds > delayToStartShooting) {
					gcController.updatePower(touchTimer.ElapsedMilliseconds);
				}
			}
		} else if (Input.GetMouseButtonUp (0) || Input.GetKeyUp("z")) {
			if (touchTimer.ElapsedMilliseconds > delayToStartShooting) {
				gcController.shootWithPowerTime(touchTimer.ElapsedMilliseconds);
			} else {	//	changing movement state
				gcController.changeMovementState();
			}
			beingClicked = false;
		}
	}
}
