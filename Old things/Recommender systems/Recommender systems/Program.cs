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


            //////////////////////////////////////////////////////////////////// Loading files  ///////////////////////////////////////////////////////////////////// 
            // Variables used in matlab 
            //double lambda = 1.5;
            double num_users = 4;
            double num_movies = 5;
            double num_features = 3;

            // Generate data according with the logic express in Matlab
            double[,] X = new double[(int)num_movies, (int)num_features];
            double[,] Y = new double[(int)num_movies, (int)num_users];
            double[,] Theta = new double[(int)num_users, (int)num_features];
            double[,] R = new double[(int)num_movies, (int)num_users];

            //filling out the matrix X
            using (TextReader readerX = File.OpenText("C:/Users/laerte/Documents/GitHub/ummatlab/Recommender systems/Data/data_x.txt"))
            {
                int i = 0;
                string line = readerX.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split('\t');
                    for (int j = 0; j < (int)num_features; j++)
                    {
                        //Console.Write(temp[j - 1]);
                        X[i, j] = Convert.ToDouble(temp[j]);
                    }
                    line = readerX.ReadLine();
                    i++;
                }
                readerX.Close();
            }
            
            //filling out the matrix Y
            using (TextReader readerY = File.OpenText("C:/Users/laerte/Documents/GitHub/ummatlab/Recommender systems/Data/data_y.txt"))
            {
                int i = 0;
                string line = readerY.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split('\t');
                    for (int j = 0; j < (int)num_users; j++)
                    {
                        //Console.Write(temp[j - 1]);
                        Y[i, j] = Convert.ToDouble(temp[j]);
                    }
                    line = readerY.ReadLine();
                    i++;
                }
                readerY.Close();
            }

            // Filling out the matrix theta
            using (TextReader readerTheta = File.OpenText("C:/Users/laerte/Documents/GitHub/ummatlab/Recommender systems/Data/data_theta.txt"))
            {
                int i = 0;
                string line = readerTheta.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split('\t');
                    for (int j = 0; j < (int)num_features; j++)
                    {
                        //Console.Write(temp[j - 1]);
                        Theta[i, j] = Convert.ToDouble(temp[j]);
                    }
                    line = readerTheta.ReadLine();
                    i++;
                }
                readerTheta.Close();
            }


            // Filling out the matrix theta
            using (TextReader readerR = File.OpenText("C:/Users/laerte/Documents/GitHub/ummatlab/Recommender systems/Data/data_r.txt"))
            {
                int i = 0;
                string line = readerR.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split('\t');
                    for (int j = 0; j < (int)num_users; j++)
                    {
                        //Console.Write(temp[j - 1]);
                        R[i, j] = Convert.ToDouble(temp[j]);
                    }
                    line = readerR.ReadLine();
                    i++;
                }
                readerR.Close();
            }



            //Reading the file to get the size of lines and columns of R and Y, which will be used in training colloborative filtering
            int num_movies_init;
            int num_users_init;
            using (TextReader readerSize = File.OpenText("C:/Users/laerte/Desktop/Y_training.txt"))
            {
                num_movies_init = 0;
                string line = readerSize.ReadLine();
                string[] temp = line.Split('\t');
                num_users_init = temp.Length;
                while (line != null)
                {
                    line = readerSize.ReadLine();
                    num_movies_init++;
                }
                readerSize.Close();
            }
            //Now we read R and Y from theirs files
            double[,] Y_training = new double[num_movies_init, num_users_init];
            double[,] R_training = new double[num_movies_init, num_users_init];
            //reading Y_training
            using (TextReader readerY = File.OpenText("C:/Users/laerte/Desktop/Y_training.txt"))
            {
                int i = 0;
                string line = readerY.ReadLine();
                string[] temp = line.Split('\t');
                while (line != null)
                {
                    for (int j = 0; j < num_users_init; j++)
                    {
                        Y_training[i, j] = Convert.ToDouble(temp[j]);
                    }
                    line = readerY.ReadLine();
                    i++;
                }
                readerY.Close();
            }
            //reading R_training
            using (TextReader readerR = File.OpenText("C:/Users/laerte/Desktop/R_training.txt"))
            {
                int i = 0;
                string line = readerR.ReadLine();
                string[] temp = line.Split('\t');
                while (line != null)
                {
                    for (int j = 0; j < num_users_init; j++)
                    {
                        R_training[i, j] = Convert.ToDouble(temp[j]);

                    }
                    line = readerR.ReadLine();
                    i++;
                }
                readerR.Close();
            }



            // Movie rating file for a user X   
            double[,] my_ratings = new double[num_movies_init, 1];
            // make sure that the data will be zero for all the other positions
            Array.Clear(my_ratings, 0, num_movies_init);
            using (TextReader reader_movies_ratings = File.OpenText("C:/Users/laerte/Documents/GitHub/ummatlab/Recommender systems/Data/movies_ratings.txt"))
            {
                int i = 0;
                string line = reader_movies_ratings.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split('\t');
                    my_ratings[Convert.ToInt32(temp[0]) - 1, 0] = Convert.ToInt32(temp[1]);

                    line = reader_movies_ratings.ReadLine();
                    i++;
                }
                reader_movies_ratings.Close();
            }

            // Movie list file for a user X   
            string[] movie_list = new string[num_movies_init];  
            using (TextReader reader_movie_list = File.OpenText("C:/Users/laerte/Documents/GitHub/ummatlab/Recommender systems/Data/movie_ids.txt"))
            {
                string line = reader_movie_list.ReadLine();
                int i = 0;
                while (line != null)
                {
                    line = line.Replace((i + 1).ToString() + " ", "");
                    movie_list[i] = line;
                    line = reader_movie_list.ReadLine(); 
                    i++;
                }
                reader_movie_list.Close();
            }

            /////////////////////////////////////////////////////////////////////// Matlab calculations /////////////////////////////////////////////////////////////////////
            
            // Create the MATLAB instance 
            MLApp.MLApp matlab = new MLApp.MLApp();
            
            // Change to the directory where the functions are located --- always use the desktop to make tests to avoid problems with pathway
            matlab.Execute(@"cd C:\Users\laerte\Desktop");


            // Calling a function to calculate the parameters to send it to cofiCostFunc
            object parameters = null;
            matlab.Feval("generateParams", 1, out parameters, X, Theta);
            object[] par = parameters as object[];              
            
            
            // Define the output to print the final result for CofiCostFunc
            //object result = null;

            // Call the MATLAB function myfunc
            matlab.Execute("clear all");     

            // Execute the function in Matlab function to check J
           // matlab.Feval("cofiCostFunc", 2, out result, (par[0]), Y, R, num_users, num_movies, num_features, lambda);

            // Checking the gradients 
            //object check = null;
            //matlab.Feval("checkCostFunction", 0, out check, lambda);

            
            // Display result [cost, grad] = costFunctionReg(initial_theta, X, y, lambda);
            //object[] res = result as object[];


            // Print J and grad from CofiCostFunc, which are the returned by this function  --- I will commment this part
            Console.WriteLine("Go to Matlab to do the interactions...");
            /*
            Console.WriteLine("Computing the cost J = " + res[0]);
            int len = ((double[,])(res[1])).Length;
            for (int n=0; n < len; n++)
            {
                Console.WriteLine("grad = " +((double[,])res[1])[n,0]);
            }
             */
            

             // Define the output to print the final result
            object result_movies_search = null;

            // Movie recommendations script
            matlab.Feval("scriptGeneration", 1, out result_movies_search, my_ratings, movie_list, Y_training, R_training);

            // Wait until fisnih
            Console.Write("Presse ENTER to close the window");
            Console.ReadLine();
            // Closing the matblab window
            //matlab.Quit();
            
        }
    }
}