using recommenderSystems.Domain;
using recommenderSystems.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Plugin
{
    public class FileSystemSvcImpl : IFileSystemSvc
    {
        ///<summary>
        ///Reading the file to get the size of lines and columns of R and Y, which will be used in training colloborative filtering.
        ///</summary>
        ///<remarks>
        ///Number of Users(columns) and Number of Jobs(rows)
        ///</remarks>
        public void detectSizeOfJobsColumns(TaskDimensions task, String path)
        {
            using (TextReader readerR = File.OpenText(path))
            {
                task.num_jobs_init = 0;
                string line = readerR.ReadLine();
                string[] temp = line.Split('\t');
                task.num_users_init = temp.Length;
                while (line != null)
                {
                    line = readerR.ReadLine();
                    task.num_jobs_init++;
                }
                readerR.Close();
            }
        }

        ///<summary>
        ///File that is going to be written with the Top ten jobs and ratings and similarity for all users
        ///</summary>
        ///<remarks>
        ///
        ///</remarks>
        public StreamWriter getResultStreamWriter()
        {
            return new StreamWriter("results.txt");
        }

        ///<summary>
        ///File that is going to be written with the averages analysis for all users
        ///</summary>
        ///<remarks>
        ///
        ///</remarks>
        public StreamWriter getAverageStreamWriter()
        {
            return new StreamWriter("averages.txt");
        }

        ///<summary>
        ///File that is going to be written with the users id and the calculated rating (average)
        ///</summary>
        ///<remarks>
        ///
        ///</remarks>
        public StreamWriter getIdandAvgStreamWriter()
        {
            return new StreamWriter("IDandAVG.txt");
        }




        //Reads the file X to get the number of features
        public double[,] getNumberOfFeaturesX(String path, TaskDimensions task)
        {
            task.num_features = 0;

            using (TextReader readerX = File.OpenText(path))
            {
                string line = readerX.ReadLine();
                if (line != null)
                {
                    string[] temp = line.Split('\t');
                    task.num_features = temp.Length;
                }
                readerX.Close();
            }

            //allocating the X matriz
            return new double[task.num_jobs_init, task.num_features];
        }

        //Reads the binary rating for the Features for each one of the Jobs
        public void readFeaturesX(double[,] X, String path, TaskDimensions task)
        {
            //reading X
            using (TextReader readerX = File.OpenText(path))
            {
                int i = 0;
                string line = readerX.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split('\t');
                    for (int j = 0; j < task.num_features; j++)
                    {
                        X[i, j] = Convert.ToDouble(temp[j]);
                    }
                    line = readerX.ReadLine();
                    i++;
                }
                readerX.Close();
            }
        }

        //Reads the file job _list to get the name of the Jobs
        public String[] readJobNames(String path, TaskDimensions task)
        {
            string[] job_list = new string[task.num_jobs_init];
            using (TextReader reader_job_list = File.OpenText(path))
            {
                string line = reader_job_list.ReadLine();
                int i = 0;
                while (line != null)
                {
                    job_list[i] = line;
                    line = reader_job_list.ReadLine();
                    i++;
                }
                reader_job_list.Close();
            }
            return job_list;
        }

        //Reads the file job _list to get the name of the Jobs
        public UserProfile[] readUserProfile(String path, TaskDimensions task)
        {
            UserProfile[] users = new UserProfile[task.num_users_init];
            using (TextReader reader_users_list = File.OpenText(path))
            {
                string line = reader_users_list.ReadLine();
                int i = 0;
                while (line != null)
                {
                    string[] temp = line.Split('\t');
                    users[i] = new UserProfile(temp[0], Convert.ToDouble(temp[1]));
                    line = reader_users_list.ReadLine();
                    i++;
                }
                reader_users_list.Close();
            }
            return users;
        }


        //Reads the file Y to fill out the matrix
        public double[,] readTrainingY(String path, TaskDimensions task, double[,] my_ratings, int user_number)
        {
            //Now we allocate R (num_users_init - 1 because the chosen user was removed from the matrix)
            double[,] Y = new double[task.num_jobs_init, task.num_users_init - 1];

            using (TextReader readerY = File.OpenText(path))
            {
                int i = 0;
                string line = readerY.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split('\t');
                    int k = 0;
                    for (int j = 0; j < task.num_users_init; j++)
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
            return Y;
        }

        //Reads the file R to fill out the matrix
        public double[,] readTrainingR(String path, TaskDimensions task, int user_number)
        {
            //Now we allocate R (num_users_init - 1 because the chosen user was removed from the matrix)
            double[,] R = new double[task.num_jobs_init, task.num_users_init - 1];

            using (TextReader readerR = File.OpenText(path))
            {
                int i = 0;
                string line = readerR.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split('\t');
                    int k = 0;
                    for (int j = 0; j < task.num_users_init; j++)
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
            return R;
        }

        //For each one of the users, writes values to a file, and return a list containg all the comparassions between the top ten 
        //jobs for him, and the other jobs (that had similarity >= 70%)
        public List<TopJobData> writeValuesToFile(StreamWriter writeText, object[] res, String[] job_list, int user_number, double[,] X)
        {
            int len = ((double[,])res[0]).Length;

            
            List<TopJobData> mylist = new List<TopJobData>();
            TopJobData aux;


            for (int i = 0; i < len; i++)
            {
                //adding a new element in the list for each user.
                aux = new TopJobData(job_list[(int)(((double[,])res[0])[i, 0]) - 1], 
                    (((double[,])res[1])[i, 0]), 
                    job_list[(int)(((double[,])res[2])[i, 0]) - 1],
                    X[(int)(((double[,])res[2])[i, 0]) - 1, 0],
                    (((double[,])res[3])[i, 0]), 
                    (((double[,])res[4])[i, 0]));
                mylist.Add(aux);

                writeText.Write(job_list[(int)(((double[,])res[0])[i, 0]) - 1] + "\t");
                writeText.Write((((double[,])res[1])[i, 0]) + "\t");
                writeText.Write(job_list[(int)(((double[,])res[2])[i, 0]) - 1] + "\t");
                writeText.Write(X[(int)(((double[,])res[2])[i, 0]) - 1, 0] + "\t");
                writeText.Write((((double[,])res[3])[i, 0]) + "\t");
                writeText.Write((((double[,])res[4])[i, 0]) + "\t");
                writeText.Write(user_number + "\n");
            }

            return mylist;
        }

        //For each one of the users, writes averages analysis to a file
        public void writeAveragesToFile(DataResult result, StreamWriter writeText, UserProfile user)
        {
            writeText.WriteLine("USER: " + user.UserID + " with rating: " + user.UserRating);
            for (int k = 0; k < result.Number_top_jobs; k++)
            {
                if (result.TopJobNames[k] != null)
                {
                    writeText.WriteLine("Job " + result.TopJobNames[k] + "\t" + result.TopJobDiff[k] + "\t" +
                        result.Mylist.ElementAt(k).PredRecJob + "\t" + result.Rating_average[k] + "\t" + 
                        result.Percentage_average[k]);
                }
            }
            writeText.WriteLine("AVGS TOTAL\t" + result.Rating_total_avg + "\t" + result.Percentage_total_avg);
            writeText.WriteLine("SELF INACCURACY\t"+ result.Self_inaccuracy.ToString("N2") + "\n");
        }

        //This method writes global pieces of information about the averages for all users in the same averages file 
        public void writeGlobalAveragesInformation(double total_avg_system, double total_user_inaccuracy,
            int numUnderEstimated, int numOverEstimated, TaskDimensions task, StreamWriter writeText,
            UserProfile[] user, double[] users_calculated_raitings)
        {
            writeText.WriteLine("Avgs total (similarity)\t" + total_avg_system);
            writeText.WriteLine("Community inaccuracy\t" + total_user_inaccuracy + "\n");
            writeText.WriteLine("Number of users that underestimated themselves\t" + numUnderEstimated);
            writeText.WriteLine("Number of users that overestimated themselves\t" + numOverEstimated + "\n");

            int numOverEstimated1 = 0, numActualy1 = 0, numUnderEstimated1 = 0;
            int numOverEstimated2 = 0, numActualy2 = 0, numUnderEstimated2 = 0;
            int numOverEstimated3 = 0, numActualy3 = 0, numUnderEstimated3 = 0;
            int numOverEstimated4 = 0, numActualy4 = 0, numUnderEstimated4 = 0; 
            int numOverEstimated5 = 0, numActualy5 = 0, numUnderEstimated5 = 0;
            int num1 = 0, num2 = 0, num3 = 0, num4 = 0, num5 = 0;
            int num1_after = 0, num2_after = 0, num3_after = 0, num4_after = 0, num5_after = 0;
            for (int i = 0; i < task.num_users_init; i++)
            {
                switch ((int)user[i].UserRating)
                {
                    case 1:
                        num1++;
                        if ((int)Math.Round(users_calculated_raitings[i], 0) == 1)
                            numActualy1++;
                        else
                            numUnderEstimated1++;
                        break;
                    case 2:
                        num2++;
                        if ((int)Math.Round(users_calculated_raitings[i], 0) < 2)
                            numOverEstimated2++;
                        else if ((int)Math.Round(users_calculated_raitings[i], 0) == 2)
                            numActualy2++;
                        else
                            numUnderEstimated2++;
                        break;
                    case 3:
                        num3++;
                        if ((int)Math.Round(users_calculated_raitings[i], 0) < 3)
                            numOverEstimated3++;
                        else if ((int)Math.Round(users_calculated_raitings[i], 0) == 3)
                            numActualy3++;
                        else
                            numUnderEstimated3++;
                        break;
                    case 4:
                        num4++;
                        if ((int)Math.Round(users_calculated_raitings[i], 0) < 4)
                            numOverEstimated4++;
                        else if ((int)Math.Round(users_calculated_raitings[i], 0) == 4)
                            numActualy4++;
                        else
                            numUnderEstimated4++;
                        break;
                    default:
                        num5++;
                        if ((int)Math.Round(users_calculated_raitings[i], 0) < 5)
                            numOverEstimated5++;
                        else 
                            numActualy5++;
                        break;
                }
                switch ((int)Math.Round(users_calculated_raitings[i], 0))
                {
                    case 1:
                        num1_after++;
                        break;
                    case 2:
                        num2_after++;
                        break;
                    case 3:
                        num3_after++;
                        break;
                    case 4:
                        num4_after++;
                        break;
                    default:
                        num5_after++;
                        break;
                }
            }

            writeText.WriteLine("Number of self ratings (before calculation)");
            writeText.WriteLine("amount of 1:\t" + num1);
            writeText.WriteLine("amount of 2:\t" + num2);
            writeText.WriteLine("amount of 3:\t" + num3);
            writeText.WriteLine("amount of 4:\t" + num4);
            writeText.WriteLine("amount of 5:\t" + num5);

            writeText.WriteLine("\nNumber of self ratings (after calculation)");
            writeText.WriteLine("amount of 1:\t" + num1_after);
            writeText.WriteLine("amount of 2:\t" + num2_after);
            writeText.WriteLine("amount of 3:\t" + num3_after);
            writeText.WriteLine("amount of 4:\t" + num4_after);
            writeText.WriteLine("amount of 5:\t" + num5_after);

            this.underAndOverEstimations(writeText, 1, num1, numOverEstimated1, numActualy1, numUnderEstimated1);
            this.underAndOverEstimations(writeText, 2, num2, numOverEstimated2, numActualy2, numUnderEstimated2);
            this.underAndOverEstimations(writeText, 3, num3, numOverEstimated3, numActualy3, numUnderEstimated3);
            this.underAndOverEstimations(writeText, 4, num4, numOverEstimated4, numActualy4, numUnderEstimated4);
            this.underAndOverEstimations(writeText, 5, num5, numOverEstimated5, numActualy5, numUnderEstimated5);
            /*
            writeText.WriteLine("\nFor the people who rated themselves as 4 (" + num4 + ")");
            writeText.WriteLine("amount of those who overestimated themselves were:\t" + numOverEstimated4);
            writeText.WriteLine("amount of those who estimated themselves correctly were:\t" + numActualy4);
            writeText.WriteLine("amount of those who underestimated themselves were:\t" + numUnderEstimated4);

            writeText.WriteLine("\nFor the people who rated themselves as 5 (" + num5 + ")");
            writeText.WriteLine("amount of those who overestimated themselves were:\t" + numOverEstimated5);
            writeText.WriteLine("amount of those who estimated themselves correctly were:\t" + numActualy5);
            writeText.WriteLine("amount of those who underestimated themselves were:\t" + numUnderEstimated5);
            */
        }

        public void underAndOverEstimations(StreamWriter writeText, int id, int num, int numOverEstimated, int numActualy, int numUnderEstimated)
        {
            writeText.WriteLine("\nFor the people who rated themselves as " + id + " (" + num + ")");
            writeText.WriteLine("amount of those who overestimated themselves were:\t" + numOverEstimated);
            writeText.WriteLine("amount of those who estimated themselves correctly were:\t" + numActualy);
            writeText.WriteLine("amount of those who underestimated themselves were:\t" + numUnderEstimated);
        }

    }
}
