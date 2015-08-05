using recommenderSystems.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems.Service.Plugin
{
    public class RecruiteeSvcImpl : IRecruiteeSvc
    {
        //user profile (just the first information of the user profile)
        public String[] selectRecruiteeNames()
        {
            try
            {
                RecruiteeService.ServiceWCFClient svc = new RecruiteeService.ServiceWCFClient();
                Guid[] recNames = svc.selectRecruiteeNames();
                return Array.ConvertAll(recNames, x => x.ToString());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //user self ratings
        public double[] selectRecruiteeSkills()
        {
            try
            {

                RecruiteeService.ServiceWCFClient svc = new RecruiteeService.ServiceWCFClient();
                RecruiteeService.RecruiteeSkillDto[] recSkills = svc.selectAllRecruiteeSkill();
                double[] result = new double[recSkills.Length];
                for (int n = 0; n < recSkills.Length; n++)
                {
                    switch (recSkills[n].SkillId)
                    {
                        case "SKI01":
                            result[n] = 1;
                            break;
                        case "SKI02":
                            result[n] = 2;
                            break;
                        case "SKI03":
                            result[n] = 3;
                            break;
                        case "SKI04":
                            result[n] = 4;
                            break;
                        case "SKI05":
                            result[n] = 5;
                            break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
