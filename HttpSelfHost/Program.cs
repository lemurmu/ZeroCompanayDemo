using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpSelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServer();
            String inputStr = null;
            while ((inputStr = Console.ReadLine()).ToLower() != "exit")
            {

            }
        }
        static public void StartServer()
        {
            WebApi.WebServer.MessageEvent += WebServer_MessageEvent;
            WebApi.WebServer.Start();
        }

        private static void WebServer_MessageEvent(string msg)
        {
            Console.WriteLine(msg);
        }

    }
}
