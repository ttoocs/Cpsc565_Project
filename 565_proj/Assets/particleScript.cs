using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleScript : MonoBehaviour {

    float start_time;
 	// Use this for initialization
	//Matrix cry;

	void Start () {
        start_time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= start_time + 2f)
            Destroy(gameObject);
	}
}
