using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Domain
{
    public class TopJobData
    { 
        //This is a recommend job from the  list of top jobs. This will be on the first column at result file. 
        private string recJobName;
        public string RecJobName
        {
            get { return recJobName; }
            set { recJobName = value; }
        }

        //This is a recommend job rate from our colaboration filtering. This will be on the second column at result file.
        private double predRecJob;
        public double PredRecJob
        {
            get { return predRecJob; }
            set { predRecJob = value; }
        }

        //This may be any job from the original list of jobs that has >=70% in similarity compared with the recommend 
        //job (recJobName). This will be on the third column at result file.
        private string compJobName;
        public string CompJobName
        {
            get { return compJobName; }
            set { compJobName = value; }
        }

        //Difficulty for the original job above
        private double compJobDiff;
        public double CompJobDiff
        {
            get { return compJobDiff; }
            set { compJobDiff = value; }
        }

        //This is the rate from the original job (compJobName). This will be on the fourth column at result file.
        private double origRatJobComp;
        public double OrigRatJobComp
        {
            get { return origRatJobComp; }
            set { origRatJobComp = value; }
        }

        //this is the similarity, which is measured by the Manhattam distance. It's always >= 70%. This will be on 
        //the fifth column at result file.
        private double similarility;
        public double Similarility
        {
            get { return similarility; }
            set { similarility = value; }
        }

        //Constructor
        public TopJobData(string recJobName, double predRecJob, string compJobName, double compJobDiff, double origRatJobComp, double similarility)
        {
            this.recJobName = recJobName;
            this.predRecJob = predRecJob;
            this.compJobName = compJobName; 
            this.compJobDiff = compJobDiff;
            this.origRatJobComp = origRatJobComp;
            this.similarility = similarility;
        }

    }
}
