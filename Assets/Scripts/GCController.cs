using UnityEngine;
using System.Collections;
using System.Diagnostics; 

public class GCController : InteractiveObject {

	/* missile */
	Stopwatch shootPowerTimer;

	float currentPower;

	// Use this for initialization
	void Start () {
		setup ();
		shootPowerTimer = new Stopwatch ();
//		Camera.main.transform.LookAt (viewAngle);
	}
	
	// Update is called once per frame
	void Update () {
		inputHandler ();
		if (isWalking) {
			move();
		}
		alignGCOrientation ();
		
//		if (life <= 0) {
//			topController.deathNotification(this.team);
//			Destroy(gameObject);
//		}

	}

	public override void move() {
		print ("move!!!!!");

		Vector3 camDir = Camera.main.transform.forward;
		Vector3 moveDir = new Vector3(camDir.x, 0, camDir.z);
		moveDir.Normalize();

//		print ("moveDir " + moveDir);
		
		Vector3 oldPos = transform.position;
		Vector3 newPos = oldPos + moveDir * Constants.moveSpeedMultiplier;
		transform.position = newPos;
		Camera.main.transform.position = transform.position + transform.forward;
	}

	void alignGCOrientation() {
		
		Vector3 camDir = Camera.main.transform.forward;
		Vector3 CGForward = new Vector3(camDir.x, 0, camDir.z);
		CGForward.Normalize();

		transform.forward = CGForward;

		viewAngle = camDir;
		Camera.main.transform.position = transform.position + transform.forward;

	}

	public void changeMovementState() {
		print ("changeMovementState");

		isWalking = isWalking ? false : true;
		print ("isWalking: " + isWalking);
	}

	public void shootWithPowerTime(long powerTime) {
		print ("shootWithPowerTime: " + powerTime);

		float power;
		power = Mathf.Min(Constants.maxPower, powerTime / 10f);
		power = Mathf.Max(Constants.minPower, power);
		print ("shoot with power: " + power);
		this.shoot(power);
		this.currentPower = 0;
	}

	public void updatePower(long powerTime) {
		this.currentPower = powerTime / 100;
//		print ("currentPower: " + currentPower);


	}

	public float getCurrentPower() {
		return this.currentPower;
	}
	
	// for testing only
	void translate(Vector3 dir) {
		Vector3 oldPos = transform.position;
		Vector3 newPos = oldPos + dir * 0.1f;
		transform.position = newPos;
		
		// TODO: set the camera position and view to match the GC's
		Camera.main.transform.position = transform.position;
	}

	void inputHandler() {
		
		if (Input.GetKey ("d")) {
			this.translate (transform.right);
		}
		
		if (Input.GetKey ("a")) {
			this.translate (-transform.right);
		}
		
		if (Input.GetKey ("s")) {
			this.translate (-transform.forward);
		}
		
		if (Input.GetKey ("w")) {
			this.translate (transform.forward);
		}
		
		if (Input.GetKey ("r")) {
			this.move ();
		}

		if (Input.GetKey ("right")) {
			Camera.main.transform.Rotate (new Vector3 (0, 2, 0));
		}
		
		if (Input.GetKey ("left")) {
			Camera.main.transform.Rotate (new Vector3 (0, -2, 0));
		}
		
		if (Input.GetKey ("down")) {
			Camera.main.transform.Rotate (new Vector3 (2, 0, 0));
		}
		
		if (Input.GetKey ("up")) {
			Camera.main.transform.Rotate (new Vector3 (-2, 0, 0));
		}

		
		// shooting
//		if (Input.GetKeyDown ("z")) {
		//			shootPowerTimer.Reset();
		//			shootPowerTimer.Start();
//			print ("prepare shooting");
//		} else if (Input.GetKeyUp ("z")) {
		//			long powerTime = shootPowerTimer.ElapsedMilliseconds;
//			float power;
//			power = Mathf.Min(40f, powerTime / 100f);
//			power = Mathf.Max(6f, power);
//			print ("shoot with power: " + power);
//			this.shoot(power);
//		}
	}
}
