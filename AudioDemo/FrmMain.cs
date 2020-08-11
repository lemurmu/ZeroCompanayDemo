using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioDemo
{
    public partial class FrmMain : Form
    {
        const bool leftStatus = false;

        public FrmMain()
        {
            InitializeComponent();
           
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
           // this.Width = 1650;
          ///  this.Height = 600;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(() =>

            {

                this.BeginInvoke(new MethodInvoker(() => { toolStripLabel1.Text = leftStatus ? "左声道" : "右声道"; }));

                List<double> list = new List<double>();

                StreamReader sr = new StreamReader("data.txt", Encoding.Default);

                int m = 0;

                while (!sr.EndOfStream)
                {
                    if (leftStatus)
                    {
                        if (m % 2 == 0)
                            list.Add(double.Parse(sr.ReadLine()));
                    }
                    else
                    {

                        if (m % 2 != 0)
                            list.Add(double.Parse(sr.ReadLine()));

                    }

                    m++;

                }

                sr.Close();

                Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                Graphics g = Graphics.FromImage(bitmap);

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                g.DrawLine(new Pen(Color.Black, 5), new Point(20, 20), new Point(20, 540));

                g.DrawLine(new Pen(Color.Black, 5), new Point(20, 290), new Point(1600, 290));



                int k = list.Count / 1550;



                for (int i = 0; i < list.Count; i++)

                {

                    g.DrawLine(new Pen(Color.Green, 1), new Point(20 + i / k, 290), new Point(20 + i / k, 290 + (int)(list[i] * 250 * 2)));

                }

                this.pictureBox1.Image = bitmap;

            })).Start();
        }
    }
}
