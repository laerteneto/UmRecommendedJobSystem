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
    public class Driver
    {
        public static void Main(string[] args)
        {
            //Object to Hold Task Parameters
            TaskDimensions task = new TaskDimensions();

            //Dictionary that contains a list of top ten jobs compared with other jobs for each user
            Dictionary<int, List<TopJobData>> finalList = new Dictionary<int, List<TopJobData>>();

            //User number to start proccessing
            int user_number = 1;

            //Load File System Service
            IFileSystemSvc svc = new FileSystemSvcImpl();

            //Method call to get the number of jobs and users from the file Y
            svc.detectSizeOfJobsColumns(task, Directory.GetCurrentDirectory()+"/files/Y1.txt");

            //Method call to get the number of features from the file X, and allocating the X matrix
            double[,] X = svc.getNumberOfFeaturesX(Directory.GetCurrentDirectory() + "/files/X1.txt", task);
            svc.readFeaturesX(X, Directory.GetCurrentDirectory() + "/files/X1.txt", task);

            //Method call to get the jobs names
            String[] job_list = svc.readJobNames(Directory.GetCurrentDirectory() + "/files/job_names1.txt", task);

            //method that return the users profile
            UserProfile[] users_profile = svc.readUserProfile(Directory.GetCurrentDirectory() + "/files/user_table.txt", task);

            //Creating a variable to write in a File the job recommendations and comparisons
            //Load File Writer
            StreamWriter writeTextResult = svc.getResultStreamWriter();
            StreamWriter writeTextAverages = svc.getAverageStreamWriter();

            double total_avg_system = 0;
            double total_user_inaccuracy = 0;

            while (user_number <= task.num_users_init)
            {
                // Movie rating file for a user   
                double[,] my_ratings = new double[task.num_jobs_init, 1];

                //Now we read R and Y from theirs files (-1 because I will remove the chosen user from the matrixes)
                double[,] Y = svc.readTrainingY(Directory.GetCurrentDirectory() + "/files/Y1.txt", task, my_ratings, user_number);
                double[,] R = svc.readTrainingR(Directory.GetCurrentDirectory() + "/files/R1.txt", task, user_number);

                //Creating a MatLab reference to execute the recommended job script
                IMatlabSvc matSvc = new MatlabSvcImpl();
                object[] res = matSvc.executeFilter(task, job_list, Directory.GetCurrentDirectory()+ "/files", my_ratings, Y, R, X, user_number);

                //Each time creates a  to be used to write the recommended jobs in a file
                List<TopJobData> mylist = svc.writeValuesToFile(writeTextResult, res, job_list, user_number);

                //Calculate Averages for Jobs for a User
                DataResult avgs = new DataResult(mylist, 10, users_profile[user_number - 1]);
                avgs.AverageForEachJob();
                svc.writeAveragesToFile(avgs, writeTextAverages, users_profile[user_number - 1]);

                total_avg_system += avgs.Percentage_total_avg;
                total_user_inaccuracy += avgs.Self_inaccuracy;
                //adding the list at the Dictionary for each user
                
                user_number++;
            }

            total_avg_system /= task.num_users_init;
            writeTextAverages.WriteLine("AVGS TOTAL\t" + total_avg_system);
            total_user_inaccuracy /= task.num_users_init;
            writeTextAverages.WriteLine("COMMUNITY INACCURACY\t" + total_user_inaccuracy);

            writeTextResult.Close();
            writeTextAverages.Close();

            //creating a instance of DataResult to be used to write the averages in a file
            Console.WriteLine("DONE");

            //Wait until fisnih
            Console.ReadLine();
        }
    }
}
