using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            var bootstrap = BootstrapFactory.CreateBootstrap();

            if (!bootstrap.Initialize()) //启动SuperSocket
            {
                Console.WriteLine("初始化失败!");
                Console.ReadKey();
                return;
            }
            var result = bootstrap.Start();

            Console.WriteLine("服务正在启动: {0}!", result);

            if (result == StartResult.Failed)
            {
                Console.WriteLine("服务启动失败!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("服务启动成功，请按'q'停止服务!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //停止服务
            bootstrap.Stop();
            Console.WriteLine("服务已停止!");
            Console.ReadKey();


        }
    }
}
