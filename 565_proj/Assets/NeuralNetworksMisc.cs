#define ADD_BIAS
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
            #if ADD_BIAS
                weights = new Matrix(in_size + 1, out_size,true);
            #else
			    weights = new Matrix(in_size, out_size,true);   //Random weights!
            #endif
            //Need to randomize weights.
		}

		//Forward Propagation.
		public Matrix Forward(Matrix xin)
		{

            //Add-in a 1 value to each row? -> New Matrix?!
            //this.x = x; //Saves the input for back-propagation.
            #if ADD_BIAS
                this.x = Matrix.AddBias(xin);
            #else
                this.x = xin;
            #endif
            if(x.Rows != batchsize){
                throw new Exception("Batch-size is inconsistant.");
            }
            
            if(x.Columns != weights.Rows){
                throw new Exception("Inconsistant number of inputs for forward.");
            }

            

			nodes = Matrix.Transform(func.func, x* weights);    //Saves nodes post-activation.

			return nodes;

		} 

		//Backward Propagation.
		public Matrix Backwards(Matrix yin, Boolean end=false, Boolean learn=true)
		{
            Matrix y = yin;
            #if ADD_BIAS
                if(!end){
                    y = Matrix.RemoveBias(yin);
                }
            #endif
            if(y.Rows != batchsize){
                throw new Exception("Batch-size is inconsistant.");
            }
            if(y.Columns != nodes.Columns){
                Debug.Log("Y: "+y.Columns+" nodes: "+nodes.Columns);
                throw new Exception("Inconsistant number of inputs for backward.");
            }

            Matrix err;
            Matrix delta;

            //This only works on the last layer. >:
            if(end){
    			err = y - nodes; //Only supports "last"
            }else{
                err = y; //Takes in error from prior result. 
            }
    	    delta = Matrix.HadamardProd(err , Matrix.Transform(func.deriv,nodes)); //Adjust for the slope
     
    		if (learn)
    		{
    			weights = weights + (Matrix.Transpose(x) * delta); //Apply the values.
    		}
               
    		return delta * Matrix.Transpose(weights);               
            
		}


	}

    public class Network{
        Layer[] Layers;

        //Sizes: The size of A: input, <hidden networks>, output

        public Network(int[] sizes, Neural_func function, int batchsize=1){

            Layers = new Layer[sizes.Length - 1];

            for(int i = 0; i < Layers.Length ; i++){   //Generates all the layers. (Minus one for the output).
                Layers[i] = new Layer(sizes[i],sizes[i+1],function,batchsize);
            }
        }

        public Matrix Forward(Matrix x){
            Matrix carry = x;
            for(int i=0; i< Layers.Length; i++){
                carry = Layers[i].Forward(carry);
            }
            return carry;
        }

        public Matrix Backward(Matrix y){
            Matrix carry = y;
            if(Layers[Layers.Length -1] == null){
                Debug.Log("ffs");
            }
            carry = Layers[Layers.Length-1].Backwards(carry,true);
            for(int i=Layers.Length - 2 ; i > 0; i--){
                //Debug.Log("At: "+i);
                carry = Layers[i].Backwards(carry);
            }
            return carry;
        }
    
    }

}
