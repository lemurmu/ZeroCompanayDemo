using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace FFmpeg
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            //ProcessStartInfo ps = new ProcessStartInfo();
            //ps.CreateNoWindow = true;
            //ps.WindowStyle = ProcessWindowStyle.Hidden;
            //ps.FileName = @"D:\ffmpeg\ffmpeg\bin\ffmpeg.exe";
            //ps.Arguments = "play";

            //Process proc = new Process();
            //proc.StartInfo = ps;
            //proc.WaitForExit();//不等待完成就不调用此方法
            //proc.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
            //new FrmMain().Test1();
            //Console.ReadKey();
        }
    }
}
