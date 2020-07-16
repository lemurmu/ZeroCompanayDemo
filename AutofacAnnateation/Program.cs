using Autofac;
using Autofac.Annotation;
using Autofac.Configuration.Test.test3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacAnnateation
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            // autofac打标签模式
            builder.RegisterModule(new AutofacAnnotationModule(typeof(Program).Assembly));

            var container = builder.Build();

            var startup = container.Resolve<Startup>();

            //startup.Info("start app.......");

            startup.Trace("trace.........");


            var a12 = container.Resolve<TestModel91>();

           // testc(a12);

            Console.ReadKey();

        }

        static async void testc(TestModel91 a12)
        {

            await a12.Say();
            await a12.SayAfter();
            await a12.SayArround();
        }
    }
}
