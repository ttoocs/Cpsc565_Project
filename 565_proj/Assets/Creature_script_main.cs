using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature_script_main : MonoBehaviour {
    // Use this for initialization
    //public ScriptableObject brain;

    public GameObject[] bodyParts;
    private GameObject[] myBodyParts;

    private Rigidbody body;
	private float scale = 0.7f;

	void Start () {
        //	GameObject.Find ("Creature");
		myBodyParts = new GameObject[bodyParts.Length];
        body = GetComponent<Rigidbody>();
		//body.velocity = new Vector3 (1,0,0);
       body.velocity = (new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100))).normalized * 10;
		int c = 0;
        foreach(GameObject o in bodyParts)
        {
            myBodyParts[c] = Instantiate < GameObject >(o);
			myBodyParts [c].transform.position = this.transform.position+this.transform.forward; 
			myBodyParts [c].transform.parent = this.transform;
			myBodyParts [c].transform.localScale = new Vector3(scale, scale, scale);
            c++;
        }
		this.gameObject.name = "Scott said this";
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //brain.
        brain();
	}

    //this one is a prototype
    void brain()
    {
        if (Random.Range(0, 100) < 10)
        {
            body.velocity =  Quaternion.AngleAxis(Random.Range(-45,45), Vector3.up)*body.velocity;
           // body.velocity = new Vector3(0,0,10);
        }
        transform.rotation = Quaternion.LookRotation(body.velocity);
    }
		
}
