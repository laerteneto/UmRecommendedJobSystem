using recommenderSystems.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Plugin
{
    public class JobSvcImpl : IJobSvc
    {
        //jobs ids (used in expressions file)
        public string[] selectExpressionNames()
        {
            try
            {
                NewJobService.ServiceWCFClient svc = new NewJobService.ServiceWCFClient();
                Guid[] jobNames = svc.selectExpressionNames();
                return Array.ConvertAll(jobNames, x => x.ToString());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //job difficulty (used in X)
        public double[] selectExpressionDifficulty()
        {
            try
            {
                NewJobService.ServiceWCFClient svc = new NewJobService.ServiceWCFClient();
                return svc.selectExpressionDifficulty();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
