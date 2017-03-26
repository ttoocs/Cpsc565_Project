using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNN : MonoBehaviour {

    int hidden_size = 100;   //Num of hidden layer neruons
    int seq_length = 25;      //Num of steps to unroll the RNN
    int input_size  = 10;
    int output_size = 10;
    float learning_rate = 0.1f; 
    
    float[,] Wxh; //Input
    float[,] Whh; //Hidden 
    float[,] Why; //Output

    float[,] bh; //Hidden Bias
    float[,] by; //Output Bias
    
	// Use this for initialization
	void Start () {
        Wxh = scale_mult(0.1f, randNbyN(hidden_size, input_size)); //Input
        Whh = scale_mult(0.1f, randNbyN(hidden_size, hidden_size)); //Hidden 
        Why = scale_mult(0.1f, randNbyN(output_size, hidden_size)); //Output

        bh = new float[hidden_size,1];    //Hidden Bias
        by = new float[output_size,1];    //Output Bias.

	}
	
	// Update is called once per frame
	void Update () {
		
	}
   
    //These are dumb and brute-force-esk.
   float[] randN(int len){
        float[] ret = new float[len];
        for( int i=0; i <  len; i++){
            ret[i] = Random.Range(0f,1f);
        }
        return ret;
   }
   float[,] randNbyN(int x, int y){
        float[,] ret = new float[x,y];
        for(int i=0; i < x ; i++){
            for (int j=0; j < y; j++){
                 ret[i,j] = Random.Range(0f,1f);
            }
        }
        return ret;
   }

/*
   float[]  scale_mult(float scaler, float[] vector){
        float[] ret = new float[vector.Length];
        for (int i=0; i < vector.Length; i++){
            ret[i] = vector[i]*scaler;
        }
        return ret;
   } */
   
   float[,]  scale_mult(float scaler, float[,] vector){
        float[,] ret = new float[vector.GetLength(0),vector.GetLength(1)];
        for (int i=0; i < vector.GetLength(0); i++){
            for (int j=0; j < vector.GetLength(1); j++){
                ret[i,j] = vector[i,j]*scaler;
            }
        }
        return ret;
   }

   
    float[,] matrix_mult(float[,] m1, float[,] m2){
        if(m1.GetLength(1) != m2.GetLength(0)){
            Debug.Log("Invalid matrix mult");
            return null;
        }
        float [,] ret = new float[m1.GetLength(0),m2.GetLength(1)];
        for (int i=0; i < ret.GetLength(0); i++){
            for (int j=0; j < ret.GetLength(1); j++){
                ret[i,j] = 0;
                for( int m=0; m < m1.GetLength(1); m++){
                    ret[i,j] += m1[i,m]*m2[m,j];
                //Idk, stuff.
                }
            }
        }     
        return ret;
    }
    float[,] dot(float[,] m1, float[,] m2){
        return null; //Numpy.matrix.dot BS.
    }
}
