using recommenderSystems.Domain;
using recommenderSystems.Service.Interface;
using recommenderSystems.Service.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems
{
    public class DriverFile
    {
        public static void Main(string[] args)
        {
            //Object to Hold Task Parameters
            TaskDimensions task = new TaskDimensions();

            //User number to start proccessing
            int user_number = 1;

            //Load File System Service
            IFileSystemSvc svc = new FileSystemSvcImpl();

            //Method call to get the number of jobs and users from the file Y
            svc.detectSizeOfJobsColumns(task, Directory.GetCurrentDirectory()+"/files/new_Y_53.txt");

            //Method call to get the number of features from the file X, and allocating the X matrix
            double[,] X = svc.getNumberOfFeaturesX(Directory.GetCurrentDirectory() + "/files/X.txt", task);
            svc.readFeaturesX(X, Directory.GetCurrentDirectory() + "/files/X.txt", task);

            //Method call to get the jobs names
            String[] job_list = svc.readJobNames(Directory.GetCurrentDirectory() + "/files/expressions.txt", task);

            //method that return the users profile
            UserProfile[] users_profile = svc.readUserProfile(Directory.GetCurrentDirectory() + "/files/new_user_table_53.txt", task);

            //Creating a variable to write in a File the job recommendations and comparisons
            //Load File Writer
            StreamWriter writeTextResult = svc.getResultStreamWriter();
            StreamWriter writeTextAverages = svc.getAverageStreamWriter();
            StreamWriter writeText = svc.getIdandAvgStreamWriter();
            StreamWriter writeTextDiff = svc.getDifficultyStreamWriter();
   

            
            double[] users_calculated_raitings = new double[task.num_users_init];

            double total_rating_avg_system = 0;
            double total_similarity_avg_system = 0;
            double total_inaccuracy_system = 0;

            while (user_number <= task.num_users_init)
            {
                // job rating file for a user   
                double[,] my_ratings = new double[task.num_jobs_init, 1];

                //Now we read R and Y from theirs files (-1 because I will remove the chosen user from the matrixes)
                double[,] Y = svc.readTrainingY(Directory.GetCurrentDirectory() + "/files/new_Y_53.txt", task, my_ratings, user_number);
                double[,] R = svc.readTrainingR(Directory.GetCurrentDirectory() + "/files/new_R_53.txt", task, user_number);

                //Creating a MatLab reference to execute the recommended job script
                IMatlabSvc matSvc = new MatlabSvcImpl();
                object[] res = matSvc.executeFilter(task, job_list, Directory.GetCurrentDirectory()+ "/files", my_ratings, Y, R, X, user_number);
                
                
                //Each time creates a  to be used to write the recommended jobs in a file
                List<TopJobData> mylist = svc.writeValuesToFile(writeTextResult, res, job_list, user_number, X);
                

                //Calculate Averages for Jobs for a User
                DataResult avgs = new DataResult(mylist, mylist.Count, users_profile[user_number - 1]);
                avgs.AverageForEachJob();
                svc.writeAveragesToFile(avgs, writeTextAverages, users_profile[user_number - 1]);
                
                total_rating_avg_system += avgs.Rating_total_avg;
                total_similarity_avg_system += avgs.Percentage_total_avg;
                total_inaccuracy_system += avgs.Self_inaccuracy;
                //adding the list at the Dictionary for each user

                //ID and AVGs file
                writeText.WriteLine(users_profile[user_number - 1].UserID + "\t" + avgs.Rating_total_avg);
                
                
                users_calculated_raitings[user_number - 1] = avgs.Rating_total_avg;

                //writing in the difficulty file
                svc.writeDifficultyToFile(writeTextDiff, avgs);


                //used to inC:\Users\larissaf\Desktop\FinaleVersionCrowd\recommenderSystems\Driver.cssert recommended jobs for a user in the database
                IElasticSvc es = new ElasticSvcImpl();
                es.insertRecommenderJob(avgs);
                

                user_number++;

            }

            total_rating_avg_system /= task.num_users_init;
            total_similarity_avg_system /= task.num_users_init;
            total_inaccuracy_system /= task.num_users_init;
            //writing some more global information
            svc.writeGlobalAveragesInformation(total_rating_avg_system, total_similarity_avg_system, total_inaccuracy_system, 
                 task, writeTextAverages, users_profile, users_calculated_raitings);
            


            //closing the three files
            writeText.Close();
            writeTextResult.Close();
            writeTextAverages.Close();
            writeTextDiff.Close();


            /* 
             * Used to insert rating for task performed by workers. (User interface need to be built)
             * 
            double[,] full_Y = svc.readFullY(Directory.GetCurrentDirectory() + "/files/Y.txt", task);
            IElasticSvc e = new ElasticSvcImpl();
            e.InsertRatings(job_list, users_profile, full_Y);
            */


            
            Console.WriteLine("DONE");

            //Wait until fisnih
            Console.ReadLine();
        }
    }
}
