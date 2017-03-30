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
		Neural_func Neural_sigmoid = new Neural_func(sigmoid, sigmoid_deriv);

		public static double sigmoid(double val)
		{
			return 1 / (1 + Math.Pow(Math.E, -val));
		}
		public static double sigmoid_deriv(double val)
		{
			return sigmoid(val) * (1 - sigmoid(val));
		}

	}

	//Simple 1D-input Layer class.
	public class Layer
	{
		Neural_func func;	//The function given to it.
		Matrix nodes;	//Cache of values.
		Matrix weights;
		//Matrix last_value.

		public Layer(int in_size, int out_size, Neural_func function)
		{
			func = function;
			nodes = new Matrix(in_size, out_size);
			weights = new Matrix(out_size, in_size);
		}

		//Forward Propagation.
		public Matrix Forward(Matrix x)
		{
			nodes = x* weights;
			return Matrix.Transform(func.func, nodes);

		}

		//Backward Propagation.
		public Matrix Backwards(Matrix y, Boolean learn=true)
		{
			Matrix err = y - nodes; //Only supports "last"
			Matrix delta = err * Matrix.Transform(func.deriv,err); //Adjust for the slope

			if (learn)
			{
				weights += delta; //Apply (ie, learn)
			}

			return new Matrix(0, 0); //Query: How does this get set?
		}


	}


}
