using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Interface
{
    interface IJobSvc
    {
        string[] selectExpressionNames();
        double[] selectExpressionDifficulty();
    }
}
