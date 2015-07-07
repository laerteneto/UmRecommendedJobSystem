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
            svc.detectSizeOfJobsColumns(task, Directory.GetCurrentDirectory()+"/files/Y.txt");

            //Method call to get the number of features from the file X, and allocating the X matrix
            double[,] X = svc.getNumberOfFeaturesX(Directory.GetCurrentDirectory() + "/files/X.txt", task);
            svc.readFeaturesX(X, Directory.GetCurrentDirectory() + "/files/X.txt", task);

            //Method call to get the jobs names
            String[] job_list = svc.readJobNames(Directory.GetCurrentDirectory() + "/files/expressions.txt", task);

            //method that return the users profile
            UserProfile[] users_profile = svc.readUserProfile(Directory.GetCurrentDirectory() + "/files/user_table.txt", task);

            //Creating a variable to write in a File the job recommendations and comparisons
            //Load File Writer
            StreamWriter writeTextResult = svc.getResultStreamWriter();
            StreamWriter writeTextAverages = svc.getAverageStreamWriter();

/*new*/
            //file for sadallo
            StreamWriter writeText = new StreamWriter("IDandAVG.txt");
            //new variables
            int numUnderEstimated = 0, numOverEstimated = 0;
            double[] users_calculated_raitings = new double[task.num_users_init];
/*End new*/

            double total_avg_system = 0;
            double total_user_inaccuracy = 0;

            while (user_number <= task.num_users_init)
            {
                // Movie rating file for a user   
                double[,] my_ratings = new double[task.num_jobs_init, 1];

                //Now we read R and Y from theirs files (-1 because I will remove the chosen user from the matrixes)
                double[,] Y = svc.readTrainingY(Directory.GetCurrentDirectory() + "/files/Y.txt", task, my_ratings, user_number);
                double[,] R = svc.readTrainingR(Directory.GetCurrentDirectory() + "/files/R.txt", task, user_number);

                //Creating a MatLab reference to execute the recommended job script
                IMatlabSvc matSvc = new MatlabSvcImpl();
                object[] res = matSvc.executeFilter(task, job_list, Directory.GetCurrentDirectory()+ "/files", my_ratings, Y, R, X, user_number);

                //Each time creates a  to be used to write the recommended jobs in a file
                List<TopJobData> mylist = svc.writeValuesToFile(writeTextResult, res, job_list, user_number);

                //Calculate Averages for Jobs for a User
                DataResult avgs = new DataResult(mylist, mylist.Count, users_profile[user_number - 1]);
                avgs.AverageForEachJob();
                svc.writeAveragesToFile(avgs, writeTextAverages, users_profile[user_number - 1]);

                total_avg_system += avgs.Percentage_total_avg;
                total_user_inaccuracy += avgs.Self_inaccuracy;
                //adding the list at the Dictionary for each user

/*new*/
                //file for sadallo
                writeText.WriteLine(users_profile[user_number - 1].UserID + "\t" + avgs.Rating_total_avg);

                if (avgs.Self_inaccuracy > 0)
                    numOverEstimated++;
                else
                    numUnderEstimated++;
                users_calculated_raitings[user_number - 1] = avgs.Rating_total_avg;
/*End new*/
                user_number++;
            }


            total_avg_system /= task.num_users_init;
            writeTextAverages.WriteLine("Avgs total (similarity)\t" + total_avg_system);
            total_user_inaccuracy /= task.num_users_init;
            writeTextAverages.WriteLine("Community inaccuracy\t" + total_user_inaccuracy + "\n");


/*new*/
            //file for sadallo
            writeText.Close();
            writeTextAverages.WriteLine("Number of users that underestimated themselves\t" + numUnderEstimated);
            writeTextAverages.WriteLine("Number of users that overestimated themselves\t" + numOverEstimated + "\n");
/*End new*/


/*new*/
            int numOverEstimated4 = 0, numActualy4 = 0, numUnderEstimated4 = 0;
            int numOverEstimated5 = 0, numActualy5 = 0, numUnderEstimated5 = 0;
            int num1 = 0, num2 = 0, num3 = 0, num4 = 0, num5 = 0;
            int num1_after = 0, num2_after = 0, num3_after = 0, num4_after = 0, num5_after = 0;
            for (int i = 0; i < task.num_users_init; i++)
            {
                switch ((int)users_profile[i].UserRating)
                {
                    case 1:
                        num1++;
                        break;
                    case 2:
                        num2++;
                        break;
                    case 3:
                        num3++;
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
                        else if ((int)Math.Round(users_calculated_raitings[i], 0) == 5)
                            numActualy5++;
                        else
                            numUnderEstimated5++;
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

            writeTextAverages.WriteLine("Number of self ratings (before calculation)");
            writeTextAverages.WriteLine("amount of 1:\t" + num1);
            writeTextAverages.WriteLine("amount of 2:\t" + num2);
            writeTextAverages.WriteLine("amount of 3:\t" + num3);
            writeTextAverages.WriteLine("amount of 4:\t" + num4);
            writeTextAverages.WriteLine("amount of 5:\t" + num5);

            writeTextAverages.WriteLine("\nNumber of self ratings (after calculation)");
            writeTextAverages.WriteLine("amount of 1:\t" + num1_after);
            writeTextAverages.WriteLine("amount of 2:\t" + num2_after);
            writeTextAverages.WriteLine("amount of 3:\t" + num3_after);
            writeTextAverages.WriteLine("amount of 4:\t" + num4_after);
            writeTextAverages.WriteLine("amount of 5:\t" + num5_after);

            writeTextAverages.WriteLine("\nFor the people who rated themselves as 4 (" + num4 + ")");
            writeTextAverages.WriteLine("amount of those who overestimated themselves were:\t" + numOverEstimated4);
            writeTextAverages.WriteLine("amount of those who estimated themselves correctly were:\t" + numActualy4);
            writeTextAverages.WriteLine("amount of those who underestimated themselves were:\t" + numUnderEstimated4);

            writeTextAverages.WriteLine("\nFor the people who rated themselves as 5 (" + num5 + ")");
            writeTextAverages.WriteLine("amount of those who overestimated themselves were:\t" + numOverEstimated5);
            writeTextAverages.WriteLine("amount of those who estimated themselves correctly were:\t" + numActualy5);
            writeTextAverages.WriteLine("amount of those who underestimated themselves were:\t" + numUnderEstimated5);
/*End new*/

            writeTextResult.Close();
            writeTextAverages.Close();

            //creating a instance of DataResult to be used to write the averages in a file
            Console.WriteLine("DONE");

            //Wait until fisnih
            Console.ReadLine();
        }
    }
}
