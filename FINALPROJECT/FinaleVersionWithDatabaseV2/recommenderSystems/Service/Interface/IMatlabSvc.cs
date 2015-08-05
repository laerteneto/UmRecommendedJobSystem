using recommenderSystems.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Interface
{
    interface IMatlabSvc
    {
        bool changeDirectory(String path);
        object[] executeFilter(TaskDimensions task, String[] job_list, String path, double[,] my_ratings, double[,] Y, double[,] R, double[,] X);
    }
}
