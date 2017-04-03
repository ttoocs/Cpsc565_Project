using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Collections;
//using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace NeuralNet
{

   	public class Matrix 
   	{
      private double[,] matrix;
      private int rows, columns;

      public int Columns
      {
         get { return columns; }
      }
      
      public int Rows
      {
         get { return rows; }
      }

      public double[,] Elements
      {
         get { return matrix; }
      }
      #if __MonoCS__
         //Nothin
      #else
         [System.Runtime.CompilerServices.IndexerName("Matrix")]
      #endif
      public double this[int x, int y]
      {
         get
         {
            if(x < rows && y < columns && x >= 0 && y >= 0)
            {
               return matrix[x, y];
            }
            else
               throw new IndexOutOfRangeException();
         }
         set
         {
            if (x < rows && y < columns && x >= 0 && y >= 0)
            {
               matrix[x, y] = value;
            }
            else
               throw new IndexOutOfRangeException();
         }

      }
        

      public Matrix(int rows, int columns, params double[] elements)
      {
         this.rows = rows;
         this.columns = columns;
         matrix = new double[rows,columns];
         if (elements.Length != rows * columns)
            throw new Exception();
         for (int i = 0; i < rows; i++)
         {
            for (int y = 0; y < columns; y++)
            {
               matrix[i,y] = elements[ columns*i + y];
            }
         }
      }

      public Matrix(int rows, int columns, double[,] elements)
      {
            this.rows = rows;
            this.columns = columns;
            matrix = new double[rows, columns];
            if (elements.GetLength(0) != rows || elements.GetLength(1) != columns)
                throw new Exception("ROWS NOT EQUAL TO THE ELEMENTS");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = elements[i,j];
                }
            }

      }

      public Matrix(int rows, int columns, bool randomize=true)
      {
         this.rows = rows;
         this.columns = columns;
         matrix = new double[rows, columns];
         //initilzed to 0.0
         

         if(randomize){
            System.Random rnd= NeuralNet.Misc.rnd;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = 2*rnd.NextDouble() -1f; //Range of (-1 to 1);
                }
            }
        }
      }


      public bool SetElement(int xCord, int yCord, double val)
      {
         if(xCord < rows && yCord < columns)
         {
            matrix[xCord, yCord] = val;
            return true;
         }
         return false;
      }

      public override string ToString()
      {
         string temp = "";
         for (int i = 0; i < rows; i++)
         {
            temp += "[";
            for (int j = 0; j < columns; j++)
            {
               temp += matrix[i, j] + ",";
            }
            temp += "]\n";
         }
         return temp;
      }

      public static Matrix Transform(ActivationFunction function, Matrix mat1)
      {
         Matrix tempMat = new Matrix(mat1.Rows, mat1.Columns);
         for (int i = 0; i < mat1.Rows; i++)
         {
            for (int j = 0; j < mat1.Columns; j++)
            {
               tempMat[i, j] = function(mat1[i, j]);
            }
         }
         return tempMat;
      } 

      public static Matrix Transpose(Matrix mat1)
      {
         Matrix temp = new Matrix(mat1.Columns, mat1.Rows);
         for (int i = 0; i < mat1.Rows; i++)
         {
            for (int j = 0; j < mat1.Columns; j++)
            {
               temp[j, i] = mat1[i, j];
            }
         }
         return temp;
      }

      public static Matrix HadamardProd(Matrix mat1, Matrix mat2)
      {
         if(mat1.Rows == mat2.Rows && mat1.Columns == mat2.Columns)
         {
            Matrix temp = new Matrix(mat1.Rows, mat1.Columns);
            for (int i = 0; i < temp.Rows; i++)
            {
               for (int j = 0; j < temp.Columns; j++)
               {
                  temp[i, j] = mat1[i, j] * mat2[i, j];
               }
            }
            return temp;
         }
		//	return null;
         throw new Exception("Matrix sizes arn't the same!");
      }

      public static Matrix operator- (Matrix mat1, Matrix mat2)
      {
        
         if(mat1.Rows == mat2.Rows && mat1.Columns == mat2.Columns)
         {
            Matrix temp = new Matrix(mat1.Rows, mat2.Columns);
            for (int i = 0; i < mat1.Rows; i++)
            {
               for (int j = 0; j < mat1.Columns; j++)
               {
                  temp.SetElement(i, j, (mat1.Elements[i, j] - mat2.Elements[i, j]) );
               }
            }
            return temp;
         }
         throw new Exception("Matrices are not the same size!");
      }
      
      public static Matrix operator+ (Matrix mat1, Matrix mat2)
      {
         if (mat1.Rows == mat2.Rows && mat1.Columns == mat2.Columns)
         {
            Matrix temp = new Matrix(mat1.Rows, mat1.Columns);
            for (int i = 0; i < mat1.Rows; i++)
            {
               for (int j = 0; j < mat1.Columns; j++)
               {
                  temp[i,j] = (mat1[i, j] + mat2[i, j]);
               }
            }
            return temp;
         }
			//return null;
         throw new Exception("Dimensions do not match");
      }


        //I NEED DIS.
      public static Matrix operator- (Matrix mat1, double d)
      {

            Matrix temp = new Matrix(mat1.Rows, mat1.Columns);
            for (int i = 0; i < mat1.Rows; i++)
            {
               for (int j = 0; j < mat1.Columns; j++)
               {
                  temp.SetElement(i, j, (mat1.Elements[i, j] - d) );
               }
            }
            return temp;
         

      }

		//Scaler multiplicatoin.
      public static Matrix operator* (double scaler, Matrix mat1)
      {
         Matrix temp = new Matrix(mat1.Rows, mat1.Columns);
         for (int i = 0; i < mat1.Rows; i++)
         {
            for (int j = 0; j < mat1.Columns; j++)
            {
               temp[i, j] = mat1[i, j] * scaler;
            }
         }
         return temp;
      }
		//Scaler Multiplication.
      public static Matrix operator* (Matrix mat1, double scaler)
      {
         return scaler * mat1;
      }


      
        //Attempting to thread this!
		//Matrix multiplication
      public static Matrix operator* (Matrix mat1, Matrix mat2)
      {
         return threaded_Mult(mat1,mat2);
         if (mat1.Columns == mat2.Rows)
         {
            double[] temp = new double[mat1.Rows * mat2.Columns];
            //get rows of mat1 and coloums of mat2
            for (int i = 0; i < mat1.Rows; i++)
            {
               for (int j = 0; j < mat2.Columns; j++)
               {
                  for (int k = 0; k < mat1.Columns; k++)
                  {
                     temp[i * mat2.Columns + j] += mat1[i, k] * mat2[k, j];
                  }
               }
            }
            return new Matrix(mat1.Rows, mat2.Columns, temp);
         }
         throw new Exception("Invalid matrices for multiplication");
      }

      public static double DotProduct(Matrix mat1, Matrix mat2)
      {
         if(mat1.Columns == 1 && mat2.Columns == 1 && mat1.Rows == mat2.Rows)
         {
            double temp = 0;
            for (int i = 0; i < mat1.Rows; i++)
            {
               temp += (mat1[i, 0] * mat2[i, 0]);
            }
            return temp;
         }
         throw new Exception("Either the matrices are not the same sizes or a re not vectors!");
      }
      

        //TODO: Set-row
        //TODO: Get-row
        //TODO: Set-col
        //TODO: Get-col

        public static Matrix AddBias(Matrix mat1)
        {
            Matrix temp = new Matrix(mat1.Rows, mat1.Columns +1);
            
            for (int i = 0; i < temp.Rows; i++){
                temp[i,0] = 1;  //Add the bias
            }
            
            //Copy the rest of the matrix
            for (int i = 0; i < mat1.Rows; i++)
            {
               for (int j = 0; j < mat1.Columns; j++)
               {
                  temp[i, j+1] = mat1[i, j];
               }
            }
            
            return temp;
        }

        public static Matrix RemoveBias(Matrix mat1)
        {
            Matrix temp = new Matrix(mat1.Rows, mat1.Columns -1);
            
            //Copy the rest of the matrix
            for (int i = 0; i < mat1.Rows; i++)
            {
               for (int j = 0; j < mat1.Columns-1; j++)
               {
                  temp[i, j] = mat1[i, j+1];
               }
            }
            
            return temp;
        }
        

        public static Matrix threaded_Mult(Matrix mat1, Matrix mat2){
         LinkedList<Thread> threads = new LinkedList<Thread>();
         if (mat1.Columns == mat2.Rows)
         {
            double[,] temp = new double[mat1.rows,mat2.columns];
            for(int dcol=0; dcol < mat2.columns; dcol++){
                    //Thread t = new Thread(() => threaded_Mult_Helper(mat1,mat2,temp,dcol));
                    //t.Start();
                    //threads.AddFirst(t);
                     threaded_Mult_Helper(mat1,mat2,temp,dcol);
            }
            foreach(Thread t in threads){  //Wait for the threads!
                t.Join(0);
            }
            Thread.Sleep(1000);
            return new Matrix(mat1.Rows, mat2.Columns, temp);
         }


         throw new Exception("Invalid matrices for multiplication");                    
        }

       public static void threaded_Mult_Helper(Matrix mat1, Matrix mat2, double[,] temp, int dcol){
            //Dcol: the colum of values we're computing.
            for(int currow=0; currow<mat2.rows; currow++){
                double sum=0;
                //Debug.Log("fs");
                //temp[currow,dcol]=3;
                for(int a=0; a < mat1.columns; a++){
                    sum+= mat1[currow,a]*mat2[a,dcol];  //OUT OF BOUDNS EXCEPTION. FFS,
                    
                }
                //Debug.Log("wo:"+currow+":"+dcol+"= "+sum);
                temp[currow,dcol]=sum;
            }
       }


   }

}
