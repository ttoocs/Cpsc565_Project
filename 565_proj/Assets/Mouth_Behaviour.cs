using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth_Behaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.gameObject.name = "Scott said this";
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
