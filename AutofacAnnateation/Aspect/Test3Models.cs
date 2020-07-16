using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Autofac.Annotation;
using Autofac.Aspect;
using Castle.DynamicProxy;

namespace Autofac.Configuration.Test.test3
{
    public class TestHelloBefor : AspectBeforeAttribute
    {
        public override Task Before(AspectContext aspectContext)
        {
           // var aa1 = aspectContext.ComponentContext.Resolve<TestModel81>();
            Console.WriteLine("TestHelloBefor");
            return Task.CompletedTask;
        }
    }

    public class TestHelloAfter : AspectAfterAttribute
    {

        public override Task After(AspectContext aspectContext)
        {
           // if(aspectContext.Exception!=null) Console.WriteLine(aspectContext.Exception.Message);
            Console.WriteLine("TestHelloAfter");
            return Task.CompletedTask;
        }
    }


    public class TestHelloArround : AspectAroundAttribute
    {
        public override Task After(AspectContext aspectContext)
        {
            if (aspectContext.Exception != null) Console.WriteLine(aspectContext.Exception.Message);
            Console.WriteLine("TestHelloArround");
            return Task.CompletedTask;
        }

        public override Task Before(AspectContext aspectContext)
        {
            Console.WriteLine("TestHelloArround.Before");
            return Task.CompletedTask;
        }
    }


    [Component]
    [Aspect]
    public class TestModel91
    {

        [TestHelloBefor]
        public virtual async Task Say()
        {
            Console.WriteLine("say");
            await Task.Delay(1000);
        }

        [TestHelloAfter]
        public virtual async Task<string> SayAfter()
        {
            Console.WriteLine("SayAfter");
            await Task.Delay(1000);
            return "SayAfter";
        }

        [TestHelloArround]
        public virtual async Task<string> SayArround()
        {
            Console.WriteLine("SayArround");
            await Task.Delay(1000);
            return "SayArround";
        }
    }

    
}
