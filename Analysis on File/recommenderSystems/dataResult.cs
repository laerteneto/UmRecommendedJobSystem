using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace recommenderSystems
{
    class dataResult
    {
        private double rating_total_avg;

        public double Rating_total_avg
        {
            get { return rating_total_avg; }
            set { rating_total_avg = value; }
        }

        private double percentage_total_avg;

        public double Percentage_total_avg
        {
            get { return percentage_total_avg; }
            set { percentage_total_avg = value; }
        }

        private int number_top_jobs;

        public int Number_top_jobs
        {
            get { return number_top_jobs; }
            set { number_top_jobs = value; }
        }

        private List<MyData> list;

        public List<MyData> List
        {
          get { return list; }
          set { list = value; }
        }

        private double[] rating_average; 

        public double[] Rating_average
        {
            get { return rating_average; }
            set { rating_average = value; }
        }

        private string[] topJobNames;

        public string[] TopJobNames
        {
            get { return topJobNames; }
            set { topJobNames = value; }
        }

        private double[] percentage_average;

        public double[] Percentage_average
        {
            get { return percentage_average; }
            set { percentage_average = value; }
        }



        //contructor
        public dataResult(List<MyData> list, int number_top_jobs)
        {
            this.list = list;
            this.number_top_jobs = number_top_jobs;
        }



        public void AverageForEachJob()
        {
            rating_average = new double[number_top_jobs]; 
            percentage_average = new double[number_top_jobs];
            this.topJobNames = new string[number_top_jobs];
            string top_job;
            double recom_rate;

            int i = 0;
            int k = 0;
            double sum = 0, sum_similarity = 0;
            int quantity = 0;


            while (i < number_top_jobs)
            {
                    top_job = list.ElementAt(k).RecJobName;
                    recom_rate = list.ElementAt(k).PredRecJob;
                    sum += list.ElementAt(k).OrigRatJobComp;
                    sum_similarity += list.ElementAt(k).Similarility;
                    quantity++;

                    if (i != 9)
                    {
                        if (k == list.Count - 1)
                        {
                            return;
                        }
                        if (!top_job.Equals(list.ElementAt(k + 1).RecJobName))
                        {
                            topJobNames[i] = top_job;
                            rating_average[i] = sum / quantity; //average
                            percentage_average[i] = sum_similarity / quantity;
                            i++;
                            quantity = 0;
                            sum = 0;
                            sum_similarity = 0;
                        }
                     
                    }
                    else if (k == list.Count - 1) //if it is the end of the list
                    {
                        topJobNames[i] = top_job;
                        rating_average[i] = sum / quantity; //average
                        percentage_average[i] = sum_similarity / quantity;
                        i++;
                        quantity = 0;
                        sum = 0;
                        sum_similarity = 0;
                    }
                    k++;

            }

            rating_total_avg = 0;
            //setting the value of the total average for the user
            for (int j = 0; j < number_top_jobs; j++)
            {
                rating_total_avg += rating_average[j];
            }
            rating_total_avg /= number_top_jobs;


            percentage_total_avg = 0;
            //setting the value of the total average percentage for the user
            for (int j = 0; j < number_top_jobs; j++)
            {
                percentage_total_avg += percentage_average[j];
            }
            percentage_total_avg /= number_top_jobs;
        }


        public void WriteInAFile(Dictionary<int, List<MyData>> finalList)
        {
            StreamWriter writetext = new StreamWriter("averages.txt");

            for (int i = 0; i < finalList.Count; i++)
            {
                dataResult avgs = new dataResult(finalList.Values.ElementAt(i), number_top_jobs);
                avgs.AverageForEachJob();
                writetext.WriteLine("USER: " + (i+1));
                for (int k = 0; k < number_top_jobs; k++)
                {
                    writetext.WriteLine("Job " + avgs.TopJobNames[k] + "\t" + avgs.Rating_average[k] + "\t" + avgs.Percentage_average[k]);
                }
                writetext.WriteLine("AVGS TOTAL\t"  + avgs.Rating_total_avg + "\t" + avgs.Percentage_total_avg + "\n");

            }            
            writetext.Close();
        }



    }
}
