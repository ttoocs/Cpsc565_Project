
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NeuralNet;

public class Eye_script : MonoBehaviour {

    public float radius = 15f;
    public static int num_return = 5;      //Number of nearest things to return
    public static int ret_scale = 2;       //Number of elements per thing to return.
    public bool localized = true;          //Localize to the parent body.

    private GameObject creature;
    private GameObject ground;

    public  GameObject[] goodHits;

    public Matrix last;
    public bool getStr = false;
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
        return num_return * ret_scale;
    }

    public Matrix look(){

        //TODO: Rotation of the gameObject (?)
            //Ie: if a/the mouth is facing us or not. (Could just be a 0 or and 1).

        if(!setup)
            Start();

        last = new Matrix(1,num_return*ret_scale,false);  //Matrix-esk return values. (For NN)

        Vector3 center = this.gameObject.transform.position;
        if(localized)
            center = this.transform.parent.position;

        Collider[] hitColliders = Physics.OverlapSphere(center,radius);//,this.gameObject.layer,QueryTriggerInteraction.Collide); //COLLIDES WITH TRIGGERS

        // Sort HitColliders by proximity.
        hitColliders = hitColliders.OrderBy((Collider arg) => (arg.transform.position - creature.transform.position).magnitude).ToArray();


        //loop though results filtering out unwanted entities
        goodHits = new GameObject[num_return];
        Collider col;
        int i=0;    //HitColliders index
        int j=0;    //Good_hits index
        while(i < hitColliders.Length && j<goodHits.Length){
            col = hitColliders[i];
            //if(creature.GetComponent<Creature_script_main>().myBodyParts.Contains(col.gameObject)){
                //Other "good" conditions here.
        if(col.gameObject.tag == creature.gameObject.tag){
                if(col.gameObject != ground && col.gameObject != creature){
                    goodHits[j] = col.gameObject;
                    j++;
                }
                }
            //}

            i++;
        }

        //Put it in the matrix:
        for(int x = 0; x < j*ret_scale; x+=ret_scale){ //Per each chunk-assigment.
          GameObject curObj = goodHits[x/ret_scale]; //Get next element
          
          //Get XYZ:
          Vector3 funPos = curObj.gameObject.transform.position;
          Vector3 funVec = funPos-creature.transform.position;
          Quaternion relative = Quaternion.Inverse(creature.transform.rotation) * curObj.transform.rotation;   //I have no idea what this is.

          //Calculate an angle:
          /*
          double angle = Mathf.Acos(Vector3.Dot(creature.transform.forward,Vector3.Normalize(fun-creature.transform.position)));
          Vector3 c = Vector3.Cross (creature.transform.forward, Vector3.Normalize(fun-creature.transform.position));
          if (c.x * c.y * c.z < 0){
            angle = -angle + 2*Mathf.PI; //Correct the angle
            angle -= Mathf.PI;
          }
          */
          //Vector3 deltaV = curObj - creature.transform.position;
          //double angle = Matf.Atan(Vector3.Project(deltaV,creature.transform.forward).magnitude / deltaV.magnitude);
          double angle = Vector3.SignedAngle(creature.transform.forward, funVec, Vector3.up);
          double nnAngle = (angle / 180.0); // Makes an angle more suitable for NN (range -1 to 1)

          //Vector3 dir = (fun.gameObject.transform.position - transform.position);
          //dir = fun.gameObject.transform.InverseTransformDirection(dir);
          //angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

          //if(localized){
            //fun = fun-creature.transform.position;
          //  funPos = creature.transform.InverseTransformDirection(fun);  //This seems like it's more like I want, but idk.
          //}

          last [0, x] = nnAngle;
          last [0, x+1] = funVec.magnitude / ( (double) radius );
          //last[0,x]   = fun[0];
          //last[0,x+1] = fun[1]; //Manually encode the colum
          //last[0,x+2] = fun[2];

      ///*
          //last[0,x+3] = relative[0];
          //last[0,x+4] = relative[1];
          //last[0,x+5] = relative[2];
          //last[0,x+6] = relative[3];
          //*/

        }
        if(getStr){
          last_str = last.ToString();   //Good for debugging.
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
