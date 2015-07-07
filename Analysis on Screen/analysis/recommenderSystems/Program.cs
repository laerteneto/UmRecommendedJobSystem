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
            // Variables used in matlab 
            //double lambda = 1.5;

            //Reading the file to get the size of lines and columns of R and Y, which will be used in training colloborative filtering
            int num_jobs_init;
            int num_users_init;
            int num_features;
            using (TextReader readerR = File.OpenText("C:/Users/larissaf/Desktop/files/Y.txt"))
            {
                num_jobs_init = 0;
                string line = readerR.ReadLine();
                string[] temp = line.Split('\t');
                num_users_init = temp.Length;
                while (line != null)
                {
                    line = readerR.ReadLine();
                    num_jobs_init++;
                }
                readerR.Close();
            }
            //it needs to be between 1 and num_users_init (PUT A VERIFICATION HERE)
            Console.Write("Write down the user number that will receive job recommendation from 1 to {0}\n", num_users_init);
            string read = Console.ReadLine();
            int user_number = Convert.ToInt32(read); 


            //Now we read R and Y from theirs files (-1 because I will remove the chosen user from the matrixes)
            double[,] Y = new double[num_jobs_init, num_users_init - 1];
            double[,] R = new double[num_jobs_init, num_users_init - 1];

            // Movie rating file for a user   
            double[,] my_ratings = new double[num_jobs_init, 1];

            //reading Y_training
            using (TextReader readerY = File.OpenText("C:/Users/larissaf/Desktop/files/Y.txt"))
            {
                int i = 0;
                string line = readerY.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split('\t');
                    int k = 0;
                    for (int j = 0; j < num_users_init; j++)
                    {
                        if (j != (user_number - 1))
                        {
                            Y[i, k] = Convert.ToDouble(temp[j]);
                            k++;
                        }
                        else
                            my_ratings[i, 0] = Convert.ToDouble(temp[j]);
                    }
                    line = readerY.ReadLine();
                    i++;
                }
                readerY.Close();
            }
            //reading R_training
            using (TextReader readerR = File.OpenText("C:/Users/larissaf/Desktop/files/R.txt"))
            {
                int i = 0;
                string line = readerR.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split('\t');
                    int k = 0;
                    for (int j = 0; j < num_users_init; j++)
                    {
                        if (j != (user_number - 1))
                        {
                            R[i, k] = Convert.ToDouble(temp[j]);
                            k++;
                        }
                    }
                    line = readerR.ReadLine();
                    i++;
                }
                readerR.Close();
            }

            //reading X to get the number of features
            using (TextReader readerX = File.OpenText("C:/Users/larissaf/Desktop/files/X.txt"))
            {
                num_features = 0;
                string line = readerX.ReadLine();
                if (line != null)
                {
                    string[] temp = line.Split('\t');
                    num_features = temp.Length;
                }
                readerX.Close();
            }
            //allocating the X matriz
            double[,] X = new double[num_jobs_init, num_features];

            //reading X_training
            using (TextReader readerX = File.OpenText("C:/Users/larissaf/Desktop/files/X.txt"))
            {
                int i = 0;
                string line = readerX.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split('\t');
                    for (int j = 0; j < num_features; j++)
                    {
                        X[i, j] = Convert.ToDouble(temp[j]);
                    }
                    line = readerX.ReadLine();
                    i++;
                }
                readerX.Close();
            }

            

            // Job names   
            string[] job_list = new string[num_jobs_init];
            using (TextReader reader_movie_list = File.OpenText("C:/Users/larissaf/Desktop/files/job_names.txt"))
            {
                string line = reader_movie_list.ReadLine();
                int i = 0;
                while (line != null)
                {
                    job_list[i] = line;
                    line = reader_movie_list.ReadLine();
                    i++;
                }
                reader_movie_list.Close();
            }

            ////////////////////////////////////////////Matlab calculations///////////////////////////////////////////

            // Create the MATLAB instance 
            MLApp.MLApp matlab = new MLApp.MLApp();

            // Change to the directory where the functions are located --- always use the desktop to make tests to avoid problems with pathway
            matlab.Execute(@"cd C:\Users\larissaf\Desktop\files");

            // Define the output to print the final result
            object result_movies_search = null;

            matlab.Execute("my_ratings");


            // Movie erecommendations script
            matlab.Feval("scriptGeneration", 1, out result_movies_search, my_ratings, job_list, Y, R, X, num_features);     

            // Wait until fisnih
            Console.ReadLine();

        }
    }
}
