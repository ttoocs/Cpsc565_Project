
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NeuralNet;

public class Eye_script : MonoBehaviour {
    
    public float radius = 15f;
    public static int num_return = 2;      //Number of nearest things to return
    public bool localized = true;   //Localize to the parent body.
    
    private GameObject creature;
    private GameObject ground;
    
    public  GameObject[] goodHits;

    public Matrix last;
    public string last_str;
  
    private bool setup = false;
	void Start () {
        if(setup)
            return;
        creature = this.gameObject.transform.parent.gameObject;
        //Debug.Log(creature.GetComponent<Creature_script_main>().myBodyParts);
        ground =  GameObject.Find("ground");
		setup = true;
	}
	
	// Update is called once per frame
	//void FixedUpdate() {
	//    Debug.Log(look());	
	//}
    
    public static int ret_size(){
        return num_return *3;
    }
    
    public Matrix look(){
    
        //TODO: Rotation of the gameObject (?)
            //Ie: if a/the mouth is facing us or not. (Could just be a 0 or and 1).

        if(!setup)
            Start();

        last = new Matrix(1,num_return*3,false);  //Matrix-esk return values. (For NN) (3 values per position: xyz)
        
        Vector3 center = this.gameObject.transform.position;
        if(localized)
            center = this.transform.parent.position;
        
        Collider[] hitColliders = Physics.OverlapSphere(center,radius);//,this.gameObject.layer,QueryTriggerInteraction.Collide); //COLLIDES WITH TRIGGERS
        
        // Sort HitColliders by proximity.
        hitColliders = hitColliders.OrderBy((Collider arg) => (arg.transform.position - creature.transform.position).magnitude).ToArray();
        

        goodHits = new GameObject[num_return];


        //loop things:
        Collider col;
        int i=0;    //HitColliders index
        int j=0;    //Good_hits index
        while(i < hitColliders.Length && j<goodHits.Length){
            col = hitColliders[i];
            //if(creature.GetComponent<Creature_script_main>().myBodyParts.Contains(col.gameObject)){
                //Other "good" conditions here.
                if(col.gameObject.tag == "Creature"){
                if(col.gameObject != ground && col.gameObject != creature){
                    goodHits[j] = col.gameObject;
                    j++;
                }
                }
            //}

            i++;
        }
        //Put it in the matrix:

       
        for(int x = 0; x < j*3; x+=3){
            Vector3 fun = goodHits[x/3].gameObject.transform.position;
            if(localized)
                //fun = fun-creature.transform.position;
                fun = creature.transform.InverseTransformDirection(fun);  //This seems like it's more like I want, but idk.
                
            last[0,x]   = fun[0];
            last[0,x+1] = fun[1];
            last[0,x+2] = fun[2];

        }
        
        //last_str = last.ToString();   //Good for debugging.
        return(last);
    }       

    public static GameObject newEye(){
        Vector3 scale = new Vector3(0.1f,0.1f,0.1f);
        GameObject eye =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
        eye.GetComponent<Transform>().localScale = scale;
        //TODO: Check for other scales n things
        //TODO: Raycast n things (in a script?)
        eye.AddComponent(typeof(Eye_script));
        return eye;
    }
}
