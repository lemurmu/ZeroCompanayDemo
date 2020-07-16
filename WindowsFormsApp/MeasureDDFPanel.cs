using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AircraftControl
{
    public partial class MeasureDDFPanel : UserControl
    {
        public MeasureDDFPanel()
        {
            InitializeComponent();
            //m_Points = new Dictionary<int, DataPoint>();
            //Dictionary<int, DataPoint> dic1_SortedByKey = m_Points.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value); ;
            // InitChar();


            this.m_Chart.Series[0].Points.AddXY(0, 65);
            this.m_Chart.Series[0].Points.AddXY(60, 8);
            this.m_Chart.Series[0].Points.AddXY(120, 158);
            this.m_Chart.Series[0].Points.AddXY(180, 87);
            this.m_Chart.Series[0].Points.AddXY(240, 12);
            this.m_Chart.Series[0].Points.AddXY(300, 25);
            this.m_Chart.Series[0].Points.AddXY(0, 65);
        }

        #region 属性

        public Dictionary<int, double> Ponts
        {
            get { return m_Points; }
        }

        #endregion
        #region 内部方法
        Dictionary<int, double> m_Points = new Dictionary<int, double>();
        private void InitChar()
        {
            DataPoint a = this.m_Chart.Series[0].Points[30];
            int i = 0;
            this.m_Chart.Series[0].Points.Clear();
            //this.m_Chart.Series[0].Points.ToList().FindIndex();
            List<DataPoint> datapoints = new List<DataPoint>();
            for (i = 0; i <= 360; i++)
            {
                DataPoint datapoint = new DataPoint(i, -20D);
                datapoints.Add(datapoint);
                this.m_Chart.Series[0].Points.Add(datapoint);
            }
        }
        int FindIndex(int _index, double data)
        {
            int index = 0;
            if (_index == 360)
            {
                _index = 0;
            }

            if (m_Points.Keys.Contains(_index))
            {
                index = m_Points.ToList().FindIndex(t => t.Key == _index);
            }
            else
            {
                DataPoint point = new DataPoint(_index, data);
                Dictionary<int, double> newPoints = null;
                //if (IsFirst)
                //{
                //    m_Points.Add(_index, data);
                //    point = new DataPoint(_index, data);
                //    newPoints = m_Points.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                //    m_Points = newPoints;
                //    index = newPoints.ToList().FindIndex(t => t.Key == _index);
                //    this.m_Chart.Series[0].Points.Insert(index, point);
                //    IsFirst = false;
                //    _index = _index + 1;
                //}
                if (this.m_Chart.Series[0].Points.Count > 0)
                {
                    this.m_Chart.Series[0].Points.RemoveAt(m_Points.Count);
                }

                m_Points.Add(_index, data);

                newPoints = m_Points.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                m_Points = newPoints;
                index = newPoints.ToList().FindIndex(t => t.Key == _index);
                this.m_Chart.Series[0].Points.Insert(index, point);


                this.m_Chart.Series[0].Points.Add(this.m_Chart.Series[0].Points[0]);
            }

            return index;
        }

        short MaxValue(short[] num)
        {
            short _maxValue = num[0];
            for (short i = 1; i < num.Length; i++)
            {
                if (_maxValue < num[i])
                {
                    _maxValue = num[i];
                }
            }
            return _maxValue;
        }
        short MinValue(short[] num)
        {
            short _minValue = num[0];
            for (short i = 1; i < num.Length; i++)
            {
                if (_minValue > num[i])
                {
                    _minValue = num[i];
                }
            }
            return _minValue;
        }

        void ChangeChartRange(float _maxValue, float _minValue)
        {
            if (this.InvokeRequired)
            {
                if (ScanAsync == null || ScanAsync.IsCompleted)
                    ScanAsync = this.BeginInvoke(new Action<float, float>(ChangeChartRange), _maxValue, _minValue);
            }
            else
            {
                m_Chart.ChartAreas[0].AxisY.Maximum = _maxValue / 10 * 10;
                if (_maxValue % 10 != 0)
                {
                    m_Chart.ChartAreas[0].AxisY.Maximum += 10;
                }

                if (m_Chart.ChartAreas[0].AxisY.Maximum == _minValue)
                {
                    m_Chart.ChartAreas[0].AxisY.Maximum += 10;
                }
            }
        }
        #endregion


        #region 公共方法
        IAsyncResult ScanAsync = null;

        public void FreqChange()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(FreqChange));
            }
            else
            {
                _maxValue = 0;
                _minValue = 100;
                m_Points.Clear();
                this.m_Chart.Series[0].Points.Clear();
            }
        }

        public void InitChart(short _level)
        {
            if (this.InvokeRequired)
            {
                if (ScanAsync == null || ScanAsync.IsCompleted)
                    ScanAsync = this.BeginInvoke(new Action<short>(InitChart), _level);
            }
            else
            {
                m_Chart.Series[0].Points[0].YValues[0] = _level;
                m_Chart.Series[0].Points[0].YValues[120] = _level;
                m_Chart.Series[0].Points[0].YValues[240] = _level;
            }
        }
        float _maxValue = 0;
        float _minValue = 100;
        public void MeasureSetDDF(float _data, int _ddf)
        {
            if (this.InvokeRequired)
            {
                if (ScanAsync == null || ScanAsync.IsCompleted)
                    ScanAsync = this.BeginInvoke(new Action<float, int>(MeasureSetDDF), _data, _ddf);
            }
            else
            {
                if (_maxValue < _data)
                    _maxValue = _data;
                if (_minValue > _data)
                    _minValue = _data;
                int index = FindIndex(_ddf, _data);
                ChangeChartRange(_maxValue, _minValue);
                m_Chart.Series[0].Points[index].YValues[0] = _data;
                m_Chart.Series.Invalidate();
            }
        }
        #endregion


        public void AddPoint(float _data, int _ddf)
        {
            if (this.InvokeRequired)
            {
                if (ScanAsync == null || ScanAsync.IsCompleted)
                    ScanAsync = this.BeginInvoke(new Action<float, int>(AddPoint), _data, _ddf);
            }
            else
            {
                if (_maxValue < _data)
                    _maxValue = _data;
                if (_minValue > _data)
                    _minValue = _data;
                //  int index = FindIndex(_ddf, _data);
                ChangeChartRange(_maxValue, _minValue);
                var points = m_Chart.Series[0].Points.Where(t => (Convert.ToInt32(t.XValue)) == _ddf).ToList();
                if (points.Count > 0)
                {
                    int index = m_Chart.Series[0].Points.ToList().FindIndex(t => Convert.ToInt32(t.XValue) == _ddf);
                    m_Chart.Series[0].Points[index].YValues[0] = _data;
                }
                else
                {
                    m_Chart.Series[0].Points.AddXY(_ddf, _data);
                }
                //m_Chart.Series[0].Points[index].YValues[0] = _data;
                //  m_Chart.Series.Invalidate();
            }
        }

        //public void AddFirstData()
        //{
        //    this.BeginInvoke(new Action(() =>
        //    {
        //        m_Chart.Series[0].Points.Add(m_Chart.Series[0].Points[0]);
        //    }), null, null);
        //}
        private void m_Chart_Click(object sender, EventArgs e)
        {

        }
    }
}
