using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam_movement : MonoBehaviour {
    UnityEngine.GameObject body;
    public float camspeed;
	// Use this for initialization
	void Start () {
        body = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            //Debug.Log("theutwe");
            body.transform.position += body.transform.forward;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            body.transform.position += body.transform.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            body.transform.position -= body.transform.right;
        }

        if (Input.GetKey(KeyCode.S))
        {
            body.transform.position -= body.transform.forward;
        }
    }
}
