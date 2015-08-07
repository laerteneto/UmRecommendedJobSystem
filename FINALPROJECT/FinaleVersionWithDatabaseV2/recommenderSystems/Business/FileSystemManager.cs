using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using recommenderSystems.Exceptions.Service;
using recommenderSystems.Service.Interface;
using recommenderSystems.Domain;
using System.IO;
using recommenderSystems.NewRecruiteeService;


namespace recommenderSystems.Business
{
    public class FileSystemManager : BusinessManager
    {
        public bool detectSizeOfJobsColumns(TaskDimensions task, String path)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.detectSizeOfJobsColumns(task, path);
            }
            catch (ServiceLoadException ex)
            {
                return false;
            }
        }

        public double[,] getNumberOfFeaturesX(String path, TaskDimensions task)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.getNumberOfFeaturesX(path, task);
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

        public StreamWriter getResultStreamWriter()
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.getResultStreamWriter();
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

        public StreamWriter getAverageStreamWriter()
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.getAverageStreamWriter();
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

        public StreamWriter getIdandAvgStreamWriter()
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.getIdandAvgStreamWriter();
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

        public StreamWriter getDifficultyStreamWriter()
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.getDifficultyStreamWriter();
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

        public bool readFeaturesX(double[,] X, String path, TaskDimensions task)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.readFeaturesX(X, path, task);
            }
            catch (ServiceLoadException ex)
            {
                return false;
            }
        }

        public String[] readJobNames(String path, TaskDimensions task)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.readJobNames(path, task);
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

        public double[,] readTrainingY(String path, TaskDimensions task, double[,] my_ratings, int user_number)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.readTrainingY(path, task, my_ratings, user_number);
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

        public double[,] readTrainingR(String path, TaskDimensions task, int user_number)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.readTrainingR(path, task, user_number);
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

        public List<TopJobData> writeValuesToFile(StreamWriter writeText, object[] res, String[] job_list, int user_number, double[,] X)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.writeValuesToFile(writeText, res, job_list, user_number, X);
        
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

        public bool writeAveragesToFile(DataResult result, StreamWriter writeText, UserProfile user)        
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.writeAveragesToFile(result, writeText, user);
            }
            catch (ServiceLoadException ex)
            {
                return false;
            }
        }

        public bool writeGlobalAveragesInformation(double total_rating_avg_systemm, double total_similarity_avg_system, double total_inaccuracy_system, TaskDimensions task, StreamWriter writeText, UserProfile[] user, double[] users_calculated_raitings)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.writeGlobalAveragesInformation(total_rating_avg_systemm, total_similarity_avg_system, total_inaccuracy_system, task, writeText, user, users_calculated_raitings);
            }
            catch (ServiceLoadException ex)
            {
                return false;
            }
        }

        public bool underAndOverEstimations(StreamWriter writeText, int id, int num, int numOverEstimated, int numActualy, int numUnderEstimated)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.underAndOverEstimations(writeText, id, num, numOverEstimated, numActualy, numUnderEstimated);        
            }
            catch (ServiceLoadException ex)
            {
                return false;
            }
        }

        public UserProfile[] readUserProfile(String path, TaskDimensions task)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.readUserProfile(path, task);
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

        public bool writeDifficultyToFile(StreamWriter writeText, DataResult avgs)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.writeDifficultyToFile(writeText, avgs);
            }
            catch (ServiceLoadException ex)
            {
                return false;
            }
        }

        public double[,] readFullY(String path, TaskDimensions task)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.readFullY(path, task);
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

        public bool writeFiles(String[] job_list, double[,] new_X, UserProfile[] users_profile, double[,] Y)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.writeFiles(job_list, new_X, users_profile, Y);
            }
            catch (ServiceLoadException ex)
            {
                return false;
            }
        }

        public List<RecruiteeDto> readIDandAVG(String path)
        {
            try
            {
                IFileSystemSvc svc = (IFileSystemSvc)this.getService(typeof(IFileSystemSvc).Name);
                return svc.readIDandAVG(path);
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }




    }
}
