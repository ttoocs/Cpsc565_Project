#define ADD_MOUTH
#define ADD_EYE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature_script_main : MonoBehaviour {

    public ParticleSystem deathParticles;
    public GameObject mouthPart;
    private List<GameObject> myBodyParts;

    private float age;
    private float health;
    private float food;

    private float max_age;
    private float max_health;
    private float max_food;

    //variables related to the damage method
    private Color trueColor;
    private float damageDuration = 0.3f;
    private float timeOfDamage;

    private Rigidbody body;

    //TODO: Health  - Apprently done by camilo
    //TODO: Energy  - Apprently done by camilo
    //TODO: EYES - Raycast
    //TODO: "EAT" (Passive or Active, or both)
    //TODO: Reproduce - apprently done by camilo (buggy?)
    //TODO: Correct the spawnthing (??)
    //TODO: Add mutations
    //TODO: Add some base food source (vegitation).
    //TODO: RNN brain.

	void Start () {

        myBodyParts = new List<GameObject>();

        body = GetComponent<Rigidbody>();
		//body.velocity = new Vector3 (1,0,0);
        body.velocity = (new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100))).normalized * 10;
        
        #if ADD_EYE
            GameObject eye = Creature_bits.newEye();
            AddBodyPart(eye, (this.transform.forward+new Vector3(0,.4f,0)));
        #endif

        #if ADD_MOUTH
            GameObject mouth = Instantiate(mouthPart);
            AddBodyPart(mouth,this.transform.forward);
#endif

        max_age = 300*12*2; //300 ~= 5secs, *12 make sit about a minute
        max_health = 100;
        max_food = 10000;

        age = 0;
        food = max_food/2;
        health = max_health;

        trueColor = GetComponent<Renderer>().material.color;
    } 
	
    //Add's a bodypart!
    void AddBodyPart(GameObject part){
        AddBodyPart(part,new Vector3(0,0,0));
    }

    void AddBodyPart(GameObject part, Vector3 pos_offset){
        this.myBodyParts.Add(part); //Add it to the list of parts.
        part.transform.position = this.transform.position + pos_offset; //Put it in it's place
        part.transform.parent = this.transform; //Attach it.
    }

    void Update()
    {
        displayDamage();
    }


    // Update is called once per frame
    void FixedUpdate () {
        //brain.
        brain();
        food--;
        age++;
        if (food < 0)
        {
            food = 0;
            takeDamage(1);
        }
        if (health <= 0 || age > max_age)
            die();
        if (food > max_food*0.7 && health > max_health/2)
            reproduce();
		/*if (body.position.y > 1) {
			Debug.Log ("Flying");
		}*/
	}

    //this one is a prototype
    void brain()
    {
        if (Random.Range(0, 100) < 10)
        {
            body.velocity =  Quaternion.AngleAxis(Random.Range(-45,45), Vector3.up)*body.velocity;
           // body.velocity = new Vector3(0,0,10);
        }
		if (body.velocity != new Vector3 (0, 0, 0)) {
			transform.rotation = Quaternion.LookRotation (body.velocity);
		}
    }

    void displayDamage()
    {
        if (Time.time < timeOfDamage+damageDuration)
        {
            Color color = GetComponent<Renderer>().material.color;
            color.r = 1;
            color.g *= 0.5f;
            color.b *= 0.5f;
            GetComponent<Renderer>().material.color = color;
        }

        else
        {
            GetComponent<Renderer>().material.color = trueColor;
        }
    }

    void reproduce()
    {
        Instantiate(gameObject);
    }

    public void nomNomNom(int foodAmount)
    {
        food += foodAmount;
        if (food > max_food)
            food = max_food;
    }

    void die()
    {
        Instantiate(deathParticles, this.transform.position, Quaternion.identity);

        foreach (GameObject bodyPart in myBodyParts)
            Destroy(bodyPart);
        Destroy(gameObject);
    }

    public void takeDamage(int damage)
    {
        timeOfDamage = Time.time;
        health -= damage;
    }
		
}
