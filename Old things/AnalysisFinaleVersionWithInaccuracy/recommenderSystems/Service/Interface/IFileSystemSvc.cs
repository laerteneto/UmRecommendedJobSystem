using recommenderSystems.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Interface
{
    public interface IFileSystemSvc
    {
        void detectSizeOfJobsColumns(TaskDimensions task, String path);
        double[,] getNumberOfFeaturesX(String path, TaskDimensions task);
        StreamWriter getResultStreamWriter();
        StreamWriter getAverageStreamWriter();
        void readFeaturesX(double[,] X, String path, TaskDimensions task);
        String[] readJobNames(String path, TaskDimensions task);
        double[,] readTrainingY(String path, TaskDimensions task, double[,] my_ratings, int user_number);
        double[,] readTrainingR(String path, TaskDimensions task, int user_number);
        List<TopJobData> writeValuesToFile(StreamWriter writeText, object[] res, String[] job_list, int user_number);
        void writeAveragesToFile(DataResult result, StreamWriter writeText, UserProfile user);
        UserProfile[] readUserProfile(String path, TaskDimensions task);
        
    }
}
