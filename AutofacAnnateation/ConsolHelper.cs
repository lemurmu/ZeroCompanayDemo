using Autofac.Annotation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacAnnateation
{
    [Component("ConsolHelper")]
    public class ConsolHelper:IConsolsHelper
    {
        public void Info(string msg)
        {
            Console.WriteLine("info:" + msg);
        }

        public void Debug(string msg)
        {
            Console.WriteLine("debug" + msg);
        }

        public void Error(string msg)
        {
            Console.WriteLine("error:" + msg);
        }

        public void Trace(string msg)
        {
            Console.WriteLine("trace:" + msg);
        }



    }
}
