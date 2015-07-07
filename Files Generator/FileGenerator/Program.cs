using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            int jobs = 500;
            int users = 1000;
            int features = 10;
            string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

            //CREATING THE FILES Y AND R

            Random rnd = new Random(); 
            int r_value, y_value;
            StreamWriter writeR = new StreamWriter("R.txt");
            StreamWriter writeY = new StreamWriter("Y.txt");

            for (int i = 0; i < jobs; i++) //jobs
            {
                for (int j = 0; j < users; j++) //users
                {
                    r_value = rnd.Next(0, 2); //between 0 and 1
                    writeR.Write(r_value);
                    if (r_value == 0)
                    {
                        writeY.Write(0);
                    }
                    else
                    {
                        y_value = rnd.Next(1, 6); //between 1 and 5
                        writeY.Write(y_value);
                    }
                    if (j != users - 1)
                    {
                        writeY.Write("\t");
                        writeR.Write("\t");
                    }
                }
                if (i != jobs - 1)
                {
                    writeY.Write("\n");
                    writeR.Write("\n");
                }
            }            
            writeR.Close();
            writeY.Close();

            //CREATING THE FILE JOB_NAMES

            int k = 0; //used to loop through the alphabet
            int index = 1;
            StreamWriter writeJobNames = new StreamWriter("job_names.txt");
            for (int i = 0; i < jobs; i++) //jobs
            {
                if (k == 26)
                {
                    k = 0; 
                    index++;
                }
                writeJobNames.Write(alphabet[k] + index);
                if (i != jobs - 1)
                {
                    writeJobNames.Write("\n");
                }
                k++;
            }
            writeJobNames.Close();

            //CREATING THE FILE X

            int x_value;
            StreamWriter writeX = new StreamWriter("X.txt");
            for (int i = 0; i < jobs; i++)
            {
                for (int j = 0; j < features; j++)
                {
                    x_value = rnd.Next(0, 2); //between 0 and 1
                    writeX.Write(x_value);
                    if (j != features - 1)
                    {
                        writeX.Write("\t");
                    }
                }
                if (i != jobs - 1)
                {
                    writeX.Write("\n");
                }
            }
            writeX.Close();
        }
    }
}
