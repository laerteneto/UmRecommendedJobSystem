using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Interface
{
    interface IElasticSvc
    {
        bool insertRecommenderJob(DataResult avgs);
        double[,] SelectRatings(String[] expressions, UserProfile[] users);
        void InsertRatings(String[] expressions, UserProfile[] users, double[,] Y);
    }
}
