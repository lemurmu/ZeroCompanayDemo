using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelProgramTest
{

    /// <summary>
    /// 定义功能接口
    /// </summary>
    /// <typeparam name="INPUT"></typeparam>
    /// <typeparam name="OUTPUT"></typeparam>
    public interface IPipelineStep<INPUT, OUTPUT>
    {
        OUTPUT Process(INPUT input);
    }


    public class DoubleToIntStep : IPipelineStep<double, int>
    {
        public int Process(double input)
        {
            return Convert.ToInt32(input);
        }
    }

    public class IntTostringStep : IPipelineStep<int, string>
    {
        public string Process(int input)
        {
            return Convert.ToString(input);
        }
    }

    /// <summary>
    /// 定义扩展
    /// </summary>
    public static class IPipelineStepExtension
    {
        public static OUTPUT Step<INPUT, OUTPUT>(this INPUT input, IPipelineStep<INPUT, OUTPUT> step)
        {
            return step.Process(input);
        }
    }

    /// <summary>
    /// DI 中使用管道式编程
    /// </summary>
    /// <typeparam name="INPUT"></typeparam>
    /// <typeparam name="OUTPUT"></typeparam>
    public abstract class PipelineStep<INPUT, OUTPUT>
    {
        public Func<INPUT, OUTPUT> Func { get; protected set; }//放到具体实现的子类中赋值

        public OUTPUT Process(INPUT input)
        {
            return Func(input);//调用
        }
    }

    public class Travel : PipelineStep<double, string>
    {
        public Travel()
        {
            //构造函数注入
            Func = input => input.Step(new DoubleToIntStep())
                          .Step(new IntTostringStep());
        }
    }

}
