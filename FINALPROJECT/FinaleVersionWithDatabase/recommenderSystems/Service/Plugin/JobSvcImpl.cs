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
            JobService.ServiceWCFClient svc = new JobService.ServiceWCFClient();
            Guid[] jobNames = svc.selectExpressionNames();
            return Array.ConvertAll(jobNames, x => x.ToString());
        }

        //job difficulty (used in X)
        public double[] selectExpressionDifficulty()
        {
            JobService.ServiceWCFClient svc = new JobService.ServiceWCFClient();
            return svc.selectExpressionDifficulty();
        }
    }
}
