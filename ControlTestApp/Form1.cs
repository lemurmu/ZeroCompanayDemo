using AircraftControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //this.bearingControl1.Statistics = new LD.Platform.Dsp.DFProcess();
            //    this.bearingControl1.Statistics.Write(0, 2);
            //    this.bearingControl1.Statistics.Write(60, 23);
            //    this.bearingControl1.Statistics.Write(120, 58);
            //    this.bearingControl1.Statistics.Write(180, 78);
            //    this.bearingControl1.Statistics.Write(240, 78);
            //    this.bearingControl1.Statistics.Write(360, 78);
            //    this.bearingControl1.Plot();
            // this.chart1.Series.Clear();
            //   this.chart1.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series { ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar });
            //measureDDFPanel = new MeasureDDFPanel();
            //measureDDFPanel.Dock = DockStyle.Fill;

            ////  panel.InitChart(0);
            //this.Controls.Add(measureDDFPanel);

            InitAatennaMap();
            InitPatterChart();

        }
        Random rd = new Random();
        private readonly Dictionary<int, int> antennaMapping = new Dictionary<int, int>();//天线映射表
        /// <summary>
        /// 初始化天线映射表
        /// 1，0°
        /// 2，60°
        /// 3，120°
        /// 4，180°
        /// 5，240°
        /// 6，300°
        /// </summary>
        void InitAatennaMap()
        {
            antennaMapping.Clear();
            int index = 0;
            int step = 360 / 6;
            for (int i = 1; i <= 6; i++)
            {
                antennaMapping.Add(i, index);
                index += step;
            }
        }


        private void InitPatterChart()
        {
            for (int i = 1; i <= 6; i++)
            {
                measureDDFPanel1.MeasureSetDDF(20, antennaMapping[i]);//绘制方向图
            }
        }

        void Start()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(Start));
            }
            for (int i = 1; i <= 6; i++)
            {
                this.measureDDFPanel1.MeasureSetDDF(rd.Next(0, 100), antennaMapping[i]);//绘制方向图
                Thread.Sleep(10);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        int index = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            //Task.Run(() =>
            //{
            //    Start();
            //});
            this.measureDDFPanel1.MeasureSetDDF(rd.Next(0, 100),index);//绘制方向图
            index += 60;
        }
    }
}
