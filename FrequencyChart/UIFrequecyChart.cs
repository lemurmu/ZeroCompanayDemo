using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Drawing.Drawing2D;
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting;
using System.Threading;
using System.IO;
using DevExpress.Utils.Extensions;
using DevExpress.XtraSplashScreen;

namespace FrequencyChart
{
    public partial class UIFrequecyChart : DevExpress.XtraEditors.XtraUserControl
    {
        public UIFrequecyChart()
        {
            InitializeComponent();
           // Init();
        }

        readonly string[] files = { "Audio\\test1.wav", "Audio\\test2.wav", "Audio\\test3.wav", "Audio\\test4.wav", "Audio\\test5.wav" };
        readonly string[] Honeyfiles = { "Honey\\1.wav", "Honey\\2.wav", "Honey\\3.wav", "Honey\\4.wav", "Honey\\2.wav" };
        readonly double[] nums = new double[] { 0.1235468, -0.14500 };
        readonly string receiverFile = "ReceiverData\\";
        readonly Dictionary<string, List<double>> receiverDict = new Dictionary<string, List<double>>();

        const int receiverFileCount = 1200;

        int fileIndex = 1;
        bool leftStatus = true;
        const int byteSample = 2;
        const int dataPosition = 40;
        Random rd = new Random();
        List<double> data;

        private async void Init()
        {
            SwiftPlotDiagram diagram = (SwiftPlotDiagram)(this.chartControl1.Diagram);
            diagram.AxisY.WholeRange.SetMinMaxValues(-50, 50);
            diagram.AxisX.WholeRange.SetMinMaxValues(1, 5981);
            diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;
            panel2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            IOverlaySplashScreenHandle handle= SplashScreenManager.ShowOverlayForm(this);
            for (int k = 0; k < receiverFileCount; k++)
            {
                string filename = $"{receiverFile}{k + 1}_data.txt";
                List<double> data = await GetReceiverData(filename);
                receiverDict.Add(filename, data);
            }
            SplashScreenManager.CloseOverlayForm(handle);
            
            timer1.Enabled = true;
            timer1.Interval = 1;
        }
        async void Plot()
        {
            data = await GetData("Honey\\1.wav");
            for (int i = 0; i < data.Count; i++)
            {
                data[i] += nums[rd.Next(0, 2)];
            }

            SwiftPlotDiagram diagram = (SwiftPlotDiagram)(this.chartControl1.Diagram);
            double min = data.Min();
            double max = data.Max();
            diagram.AxisY.WholeRange.SetMinMaxValues(min, max);
            diagram.AxisX.WholeRange.SetMinMaxValues(1, 1280);
            SeriesPoint[] seriesPoints = new SeriesPoint[data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                seriesPoints[i] = new SeriesPoint(i, data[i]);
            }
            this.chartControl1.Series[0].Points.Clear();
            this.chartControl1.Series[0].Points.AddRange(seriesPoints);
        }

        void MoveStrips()
        {
            Diagram diagram = this.chartControl1.Diagram;
            Strip currentFrameStrip = null;
            if (diagram is SwiftPlotDiagram)
            {
                currentFrameStrip = ((SwiftPlotDiagram)diagram).AxisX.Strips[0];
            }
            else if (diagram is XYDiagram)
            {
                currentFrameStrip = ((XYDiagram)diagram).AxisX.Strips[0]; 
            }

            if (currentFrameStrip == null) { return; }
            currentFrameStrip.MinLimit.AxisValue = TimeSpan.MinValue;
            currentFrameStrip.MaxLimit.AxisValue = TimeSpan.MaxValue; 
            currentFrameStrip.MinLimit.AxisValue = TimeSpan.FromSeconds(0.08952);
            currentFrameStrip.MaxLimit.AxisValue = TimeSpan.FromSeconds(0.324852);
        }
        Task<List<double>> GetData()
        {
            return Task.Run(() =>
            {
                List<double> list = new List<double>();
                int index = rd.Next(0, 5);
                string[] data = LoadWav(Honeyfiles[index]);
                int m = 0;

                for (int i = 0; i < data.Length; i++)
                {
                    if (leftStatus)
                    {
                        if (m % 2 == 0)
                            list.Add(double.Parse(data[i]));
                    }
                    else
                    {
                        if (m % 2 != 0)
                            list.Add(double.Parse(data[i]));
                    }
                    m++;
                }
                return list;
            });
        }

