using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket
{
    /// <summary>
    /// 不用自己启动 在同一程序集中SuperSocket会通过反射获取
    /// 如果要主动启动 可以在配置文件中配置 并且可以配置 在其他程序集中的command
    /// </summary>
    public class HELLO : CommandBase<MySession, StringRequestInfo>
    {
        /// <summary>
        /// 自定义执行命令方法，注意传入的变量session类型为MySession
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        public override void ExecuteCommand(MySession session, StringRequestInfo requestInfo)
        {
            session.Send("hello fuck you !man ");
        }
    }


    public class READY : CommandBase<MySession, StringRequestInfo>
    {
        public override void ExecuteCommand(MySession session, StringRequestInfo requestInfo)
        {
            session.Send("READY go");
        }
    }
}
