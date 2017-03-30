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

}
