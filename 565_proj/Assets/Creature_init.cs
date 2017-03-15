using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature_init : MonoBehaviour {

	public GameObject creature;

	// Use this for initialization
	void Start () {
		for (int i = 0; i <= 200; i++) {
			Instantiate(creature, new Vector3 (Random.Range(-50f, 50f), 0.6f, Random.Range(-50f, 50f)),  Quaternion.identity );// Quaternion.identity );
			//GameObject a = Instantiate(creature);
			//a.transform.position = new Vector3 (Random.Range (-50f, 50f), 0.5f, Random.Range (-50f, 50f));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision collision) 
	{
		Debug.Log ("fml");
		if (collision.gameObject == this.transform.parent.gameObject || collision.gameObject.name=="Scott said this") 
		{
			Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
		}

	}
}
