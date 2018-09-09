using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature_init : MonoBehaviour {

  public GameObject creature;

  public int num_spawn= 100;
  public float xbound = 60f;
  public float ybound = 60f;
  public float height =2.6f;
  // Use this for initialization
  void Start () {
        //creature = this.gameObject();

    //int num_spawn = 100;
    for (int i = 0; i < num_spawn; i++) {
      Instantiate(creature, new Vector3 (Random.Range(-xbound, xbound), height, Random.Range(-ybound, ybound)),  Quaternion.identity );

    }

  }

  // Update is called once per frame
  void Update () {
      if(GameObject.FindGameObjectsWithTag("Creature").Length < num_spawn){
            Instantiate(creature, new Vector3 (Random.Range(-xbound, xbound), height, Random.Range(-ybound, ybound)),  Quaternion.identity );
        }
  }
}
