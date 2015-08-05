using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Interface
{
    public interface IRecruiteeSvc : IService
    {
        String[] selectRecruiteeNames();
        double[] selectRecruiteeSkills();
    }
}
