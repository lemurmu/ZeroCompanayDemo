using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperSocketClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            SSClient client = new SSClient();
            client.Connect("127.0.0.1", 2000);

            int sleeptime = 100;
            while (!client.IsConnect)
            {
                Thread.Sleep(sleeptime);
                sleeptime += 100;
                if (sleeptime >= 3000)
                {
                    Console.WriteLine("连接超时!");
                    break;
                }
            }

            if (client.IsConnect)
            {
                //client.SendCommand("app", "Sys:info:all\r\n");//一定要加换行符!!!!!!不然服务器收不到数据!!!!!!!
                client.SendCommand("ECHO", "ABCDEF");
                Console.WriteLine("请按'q'断开连接!");
            }


            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }
            client.DisConnect();
        }
    }
}
