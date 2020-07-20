using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceProcess;
using System.Text;
using System.Xml;

namespace LD.Platform.Function
{
    public class Helper
    {
        #region API_View

        public static int WM_USER = 0x0400;

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg,
            IntPtr wParam, IntPtr lParam);

        [DllImport("user32.DLL", EntryPoint = "PostMessage")]
        public static extern bool PostMessage(IntPtr hwnd, uint msg,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        protected static extern int WritePrivateProfileString(
            string lpApplicationName,
            string lpKeyName,
            string lpString,
            string lpFileName
            );

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        protected static extern int GetPrivateProfileString(
            string lpApplicationName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            int nSize,
            string lpFileName
            );

        [DllImport("kernel32.dll", EntryPoint = "AllocConsole")]
        protected static extern bool AllocConsole();

        [DllImport("kernel32.dll", EntryPoint = "FreeConsole")]
        protected static extern bool FreeConsole();

        [DllImport("kernel32.dll", EntryPoint = "FreeConsole")]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "FreeConsole")]
        public static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        protected static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceCounter")]
        protected static extern bool QueryPerformanceCounter(IntPtr _Count);

        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceFrequency")]
        protected static extern bool QueryPerformanceFrequency(IntPtr _Freq);

        [DllImport("kernel32.dll", EntryPoint = "SetLocalTime")]
        protected static extern int SetLocalTime(ref SystemTime _SystemTime);

        [DllImport("gdi32.dll   ")]
        public static extern bool BitBlt(
        IntPtr hdcDest,   //   handle   to   destination   DC    
        int nXDest,   //   x-coord   of   destination   upper-left   corner    
        int nYDest,   //   y-coord   of   destination   upper-left   corner    
        int nWidth,   //   width   of   destination   rectangle    
        int nHeight,   //   height   of   destination   rectangle    
        IntPtr hdcSrc,   //   handle   to   source   DC    
        int nXSrc,   //   x-coordinate   of   source   upper-left   corner    
        int nYSrc,   //   y-coordinate   of   source   upper-left   corner    
        uint dwRop   //   raster   operation   code    
        );
        /* Ternary raster operations */
        const uint SRCCOPY = 0x00CC0020; /* dest = source */
        const uint SRCPAINT = 0x00EE0086; /* dest = source OR dest */
        const uint SRCAND = 0x008800C6; /* dest = source AND dest */
        const uint SRCINVERT = 0x00660046; /* dest = source XOR dest */
        const uint SRCERASE = 0x00440328; /* dest = source AND (NOT dest ) */
        const uint NOTSRCCOPY = 0x00330008; /* dest = (NOT source) */
        const uint NOTSRCERASE = 0x001100A6; /* dest = (NOT src) AND (NOT dest) */
        const uint MERGECOPY = 0x00C000CA; /* dest = (source AND pattern) */
        const uint MERGEPAINT = 0x00BB0226; /* dest = (NOT source) OR dest */
        const uint PATCOPY = 0x00F00021; /* dest = pattern */
        const uint PATPAINT = 0x00FB0A09; /* dest = DPSnoo */
        const uint PATINVERT = 0x005A0049; /* dest = pattern XOR dest */
        const uint DSTINVERT = 0x00550009; /* dest = (NOT dest) */
        const uint BLACKNESS = 0x00000042; /* dest = BLACK */
        const uint WHITENESS = 0x00FF0062; /* dest = WHITE */

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("gdi32.dll")]
        public static extern int SetROP2(int h, int op);
        [DllImport("gdi32.dll")]
        static public extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        static public extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        [DllImport("gdi32.dll")]
        static public extern IntPtr DeleteDC(IntPtr hDC);

        internal enum ShowWindowEnum { Hide = 0, ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3, Maximize = 3, ShowNormalNoActivate = 4, Show = 5, Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8, Restore = 9, ShowDefault = 10, ForceMinimized = 11 };

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        internal static extern int SetWindowPos(
        IntPtr hwnd,
        int hWndInsertAfter,
        int x,
        int y,
        int cx,
        int cy,
        int wFlags
        );

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion

        /// <summary>
        /// Windows系统时间结构定义
        /// </summary>
        [Serializable()]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct SystemTime
        {
            [MarshalAs(UnmanagedType.U2)]
            public ushort wYear;
            [MarshalAs(UnmanagedType.U2)]
            public ushort wMonth;
            [MarshalAs(UnmanagedType.U2)]
            public ushort wDayOfWeek;
            [MarshalAs(UnmanagedType.U2)]
            public ushort wDay;
            [MarshalAs(UnmanagedType.U2)]
            public ushort wHour;
            [MarshalAs(UnmanagedType.U2)]
            public ushort wMinute;
            [MarshalAs(UnmanagedType.U2)]
            public ushort wSecond;
            [MarshalAs(UnmanagedType.U2)]
            public ushort wMilliseconds;

            public SystemTime(DateTime _Time)
            {
                wYear = (ushort)_Time.Year;
                wMonth = (ushort)_Time.Month;
                wDayOfWeek = (ushort)_Time.DayOfWeek;
                wDay = (ushort)_Time.Day;
                wHour = (ushort)_Time.Hour;
                wMinute = (ushort)_Time.Minute;
                wSecond = (ushort)_Time.Second;
                wMilliseconds = (ushort)_Time.Millisecond;
            }
        }

        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Filename">文件名称</param>
        /// <param name="Section">配置文件主节点</param>
        /// <param name="Key">配置文件子节点</param>
        /// <param name="Value">值</param>
        public static void WriteToIni(string Filename, string Section, string Key, string Value)
        {
            string buff;
            try
            {
                buff = Value + "\0";
                WritePrivateProfileString(Section, Key, buff, Filename);
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="Filename">文件名称</param>
        /// <param name="Section">配置文件主节点</param>
        /// <param name="Key">配置文件子节点</param>
        /// <returns>值</returns>
        public static string ReadFromIni(string Filename, string Section, string Key)
        {
            try
            {
                StringBuilder buff = new StringBuilder(128);
                GetPrivateProfileString(Section, Key, "", buff, 128, Filename);
                return buff.ToString();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 得到所给的类型
        /// </summary>
        /// <param name="assembly">程序集路径</param>
        /// <param name="type">要比较的接口类型,在out_Type中返回的类型必须是从该接口继承而来的类型</param>
        /// <returns>返回值，null为错误，否则成功</returns>
        public static Type GetTypeFromAssembly(string assembly, Type type)
        {
            try
            {
                if (!File.Exists(assembly))
                    return null;
                Assembly _Assembly = Assembly.LoadFrom(assembly);
                Type[] _Types = _Assembly.GetTypes();
                foreach (Type _Type in _Types)
                {
                    Type[] _Types_ = _Type.GetInterfaces();
                    foreach (Type _Type_ in _Types_)
                    {
                        if (_Type_.AssemblyQualifiedName.Equals(type.AssemblyQualifiedName))
                        {
                            return _Type;
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public static IEnumerable<Type> GetTypesFromAssembly(string assembly, Type type)
        {
            if (!File.Exists(assembly))
                yield break;
            Assembly _Assembly = Assembly.LoadFrom(assembly);
            Type[] _Types = _Assembly.GetTypes();
            foreach (Type _Type in _Types)
            {
                Type[] _Types_ = _Type.GetInterfaces();
                foreach (Type _Type_ in _Types_)
                {
                    if (_Type_.AssemblyQualifiedName.Equals(type.AssemblyQualifiedName))
                    {
                        yield return _Type;
                    }
                }
            }
        }

        /// <summary>
        /// 获取程序集中的实例
        /// </summary>
        /// <param name="assembly">程序集路径</param>
        /// <param name="type">要获取实例的类型</param>
        /// <returns>返回值，null为错误，否则成功</returns>
        public static object GetInstanceFromAssembly(string assembly, Type type)
        {
            Type objType = GetTypeFromAssembly(assembly, type);
            if (objType == null)
                return null;
            try
            {
                return Activator.CreateInstance(objType);
            }
            catch//(Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// IP地址合法性校验
        /// </summary>
        /// <param name="In_IpString">IP地址</param>
        /// <returns>返回0,合法;其它不合法</returns>
        public static bool CheckIp(string In_IpString)
        {
            string[] IpArr = null;
            if (In_IpString == null)
                return false;
            if (In_IpString == "")
                return false;
            IpArr = In_IpString.Split(new char[1] { '.' });
            if (IpArr.Length != 4)
                return false;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < IpArr[i].Length; j++)
                {
                    if (!char.IsNumber(IpArr[i], j))
                    {
                        return false;
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (int.Parse(IpArr[i]) < 0 || int.Parse(IpArr[i]) > 254)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查是否是数字
        /// </summary>
        /// <param name="str">要检查的字符串</param>
        /// <returns>是数字返回为true,否则返回为false</returns>
        public static bool IsNumber(string str)
        {
            if (str == null)
                return false;
            str = str.Replace(" ", "");
            if (str.Length == 0)
                return false;
            string[] _strs = str.Split(new char[] { '.' });
            if (_strs.Length > 2)
                return false;
            ASCIIEncoding ascii = new ASCIIEncoding();

            for (int i = 0; i < _strs.Length; i++)
            {
                byte[] bytestr = ascii.GetBytes(_strs[i]);
                int j = 0;
                foreach (byte c in bytestr)
                {
                    if (c < 48 || c > 57)
                    {
                        if (i == 0 && j == 0 && c == 45)
                            continue;
                        else
                            return false;
                    }
                    j++;
                }
            }
            return true;
        }

        /// <summary>
        /// 将字符格式的值转换成对应基本类型，注意，只能转换成基本类型，对自定义的类型无效
        /// </summary>
        /// <param name="_Type">对应的基本类型</param>
        /// <param name="_Value">对应的字符串值</param>
        /// <returns>返回一个object值</returns>
        public static object GetValue(Type _Type, string _Value)
        {
            try
            {
                if (_Type == null || _Value == null)
                    return null;
                if (_Value.Replace(" ", "") == "")
                {
                    if (_Type == typeof(string))
                        return "";
                    else if (_Type == typeof(char))
                    {
                        return ' ';
                    }
                    else
                        return 0;
                }

                object value = null;
                if (_Type == typeof(string))
                {
                    value = _Value;
                }
                else if (_Type == typeof(bool))
                {
                    value = bool.Parse(_Value);
                }
                else if (_Type == typeof(byte))
                {
                    value = byte.Parse(_Value);
                }
                else if (_Type == typeof(char))
                {
                    value = char.Parse(_Value);
                }
                else if (_Type == typeof(decimal))
                {
                    value = decimal.Parse(_Value);
                }
                else if (_Type == typeof(float))
                {
                    value = float.Parse(_Value);
                }
                else if (_Type == typeof(double))
                {
                    value = double.Parse(_Value);
                }
                else if (_Type == typeof(int))
                {
                    value = int.Parse(_Value);
                }
                else if (_Type == typeof(long))
                {
                    value = long.Parse(_Value);
                }
                else if (_Type == typeof(sbyte))
                {
                    value = sbyte.Parse(_Value);
                }
                else if (_Type == typeof(short))
                {
                    value = short.Parse(_Value);
                }
                else if (_Type == typeof(uint))
                {
                    value = uint.Parse(_Value);
                }
                else if (_Type == typeof(ulong))
                {
                    value = ulong.Parse(_Value);
                }
                else if (_Type == typeof(ushort))
                {
                    value = ushort.Parse(_Value);
                }
                else
                {
                    throw new Exception("No Type");
                }
                return value;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// 将结构封送到非托管内存
        /// </summary>
        /// <param name="_Type">结构的类型</param>
        /// <param name="_Obj">结构的实例</param>
        /// <returns>非托管内存起始地址</returns>
        public static IntPtr StructToPtr(Type _Type, object _Obj)
        {
            int _len = Marshal.SizeOf(_Type);
            IntPtr _Ptr = Marshal.AllocHGlobal(_len);
            Marshal.StructureToPtr(_Obj, _Ptr, true);
            return _Ptr;
        }

        /// <summary>
        /// 将结构转换成字节数组
        /// </summary>
        /// <param name="structObj">一个结构</param>
        /// <returns>返回表征该结构的字节数组</returns>
        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }

        /// <summary>
        /// 将结构填充到字节数组
        /// </summary>
        /// <param name="data">要填充的字节数组</param>
        /// <param name="structObj">一个结构</param>
        /// <param name="offset">要填充的字节数组的偏移位置</param>
        public static void StructToBytes(ref byte[] data, object structObj, ref int offset)
        {
            int size = Marshal.SizeOf(structObj);
            // byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, data, offset, size);
            Marshal.FreeHGlobal(structPtr);
            offset += size;
        }

        public static string BytesToString(byte[] bytes)
        {
            if (bytes == null)
                return null;
            int i = 0;
            for (i = 0; i < bytes.Length && bytes[i] > 0; i++)
            {
            };
            if (i == 0)
                return "";
            return Encoding.Default.GetString(bytes, 0, i);
        }

        /// <summary>
        /// 将字节数组转换成结构
        /// </summary>
        /// <param name="bytes">表示一个结构的字节数组</param>
        /// <param name="type">结构的类型</param>
        /// <returns>返回该类型的结构</returns>
        public static object BytesToStuct(byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, structPtr, size);
            object obj = Marshal.PtrToStructure(structPtr, type);
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }

        public static T BytesToStuct<T>(byte[] bytes)
        {
            Type type = typeof(T);
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return default(T);
            }
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, structPtr, size);
            object obj = Marshal.PtrToStructure(structPtr, type);
            Marshal.FreeHGlobal(structPtr);
            return (T)obj;
        }

        /// <summary>
        /// 将字节数组转换成结构
        /// </summary>
        /// <param name="bytes">表示一个结构的字节数组</param>
        /// <param name="offset">该结构在字节数组中的偏移量</param>
        /// <param name="type">结构的类型</param>
        /// <returns>返回该类型的结构</returns>
        public static object BytesToStuct(byte[] bytes, int offset, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, offset, structPtr, size);
            object obj = Marshal.PtrToStructure(structPtr, type);
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }

        /// <summary>
        /// 将数组封送到非托管内存
        /// </summary>
        /// <param name="_Data">数组</param>
        /// <returns>非托管内存起始地址</returns>
        public static IntPtr ArrayToPtr(byte[] _Data)
        {
            if (_Data == null)
                return new IntPtr();
            IntPtr _adress = Marshal.AllocHGlobal(_Data.Length);
            Marshal.Copy(_Data, 0, _adress, _Data.Length);
            return _adress;
        }

        /// <summary>
        /// 清除非托管内存
        /// </summary>
        /// <param name="_adress">非托管内存起始地址</param>
        public static void FreeHGlobal(IntPtr _adress)
        {
            Marshal.FreeHGlobal(_adress);
        }

        //public static StationTypes ConverStationTypes(string type)
        //{
        //    StationTypes stationType = StationTypes.UNKNOWN;
        //    if (string.Compare(type, StationTypes.BOAT.ToString(), true) == 0)
        //        stationType = StationTypes.BOAT;
        //    else if (string.Compare(type, StationTypes.CAR.ToString(), true) == 0)
        //        stationType = StationTypes.CAR;
        //    else if (string.Compare(type, StationTypes.CENTER.ToString(), true) == 0)
        //        stationType = StationTypes.CENTER;
        //    else if (string.Compare(type, StationTypes.FIX.ToString(), true) == 0)
        //        stationType = StationTypes.FIX;
        //    else if (string.Compare(type, StationTypes.FLY.ToString(), true) == 0)
        //        stationType = StationTypes.FLY;
        //    else if (string.Compare(type, StationTypes.HAND.ToString(), true) == 0)
        //        stationType = StationTypes.HAND;
        //    else if (string.Compare(type, StationTypes.MINI.ToString(), true) == 0)
        //        stationType = StationTypes.MINI;
        //    else if (string.Compare(type, StationTypes.RF.ToString(), true) == 0)
        //        stationType = StationTypes.RF;
        //    else if (string.Compare(type, StationTypes.TRAGBAR.ToString(), true) == 0)
        //        stationType = StationTypes.TRAGBAR;
        //    return stationType;
        //}

        public static Type ConverToType(string type)
        {
            Type ret = typeof(string);
            if (type.ToUpper() == typeof(bool).ToString().ToUpper())
            {
                ret = typeof(bool);
            }
            else if (type.ToUpper() == typeof(byte).ToString().ToUpper())
            {
                ret = typeof(byte);
            }
            else if (type.ToUpper() == typeof(sbyte).ToString().ToUpper())
            {
                ret = typeof(sbyte);
            }
            else if (type.ToUpper() == typeof(char).ToString().ToUpper())
            {
                ret = typeof(char);
            }
            else if (type.ToUpper() == typeof(decimal).ToString().ToUpper())
            {
                ret = typeof(decimal);
            }
            else if (type.ToUpper() == typeof(float).ToString().ToUpper())
            {
                ret = typeof(float);
            }
            else if (type.ToUpper() == typeof(double).ToString().ToUpper())
            {
                ret = typeof(double);
            }
            else if (type.ToUpper() == typeof(short).ToString().ToUpper())
            {
                ret = typeof(short);
            }
            else if (type.ToUpper() == typeof(Int16).ToString().ToUpper())
            {
                ret = typeof(Int16);
            }
            else if (type.ToUpper() == typeof(ushort).ToString().ToUpper())
            {
                ret = typeof(ushort);
            }
            else if (type.ToUpper() == typeof(UInt16).ToString().ToUpper())
            {
                ret = typeof(UInt16);
            }
            else if (type.ToUpper() == typeof(int).ToString().ToUpper())
            {
                ret = typeof(int);
            }
            else if (type.ToUpper() == typeof(Int32).ToString().ToUpper())
            {
                ret = typeof(Int32);
            }
            else if (type.ToUpper() == typeof(uint).ToString().ToUpper())
            {
                ret = typeof(uint);
            }
            else if (type.ToUpper() == typeof(UInt32).ToString().ToUpper())
            {
                ret = typeof(UInt32);
            }
            else if (type.ToUpper() == typeof(long).ToString().ToUpper())
            {
                ret = typeof(long);
            }
            else if (type.ToUpper() == typeof(Int64).ToString().ToUpper())
            {
                ret = typeof(Int64);
            }
            else if (type.ToUpper() == typeof(ulong).ToString().ToUpper())
            {
                ret = typeof(ulong);
            }
            else if (type.ToUpper() == typeof(UInt64).ToString().ToUpper())
            {
                ret = typeof(UInt64);
            }
            else if (type.ToUpper() == typeof(Enum).ToString().ToUpper())
            {
                ret = typeof(Enum);
            }
            return ret;
        }

        public static object ConverToObj(Type type, string value)
        {
            object ret = value;
            if (type == typeof(bool))
            {
                ret = bool.Parse(value);
            }
            else if (type == typeof(byte))
            {
                ret = byte.Parse(value);
            }
            else if (type == typeof(sbyte))
            {
                ret = sbyte.Parse(value);
            }
            else if (type == typeof(char))
            {
                ret = char.Parse(value);
            }
            else if (type == typeof(decimal))
            {
                ret = decimal.Parse(value);
            }
            else if (type == typeof(float))
            {
                ret = float.Parse(value);
            }
            else if (type == typeof(double))
            {
                ret = double.Parse(value);
            }
            else if (type == typeof(short))
            {
                ret = short.Parse(value);
            }
            else if (type == typeof(Int16))
            {
                ret = Int16.Parse(value);
            }
            else if (type == typeof(ushort))
            {
                ret = ushort.Parse(value);
            }
            else if (type == typeof(UInt16))
            {
                ret = UInt16.Parse(value);
            }
            else if (type == typeof(int))
            {
                ret = int.Parse(value);
            }
            else if (type == typeof(Int32))
            {
                ret = Int32.Parse(value);
            }
            else if (type == typeof(uint))
            {
                ret = uint.Parse(value);
            }
            else if (type == typeof(UInt32))
            {
                ret = UInt32.Parse(value);
            }
            else if (type == typeof(long))
            {
                ret = long.Parse(value);
            }
            else if (type == typeof(Int64))
            {
                ret = Int64.Parse(value);
            }
            else if (type == typeof(ulong))
            {
                ret = ulong.Parse(value);
            }
            else if (type == typeof(UInt64))
            {
                ret = UInt64.Parse(value);
            }
            else if (type == typeof(Enum))
            {
                ret = value;
            }
            return ret;
        }

        /// <summary>
        /// 设置操作系统时间
        /// </summary>
        /// <param name="_SystemTime">系统时间</param>
        public static void SetSystemTime(DateTime _DateTime)
        {
            SystemTime _SystemTime = new SystemTime();
            _SystemTime.wYear = (ushort)_DateTime.Year;
            _SystemTime.wMonth = (ushort)_DateTime.Month;
            _SystemTime.wDay = (ushort)_DateTime.Day;
            _SystemTime.wHour = (ushort)_DateTime.Hour;
            _SystemTime.wMinute = (ushort)_DateTime.Minute;
            _SystemTime.wSecond = (ushort)_DateTime.Second;
            _SystemTime.wMilliseconds = (ushort)_DateTime.Millisecond;
            _SystemTime.wDayOfWeek = 0;
            SetLocalTime(ref _SystemTime);
        }

        /// <summary>
        /// 通过在内存中执行序列化/反序列化操作，克隆一个对象
        /// </summary>
        public static object CloneObjectBySerialize(object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            object newObject = null;
            using (MemoryStream fs = new MemoryStream())
            {
                formatter.Serialize(fs, obj);
                fs.Seek(0, SeekOrigin.Begin);
                newObject = formatter.Deserialize(fs);
            }
            return newObject;
        }

        private static object errorLock = new object();
        /// <summary>
        /// 写入错误日志文件
        /// </summary>
        /// <param name="ex">错误类</param>
        /// <param name="path">文件路径</param>
        public static void WriteError(Exception ex, string path)
        {
            lock (errorLock)
            {
                string err = "";
                try
                {
                    err += "[Time]: " + DateTime.Now.ToString() + "\r\n";
                    err += "[Description]: " + ex.Message.ToString() + "\r\n";
                    err += "[Error Source]: " + ex.Source + "\r\n";
                    err += "[Error Position]: " + ex.StackTrace + "\r\n\r\n";
                    err += "**************************************************************\r\n\r\n";

                    if (!File.Exists(path))
                    {
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.Write(err);
                        }
                        return;
                    }
                    if (File.ReadAllBytes(path).Length > 1024 * 20)
                    {
                        File.Delete(path);
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.Write(err);
                        }
                        return;
                    }
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.Write(err);
                    }

                }
                catch { }
            }
        }

        /// <summary>
        /// 用指定的格式存储XML文件
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <param name="doc">XML文件</param>
        public static bool SaveXMLDocment(string file, XmlDocument doc)
        {
            try
            {
                if (doc == null)
                    return false;
                if (!(doc.FirstChild is System.Xml.XmlDeclaration))
                {
                    XmlDeclaration _XmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                    doc.InsertBefore(_XmlDeclaration, doc.DocumentElement);
                }
                System.Xml.XmlTextWriter _Writer
                    = new System.Xml.XmlTextWriter(file, System.Text.Encoding.GetEncoding("utf-8"));
                _Writer.Formatting = System.Xml.Formatting.Indented;
                doc.WriteTo(_Writer);
                _Writer.Flush();
                _Writer.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static double PII()
        {
            return 4.0 * Math.Atan(1.0);
        }

        //public static Datagram TaskRecData(Socket s)
        //{
        //    int length = Marshal.SizeOf(typeof(DatagramFormat));
        //    byte[] data = new byte[length];
        //    try
        //    {
        //        if (!SocketReceive(s, ref data, 0, length))
        //            return null;
        //        DatagramFormat format = (DatagramFormat)Helper.BytesToStuct(data, typeof(DatagramFormat));
        //        if (format.nMagic != 0xEEEEEEEE)
        //            return null;
        //        byte[] buffer = new byte[format.nLength];
        //        Buffer.BlockCopy(data, 0, buffer, 0, length);
        //        if (format.nLength - length > 0)
        //        {
        //            if (!SocketReceive(s, ref buffer, length, (int)(format.nLength - length)))
        //                return null;
        //        }
        //        Datagram dGram = Datagram.Conver(buffer);
        //        return dGram;
        //    }
        //    catch(SocketException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public static void SetHeartBeat(Socket s)
        {
            byte[] inOptionValues = new byte[4 * 3];
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, 4);//一回送信間隔 
            BitConverter.GetBytes((uint)1000).CopyTo(inOptionValues, 8);//二回送信間隔 
            s.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
        }

        public static bool SocketReceive(Socket socket, ref byte[] data, int offset, int length)
        {
            try
            {
                int perRec = 0;
                while (length > 0)
                {
                    perRec = socket.Receive(data, offset, length, SocketFlags.None);
                    if (perRec == 0)
                        return false;
                    length -= perRec;
                    offset += perRec;
                }
                return true;
            }
            catch (SocketException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool UdpSend(UdpClient pUdp, string ip, ushort port, byte[] data)
        {
            int length = data.Length;
            int offset = 0;
            int perSend = 0;
            byte[] tmpData = data;
            while (length > 0)
            {
                try
                {
                    perSend = pUdp.Send(tmpData, tmpData.Length, ip, port);

                    length -= perSend;
                    offset += perSend;
                    if (length > 0)
                    {
                        tmpData = new byte[length];
                        Buffer.BlockCopy(data, offset, tmpData, 0, length);
                    }
                }
                catch (SocketException)
                {
                    return false;
                }

            }
            return true;
        }

        public static bool UdpSend(string ip, ushort port, byte[] data)
        {
            UdpClient pUdp = new UdpClient();
            int length = data.Length;
            int offset = 0;
            int perSend = 0;
            byte[] tmpData = data;
            while (length > 0)
            {
                try
                {
                    perSend = pUdp.Send(tmpData, tmpData.Length, ip, port);

                    length -= perSend;
                    offset += perSend;
                    if (length > 0)
                    {
                        tmpData = new byte[length];
                        Buffer.BlockCopy(data, offset, tmpData, 0, length);
                    }
                }
                catch (SocketException)
                {
                    return false;
                }

            }
            return true;
        }

        public static bool SocketSend(Socket socket, byte[] data)
        {
            if (socket == null || !socket.Connected || data == null || data.Length < 1)
                return false;
            try
            {
                int length = data.Length;
                int offset = 0;
                int perSend = 0;
                while (length > 0)
                {
                    perSend = socket.Send(data, offset, length, SocketFlags.None);
                    length -= perSend;
                    offset += perSend;
                }
                return true;
            }
            catch (SocketException e)
            {
                throw e;
            }
        }

        public static bool SocketSend(Socket socket, string cmd, string endstr = "\n")
        {
            if (string.IsNullOrEmpty(cmd))
                return false;
            if (cmd[cmd.Length - 1] != endstr[0])
                cmd += endstr;
            byte[] data = Encoding.Default.GetBytes(cmd);
            return SocketSend(socket, data);
        }

        public static bool Communicate(IPEndPoint endpoint, string cmd, string retEquil = "")
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(endpoint);
                client.NoDelay = true;
                Helper.SetHeartBeat(client.Client);
                client.LingerState = new LingerOption(false, 0);
                if (!SocketSend(client.Client, cmd))
                {
                    return false;
                }
                StreamReader reader = new StreamReader(client.GetStream(), Encoding.Default);
                if (retEquil != "")
                {
                    string ret = reader.ReadLine();
                    client.Close();
                    client.Dispose();
                    if (ret == null)
                        return false;
                    if (ret.ToUpper().IndexOf(retEquil) >= 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    client.Close();
                    client.Dispose();
                    return true;
                }
            }
            catch (SocketException ex)
            {
                client.Dispose();
                throw ex;
            }
            catch (Exception e)
            {
                client.Dispose();
                throw e;
            }
        }

        public static bool Communicate(TcpClient client, string cmd, string retEquil = "")
        {
            if (!client.Connected)
            {
                return false;
            }
            try
            {
                client.LingerState = new LingerOption(false, 0);
                if (!SocketSend(client.Client, cmd))
                {
                    return false;
                }
                StreamReader reader = new StreamReader(client.GetStream(), Encoding.Default);
                if (retEquil != "")
                {
                    string ret = reader.ReadLine();
                    if (ret == null)
                        return false;
                    if (ret.ToUpper().IndexOf(retEquil) >= 0)
                        return true;
                    else
                        return false;
                }
                else
                    return true;
            }
            catch (SocketException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string Query(TcpClient client, string cmd)
        {
            if (!client.Connected)
            {
                return null;
            }

            try
            {
                client.LingerState = new LingerOption(false, 0);
                if (!SocketSend(client.Client, cmd))
                {
                    return null;
                }
                StreamReader reader = new StreamReader(client.GetStream(), Encoding.Default);
                string ret = reader.ReadLine();
                return ret;
            }
            catch (SocketException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string Query(IPEndPoint endpoint, string cmd)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(endpoint);
                client.NoDelay = true;
                Helper.SetHeartBeat(client.Client);
                client.LingerState = new LingerOption(false, 0);
                if (!SocketSend(client.Client, cmd))
                {
                    client.Close();
                    client.Dispose();
                    return null;
                }
                StreamReader reader = new StreamReader(client.GetStream(), Encoding.Default);
                string ret = reader.ReadLine();
                client.Close();
                client.Dispose();
                return ret;
            }
            catch (SocketException ex)
            {
                client.Close();
                client.Dispose();
                throw ex;
            }
            catch (Exception e)
            {
                client.Close();
                client.Dispose();
                throw e;
            }
        }

        public static string QueryParams(TcpClient client, string cmd)
        {
            if (!client.Connected)
            {
                return null;
            }

            try
            {
                client.LingerState = new LingerOption(false, 0);
                if (!SocketSend(client.Client, cmd))
                {
                    return null;
                }
                byte[] head = new byte[2];
                if (!SocketReceive(client.Client, ref head, 0, 2) || head[0] != '#')
                {
                    return string.Empty;
                }
                int headlength = head[1] - 48;

                byte[] lenByte = new byte[headlength];
                if (!SocketReceive(client.Client, ref lenByte, 0, headlength))
                {
                    return string.Empty;
                }

                int length = 0;
                string slen = Encoding.Default.GetString(lenByte);
                if (!int.TryParse(slen, out length))
                    return string.Empty;
                byte[] data = new byte[length];
                if (!SocketReceive(client.Client, ref data, 0, length))
                {
                    return string.Empty;
                }
                return Encoding.Default.GetString(data);
            }
            catch (SocketException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static byte[] QueryParambytes(TcpClient client, string cmd)
        {
            if (!client.Connected)
            {
                return null;
            }
            try
            {
                client.LingerState = new LingerOption(false, 0);
                if (!SocketSend(client.Client, cmd))
                {
                    return null;
                }
                byte[] head = new byte[2];
                if (!SocketReceive(client.Client, ref head, 0, 2) || head[0] != '#')
                {
                    return null;
                }
                int headlength = head[1] - 48;

                byte[] lenByte = new byte[headlength];
                if (!SocketReceive(client.Client, ref lenByte, 0, headlength))
                {
                    return null;
                }
                int length = 0;
                string slen = Encoding.Default.GetString(lenByte);
                if (!int.TryParse(slen, out length))
                    return null;
                int offset = 2 + headlength;
                byte[] data = new byte[offset + length];
                data[0] = head[0];
                data[1] = head[1];
                Buffer.BlockCopy(lenByte, 0, data, 2, headlength);
                if (!SocketReceive(client.Client, ref data, offset, length))
                {
                    return null;
                }
                return data;
            }
            catch (SocketException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string QueryParams(IPEndPoint endpoint, string cmd)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(endpoint);
                client.NoDelay = true;
                Helper.SetHeartBeat(client.Client);
                client.LingerState = new LingerOption(false, 0);
                string ret = QueryParams(client, cmd);
                client.Close();
                client.Dispose();
                return ret;
            }
            catch (SocketException ex)
            {
                client.Close();
                client.Dispose();
                throw ex;
            }
            catch (Exception e)
            {
                client.Close();
                client.Dispose();
                throw e;
            }
        }

        public static IDictionary<string, string> SplitParam(string keyValues)
        {
            IDictionary<string, string> rets = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string[] Params = keyValues.Split(new char[] { ';' });
            if (Params == null)
                return null;
            foreach (string param in Params)
            {
                string[] keyValue = param.Split(new char[] { '=' });
                if (keyValue != null && keyValue.Length > 1)
                {
                    rets.Add(keyValue[0], keyValue[1]);
                }
            }
            if (rets.Count > 0)
                return rets;
            else
                return null;
        }

        public static XmlElement TypeToXml(XmlDocument doc, object obj)
        {
            if (doc == null || obj == null)
                return null;
            Type type = obj.GetType();
            XmlElement node = doc.CreateElement(type.Name);
            PropertyInfo[] propertyInfoes
                    = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in propertyInfoes)
            {
                XmlElement childNode = doc.CreateElement(propertyInfo.Name);
                childNode.InnerText = propertyInfo.GetValue(obj, null).ToString();
                node.AppendChild(childNode);
            }
            return node;
        }

        public static XmlDocument TypeToXmlDoc(object obj)
        {
            if (obj == null)
                return null;
            XmlDocument doc = new XmlDocument();
            Type type = obj.GetType();
            XmlElement node = doc.CreateElement(type.Name);
            PropertyInfo[] propertyInfoes
                    = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in propertyInfoes)
            {
                XmlElement childNode = doc.CreateElement(propertyInfo.Name);
                childNode.InnerText = propertyInfo.GetValue(obj, null).ToString();
                node.AppendChild(childNode);
            }
            doc.AppendChild(node);
            return doc;
        }

        public static string TypeToString(object obj)
        {
            if (obj == null)
                return null;
            XmlDocument doc = new XmlDocument();
            Type type = obj.GetType();
            XmlElement node = doc.CreateElement(type.Name);
            PropertyInfo[] propertyInfoes
                    = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in propertyInfoes)
            {
                XmlElement childNode = doc.CreateElement(propertyInfo.Name);
                childNode.InnerText = propertyInfo.GetValue(obj, null).ToString();
                node.AppendChild(childNode);
            }
            doc.AppendChild(node);
            return doc.OuterXml;
        }

        public static string GetNumber(string val)
        {
            bool dot = false;
            int len = val.Length;
            string tmp = "";
            for (int i = 0; i < len; i++)
            {
                if (val[i] <= 57 && val[i] >= 48)
                    tmp += val[i];
                else if (val[i] == '.' && !dot)
                {
                    dot = true;
                    if (i == 0)
                        tmp += "0";
                    tmp += val[i];
                }
                else
                    break;
            }
            return tmp;
        }

        public static double ConverUnit(string ps, string name = "")
        {
            if (ps.ToUpper().IndexOf("G") > 0)
            {
                return 1000000000;
            }
            else if (ps.ToUpper().IndexOf("M") > 0)
            {
                return 1000000;
            }
            else if (ps.ToUpper().IndexOf("K") > 0)
            {
                return 1000;
            }
            else
            {
                if (string.Compare(name, "CenterFreq", true) == 0 ||
                    string.Compare(name, "StartFreq", true) == 0 ||
                    string.Compare(name, "StopFreq", true) == 0)
                    return 1000000;
                else if (string.Compare(name, "SpectrumSpan", true) == 0 ||
                    string.Compare(name, "FilterSpan", true) == 0 ||
                    string.Compare(name, "StepFreq", true) == 0)
                    return 1000;
                else
                    return 1;
            }
        }

        /// <summary>
        /// 获取Remoting的URL字符串
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口号</param>
        /// <param name="name">注册名称</param>
        /// <param name="type">通讯方式:tcp、Http等</param>
        /// <returns>URL字符串</returns>
        public static string GetRemotingUrl(string ip, ushort port, string name, string type = "Tcp")
        {
            return type + "://" + ip + ":" + port.ToString() + "/" + name;
        }

        public static bool PortIsAvailable(int port, bool bUdpPort = true, bool bTcpPortListenPort = true, bool bTcpConnectPort = true)
        {
            if (port < 1 || port > 65535)
                return false;
            //获取本地计算机的网络连接和通信统计数据的信息
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            //返回本地计算机上的所有UDP监听程序
            if (bUdpPort)
            {
                IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();
                foreach (IPEndPoint ep in ipsUDP)
                {
                    if (port == ep.Port)
                        return false;
                }
            }
            ////返回本地计算机上的所有Tcp监听程序
            if (bTcpPortListenPort)
            {
                IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();
                foreach (IPEndPoint ep in ipsTCP)
                {
                    if (port == ep.Port)
                        return false;
                }
            }
            //返回本地计算机上的Internet协议版本4(IPV4 传输控制协议(TCP)连接的信息。
            if (bTcpConnectPort)
            {
                TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
                foreach (TcpConnectionInformation conn in tcpConnInfoArray)
                {
                    if (port == conn.LocalEndPoint.Port)
                        return false;
                }
            }
            return true;
        }

        /// <summary>  
        /// 将一个方法function异步运行，在执行完毕时执行回调callback  
        /// </summary>  
        /// <param name="function">异步方法，该方法没有参数，返回类型必须是void</param>  
        /// <param name="callback">异步方法执行完毕时执行的回调方法，该方法没有参数，返回类型必须是void</param>  
        public static async void RunAsync(Action function, Action callback)
        {
            Func<System.Threading.Tasks.Task> taskFunc = () =>
            {
                return System.Threading.Tasks.Task.Run(() =>
                {
                    function();
                });
            };
            await taskFunc();
            if (callback != null)
                callback();
        }

        /// <summary>  
        /// 将一个方法function异步运行，在执行完毕时执行回调callback  
        /// </summary>  
        /// <typeparam name="TResult">异步方法的返回类型</typeparam>  
        /// <param name="function">异步方法，该方法没有参数，返回类型必须是TResult</param>  
        /// <param name="callback">异步方法执行完毕时执行的回调方法，该方法参数为TResult，返回类型必须是void</param>  
        public static async void RunAsync<TResult>(Func<TResult> function, Action<TResult> callback)
        {
            Func<System.Threading.Tasks.Task<TResult>> taskFunc = () =>
            {
                return System.Threading.Tasks.Task.Run(() =>
                {
                    return function();
                });
            };
            TResult rlt = await taskFunc();
            if (callback != null)
                callback(rlt);
        }

        public static Process RunningOnce()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //Loop through the running processes in with the same name

            Process proc = null;
            foreach (Process process in processes)
            {
                //Ignore the current process
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file.
                    //if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    //{
                    //    //Return the other process instance.
                    //    proc =  process;
                    //    break;
                    //}
                    if (process.ProcessName == current.ProcessName)
                    {
                        //Return the other process instance.
                        proc = process;
                        break;
                    }
                }
            }
            if (proc == null)
            {
                return null;
            }
            else
            {
                ShowWindow(proc.MainWindowHandle, ShowWindowEnum.ShowDefault);
                SetWindowPos(proc.MainWindowHandle, -1, 0, 0, 0, 0, 0x001 | 0x002 | 0x040);
                SetWindowPos(proc.MainWindowHandle, -2, 0, 0, 0, 0, 0x001 | 0x002 | 0x040);
                SetForegroundWindow(proc.MainWindowHandle);
                return proc;
            }
        }

        public static void ActiveProcess(int id)
        {
            Process[] processes = Process.GetProcesses();
            //Loop through the running processes in with the same name

            Process proc = null;
            foreach (Process process in processes)
            {
                if (process.Id == id)
                {
                    proc = process;
                    break;
                }
            }
            if (proc == null)
            {
                return;
            }
            else
            {
                ShowWindow(proc.MainWindowHandle, ShowWindowEnum.ShowDefault);
                SetWindowPos(proc.MainWindowHandle, -1, 0, 0, 0, 0, 0x001 | 0x002 | 0x040);
                SetWindowPos(proc.MainWindowHandle, -2, 0, 0, 0, 0, 0x001 | 0x002 | 0x040);
                SetForegroundWindow(proc.MainWindowHandle);
            }
        }

        public static bool ServiceOperate(string serviceName, bool serviceStart = true, int timeoutinSeconds = 15)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    TimeSpan timeout = new TimeSpan(0, 0, 15);
                    //开
                    if (serviceStart)
                    {
                        if (sc.Status != ServiceControllerStatus.Running)
                        {
                            sc.Start();
                            sc.WaitForStatus(ServiceControllerStatus.Running, timeout);
                        }
                    }
                    else
                    {
                        if (sc.Status != ServiceControllerStatus.Stopped)
                        {
                            sc.Stop();
                            sc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }


    }
}
