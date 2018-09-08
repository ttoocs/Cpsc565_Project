using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NeuralNet{

public class RNN : MonoBehaviour {

/*
    int hidden_size = 100;   //Num of hidden layer neruons
    int seq_length = 25;      //Num of steps to unroll the RNN
    int input_size  = 10;
    int output_size = 10;
    float learning_rate = 0.1f;

    Matrix Wxh; //Input
  Matrix Whh; //Hidden
  Matrix Why; //Output

  Matrix bh; //Hidden Bias
  Matrix by; //Output Bias
 */

  // Use this for initialization
  void Start () {

    /*
        Wxh = scale_mult(0.1f, randNbyN(hidden_size, input_size)); //Input
        Whh = scale_mult(0.1f, randNbyN(hidden_size, hidden_size)); //Hidden
        Why = scale_mult(0.1f, randNbyN(output_size, hidden_size)); //Output

        bh = new float[hidden_size,1];    //Hidden Bias
        by = new float[output_size,1];    //Output Bias.
    */
  }

  // Update is called once per frame
  void Update () {

  }

}

}
