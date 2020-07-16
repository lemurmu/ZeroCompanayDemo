using Autofac.Aspect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacAnnateation
{

    [Pointcut("info", Class = "ConsolHelper", Method = "*")]
    //[Pointcut("zz",Class = "ConsolHelper", Method = "Trace")]
    public class LogAspect
    {
        [Before("info")]
        public void Before()
        {
            Console.WriteLine("Before");
        }

        [After("info")]
        public void After()
        {
            Console.WriteLine("After");
        }

        [Before("zz")]
        public void Before1()
        {
            Console.WriteLine("ready Go!");
        }

        [After("zz")]
        public void After1()
        {
            Console.WriteLine("finish!");
        }


        [Before("name2")]
        public void Before2()
        {
            Console.WriteLine("Before2");
        }

        [After("name2")]
        public void After2()
        {
            Console.WriteLine("After2");
        }

        [Around("name3")]
        public async Task Around(PointcutContext context)
        {
            Console.WriteLine(context.InvocationMethod.Name + "-->Start");
            await context.Proceed();
            Console.WriteLine(context.InvocationMethod.Name + "-->End");
        }


        [Before("name4")]
        public async Task Before4()
        {
            await Task.Delay(1000);
            Console.WriteLine("Before4");
        }

        [After("name4")]
        public async Task After4()
        {
            await Task.Delay(1000);
            Console.WriteLine("After4");
        }




    }
}
