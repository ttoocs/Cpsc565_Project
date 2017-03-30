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
            if (elements.Length != rows || elements.GetLength(0) != columns)
                throw new Exception();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = elements[i,j];
                }
            }

      }

      public Matrix(int rows, int columns)
      {
         this.rows = rows;
         this.columns = columns;
         matrix = new double[rows, columns];
         //initilzed to 0.0

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
			return null;
         //throw new MatrixException();
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
			return null;
         //throw new MatrixException("Dimensions do not match");
      }

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

      public static Matrix operator* (Matrix mat1, double scaler)
      {
         return scaler * mat1;
      }

      public static Matrix operator* (Matrix mat1, Matrix mat2)
      {
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
      

   }
}
