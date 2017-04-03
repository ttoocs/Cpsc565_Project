
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNet;

public class Eye_script : MonoBehaviour {
    
    public float radius = 40f;
    public static int num_return = 4;      //Number of nearest things to return
    public bool localized = true;   //Localize to the parent body.
    
    private GameObject creature;
    private GameObject ground;
    
    private Collider[] hitColliders;
    public GameObject[] goodHits;
    public Matrix last;
  
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
    
        if(!setup)
            Start();

        last = new Matrix(1,num_return*3,false);  //Matrix-esk return values. (For NN) (3 values per position: xyz)
        
        Vector3 center = this.gameObject.transform.position;
        if(localized)
            center = this.transform.parent.position;
        
        hitColliders = Physics.OverlapSphere(center,radius);//,this.gameObject.layer,QueryTriggerInteraction.Collide); //COLLIDES WITH TRIGGERS
        
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
                if(col.gameObject != ground){
                    goodHits[j] = col.gameObject;
                    j++;
                }
                }
            //}
            

            i++;
        }
        //Put it in the matrix:
        for(int x = 0; x < j*3; x+=3){
            last[0,x]   = goodHits[x/3].gameObject.transform.position[0];
            last[0,x+1] = goodHits[x/3].gameObject.transform.position[1];
            last[0,x+2] = goodHits[x/3].gameObject.transform.position[2];
            if(localized){
                last[0,x]   -= creature.transform.position[0];
                last[0,x+1] -= creature.transform.position[1];
                last[0,x+2] -= creature.transform.position[2];
            }
        }
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
