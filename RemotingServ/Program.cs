using ITestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using TestModel;

namespace RemotingServ
{
    class Program
    {
        static void Main(string[] args)
        {
            IChannel tcpServerChannel = new TcpServerChannel(4008);

            ChannelServices.RegisterChannel(tcpServerChannel,false);

            Console.WriteLine("注册对象testModel......");
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(Model), "testModel", WellKnownObjectMode.Singleton);

            Console.WriteLine("注册对象ModelImple......");
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ModelImple), "ModelImple", WellKnownObjectMode.Singleton);

            Console.WriteLine($">>>>>>启动服务【testModel】,服务来源【{nameof(Model)}】\n<<----------------------->>");

            Console.WriteLine($">>>>>>启动服务【ModelImple】,服务来源【{nameof(ModelImple)}】\n<<----------------------->>");
            Console.ReadKey();

            

        }
    }
}
