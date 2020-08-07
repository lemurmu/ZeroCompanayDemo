#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainCos
{
    public class Test
    {
        [Conditional("DEBUG")]
        public void Debug()
        {
            Console.WriteLine("测试DEBUG.............");
        }
    }
}
