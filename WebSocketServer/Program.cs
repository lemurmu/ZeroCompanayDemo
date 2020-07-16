using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            FleckLog.Level = LogLevel.Debug;
            var allSockets = new List<IWebSocketConnection>();

            var server = new WebSocketServer("ws://0.0.0.0:8080");

            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    allSockets.Add(socket);
                };

                socket.OnClose = () => // 当关闭Socket链接十执行此方法
                {
                    Console.WriteLine("Close!");
                    allSockets.Remove(socket);
                };

                socket.OnMessage = message => // 接收客户端发送过来的信息
                {
                    Console.WriteLine(message);
                    allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                };

            });

            var input = Console.ReadLine();
            while (input != "exit")
            {
                foreach (var socket in allSockets.ToList())
                {
                    socket.Send(input);
                }
                input = Console.ReadLine();
            }
        }
    }
}
