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

namespace FrequencyChart
{
    public partial class UIFrequecyChart : DevExpress.XtraEditors.XtraUserControl
    {
        public UIFrequecyChart()
        {
            InitializeComponent();
            Plot();
        }

        Random rd = new Random();
        void Plot()
        {
            for (int i = 0; i < 100; i++)
            {
                chartControl1.Series[0].Points.Add(new SeriesPoint(i, rd.Next(2, 50)));
                chartControl2.Series[0].Points.Add(new SeriesPoint(i, rd.Next(2, 50)));
            }
        }

        private void UIFrequecyChart_Load(object sender, EventArgs e)
        {
            panel2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            Graphics gp = Graphics.FromImage(pictureEdit1.Image); ;
            Rectangle rect = new Rectangle(30, 40, 400, 200);
            LinearGradientBrush lb = new LinearGradientBrush(rect, Color.Red, Color.Green, LinearGradientMode.Horizontal);
            ColorBlend cb = new ColorBlend(4);
            Color[] colorArray = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow };
            float[] positionArray = new float[] { 0f, 0.33f, 0.67f, 1f };
            cb.Colors = colorArray;
            cb.Positions = positionArray;
            lb.InterpolationColors = cb;
            gp.FillRectangle(lb, rect);

            gp.Dispose();
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
    }
}
