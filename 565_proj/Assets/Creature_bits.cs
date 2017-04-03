using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature_bits : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    ///EYES
    
    public static GameObject newEye()
    {
        Vector3 scale = new Vector3(0.1f,0.1f,0.1f);
        GameObject eye =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
        eye.GetComponent<Transform>().localScale = scale;
        //TODO: Check for other scales n things
        //TODO: Raycast n things (in a script?)
        eye.AddComponent(typeof(Eye_script));
        return eye;
    }
    
    

}
