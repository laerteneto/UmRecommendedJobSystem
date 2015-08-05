using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using recommenderSystems.Service.Interface;
using recommenderSystems.Service;

namespace recommenderSystems.Business
{
    public abstract class BusinessManager
    {
        protected IService getService(String name)
        {
            return (Factory.getInstance()).getService(name);
        }
    }
}
