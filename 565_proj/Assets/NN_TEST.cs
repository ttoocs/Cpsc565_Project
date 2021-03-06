using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NeuralNet{

public class NN_TEST : MonoBehaviour {

  // SIGMOID!
   //Neural_func Neural_sigmoid;


  void Start () {

       // Matrix fun = new Matrix(2,3, new double[,]{{0,1,2},{3,4,5}});
        //Matrix f2 = Matrix.Transpose(fun);


        //Debug.Log("Matrix Mult: "+(f2*fun).ToString());
        //Debug.Log("Matrix TMult: "+Matrix.threaded_Mult(f2,fun).ToString());

        Debug.Log("Testing neural network:");
        //not_tefster();
        //Simple_Back();
        //tester_2();
        network_test1();
        network_train1(true);
    }

    Network net1;
    Matrix ndset;
    void network_test1(){
        //net1 = new Network(new int[] {4,8,16,32,16,8,4,1},Neural_sigmoid);
        net1 = new Network(new int[] {4,1},Misc.sigmoidNF());
        //Matrix test1 = new Matrix(1,4,new double[,] {{1,2,3,4}});
        //Debug.Log(net1.Forward(test1).ToString()); //Yay it didn't crash!
        ndset = new Matrix(16,5, new double[,]{ {0,0,0,0,1},
                                                {0,0,0,1,0},
                                                {0,0,1,0,0},
                                                {0,0,1,1,0},
                                                {0,1,0,0,0},
                                                {0,1,0,1,0},
                                                {0,1,1,0,0},
                                                {0,1,1,1,0},
                                                {1,0,0,0,0},
                                                {1,0,0,1,0},
                                                {1,0,1,0,0},
                                                {1,0,1,1,0},
                                                {1,1,0,0,0},
                                                {1,1,0,1,0},
                                                {1,1,1,0,0},
                                                {1,1,1,1,0}});

        ndset = new Matrix(2,5,new double[,]{ {0,0,0,0,1},{1,1,1,1,0}});
        //A big thing, to train weird functions!
    }

    void network_train1(bool prnt = false){
        for(int i =0; i < ndset.Rows ; i++){
            Matrix a = new Matrix(1,4,new double[,] {{ndset.Elements[i,0],ndset.Elements[i,1],ndset.Elements[i,2],ndset.Elements[i,3]}});   //Gets the arguments.
            Matrix b = net1.Forward(a);
            if(prnt)
                Debug.Log("Training...: Input "+a.ToString() +" Target: " +  ndset.Elements[i,4] +" Output: "+ b.ToString() );
            net1.Backward(new Matrix(1,1,new double[,] {{ndset.Elements[i,4]}}));
        }
    }
    //Legacy test globals:
    Layer l1;
    Layer l2;
    public Matrix dset;
    //LEGACY TESTS:
    //A simple 1-layer back-propigation test. (Does it improve? (everything approches 1?))
    void Simple_Back(){
        Matrix v1 = new Matrix(1,1, new double[,]{{1}});
        Matrix r1 = new Matrix(1,3, new double[,]{{1,1,1}});
        //Matrix fun = new Matrix(3,3, new double[,]{{1,1,1},{2,3,4},{5,6,7}});

        //Debug.Log(fun);

        //r1 = Matrix.Transpose(r1);
        l1 = new Layer(1,3,Misc.sigmoidNF());

        Matrix a = l1.Forward(v1);
        Debug.Log(a.ToString());

        l1.Backwards(r1);
        //Debug.Log(r1.ToString());

        a = l1.Forward(v1);
        Debug.Log(a.ToString());
    }

    void tester_2(){
        l1 = new Layer(2,1, Misc.sigmoidNF());
        Matrix dataset_or = new Matrix(4,3, new double[,]{{0,0,0},{0,1,1},{1,0,1},{1,1,1}});
        Matrix dataset_left = new Matrix(4,3, new double[,]{{0,0,0},{0,1,0},{1,0,1},{1,1,1}});
       // Matrix v0 = new Matrix(1,1, new double[,]{{0}});

        dset = dataset_left;

        Debug.Log("Start:");
        train(dset,l1,true);
        for(int i =0; i<10; i++){
            for(int j =0; j<10000; j++)
                  train(dset,l1);
            train(dset,l1,true);
        }
    }

    void train(Matrix dataset, Layer layer,bool prnt=false){
        if(prnt)
            Debug.Log("Training sofar:");
        for(int i=0; i < dataset.Rows ; i++){   //For each row.
            Matrix a = null;
            if(dataset.Columns == 3){
                a = new Matrix(1,2,new double[,] {{dataset.Elements[i,0],dataset.Elements[i,1]}});   //Gets the arguments.
            }
            if(dataset.Columns == 2){
                a = new Matrix(1,2,new double[,] {{dataset.Elements[i,0],dataset.Elements[i,1]}});   //Gets the arguments.
            }
            Matrix b = layer.Forward(a);
            if(prnt)
                Debug.Log("Training...: Input "+a.ToString() +" Target: " + dataset.Elements[i,2] +" Output: "+ b.ToString() );
            layer.Backwards(new Matrix(1,1,new double[,] {{dataset.Elements[i,2]}}));
        }
    }


    void not_tester(){
        l1 = new Layer(1,5,Misc.sigmoidNF());
        l2 = new Layer(5,1,Misc.sigmoidNF());

        not_testcase(true);
        for(int i =0; i<100; i++){ //Training.
            not_testcase();
        }
        not_testcase(true);
    }

    void not_testcase(bool prnt=false){
        //Matrix TestData = new Matrix(2,2, new double[,] { {1,0}{0,1}});
        Matrix v1 = new Matrix(1,1, new double[,]{{1}});
        Matrix v0 = new Matrix(1,1, new double[,]{{0}});

            Matrix a = l2.Forward(l1.Forward(v1)); //Forward
            if(prnt){ Debug.Log("1: forward." + a.ToString());}

            l1.Backwards(l2.Backwards(v0,true));
            //Debug.Log("ffs: "+a.ToString());
            //l1.Backwards(a,false);

            a = l2.Forward(l1.Forward(v0)); //Forward
            if(prnt){ Debug.Log("0: forward."+ a.ToString());}
            l1.Backwards(l2.Backwards(v1,true));



        //}
    }

  // Update is called once per frame
  void Update () {
        for(int i = 0 ; i < 100; i++){
            network_train1();
        }
        network_train1(true);

        //for(int j =0; j < 100; j++){
        //    not_testcase();
        //}
        //not_testcase(true);

    //  for(int j =0; j<1000; j++)
        //          train(dset,l1);
        //    train(dset,l1,true);
  }
}

}
