using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Interface
{
    interface IRecruiteeSvc
    {
        String[] selectRecruiteeNames();
        double[] selectRecruiteeSkills();
    }
}
