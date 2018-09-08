//#define MOUTH_COLLIDE_DEBUG 
//#define MOUTH_COLLIDER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth_Behaviour : MonoBehaviour {

    Creature_script_main parentBehaviour;
    GameObject parent;

    // Use this for initialization
    void Start () {
    this.gameObject.name = "Mouth";
    #if MOUTH_COLLIDER
      this.gameObject.AddComponent<Rigidbody>();  //THE FIX YOU NEEDED.
      this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
      this.gameObject.GetComponent<Rigidbody>().useGravity = false;
      this.gameObject.GetComponent<BoxCollider>().isTrigger = false;
#else
      this.gameObject.GetComponent<BoxCollider>().isTrigger = true;
#endif
        parent = this.transform.parent.gameObject;
        parentBehaviour = parent.GetComponent<Creature_script_main>();
    }
  
  // Update is called once per frame
  void Update () {
  }

    //TODO: If they stay in the mouth, take damage/etc.

  #if MOUTH_COLLIDER
  void OnCollisionEnter (Collision collision) 
  #else
  void OnTriggerEnter (Collider collision) 
  #endif
  {
#if MOUTH_COLLIDE_DEBUG
      Debug.Log ("Mouth Collided");
#endif

        /* if (collision.gameObject == this.transform.parent.gameObject) 
         {
             Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
         }*/
        if (collision.gameObject.tag == parent.tag)
        {
            int damage = 1;
            collision.gameObject.GetComponent<Creature_script_main>().takeDamage(damage);
            parentBehaviour.nomNomNom(damage);
        }

  }
#if MOUTH_COLLIDER
      void OnCollisionStay(Collision collision){OnCollisionEnter(collision);    }
#else
      void OnTriggerStay(Collider collision){OnTriggerEnter(collision);}
#endif

}
