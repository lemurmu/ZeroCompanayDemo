using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace FFmpeg
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        string path = AppDomain.CurrentDomain.BaseDirectory;
        // string ffmpegePath = @"D:\ffmpeg\ffmpeg\bin\";
        string pathVedio = "";


        /// <summary>
        /// 调用ffmpeg转码
        /// </summary>
        public void DncodeByFFmpeg()
        {

            Process p = new Process();


            p.StartInfo.FileName = path + "ffmpeg.exe";

            p.StartInfo.UseShellExecute = false;
            string srcFileName = "";
            string destFileName = "";
            srcFileName = path + "InitVideo1.mp4";

            destFileName = path + "InitVideo.avi";

            p.StartInfo.Arguments = "-i " + srcFileName + " -y  -vcodec h264 -b 500000 " + destFileName;    //执行参数

            p.StartInfo.UseShellExecute = false;  ////不使用系统外壳程序启动进程
            p.StartInfo.CreateNoWindow = true;  //不显示dos程序窗口

            p.StartInfo.RedirectStandardInput = true;

            p.StartInfo.RedirectStandardOutput = true;

            p.StartInfo.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中

            p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);

            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);

            p.StartInfo.UseShellExecute = false;

            p.Start();

            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            p.BeginErrorReadLine();//开始异步读取



            p.WaitForExit();//阻塞等待进程结束

            p.Close();//关闭进程

            p.Dispose();//释放资源

            pathVedio = path + "InitVideo.avi";
            axWindowsMediaPlayer1.URL = pathVedio;
        }

        private static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {

            Console.WriteLine(e.Data);

        }

        private static void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {

            Console.WriteLine(e.Data);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (btn_Show.Text == "播放")
            {
                btn_Show.Text = "关闭";
                pathVedio = path + "InitVideo.avi";
                axWindowsMediaPlayer1.URL = pathVedio;
                // axWindowsMediaPlayer1.openPlayer(pathVedio);
            }
            else
            {
                btn_Show.Text = "播放";
                axWindowsMediaPlayer1.close();
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            DncodeByFFmpeg();
        }
    }
}
