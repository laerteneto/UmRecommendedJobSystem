using recommenderSystems.Domain;
using recommenderSystems.NewRecruiteeService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Interface
{
    public interface IFileSystemSvc : IService
    {
        bool detectSizeOfJobsColumns(TaskDimensions task, String path);
        double[,] getNumberOfFeaturesX(String path, TaskDimensions task);
        StreamWriter getResultStreamWriter();
        StreamWriter getAverageStreamWriter();
        StreamWriter getIdandAvgStreamWriter();
        StreamWriter getDifficultyStreamWriter();
        bool readFeaturesX(double[,] X, String path, TaskDimensions task);
        String[] readJobNames(String path, TaskDimensions task);
        double[,] readTrainingY(String path, TaskDimensions task, double[,] my_ratings, int user_number);
        double[,] readTrainingR(String path, TaskDimensions task, int user_number);
        List<TopJobData> writeValuesToFile(StreamWriter writeText, object[] res, String[] job_list, int user_number, double[,] X);
        bool writeAveragesToFile(DataResult result, StreamWriter writeText, UserProfile user);
        bool writeGlobalAveragesInformation(double total_rating_avg_systemm, double total_similarity_avg_system,
                                            double total_inaccuracy_system, TaskDimensions task,
                                            StreamWriter writeText, UserProfile[] user, double[] users_calculated_raitings);
        bool underAndOverEstimations(StreamWriter writeText, int id, int num, int numOverEstimated, int numActualy, int numUnderEstimated);
        UserProfile[] readUserProfile(String path, TaskDimensions task);
        bool writeDifficultyToFile(StreamWriter writeText, DataResult avgs);
        double[,] readFullY(String path, TaskDimensions task);
        bool writeFiles(String[] job_list, double[,] new_X, UserProfile[] users_profile, double[,] Y);
        List<RecruiteeDto> readIDandAVG(String path);


    }
}
