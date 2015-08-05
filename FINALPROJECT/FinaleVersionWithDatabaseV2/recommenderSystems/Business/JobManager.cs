using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using recommenderSystems.Exceptions.Service;
using recommenderSystems.Service.Interface;

namespace recommenderSystems.Business
{
    public class JobManager : BusinessManager
    {
        public string[] selectExpressionNames()
        {
            try
            {
                IJobSvc svc = (IJobSvc)this.getService(typeof(IJobSvc).Name);
                return svc.selectExpressionNames();
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

        public double[] selectExpressionDifficulty()
        {
            try
            {
                IJobSvc svc = (IJobSvc)this.getService(typeof(IJobSvc).Name);
                return svc.selectExpressionDifficulty();
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }


    }
}
