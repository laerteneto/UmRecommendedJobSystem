using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace recommenderSystems.Exceptions.Business
{
    public class BusinessValidationException : Exception
    {
        public BusinessValidationException(String message) 
            : base(message)
        {
        }
    }
}
