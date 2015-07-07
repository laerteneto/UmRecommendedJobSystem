using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using recommenderSystems;

namespace MatlabTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            int num_jobs_init;
            int num_users_init;
            int num_features;
            int user_number = 1;

            //Reading the file to get the size of lines and columns of R and Y, 
            //which will be used in training colloborative filtering.
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

            //File that is going to be written with the Top ten jobs and ratings and similarity for all users
            StreamWriter writetext = new StreamWriter("results.txt");

            //Dictionary that contains a list of top ten jobs compared with other jobs for each user
            Dictionary<int, List<MyData>> finalList = new Dictionary<int, List<MyData>>();

            //consider that we have 100 users (we can ask how may and change the while-condition)
            while (user_number <= 100)
            {
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

                /*
                using (TextReader reader_movies_ratings = File.OpenText("C:/Users/larissaf/Desktop/files/one_user_rating.txt"))
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
                }*/

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

                //this is used to do not show the matlab window
                //matlab.Visible = 0;

                // Change to the directory where the functions are located --- always use the desktop to make tests to avoid problems with pathway
                matlab.Execute(@"cd C:\Users\larissaf\Desktop\files");

                // Define the output to print the final result
                object result_movies_search = null;

                matlab.Execute("my_ratings");


                // Movie erecommendations script
                matlab.Feval("scriptGeneration", 6, out result_movies_search, my_ratings, job_list, Y, R, X, num_features);
                object[] res = result_movies_search as object[];

                int len = ((double[,])res[0]).Length;

                
                //for each one of the users, we will have a list containg all the comparassions between the top ten jobs for him, and
                //the other jobs (that had similarity >= 70%)
                List<MyData> mylist = new List<MyData>();
                MyData aux;

                for (int i = 0; i < len; i++)
                {
                    //adding a new element in the list for each user.
                    aux = new MyData(job_list[(int)(((double[,])res[0])[i, 0]) - 1], (((double[,])res[1])[i, 0]), job_list[(int)(((double[,])res[2])[i, 0]) - 1], (((double[,])res[3])[i, 0]), (((double[,])res[4])[i, 0]));
                    mylist.Add(aux);

                    writetext.Write(job_list[(int)(((double[,])res[0])[i, 0]) - 1] + "\t");
                    writetext.Write((((double[,])res[1])[i, 0]) + "\t");
                    writetext.Write(job_list[(int)(((double[,])res[2])[i, 0]) - 1] + "\t");
                    writetext.Write((((double[,])res[3])[i, 0]) + "\t");
                    writetext.Write((((double[,])res[4])[i, 0]) + "\t");
                    writetext.Write(user_number + "\n");
                }
               
                //adding the list at the Dictionary for each user
                finalList.Add(user_number, mylist);
                

                user_number++;
            } // while ends



            writetext.Close();
            
            //creating a instance of an object for user 1: (maybe send null)
            dataResult avgs = new dataResult(null, 10);
            avgs.WriteInAFile(finalList);


            Console.WriteLine("DONE");

            // Wait until fisnih
            Console.ReadLine();


        }    
    }    
}
