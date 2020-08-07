using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket
{
        /// <summary>
        /// 用来发送和接收客户端信息,一个客户端相当于一个session(会话)。
        /// 自定义连接类MySession，继承AppSession，并传入到AppSession
        /// </summary>
        public class MySession : AppSession<MySession>
        {
            /// <summary>
            /// 新连接
            /// </summary>
            //protected override void OnSessionStarted()
            //{
            //    this.Send("\r\nHello User");
            //}

       
            /// <summary>
            /// 未知的请求
            /// </summary>
            /// <param name="requestInfo"></param>
            protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
            {
                //this.Send("\r\nUnknown Request");
            }

            /// <summary>
            /// 捕捉异常并输出
            /// </summary>
            /// <param name="e"></param>
            protected override void HandleException(Exception e)
            {
                //this.Send("\r\nException: {0}", e.Message);
            }

            /// <summary>
            /// 连接关闭
            /// </summary>
            /// <param name="reason"></param>
            protected override void OnSessionClosed(CloseReason reason)
            {
                base.OnSessionClosed(reason);
            }
        }
    
}
