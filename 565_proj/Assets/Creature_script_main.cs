#define ADD_MOUTH
#define ADD_EYE
//#define HIVE_MIND
#define MODULUS_MIND
//#define AGE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NeuralNet;

public class Creature_script_main : MonoBehaviour {

    public ParticleSystem deathParticles;
    public GameObject mouthPart;
    public List<GameObject> myBodyParts; //Public used in eyes.

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

    private GameObject eye; //Because we need a better system.

    //TODO: Health  - Apprently done by camilo
    //TODO: Energy  - Apprently done by camilo
    //TODO: EYES - Raycast
    //TODO: "EAT" (Passive or Active, or both)
    //TODO: Reproduce - apprently done by camilo (buggy?)
    //TODO: Correct the spawnthing (??)
    //TODO: Add mutations
    //TODO: Add some base food source (vegitation).
    //TODO: RNN brain.

    #if HIVE_MIND
    public static NeuralNet.Network mind;
   
    #elif MODULUS_MIND
    public static int cur_mind=0;   //Used for brain distribution.
    public static int max_mind=100;  //brains!
    public static NeuralNet.Network[] minds;    //All the brains!
    public NeuralNet.Network mind;  //A brain.

    #else
    public NeuralNet.Network mind;
    #endif
        
	void Start () {

        //MIND:
#if MODULUS_MIND
        //ASSUMES SINGLE_THREADED. (ELSE PUT A LOCK HERE)

        if(minds == null){
            minds = new NeuralNet.Network[max_mind];
            for(int i=0; i < max_mind; i++){
                mind = new NeuralNet.Network(new int[] {Eye_script.ret_size(), 32,8, 5},
                                                    NeuralNet.Misc.sigmoidNF()); //Create a neural-network with input size for the eyes, and 4 outputs.

                //if(mind == null)
                //    Debug.Log("Mind is nulL!");
                minds[i] = mind;
            }
        }
        
        if(mind == null){
            mind = minds[cur_mind];
            cur_mind++;
            cur_mind = cur_mind%max_mind;
        }
        //End locked-area.
        
        //simple_train();
            
#else
        if(mind == null){
            mind = new NeuralNet.Network(new int[] {Eye_script.ret_size(), 32, 5},
                                                    NeuralNet.Misc.sigmoidNF()); //Create a neural-network with input size for the eyes, and 4 outputs.
            //simple_train();

        }
#endif



        myBodyParts = new List<GameObject>();

        body = GetComponent<Rigidbody>();
		//body.velocity = new Vector3 (1,0,0);
        //body.velocity = (new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100))).normalized * 10;
        
#if ADD_EYE
            eye = Creature_bits.newEye();
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
	
        //BRAIN HELPERS
    void simple_train(){
        //Nothing infront -> Don't do much.
        NeuralNet.Matrix zeroin = new NeuralNet.Matrix(1,4*3, new double[,] {{0,0,0,0,0,0,0,0,0,0,0,0}});
        NeuralNet.Matrix zerout = new NeuralNet.Matrix(1,4, false);
        for(int i=0; i<10; i++){
            mind.Forward(zeroin);
            mind.Backward(zerout);
        }
    }

    void train_good(float weight=2){
        mind.Backward(((mind.Forward(eye.GetComponent<Eye_script>().last))-0.5)*weight); ////TODO: Fix, something close to zero should go nearer zero!
    }

    
    void train_bad(float weight=1f){
        //mind.Forward(eye.GetComponent<Eye_script>().last); //Re-load inputs.
        NeuralNet.Misc.randomize_sub = 0.5;
        NeuralNet.Misc.randomize_weight = weight; 
        mind.Backward(NeuralNet.Matrix.Transform(NeuralNet.Misc.randomize,mind.Forward(eye.GetComponent<Eye_script>().last)));  //DO ANYTHING ELSE!
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
        #if AGE
            age++;
        #endif
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
        NeuralNet.Matrix ret = mind.Forward(eye.gameObject.GetComponent<Eye_script>().look()); //Dirty hack to get the input from the eye... isn't great..

        //Velocity = (brainoutput 1*45)*Velocity + accel
        //body.velocity = Quaternion.AngleAxis((float)(ret[0,0]*45), Vector3.up)*body.velocity + (body.transform.forward * (float)ret[0,1]);   //The rotation is based off the result.
        //food -= (float)(ret[0,1]);  //Accel costs.

        //Rotate
        //transform.rotation = Quaternion.AngleAxis((float)((ret[0,0]-0.5)*1),  Vector3.up)*transform.rotation ;
        
        transform.rotation = new Quaternion((float)(ret[0,1]-0.5)*2*Mathf.PI,(float)(ret[0,2]-0.5)*2*Mathf.PI,(float)(ret[0,3]-0.5)*2*Mathf.PI,(float)(ret[0,4]-0.5)*2*Mathf.PI);
        
        //Move
        body.velocity = (float)(ret[0,0]-0.5)*5f*transform.forward;
        food -= (float)(ret[0,1]);

        /*
        if (Random.Range(0, 100) < 10)
        {
            body.velocity =  Quaternion.AngleAxis(Random.Range(-45,45), Vector3.up)*body.velocity;
           // body.velocity = new Vector3(0,0,10);
        } */
        //Face velocity dir.
		//if (body.velocity != new Vector3 (0, 0, 0)) {
		//	transform.rotation = Quaternion.LookRotation (body.velocity);
		//}
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
        
        train_good();
        

    }

    void die()
    {
        
        train_bad();
        //Instantiate(deathParticles, this.transform.position, Quaternion.identity);

        foreach (GameObject bodyPart in myBodyParts)
            Destroy(bodyPart);
        Destroy(gameObject);
    }

    public void takeDamage(int damage)
    {
        train_bad();
        timeOfDamage = Time.time;
        health -= damage;
    }
		
}
