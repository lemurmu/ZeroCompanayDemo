using Autofac.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacAnnateation
{
    [Component("LogHelper")]
    public class LogHelper : IConsolsHelper
    {
        public void Debug(string msg)
        {
            throw new NotImplementedException();
        }

        public void Error(string msg)
        {
            throw new NotImplementedException();
        }

        public void Info(string msg)
        {
            throw new NotImplementedException();
        }

        public void Trace(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
