using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordReportHtml
{
    class Program
    {

        static void Main(string[] args)
        {
            var list = Read.ReadToHtml("1.docx");
            using (StreamWriter sw = new StreamWriter("1.html",false))
            {
                sw.WriteLine("<!DOCTYPE html>\r\n");
                sw.WriteLine("<head>\r\n");

                sw.WriteLine("<meta charset ='utf-8'/>\r\n");

                sw.WriteLine("<title>111111</title >\r\n");
                sw.WriteLine("</head>\r\n");
                sw.WriteLine("<body>\r\n");
                for (int i = 0; i < list.Count; i++)
                {
                    sw.WriteLine(list[i] + "\r\n");
                    Console.WriteLine(list[i]);
                }
                sw.WriteLine("</body>\r\n");
                sw.WriteLine("</html>\r\n");

                sw.Flush();
                sw.Close();
            }
            Console.ReadKey();

        }
    }
}
