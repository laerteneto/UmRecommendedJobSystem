using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MatlabTest
{
    class Program
    {
        static void Main(string[] args)
        {

            /////////////////////////////////////// Looking for a file  /////////////////////////////////////// 
            int m = 0, n;

            //Learning the value of m(Number of training examples) and n(number of atributes)
            using (TextReader reader = File.OpenText("C:/Users/larissa/Desktop/ex2data2.txt"))
            {
                string linha = reader.ReadLine();
                string[] temp = linha.Split(',');
                n = temp.Length - 1;
                while (linha != null)
                {
                    m++;
                    linha = reader.ReadLine();
                }
                reader.Close();
            }

            double[,] X = new double[m, n + 1];
            double[,] y = new double[m, 1];
            double[,] theta = new double[n + 1, 1];

            //filling out the matrix X, the vector y, and the vector theta
            using (TextReader reader = File.OpenText("C:/Users/larissa/Desktop/ex2data2.txt"))
            {
                int i = 0;
                string linha = reader.ReadLine();
                while (linha != null)
                {
                    string[] temp = linha.Split(',');
                    X[i, 0] = 1;
                    for (int j = 1; j < n + 1; j++)
                    {
                        //Console.Write(temp[j - 1]);
                        X[i, j] = Convert.ToDouble(temp[j - 1]);
                    }
                    y[i, 0] = Convert.ToDouble(temp[n]);
                    linha = reader.ReadLine();
                    i++;
                }
                reader.Close();
            }
            for (int i = 0; i < n + 1; i++)
            {
                theta[i, 0] = 0;
            }

            
            ////////////////////////////////////////////// PRINT //////////////////////////////////////////////// 
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n + 1; j++)
                {
                    Console.Write("X[{0}][{1}] = {2} ", i, j, X[i, j]);
                }
                Console.Write("\n");
            }

            for (int i = 0; i < m; i++)
            {
                Console.Write("y[{0}] = {1}\n", i, y[i,0]);
            }

            for (int i = 0; i < n + 1; i++)
            {
                Console.Write("theta[{0}] = {1}\n", i, theta[i,0]);
            }

            Console.ReadLine();
            
            
            ////////////////////////////////////////////Matlab calculations/////////////////////////////////////////////
            
            // Create the MATLAB instance 
            MLApp.MLApp matlab = new MLApp.MLApp();
            

            // Change to the directory where the function is located 
            matlab.Execute(@"cd C:\Users\larissa\Desktop");

            // Define the output 
            object result = null;

            // Call the MATLAB function myfunc
            matlab.Execute("clear all");
            double lambda = 1;
            matlab.Feval("costFunctionReg", 2, out result, theta, X, y, lambda);

            // Display result [cost, grad] = costFunctionReg(initial_theta, X, y, lambda);
            object[] res = result as object[];


            Console.WriteLine("Computing the cost J = " + res[0]);
            // Closing the matblab window
            //matlab.Quit();
            // Wait until fisnih
            Console.ReadLine();
            
        }
    }
}