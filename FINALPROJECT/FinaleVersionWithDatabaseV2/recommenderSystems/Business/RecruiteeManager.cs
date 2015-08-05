using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using recommenderSystems.Business;
using recommenderSystems.Service.Interface;
using recommenderSystems.Exceptions.Service;
using recommenderSystems.Domain;

namespace recommenderSystems.Business
{
    public class RecruiteeManager : BusinessManager
    {
        public String[] selectRecruiteeNames()
        {
            try
            {
                IRecruiteeSvc svc = (IRecruiteeSvc)this.getService(typeof(IRecruiteeSvc).Name);
                return svc.selectRecruiteeNames();
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }
        
        public double[] selectRecruiteeSkills()
        {
            try
            {
                IRecruiteeSvc svc = (IRecruiteeSvc)this.getService(typeof(IRecruiteeSvc).Name);
                return svc.selectRecruiteeSkills();
            }
            catch (ServiceLoadException ex)
            {
                return null;
            }
        }

    }
}
