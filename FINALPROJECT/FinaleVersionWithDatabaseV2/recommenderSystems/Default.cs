using recommenderSystems.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recommenderSystems
{
    public class Default
    {
        public static void Main(string[] args)
        {
            DriverWebManager driverWebMgr = new DriverWebManager();
            bool result = driverWebMgr.MainRoutine();

            //DriverFileManager driverFileMgr = new DriverFileManager();
            //bool result2 = driverFileMgr.MainRoutine();
        
        }
    }
}
