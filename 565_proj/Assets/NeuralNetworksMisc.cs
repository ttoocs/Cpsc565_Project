using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace NeuralNet
{
	//Allows passing through a function that takes a double.
	public delegate double ActivationFunction(double x);
   

	//A wrapper for two functions.
	public class Neural_func{
		public ActivationFunction func;
		public ActivationFunction deriv;
		public Neural_func(ActivationFunction forward, ActivationFunction back){
			this.func = forward;
			this.deriv = back;
		}
	}

	public class Misc
	{
		//Contains Misc things that are nice to have.

		//Neural_func of sigmoid:
		//public Neural_func Neural_sigmoid = new Neural_func(sigmoid, sigmoid_deriv);

		public static double sigmoid(double val)
		{
			return 1 / (1 + Math.Pow(Math.E, -val));
		}
		public static double sigmoid_deriv(double val)
		{
			return sigmoid(val) * (1 - sigmoid(val));
		}

	}

    //Assuming 1D input array

	//Simple 1D-input Layer class.
	public class Layer
	{
		Neural_func func;	//The function given to it.
		Matrix nodes;	//Cache of values.
		Matrix weights;
        Matrix x;
        int batchsize;
		//Matrix last_value.

        
		public Layer(int in_size, int out_size, Neural_func function, int batchsize=1)
		{
			func = function;
            this.batchsize = batchsize; //By default 1.
			nodes = new Matrix(batchsize, out_size,false); //No need to randomize the node-values (For standard NN)
			weights = new Matrix(in_size, out_size,true);   //Random weights!
            //Need to randomize weights.
		}

		//Forward Propagation.
		public Matrix Forward(Matrix x)
		{
            if(x.Rows != batchsize){
                throw new Exception("Batch-size is inconsistant.");
            }
            if(x.Columns != weights.Rows){
                throw new Exception("Inconsistant number of inputs for forward.");
            }
            this.x = x; //Saves the input for back-propagation.

			nodes = Matrix.Transform(func.func, x* weights);    //Saves nodes post-activation.

			return nodes;

		} 

		//Backward Propagation.
		public Matrix Backwards(Matrix y, Boolean learn=true)
		{
            if(y.Rows != batchsize){
                throw new Exception("Batch-size is inconsistant.");
            }
            if(y.Columns != nodes.Rows){
                throw new Exception("Inconsistant number of inputs for backward.");
            }

            //This only works on the last layer. >:
			Matrix err = y - nodes; //Only supports "last"

			Matrix delta = err * Matrix.Transform(func.deriv,err); //Adjust for the slope


			if (learn)
			{
				weights = weights + (Matrix.Transpose(x) * delta); //Apply the values.
			}

			return delta; //Query: How does this get set?
		}


	}


}
