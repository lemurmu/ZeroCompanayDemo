using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelProgramTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //管道式编程模式:定义功能接口->实现功能函数->组装功能函数
            //test
            double input = 1200.56445;
            string result = input.Step(new DoubleToIntStep())
                            .Step(new IntTostringStep());
            Console.WriteLine(result);

            //DI     
            var services = new ServiceCollection();
            services.AddTransient<Travel>();
            var provider = services.BuildServiceProvider();

            var travel = provider.GetService<Travel>();
            result = travel.Process(input);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
