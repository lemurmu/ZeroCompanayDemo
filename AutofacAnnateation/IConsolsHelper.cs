using Autofac.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacAnnateation
{
    public interface IConsolsHelper
    {
        void Info(string msg);
        void Debug(string msg);
        void Error(string msg);
        void Trace(string msg);
    }
}
