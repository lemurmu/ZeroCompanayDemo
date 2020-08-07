using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.ReceiveFilter
{
    //----------   与命令行协议类似，一些协议用结束符来确定一个请求.----------------
    //-------------例如, 一个协议使用两个字符 "##" 作为结束符, 于是你可以使用类 "TerminatorReceiveFilterFactory":
    /// <summary>
    /// TerminatorProtocolServer
    /// Each request end with the terminator "##"
    /// ECHO Your message##
    /// </summary>
    public class TerminatorProtocolServer : AppServer
    {
        public TerminatorProtocolServer()
            : base(new TerminatorReceiveFilterFactory("##"))//默认的请求类型是 StringRequestInfo
        {

        }

    }

    /// <summary>
    /// Your protocol likes like the format below:
    /// #part1#part2#part3#part4#part5#part6#part7#
    /// </summary>
    public class CountSpliterAppServer : AppServer
    {
        public CountSpliterAppServer()
            : base(new CountSpliterReceiveFilterFactory((byte)'#', 8)) // 7 parts but 8 separators
        {

        }
    }



    #region DefaultReceiveFilterFactory默认接收过滤工厂
    public class MyAppServer : AppServer
    {
        public MyAppServer()
            : base(new DefaultReceiveFilterFactory<MyReceiveFilter, StringRequestInfo>()) //使用默认的接受过滤器工厂 (DefaultReceiveFilterFactory)
        {

        }
    }

    public class MyReceiveFilter : FixedSizeReceiveFilter<StringRequestInfo>
    {
        public MyReceiveFilter()
            : base(9) //传入固定的请求大小
        {

        }

        protected override StringRequestInfo ProcessMatchedRequest(byte[] buffer, int offset, int length, bool toBeCopied)
        {
            //TODO: 通过解析到的数据来构造请求实例，并返回

            throw new NotImplementedException();
        }
    }

    #endregion


    #region 自定义过滤器和接收过滤器工厂
    public class YourServerApp : AppServer
    {
        public YourServerApp() : base(new YourReceiveFilterFactory("@@", "#####", "%%%%%", "&&&&&"))
        {

        }
    }

    /// <summary>
    /// 构建自己的请求类型
    /// </summary>
    public class YourReceiveFilter : TerminatorReceiveFilter<StringRequestInfo>
    {
        public YourReceiveFilter(byte[] terminator) : base(terminator)//terminator是结束符
        {


        }

        protected override StringRequestInfo ProcessMatchedRequest(byte[] data, int offset, int length)
        {
            string key = "SYSINFO";
            byte[] body = data.CloneRange(offset, length);
            string[] paras = { "A", "B" };
            return new StringRequestInfo(key, Encoding.Default.GetString(body), paras);

        }

        //More code
    }


    /// <summary>
    /// 构建自己的接收过滤器工厂
    /// </summary>

    public class YourReceiveFilterFactory : IReceiveFilterFactory<StringRequestInfo>
    {

        private string[] terminatorStr = null;
        public YourReceiveFilterFactory(params string[] terminators)
        {
            terminatorStr = terminators;
        }
        //More code
        public IReceiveFilter<StringRequestInfo> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            if (terminatorStr == null)
            {
                throw new NullReferenceException();
            }
            byte[] terminators = new byte[50];//给50个字节
            int index = 0;
            foreach (var item in terminatorStr)
            {
                byte[] tes = Encoding.Default.GetBytes(item);
                Array.Copy(tes, 0, terminators, index, tes.Length);
                index += tes.Length;
            }
            byte[] termi = new byte[index];
            Array.Copy(terminators, 0, termi, 0, index);
            return new YourReceiveFilter(termi);

        }
    }

    #endregion

    /// <summary>
    /// 定义开始结束标记的过滤器  如:!xxxxxxxxxxxxxx$
    /// </summary>
    public class MyReceiveFilter1 : BeginEndMarkReceiveFilter<StringRequestInfo>
    {
        //开始和结束标记也可以是两个或两个以上的字节
        private readonly static byte[] BeginMark = new byte[] { (byte)'!' };
        private readonly static byte[] EndMark = new byte[] { (byte)'$' };

        public MyReceiveFilter1()
            : base(BeginMark, EndMark) //传入开始标记和结束标记
        {

        }

        protected override StringRequestInfo ProcessMatchedRequest(byte[] readBuffer, int offset, int length)
        {
            //TODO: 通过解析到的数据来构造请求实例，并返回
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// FixedHeaderReceiveFilter - 头部格式固定并且包含内容长度的协议
    /// 这种协议将一个请求定义为两大部分, 第一部分定义了包含第二部分长度等等基础信息. 我们通常称第一部分为头部.
    /// 例如, 我们有一个这样的协议: 头部包含 6 个字节, 前 4 个字节用于存储请求的名字, 后两个字节用于代表请求体的长度:
    /// +-------+---+-------------------------------+
    /// |request| l |                               |
    /// | name  | e |    request body               |
    /// |  (4)  | n |                               |
    /// |       |(2)|                               |
    /// +-------+---+-------------------------------+
    /// </summary>
    class MyReceiveFilter2 : FixedHeaderReceiveFilter<BinaryRequestInfo>
    {
        public MyReceiveFilter2()
            : base(6)
        {

        }
        /// <summary>
        /// 应该根据接收到的头部返回请求体的长度
        /// </summary>
        /// <param name="header"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return (int)header[offset + 4] * 256 + (int)header[offset + 5];
        }

        /// <summary>
        /// 应该根据你接收到的请求头部和请求体返回你的请求类型的实例.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="bodyBuffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            return new BinaryRequestInfo(Encoding.UTF8.GetString(header.Array, header.Offset, 4), bodyBuffer.CloneRange(offset, length));
        }
    }
}
