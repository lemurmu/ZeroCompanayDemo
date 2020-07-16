using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Annotation;
using Autofac.Aspect;

namespace AutofacAnnateation
{
    [Component("Startup")]
    public class Startup
    {
        [Autowired("ConsolHelper")]
        private IConsolsHelper helper;

        public void Info(string msg)
        {
            helper.Info(msg);
        }

        public void Error(string msg)
        {
            helper.Error(msg);
        }

        public void Trace(string msg)
        {
            helper.Trace(msg);
        }

        public void Debug(string msg)
        {
            helper.Debug(msg);
        }
    }
}
