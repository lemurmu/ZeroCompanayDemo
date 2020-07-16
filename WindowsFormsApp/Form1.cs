using AircraftControl;
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

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            measureDDF = new MeasureDDFPanel();
            this.Controls.Add(measureDDF);

            //measureDDF.MeasureSetDDF(0, 0);
            //measureDDF.MeasureSetDDF(0, 60);

            //measureDDF.AddPoint(0, 0);
            //measureDDF.AddPoint(0, 60);

            //measureDDF.MeasureSetDDF(50, 0);
            //measureDDF.MeasureSetDDF(10, 60);
            //measureDDF.MeasureSetDDF(50, 120);
            //measureDDF.MeasureSetDDF(10, 180);
            //measureDDF.MeasureSetDDF(5, 240);
            //measureDDF.MeasureSetDDF(10, 300);

            InitAatennaMap();


            DataColumn dt = new DataColumn("sex");
            DataColumn d = dt;

            dt = new DataColumn("name");
            Console.WriteLine(dt == d);

            string a = "fff";
            string b = a;

            a = "dadatestone";
            Console.WriteLine(b);
            Console.WriteLine(a.Equals(b));
        }
        private int antennaCount = 6;//天线数量
        protected int AntennaCount { get => antennaCount; set => antennaCount = value; }
        private readonly Dictionary<int, int> antennaMapping = new Dictionary<int, int>();//天线映射表
        private double[] arr = new double[6];
        void InitAatennaMap()
        {
            antennaMapping.Clear();
            int index = 0;
            int step = 360 / AntennaCount;
            for (int i = 1; i <= AntennaCount; i++)
            {
                antennaMapping.Add(i, index);
                index += step;
                arr[i - 1] = 14.5;
                if (i == 3)
                {
                    arr[i - 1] = 54.5;
                }
            }

        }

        MeasureDDFPanel measureDDF;
        Thread th;
        Thread th1;

        private void button1_Click(object sender, EventArgs e)
        {
            th1 = new Thread(() =>
            {

                for (int i = 0; i < 1000; i++)
                {
                    Thread.Sleep(100);
                }
            });
            th1.IsBackground = true;
            th1.Start();

            th = new Thread(() =>
                          {
                              for (int i = 1; i <= AntennaCount; i++)
                              {
                                  //Thread.Sleep(100);
                                  measureDDF.MeasureSetDDF((float)arr[i - 1], antennaMapping[i]);//绘制方向图

                                  var x = antennaMapping[i];
                                  var y = arr[i - 1];
                                  Console.WriteLine("(" + x + "," + y + ")");

                                  // measureDDF.AddPoint((float)arr[i - 1], antennaMapping[i]);

                                  if (i == AntennaCount)
                                  {
                                      i = 0;
                                  }
                                  DateTime startTime = DateTime.Now;
                                  while ((DateTime.Now - startTime).TotalMilliseconds < 1000)
                                  {
                                  }
                                  //  Thread.Sleep(1000);

                                  continue;
                              }
                          });
            th.IsBackground = true;
            th.Start();


        }
    }
}
