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
        public List<TopJobData> writeValuesToFile(StreamWriter writeText, object[] res, String[] job_list, int user_number)
        {
            int len = ((double[,])res[0]).Length;

            
            List<TopJobData> mylist = new List<TopJobData>();
            TopJobData aux;


            for (int i = 0; i < len; i++)
            {
                //adding a new element in the list for each user.
                aux = new TopJobData(job_list[(int)(((double[,])res[0])[i, 0]) - 1], (((double[,])res[1])[i, 0]), job_list[(int)(((double[,])res[2])[i, 0]) - 1], (((double[,])res[3])[i, 0]), (((double[,])res[4])[i, 0]));
                mylist.Add(aux);

                writeText.Write(job_list[(int)(((double[,])res[0])[i, 0]) - 1] + "\t");
                writeText.Write((((double[,])res[1])[i, 0]) + "\t");
                writeText.Write(job_list[(int)(((double[,])res[2])[i, 0]) - 1] + "\t");
                writeText.Write((((double[,])res[3])[i, 0]) + "\t");
                writeText.Write((((double[,])res[4])[i, 0]) + "\t");
                writeText.Write(user_number + "\n");
            }

            return mylist;
        }

        //For each one of the users, writes averages analysis to a file
        public void writeAveragesToFile(DataResult result, StreamWriter writeText, int i)
        {
            writeText.WriteLine("USER: " + (i + 1));
            for (int k = 0; k < result.Number_top_jobs; k++)
            {
                if (result.TopJobNames[k] != null)
                {
                    writeText.WriteLine("Job " + result.TopJobNames[k] + "\t" + result.List.ElementAt(k).PredRecJob + "\t" + result.Rating_average[k] + "\t" + result.Percentage_average[k]);
                }
            }
            writeText.WriteLine("AVGS TOTAL\t" + result.Rating_total_avg + "\t" + result.Percentage_total_avg + "\n");
        }

    }
}
