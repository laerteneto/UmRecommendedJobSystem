﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using recommenderSystems.Service.Interface;
using recommenderSystems.Service.Plugin;

namespace recommenderSystems
{
    public class DataResult
    {
        //All information stored and calculated on the instance variables will be written in a Average File.

        //These are the names of the top (ten) jobs for a user. It's going to be written in the first column on the
        //Average File. The second column will contain the predicted rating for the respective top (ten) job.
        private string[] topJobNames;
        public string[] TopJobNames
        {
            get { return topJobNames; }
            set { topJobNames = value; }
        }

        //this is an array whose elements contain the average of the ratings of the jobs that compared with
        //each one of the top (10) jobs has a similarity of >=70%. This information will be stored at the third column.
        private double[] rating_average;
        public double[] Rating_average
        {
            get { return rating_average; }
            set { rating_average = value; }
        }

        //this is an array whose elements contain the average of the similarity between each one of the top (10) jobs 
        //and the compared jobs. This information will be stored at the fourth column.
        private double[] percentage_average;
        public double[] Percentage_average
        {
            get { return percentage_average; }
            set { percentage_average = value; }
        }

        //This is the average of the elements on rating_average array. This information will be stored at the file once for each one of the users.
        private double rating_total_avg;
        public double Rating_total_avg
        {
            get { return rating_total_avg; }
            set { rating_total_avg = value; }
        }

        //This is the average of the elements on percentage_average array. This information will be stored at the fourth column.
        private double percentage_total_avg;
        public double Percentage_total_avg
        {
            get { return percentage_total_avg; }
            set { percentage_total_avg = value; }
        }

        //This is the number of top jobs, which will determinate the size of the rating_average and percentage_average arrays.
        private int number_top_jobs;
        public int Number_top_jobs
        {
            get { return number_top_jobs; }
            set { number_top_jobs = value; }
        }

        //This is a list used to calculate the averages. 
        private List<TopJobData> list;
        public List<TopJobData> List
        {
            get { return list; }
            set { list = value; }
        }
        
        //This dictionary contains all the lists for all of the users.
        private Dictionary<int, List<TopJobData>> finalList;
        public Dictionary<int, List<TopJobData>> FinalList
        {
            get { return finalList; }
            set { finalList = value; }
        }

        //Constructor
        public DataResult(Dictionary<int, List<TopJobData>> finalList, int number_top_jobs)
        {
            this.finalList = finalList;
            this.number_top_jobs = number_top_jobs;
        }

        //Calculates the average data for each job for each user
        public void AverageForEachJob()
        {
            this.rating_average = new double[number_top_jobs];
            this.percentage_average = new double[number_top_jobs];
            this.topJobNames = new string[number_top_jobs];
            string top_job;
            double recom_rate;

            int i = 0;
            int k = 0;
            double sum = 0, sum_similarity = 0;
            int quantity = 0;

            while (k <= this.list.Count - 1)
            {
                top_job = this.list.ElementAt(k).RecJobName;
                recom_rate = this.list.ElementAt(k).PredRecJob;
                sum += this.list.ElementAt(k).OrigRatJobComp;
                sum_similarity += this.list.ElementAt(k).Similarility;
                quantity++;

                if (k == list.Count - 1) //if it is the end of the list
                {
                    this.topJobNames[i] = top_job;
                    this.rating_average[i] = sum / quantity; //average
                    this.percentage_average[i] = sum_similarity / quantity;
                    i++;
                    quantity = 0;
                    sum = 0;
                    sum_similarity = 0;
                }
                else if (!top_job.Equals(this.list.ElementAt(k + 1).RecJobName))
                {
                    this.topJobNames[i] = top_job;
                    this.rating_average[i] = sum / quantity; //average
                    this.percentage_average[i] = sum_similarity / quantity;
                    i++;
                    quantity = 0;
                    sum = 0;
                    sum_similarity = 0;
                }
                k++;
            }

            this.rating_total_avg = 0;
            //Setting the value of the total average for the user
            for (int j = 0; j < this.number_top_jobs; j++)
            {
                this.rating_total_avg += this.rating_average[j];
            }
            this.rating_total_avg /= this.number_top_jobs;


            this.percentage_total_avg = 0;
            //Setting the value of the total average percentage for the user
            for (int j = 0; j < this.number_top_jobs; j++)
            {
                this.percentage_total_avg += this.percentage_average[j];
            }
            this.percentage_total_avg /= this.number_top_jobs;
        }

        //Writes in a file the calculated averages for each job for each user in FinalList, and also the average of the 
        //percentage averages for each jobs (the total average for the System)
        public void calculateAverages()
        {
            double totalAverageforSystem = 0;
            int i;
            IFileSystemSvc svc = new FileSystemSvcImpl();
            StreamWriter writeText = svc.getAverageStreamWriter();
            for (i = 0; i < this.finalList.Count; i++)
            {
                this.list = this.finalList.Values.ElementAt(i);
                this.AverageForEachJob();
                svc.writeAveragesToFile(this, writeText, i);
                totalAverageforSystem += this.Percentage_total_avg;
            }
            writeText.WriteLine("TOTAL AVERAGE FOR THE SYSTEM " + totalAverageforSystem/i);
            writeText.Close();
        }
    }
}
