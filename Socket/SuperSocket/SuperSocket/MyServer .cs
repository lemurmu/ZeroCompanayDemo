using SuperSocket.Common;
using SuperSocket.Config;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket
{
    /// 自定义服务器类MyServer，继承AppServer，并传入自定义连接类MySession
    /// </summary>
    public class MyServer : AppServer<MySession>
    {
        private string m_PolicyFile;
        //private SubProtocols m_SubProtocols;

        protected override void OnStarted()
        {
            base.OnStarted();
            Console.WriteLine("服务器已启动");

            //注意!!!!!!!!!!!!!使用自定义命令一定要取消绑定这个数据接收事件！！！！！！！！
            //base.NewRequestReceived += NewDataArrived;
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="rootConfig"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            ///获取自定义节点
            SubProtocolsConfig setting = (SubProtocolsConfig)ConfigurationManager.GetSection("setting");
            SubProtocolCollection SubProtocols = setting.SubProtocols;
            Console.WriteLine(SubProtocols[2].Name);
            Console.WriteLine(SubProtocols["testservice"].Name);
            foreach (SubProtocol sp in SubProtocols)
            {
                string  name = sp.Name;
                string url = sp.Uri;
                string contract = sp.Contract;
                string service = sp.Service;
                Console.WriteLine(name+url+contract+service);
            }


            m_PolicyFile = config.Options.GetValue("policyFile");

            if (string.IsNullOrEmpty(m_PolicyFile))
            {
                if (Logger.IsErrorEnabled)
                    Logger.Error("Configuration option policyFile is required!");
                return false;
            }

            //m_SubProtocols = config.GetChildConfig<SubProtocols>("subProtocols");

            //if (m_SubProtocols == null)
            //{
            //    if (Logger.IsErrorEnabled)
            //        Logger.Error("The child configuration node 'subProtocols' is required!");
            //    return false;
            //}


            return true;
        }

        /// <summary>
        /// 输出新连接信息
        /// </summary>
        /// <param name="session"></param>
        protected override void OnNewSessionConnected(MySession session)
        {
            base.OnNewSessionConnected(session);
            //输出客户端IP地址
            Console.WriteLine(session.LocalEndPoint.Address.ToString() + ":" + session.LocalEndPoint.Port + "连接");
        }

        /// <summary>
        /// 输出断开连接信息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="reason"></param>
        protected override void OnSessionClosed(MySession session, CloseReason reason)
        {
            base.OnSessionClosed(session, reason);
            //输出客户端IP地址
            Console.WriteLine(session.LocalEndPoint.Address.ToString() + ":" + session.LocalEndPoint.Port + "断开连接");
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            Console.WriteLine("服务器已停止");
        }

        /// <summary>
        /// 数据接收处理
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        private void NewDataArrived(MySession session, StringRequestInfo requestInfo)
        {
            string key = requestInfo.Key;
            string body = requestInfo.Body;
            string parameters = "";
            foreach (var p in requestInfo.Parameters)
            {
                parameters += p + " ";
            }
            Console.WriteLine("Key:" + key + ";Parameters:" + parameters + ";Body:" + body);
            switch (requestInfo.Key.ToUpper())
            {
                case ("ECHO"):
                    session.Send(requestInfo.Body);
                    break;

                case ("ADD"):
                    session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
                    break;

                case ("MULT"):

                    var result = 1;

                    foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
                    {
                        result *= factor;
                    }

                    session.Send(result.ToString());
                    break;
            }
        }

    }
}

