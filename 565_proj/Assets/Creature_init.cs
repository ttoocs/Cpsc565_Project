using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature_init : MonoBehaviour {

	public GameObject creature;

	// Use this for initialization
	void Start () {
        //creature = this.gameObject();
		float xbound = 75f;
		float ybound = 75f;
		int num_spawn = 300;
		for (int i = 0; i <= num_spawn; i++) {
			Instantiate(creature, new Vector3 (Random.Range(-xbound, xbound), 0.6f, Random.Range(-ybound, ybound)),  Quaternion.identity );
		
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