        Task<List<double>> GetData(string fileName)
        {
            return Task.Run(() =>
            {
                List<double> list = new List<double>();
                string[] data = LoadWav(fileName);
                int m = 0;

                for (int i = 0; i < data.Length; i++)
                {
                    if (leftStatus)
                    {
                        if (m % 2 == 0)
                            list.Add(double.Parse(data[i]));
                    }
                    else
                    {
                        if (m % 2 != 0)
                            list.Add(double.Parse(data[i]));
                    }
                    m++;
                }
                return list;
            });
        }

        Task<List<double>> GetReceiverData(string fileName)
        {
            return Task.Run(() =>
            {
                List<double> list = new List<double>();
                string str = string.Empty;
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while (!string.IsNullOrEmpty(str = sr.ReadLine()))
                    {
                        double data = double.Parse(str);
                        list.Add(data);
                    }
                }
                return list;
            });
        }

        private void UIFrequecyChart_Load(object sender, EventArgs e)
        {
            Init();
           
        }

        private void checkEdit1_CheckStateChanged(object sender, EventArgs e)
        {

        }


        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxEdit1.SelectedIndex)
            {
                case 0:
                    chartControl1.Series[0].ChangeView(ViewType.SwiftPlot);
                    break;
                case 1:
                    chartControl1.Series[0].ChangeView(ViewType.Spline);
                    break;
                case 2:
                    chartControl1.Series[0].ChangeView(ViewType.Bar);
                    break;
                default:
                    break;
            }
        }

        private void checkEdit2_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkEdit2.Checked)
            {
                panel2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                int he = (int)((Root.Height - layoutControlItem1.Height) / 2d);
                panel1.Height = he;
                panel2.Height = he;
            }
            else
            {
                panel2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }


        string[] LoadWav(string fileName)
        {
            byte[] length = new byte[4];
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            fs.Position = dataPosition;
            fs.Read(length, 0, 4);
            byte[] content = new byte[BitConverter.ToInt32(length, 0)];
            string[] sample = new string[content.Length / byteSample];
            fs.Read(content, 0, content.Length);
            GetHex(content);
            sample = GetSample(content);
            return sample;
        }

        static void GetHex(byte[] x)
        {
            byte tmp;
            for (int i = 0; i < x.Length; i++)
            {
                tmp = Convert.ToByte(x[i].ToString("X"), 16);
                x[i] = tmp;
            }
        }

        static string[] GetSample(byte[] x)
        {
            string[] retValue = new string[x.Length / byteSample];
            for (int i = 0; i < retValue.Length; i++)
            {
                for (int j = (i + 1) * byteSample - 1; j >= i * byteSample; j--)
                {
                    retValue[i] += x[j].ToString("X");
                }
                retValue[i] = ((double)Convert.ToInt16(retValue[i], 16) / 32768).ToString("F4");
            }
            return retValue;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // Plot();
            PloatReceiverData();
            MoveStrips();
        }

        void PloatReceiverData()
        {
            string filename = $"{receiverFile}{fileIndex}_data.txt";
            List<double> data = receiverDict[filename];

            SeriesPoint[] seriesPoints = new SeriesPoint[data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                seriesPoints[i] = new SeriesPoint(i, data[i]);
            }
            this.chartControl1.Series[0].Points.Clear();
            this.chartControl1.Series[0].Points.AddRange(seriesPoints);


            if (fileIndex == receiverFileCount)
            {
                fileIndex = 1;
            }
            else
            {
                fileIndex++;
            }

        }

        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {
            this.chartControl1.Series[0].Visible = checkEdit5.Checked;
        }
    }
}
