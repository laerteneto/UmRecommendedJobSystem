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
    public class DriverWeb
    {
        public static void Main(string[] args)
        {
            //JOBS LIST and X is working but the order is different
            JobSvcImpl j = new JobSvcImpl();
            String[] job_list = j.selectExpressionNames(); //job_names
            double[] X = j.selectExpressionDifficulty(); //X
            double[,] new_X = new double[X.Length, 1];
            for (int i = 0; i < X.Length; i++)
            {
                new_X[i, 0] = X[i];
            }

            //User Profile (just the user ID, I still need the user self rating)
            RecruiteeSvcImpl r = new RecruiteeSvcImpl();
            String[] recruitee_names = r.selectRecruiteeNames();
            double[] recruitee_skill = r.selectRecruiteeSkills();
            UserProfile[] users_profile = new UserProfile[recruitee_skill.Length];
            for (int i = 0; i < recruitee_skill.Length; i++)
            {
                users_profile[i] = new UserProfile("",0);
                users_profile[i].UserID = recruitee_names[i];
                users_profile[i].UserRating = recruitee_skill[i];
            }
            //new_Y
            IElasticSvc es = new ElasticSvcImpl();
            double[,] Y = es.SelectRatings(job_list, users_profile);
            

            ///////// WRITING VARIABLES IN FILE ////////////
            FromWebToFile file = new FromWebToFile();
            file.writeFiles(job_list,new_X,users_profile,Y);




            //Object to Hold Task Parameters
            TaskDimensions task = new TaskDimensions();
            task.num_features = new_X.GetLength(1); //1 is the number of columns
            task.num_jobs_init = job_list.Length;
            task.num_users_init = recruitee_names.Length;

            //User number to start proccessing
            int user_number = 1;

            //Load File System Service
            IFileSystemSvc svc = new FileSystemSvcImpl();

            //Creating a variable to write in a File the job recommendations and comparisons
            //Load File Writer
            StreamWriter writeTextResult = svc.getResultStreamWriter();
            StreamWriter writeTextAverages = svc.getAverageStreamWriter();
            StreamWriter writeText = svc.getIdandAvgStreamWriter();
            StreamWriter writeTextDiff = svc.getDifficultyStreamWriter();

            int numUnderEstimated = 0, numOverEstimated = 0;
            double[] users_calculated_raitings = new double[task.num_users_init];

            double total_rating_avg_system = 0;
            double total_similarity_avg_system = 0;
            double total_inaccuracy_system = 0;

            while (user_number <= task.num_users_init)
            {
                double[,] my_ratings = new double[task.num_jobs_init, 1];
                double[,] new_Y = new double[task.num_jobs_init, task.num_users_init - 1];
                double[,] R = new double[task.num_jobs_init, task.num_users_init - 1];

                for (int i = 0; i < job_list.Length; i++)
                {
                    int k = 0;
                    for (int n = 0; n < users_profile.Length; n++)
                    {
                        if (n != (user_number - 1))
                        {
                            new_Y[i, k] = Y[i, n];
                            if (Y[i, n] != 0)
                                R[i, k] = 1;
                            else
                                R[i, k] = 0;
                            k++;
                        }
                        else
                            my_ratings[i, 0] = Y[i, n];
                    }
                }             
                
               
                //Creating a MatLab reference to execute the recommended job script
                IMatlabSvc matSvc = new MatlabSvcImpl();
                object[] res = matSvc.executeFilter(task, job_list, Directory.GetCurrentDirectory()+ "/files", my_ratings, new_Y, R, new_X, user_number);
                
                
                //Each time creates a  to be used to write the recommended jobs in a file
                List<TopJobData> mylist = svc.writeValuesToFile(writeTextResult, res, job_list, user_number, new_X);
                

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

                //people who under and overestimated themselves
                if (avgs.Self_inaccuracy > 0)
                    numOverEstimated++;
                else
                    numUnderEstimated++;
                users_calculated_raitings[user_number - 1] = avgs.Rating_total_avg;

                //writing in the difficulty file
                svc.writeDifficultyToFile(writeTextDiff, avgs);
                
                
                //used to insert recommended jobs for a user in the database
                es.insertRecommenderJob(avgs);
                

                user_number++;

            }

            total_rating_avg_system /= task.num_users_init;
            total_similarity_avg_system /= task.num_users_init;
            total_inaccuracy_system /= task.num_users_init;
            //writing some more global information
            svc.writeGlobalAveragesInformation(total_rating_avg_system, total_similarity_avg_system, total_inaccuracy_system, numUnderEstimated,
                numOverEstimated, task, writeTextAverages, users_profile, users_calculated_raitings);


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
