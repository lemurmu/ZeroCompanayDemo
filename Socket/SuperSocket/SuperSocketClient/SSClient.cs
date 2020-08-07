using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketClient
{
   public class SSClient
    {
        private AsyncTcpSession client;

        bool isConnect = false;
        public bool IsConnect { get => isConnect; }

        public SSClient()
        {
            client = new AsyncTcpSession();
            // 连接断开事件
            client.Closed += client_Closed;
            // 收到服务器数据事件
            client.DataReceived += client_DataReceived;
            // 连接到服务器事件
            client.Connected += client_Connected;
            // 发生错误的处理
            client.Error += client_Error;

        }
        void client_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }

        void client_Connected(object sender, EventArgs e)
        {
            Console.WriteLine("连接成功");
            isConnect = client.IsConnected;
        }

        void client_DataReceived(object sender, DataEventArgs e)
        {
            string msg = Encoding.Default.GetString(e.Data);
            Console.WriteLine(msg);
        }

        void client_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("连接断开");
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        public void Connect(string ip, int port)
        {
            client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
        }

        public void DisConnect()
        {
            client.Close();
        }

        /// <summary>
        /// 向服务器发命令行协议的数据
        /// </summary>
        /// <param name="key">命令名称</param>
        /// <param name="data">数据</param>
        public void SendCommand(string key, string data)
        {
            if (client.IsConnected)
            {
                byte[] arr = Encoding.Default.GetBytes(string.Format("{0} {1}", key, data));
                client.Send(arr, 0, arr.Length);
                Console.WriteLine(key+data);
            }
            else
            {
                throw new InvalidOperationException("未建立连接");
            }
        }
    }
}
