using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            RankDes();
        }

        //        小结TCP与UDP的区别：

        //1.基于连接与无连接；
        //2.对系统资源的要求（TCP较多，UDP少）；
        //3.UDP程序结构较简单；
        //4.流模式与数据报模式 ；

        //5.TCP保证数据正确性，UDP可能丢包，TCP保证数据顺序，UDP不保证。



        //tcp协议和udp协议的差别
        //           TCP           UDP
        //是否连接     面向连接 面向非连接
        //传输可靠性 可靠        不可靠
        //应用场合    少量数据 传输大量数据

        //速度 慢         快
        //        TCP与UDP区别总结：

        //1、TCP面向连接（如打电话要先拨号建立连接）;UDP是无连接的，即发送数据之前不需要建立连接

        //2、TCP提供可靠的服务。也就是说，通过TCP连接传送的数据，无差错，不丢失，不重复，且按序到达;UDP尽最大努力交付，即不保证可靠交付

        //3、TCP面向字节流，实际上是TCP把数据看成一连串无结构的字节流;UDP是面向报文的

        //UDP没有拥塞控制，因此网络出现拥塞不会使源主机的发送速率降低（对实时应用很有用，如IP电话，实时视频会议等）

        //4、每一条TCP连接只能是点到点的;UDP支持一对一，一对多，多对一和多对多的交互通信

        //5、TCP首部开销20字节;UDP的首部开销小，只有8个字节
        //6、TCP的逻辑通信信道是全双工的可靠信道，UDP则是不可靠信道


        //        TCP与UDP区别总结：
        //1、TCP面向连接（如打电话要先拨号建立连接）;UDP是无连接的，即发送数据之前不需要建立连接
        //2、TCP提供可靠的服务。也就是说，通过TCP连接传送的数据，无差错，不丢失，不重复，且按序到达;UDP尽最大努力交付，即不保 证可靠交付
        //3、TCP面向字节流，实际上是TCP把数据看成一连串无结构的字节流;UDP是面向报文的
        //  UDP没有拥塞控制，因此网络出现拥塞不会使源主机的发送速率降低（对实时应用很有用，如IP电话，实时视频会议等）
        //4、每一条TCP连接只能是点到点的;UDP支持一对一，一对多，多对一和多对多的交互通信
        //5、TCP首部开销20字节;UDP的首部开销小，只有8个字节
        //6、TCP的逻辑通信信道是全双工的可靠信道，UDP则是不可靠信道



        /// <summary>
        /// 冒泡排序
        /// </summary>
        static void RankDes()
        {
            int[] nums = { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };
            for (int i = 0; i < nums.Length - 1; i++)
            {
                for (int j = 0; j < nums.Length - 1 - i; j++)
                {
                    if (nums[j] > nums[j + 1])
                    {
                        int temp = nums[j];
                        nums[j] = nums[j + 1];
                        nums[j + 1] = temp;
                    }
                }
            }
            //打印数组
            for (int i = 0; i < nums.Length; i++)
            {
                Console.WriteLine(nums[i]);

            }
            Console.ReadKey();
        }
    }



}
