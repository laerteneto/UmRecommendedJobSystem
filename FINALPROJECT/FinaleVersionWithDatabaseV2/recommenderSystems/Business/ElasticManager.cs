using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using recommenderSystems.Exceptions.Service;
using recommenderSystems.Service.Interface;
using recommenderSystems.Domain;

namespace recommenderSystems.Business
{
    public class ElasticManager : BusinessManager
    {

        public bool insertRecommenderJob(DataResult avgs)
        {
            try
            {
                IElasticSvc svc = (IElasticSvc)this.getService(typeof(IElasticSvc).Name);
                return svc.insertRecommenderJob(avgs);
            }
            catch (ServiceLoadException ex)
            {
                return false;
            }

        }

        public double[,] selectRatings(String[] expressions, UserProfile[] users)
        {
            try
            {
                IElasticSvc svc = (IElasticSvc)this.getService(typeof(IElasticSvc).Name);
                return svc.selectRatings(expressions, users);
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }

        }

        public bool insertRatings(String[] expressions, UserProfile[] users, double[,] Y)
        {
            try
            {
                IElasticSvc svc = (IElasticSvc)this.getService(typeof(IElasticSvc).Name);
                return svc.insertRatings(expressions, users, Y);
            }
            catch (ServiceLoadException ex)
            {
                return false;
            }

        }

        public int[,] getYIndex(String jobID, String recruiteeID, String[] expressions, UserProfile[] users)
        {
            try
            {
                IElasticSvc svc = (IElasticSvc)this.getService(typeof(IElasticSvc).Name);
                return svc.getYIndex(jobID, recruiteeID, expressions, users);
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }

        }


    }
}
