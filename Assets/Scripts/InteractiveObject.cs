using UnityEngine;
using System.Collections;

public abstract class InteractiveObject : MonoBehaviour {
	
	protected Vector3 viewAngle;
	protected float life;
	protected int team;		//	0 or 1

	protected bool isWalking;
	
	protected TopController topController;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float getLife() {
		return life;
	}

	public void setup() {
		isWalking = false;
		life = 100;
		
		Camera.main.transform.position = transform.position;
		
		GameObject gameController = GameObject.FindGameObjectWithTag ("GameController");
		topController = gameController.GetComponent<TopController> ();
	}

	public void setTeam(int team) {
		this.team = team;
		Renderer renderer = GetComponent<Renderer> ();
		renderer.material.color = Constants.teamColors [team];
	}

	public int getTeam() {
		return team;
	}

    public abstract void move();

	// power: 40 ~ 300
	public virtual void checkDamage(Vector2 center, float power, int owner) {
		float dist = Utility.dist2 (center, new Vector2 (transform.position.x, transform.position.z));
//		print ("center: " + center);
//		print ("position: " + transform.position);
//		print ("dist: " + dist + " power: " + power);
		float damage = power / (1 + Utility.dist2 (center, new Vector2 (transform.position.x, transform.position.z)));
		damage = damage >= 10f ? damage : 0;
		life -= damage;
//		if (damage > 0)
//			print ("damage: " + damage);

		
		if (life <= 0) {
			topController.deathNotification(this.team, owner);
			Destroy(gameObject);
		}
	}

	public void shoot(float power) {
		GameObject missile = Instantiate(Resources.Load("Missile", typeof(GameObject))) as GameObject;
		MissleScript ms = missile.GetComponent<MissleScript> ();
		if (this.tag == "Player") {
			ms.setOwner (0);
		} else {
			ms.setOwner (-1);
		}
		ms.setPower (power);
		missile.transform.position = transform.forward + gameObject.transform.position;
		Rigidbody rd = missile.GetComponent<Rigidbody>();
		if (rd != null) {
			rd.velocity = viewAngle * Mathf.Sqrt(power);
		}
	}
}
