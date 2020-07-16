using Autofac.Configuration.Test.test3;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MainCos
{
    class Program
    {
        static object syncObj = new object();
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            int len = args.Length;
            Console.WriteLine($"Main函数的参数长度是{len}");
            for (int i = 0; i < len; i++)
            {
                Console.WriteLine($"Main函数参数的第{i + 1}位是{args[i]}");
            }

            //List和HashSet
            var hashset = new HashSet<string>();//不重复 无序 高性能运算 ：交集 并集 检索时间复杂度O(1) contains()
            hashset.Add("adda");

            //Dictionary和Hashtable  
            var hashTable = new Hashtable();//支持多线程中单线程写入  
            hashTable.Add("key", "value");//object 会发生拆箱装箱
            Hashtable safehashTable = Hashtable.Synchronized(hashTable); //调用 Synchronized() 方法可以获得完全线程安全的类型

            Dictionary<string, string> keyValues = new Dictionary<string, string>();//泛型 不用拆箱装箱
            keyValues.Add("key", "value");
            lock (syncObj)//这样来获取同步 syncObj没有被lock 就会lock 这时其他线程不能访问syncObj和字典 lock执行完后syncObj会被释放 其他线程才能访问字典
            {
                keyValues.Remove("key");
            }


            Create3dArray(1, 2, 4);
            // Create3dArray(d1: 1, d2: 2, d3: 4);

            var stopwatch1 = Stopwatch.StartNew();
            double[][][] arr = Create3dArray<double>(2, 12, 64);//左/右旋  频点 芯片通道
            double[][] arr1 = arr[0];//获取左旋数据
                                     //TestModel91[][][] testModel91s = new TestModel91[5][][];

            // Array array = CreateNestedArray<Double>(1, 2, 6, 8);
            stopwatch1.Stop();
            Console.WriteLine(stopwatch1.ElapsedMilliseconds);

            Random rd = new Random();

            double[,,] testArr = new double[2, 12, 64];
            for (int i = 0; i < testArr.GetLength(0); i++)
            {
                for (int j = 0; j < testArr.GetLength(1); j++)
                {
                    for (int k = 0; k < testArr.GetLength(2); k++)
                    {
                        testArr[i, j, k] = rd.Next(0, 100);
                    }
                }
            }

            var stopwatch = Stopwatch.StartNew();
            double[][][] resultArr = ConvertToNestedArray<double[][][]>(testArr);
            //   Console.WriteLine(5 / 2);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
           // Console.WriteLine(typeof(double[][][]).BaseType);

            Test4DArray();

            Test();

            //Type t = Type.GetType("System.Int32[]");
            //foreach (System.Reflection.MethodInfo mi in t.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            //{
            //    Console.WriteLine(mi.Name);
            //}

            //GetSet();

            Console.ReadKey();
        }

        public static void GetSet()
        {
            Type t = Type.GetType("System.Int32[]");
            object array = new object();
            array = t.InvokeMember("Set", BindingFlags.CreateInstance, null, array, new object[] { 10 });
            for (int i = 0; i < 10; i++)
                t.GetMethod("SetValue", new Type[2] { typeof(object), typeof(int) }).Invoke(array, new object[] { i, i });
            for (int i = 0; i < 10; i++)
                Console.WriteLine(t.GetMethod("GetValue", new Type[] { typeof(int) }).Invoke(array, new object[] { i }));
        }

        public static void Test()
        {
            Type t = Type.GetType("System.Double[][]");
            object array = new object();
            array = t.InvokeMember("Set", BindingFlags.CreateInstance, null, array, new object[] { 10 });
            object[] objs = new object[10];
            for (int i = 0; i < 10; i++)
            {
                objs[i]= new double[10];
            }
            for (int i = 0; i < 10; i++)
            {
                object value = t.GetMethod("GetValue", new Type[] { typeof(int) }).Invoke(array, new object[] { i });
                value = new double[10];
                value = t.GetMethod("GetValue", new Type[] { typeof(int) }).Invoke(array, new object[] { i });
                Console.WriteLine(value);
            }

            //array = t.InvokeMember("SetValue", BindingFlags.GetField, null, array, objs);
            //  t.GetMethod("SetValue", new Type[] { typeof(object), typeof(object[]) }).Invoke(array, new object[] { array, objs });
            //for (int i = 0; i < 10; i++)
            //    t.GetMethod("SetValue", new Type[2] { typeof(object), typeof(double[]) }).Invoke(array, new object[] { i, new double[10] });
        }


        public static void Test4DArray()
        {
            Random rd = new Random();
            var stopwatch = Stopwatch.StartNew();
            double[,,,] testArr = new double[2, 64, 64,1024];
            for (int i = 0; i < testArr.GetLength(0); i++)
            {
                for (int j = 0; j < testArr.GetLength(1); j++)
                {
                    for (int k = 0; k < testArr.GetLength(2); k++)
                    {
                        for (int l = 0; l < testArr.GetLength(3); l++)
                        {
                            testArr[i, j, k,l] = rd.Next(0, 100);
                        }
                     
                    }
                }
            }

            double[][][][] resultArr = ConvertToNestedArray<double[][][][]>(testArr);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
        public static double[][][] Create3dArray(int d1, int d2, int d3)
        {
            if (d1 <= 0 || d2 <= 0 | d3 <= 0)
            {
                throw new ArgumentException("非法参数！");
            }

            double[][][] arr = new double[d1][][];
            for (int i = 0; i < d1; i++)
            {
                arr[i] = new double[d2][];
                for (int j = 0; j < d2; j++)
                {
                    arr[i][j] = new double[d3];
                }
            }
            return arr;
        }

        public static T[][][] Create3dArray<T>(int d1, int d2, int d3)
        {
            if (d1 <= 0 || d2 <= 0 | d3 <= 0)
            {
                throw new ArgumentException("非法参数!");
            }

            T[][][] arr = new T[d1][][];
            for (int i = 0; i < d1; i++)
            {
                arr[i] = new T[d2][];
                for (int j = 0; j < d2; j++)
                {
                    arr[i][j] = new T[d3];
                }
            }
            return arr;
        }


        public static Array CreateNestedArray<T>(params int[] lengths)
        {
            if (lengths == null)
            {
                throw new ArgumentNullException();
            }
            if (lengths.Length == 0)
            {
                throw new ArgumentException("参数数目不正确！");
            }

            StringBuilder sb = new StringBuilder();
            Array arr = null;
            for (int r = lengths.Length - 1; r >= 0; r--)
            {
                sb.Append("[]");
                string arryType = typeof(T).ToString() + sb.ToString();
                object array = new object();
                Type t = Type.GetType(arryType);
                array = t.InvokeMember("Set", BindingFlags.CreateInstance, null, array, new object[] { lengths[r] });

                if (r < lengths.Length - 1)
                {
                    for (int i = 0; i < lengths[r]; i++)
                    {
                        t.GetMethod("SetValue", new Type[] { typeof(Array), typeof(Array) }).Invoke((Array)array, new object[] { i, arr });
                    }

                }
                arr = (Array)array;
            }
            return arr;
        }


        /// <summary>
        /// Json 多维数组转嵌套数组
        /// </summary>
        /// <typeparam name="T">嵌套数组类型</typeparam>
        /// <param name="dest">多维数组</param>
        /// <returns></returns>
        public static T ConvertToNestedArray<T>(object dest)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dest));
        }
    }
}
