using recommenderSystems.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Interface
{
    public interface IElasticSvc : IService
    {
        bool insertRecommenderJob(DataResult avgs);
        bool insertRatings(String[] expressions, UserProfile[] users, double[,] Y);
        int[,] getYIndex(String jobID, String recruiteeID, String[] expressions, UserProfile[] users);
        double[,] selectRatings(String[] expressions, UserProfile[] users);
        bool updateRanking(String path);

    }
}
