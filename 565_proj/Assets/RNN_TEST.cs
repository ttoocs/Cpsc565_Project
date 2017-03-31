using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NeuralNet{

public class RNN_TEST : MonoBehaviour {

	// Use this for initialization
    Neural_func Neural_sigmoid = new Neural_func(Misc.sigmoid, Misc.sigmoid_deriv);
    Layer l1;
    Layer l2;

	void Start () {
        Debug.Log("Creating neural network:");

        l1 = new Layer(2,1, Neural_sigmoid);
        Matrix dataset_or = new Matrix(4,3, new double[,]{{0,0,0},{0,1,1},{1,0,1},{1,1,1}});
        Matrix dataset_left = new Matrix(4,3, new double[,]{{0,0,0},{0,1,0},{1,0,1},{1,1,1}});
       // Matrix v0 = new Matrix(1,1, new double[,]{{0}});
        
        Matrix dset = dataset_left;

        Debug.Log("Start:");
        train(dset,l1,true);
        for(int i =0; i<10; i++){
            for(int j =0; j<1000; j++)
                  train(dset,l1);
            train(dset,l1,true);
        }
            
        //train(dataset_or,l1,true);
    
/* NOT TESTCASE:
		l1 = new Layer(1,3,Neural_sigmoid);
        l2 = new Layer(3,1,Neural_sigmoid);

        Matrix v1 = new Matrix(1,1, new double[,]{{1}});
        Matrix a = l1.Forward(v1);
        Debug.Log("Fun:");
        Debug.Log(a.ToString());
        
        not_testcase(true);
        for(int i =0; i<1000; i++){ //Training.
            not_testcase();
            
        }
        not_testcase(true);
*/
	}

    void train(Matrix dataset, Layer layer,bool prnt=false){
        if(prnt)
            Debug.Log("Training sofar:");
        for(int i=0; i < dataset.Rows ; i++){   //For each row.
            Matrix a = new Matrix(1,2,new double[,] {{dataset.Elements[i,0],dataset.Elements[i,1]}});   //Gets the arguments.
            Matrix b = layer.Forward(a);
            if(prnt)
                Debug.Log("Training...: Input "+a.ToString() +" Target: " + dataset.Elements[i,2] +" Output: "+ b.ToString() );
            layer.Backwards(new Matrix(1,1,new double[,] {{dataset.Elements[i,2]}}));
        }
    }

    void not_testcase(bool prnt=false){
        //Matrix TestData = new Matrix(2,2, new double[,] { {1,0}{0,1}});
        Matrix v1 = new Matrix(1,1, new double[,]{{1}});
        Matrix v0 = new Matrix(1,1, new double[,]{{0}});
 
        //for(int j =0; j<TestData.Rows; j++){
            Matrix a = l2.Forward(l1.Forward(v1)); //Forward
            if(prnt){ Debug.Log("1: forward."); Debug.Log(a.ToString());}
            
            a = l2.Backwards(v0);
            Debug.Log(a.ToString());
            l1.Backwards(a);

            a = l2.Forward(l1.Forward(v0)); //Forward
            if(prnt){ Debug.Log("0: forward."); Debug.Log(a.ToString());}
            l1.Backwards(l2.Backwards(v1));
            
        
            
        //}
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

}