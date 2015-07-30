using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems
{
    class FromWebToFile
    {
        public void writeFiles(String[] job_list, double[,] new_X, UserProfile[] users_profile, double[,] Y)
        {
            StreamWriter write_job_list = new StreamWriter("NEW_espression.txt");
            StreamWriter write_new_X = new StreamWriter("NEW_X.txt");
            StreamWriter write_users_profile = new StreamWriter("NEW_user_table.txt");
            StreamWriter write_Y = new StreamWriter("NEW_Y.txt");
            StreamWriter write_R = new StreamWriter("NEW_R.txt");

            for (int i = 0; i < job_list.Length; i++)
            {
                write_job_list.Write(job_list[i]);
                if (i != job_list.Length - 1)
                    write_job_list.Write("\n");
            }

            for (int i = 0; i < new_X.GetLength(0); i++)
            {
                for (int j = 0; j < new_X.GetLength(1); j++)
                {
                    write_new_X.Write(new_X[i, j]);
                    if (j != new_X.GetLength(1) - 1)
                    {
                        write_new_X.Write("\t");
                    }
                }
                if (i != new_X.GetLength(0) - 1)
                {
                    write_new_X.Write("\n");
                }
            }

            for (int i = 0; i < users_profile.Length; i++)
            {
                write_users_profile.Write(users_profile[i].UserID + "\t" + users_profile[i].UserRating);
                if (i != users_profile.Length - 1)
                {
                    write_users_profile.Write("\n");
                }
            }

            for (int i = 0; i < Y.GetLength(0); i++)
            {
                for (int j = 0; j < Y.GetLength(1); j++)
                {
                    write_Y.Write(Y[i, j] + "\t");
                    if (Y[i, j] != 0)
                    {
                        write_R.Write("1\t");
                    }
                    else
                    {
                        write_R.Write("0\t");
                    }
                }
                if (i != new_X.GetLength(0) - 1)
                {
                    write_Y.Write("\n");
                    write_R.Write("\n");
                }
            }

            write_job_list.Close();
            write_new_X.Close();
            write_users_profile.Close();
            write_Y.Close();
            write_R.Close();

        }
    }
}
