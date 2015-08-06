using recommenderSystems.Domain;
using recommenderSystems.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Plugin
{
    public class ElasticSvcImpl : IElasticSvc
    {
        public bool insertRecommenderJob(DataResult avgs)
        {
            try
            {
                bool flag = false;
                NewElasticService.ServiceWCFClient ws = new NewElasticService.ServiceWCFClient();
                for (int n = 0; n < avgs.Number_top_jobs; n++)
                {
                    NewElasticService.RecommendedJobDto job = new NewElasticService.RecommendedJobDto();
                    job.RecruiteeId = new Guid(avgs.User_profile.UserID);
                    job.JobId = new Guid(avgs.TopJobNames[n]);
                    job.PredictedRankingValue = avgs.Mylist.ElementAt(n).PredRecJob;
                    flag = ws.insertRecommendedJob(job);
                    
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        //To fill Y
        public bool insertRatings(String[] expressions, UserProfile[] users, double[,] Y)
        {
            try
            {
                NewElasticService.ServiceWCFClient svc = new NewElasticService.ServiceWCFClient();
                NewElasticService.TaskDto[] tasks = svc.selectAllTask();
                foreach (NewElasticService.TaskDto task in tasks)
                {
                    int[,] result = this.getYIndex(task.JobId.ToString(), task.RecruiteeId.ToString(), expressions, users);
                    task.Rating = Y[result[0, 0], result[0, 1]];
                    svc.updateTask(task);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //This functions receives two strings, the first is the job id, the second is the recruitee id.
        //then, it looks at the expressions array (all jobs ids) to find the index of the given JobID,
        //then, it looks at the users array (all users ids and self ratings) to find the index of the given recruitee id. 
        //then it returns the value of the rating for that job and recruitee on the Y matrix.
        public int[,] getYIndex(String jobID, String recruiteeID, String[] expressions, UserProfile[] users)
        {
            try
            {
                int column = 0, row = 0;
                for (int i = 0; i < expressions.Length; i++)
                {
                    if ((expressions[i].ToUpper()).Equals(jobID.ToUpper()))
                    {
                        row = i;
                        break;
                    }
                }
                for (int i = 0; i < users.Length; i++)
                {
                    if ((users[i].UserID.ToUpper()).Equals(recruiteeID.ToUpper()))
                    {
                        column = i;
                        break;
                    }
                }
                int[,] result = new int[1, 2];
                result[0, 0] = row;
                result[0, 1] = column;
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        //getting Y from data base
        public double[,] selectRatings(String[] expressions, UserProfile[] users)
        {
            try
            {
                double[,] Y = new double[expressions.Length, users.Length];
                NewElasticService.ServiceWCFClient svc = new NewElasticService.ServiceWCFClient();
                NewElasticService.TaskRatingDTO[] tasks = svc.selectRatings();

                foreach (NewElasticService.TaskRatingDTO task in tasks)
                {
                    int[,] result = this.getYIndex(task.JobId.ToString(), task.RecruiteeId.ToString(), expressions, users);
                    Y[result[0, 0], result[0, 1]] = (double)task.Rating;
                }
                return Y;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void updateRanking()
        {
            // call readIDandAVG
        }
    }
}
