using UnityEngine;
using System.Collections;

public class MissleScript : MonoBehaviour {

	float power;

	int owner;		//	0 is gc

	public void setPower(float power) {
		this.power = power;
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setOwner(int owner) {
		this.owner = owner;
	}

	public int getOwner() {
		return this.owner;
	}

	void OnCollisionEnter(Collision collision) {
		
		// Hitting the ground...
		if (collision.gameObject.tag == "Plane") {
			print ("colliding with the plane");

		} else if (collision.gameObject.tag == "Player") {
			
		} else if (collision.gameObject.tag == "AI") {

		}
		
		GameObject[] AIs = GameObject.FindGameObjectsWithTag("AI");
		for (int i = 0;i < AIs.Length;i++) {
			AIController aiController = AIs[i].GetComponent<AIController>();
			aiController.checkDamage(new Vector2(transform.position.x, transform.position.z), power, owner);
		}
		GameObject gc = GameObject.FindGameObjectWithTag("Player");
		if (gc != null) {
			GCController gcController = gc.GetComponent<GCController> ();
			if (gcController != null)
				gcController.checkDamage (new Vector2 (transform.position.x, transform.position.z), power, owner);
		}

		StartCoroutine ("BombEffect");
	}
	
	IEnumerator BombEffect() {
		gameObject.transform.rotation = Quaternion.identity;
		Destroy (gameObject.GetComponent<SphereCollider> ());
		Destroy (gameObject.GetComponent<Rigidbody> ());
		for (float f = 0; f <= 1f; f += 0.1f) {
			Vector3 oldScale = gameObject.transform.localScale;
			Vector3 newScale = new Vector3(oldScale.x + power / 100, oldScale.y, oldScale.z + power / 100);
			gameObject.transform.localScale = newScale;
			yield return null;
		}
		Destroy (gameObject);
	}
}
