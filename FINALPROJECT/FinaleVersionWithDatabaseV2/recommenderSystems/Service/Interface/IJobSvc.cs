using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Interface
{
    public interface IJobSvc : IService
    {
        string[] selectExpressionNames();
        double[] selectExpressionDifficulty();
    }
}
