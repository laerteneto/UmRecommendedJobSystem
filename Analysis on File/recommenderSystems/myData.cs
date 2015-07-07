using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems
{
    public class MyData
    {
        private string recJobName;

        public string RecJobName
        {
            get { return recJobName; }
            set { recJobName = value; }
        }
        private double predRecJob;

        public double PredRecJob
        {
            get { return predRecJob; }
            set { predRecJob = value; }
        }
        private string compJobName;

        public string CompJobName
        {
            get { return compJobName; }
            set { compJobName = value; }
        }
        private double origRatJobComp;

        public double OrigRatJobComp
        {
            get { return origRatJobComp; }
            set { origRatJobComp = value; }
        }
        private double similarility;

        public double Similarility
        {
            get { return similarility; }
            set { similarility = value; }
        }


        // constructor
        public MyData(string recJobName, double predRecJob, string compJobName, double origRatJobComp, double similarility)
        {
            this.recJobName = recJobName;
            this.predRecJob = predRecJob;
            this.compJobName = compJobName;
            this.origRatJobComp = origRatJobComp;
            this.similarility = similarility;
        }

    }
}
