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

namespace FrequencyChart
{
    public partial class UIFrequecyChart : DevExpress.XtraEditors.XtraUserControl
    {
        public UIFrequecyChart()
        {
            InitializeComponent();
            //Plot();
        }

        readonly string[] files = { "Audio\\test1.wav" , "Audio\\test2.wav" , "Audio\\test3.wav" , "Audio\\test4.wav" , "Audio\\test5.wav" };

        List<double> data;
        bool leftStatus = true;
        const int byteSample = 2;

        const int dataPosition = 40;

        //0x16 2byte 0002  双声道

        //0x22 2byte 0010  16位

        //0x18 4byte 0000AC44   44100采样率
        Random rd = new Random();
        async void Plot()
        {
            this.chartControl1.Series[0].Points.Clear();
            data = await GetData();

            for (int i = 0; i < data.Count; i++)
            {
                this.chartControl1.Series[0].Points.Add(new SeriesPoint(i, data[i]));
            }

        }

        Task<List<double>> GetData()
        {
            return Task.Run(() =>
            {
                List<double> list = new List<double>();
                int index = rd.Next(0, 5);
                string[] data = LoadWav(files[index]);
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

        private void UIFrequecyChart_Load(object sender, EventArgs e)
        {
            panel2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            //Graphics gp = Graphics.FromImage(pictureEdit1.Image); ;
            //Rectangle rect = new Rectangle(30, 40, 400, 200);
            //LinearGradientBrush lb = new LinearGradientBrush(rect, Color.Red, Color.Green, LinearGradientMode.Horizontal);
            //ColorBlend cb = new ColorBlend(4);
            //Color[] colorArray = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow };
            //float[] positionArray = new float[] { 0f, 0.33f, 0.67f, 1f };
            //cb.Colors = colorArray;
            //cb.Positions = positionArray;
            //lb.InterpolationColors = cb;
            //gp.FillRectangle(lb, rect);
            //gp.Dispose();
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

        private void plot_btn_Click(object sender, EventArgs e)
        {
            Plot();

        }


        string[] LoadWav(string fileName)
        {
            byte[] length = new byte[4];

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            fs.Position = dataPosition;

            fs.Read(length, 0, 4);

            byte[] content = new byte[getHexToInt(length)];

            string[] sample = new string[content.Length / byteSample];

            fs.Read(content, 0, content.Length);

            getHex(content);

            sample = getSample(content);

            return sample;
        }

        static int getHexToInt(byte[] x)

        {

            string retValue = "";

            for (int i = x.Length - 1; i >= 0; i--)

            {

                retValue += x[i].ToString("X");

            }

            return Convert.ToInt32(retValue, 16);

        }



        static void getHex(byte[] x)

        {

            byte tmp;

            for (int i = 0; i < x.Length; i++)

            {

                tmp = Convert.ToByte(x[i].ToString("X"), 16);

                x[i] = tmp;

            }

        }



        static string[] getSample(byte[] x)

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


    }
}
