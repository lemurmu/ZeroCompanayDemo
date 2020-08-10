using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Demos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace FrequencyChart
{
    public partial class FrmMain : DevExpress.XtraEditors.XtraForm
    {
        public FrmMain()
        {
            InitializeComponent();
            // Init();
            this.Controls.Clear();
            this.Controls.Add(new UIFrequecyChart { Parent = this, Dock = DockStyle.Fill });
        }


        void Init()
        {
            using (Stream stream = ReadFile("Data\\sound.bin"))
            {
                Debug.Assert(stream.Length < int.MaxValue);
                int streamLength = (int)stream.Length;
                byte[] buffer = new byte[streamLength];
                stream.Read(buffer, 0, streamLength);
                int halfStreamLength = (int)(stream.Length / 2);                   //Avoid VB issues
                short[] sampleBuffer = new short[halfStreamLength];
                System.Buffer.BlockCopy(buffer, 0, sampleBuffer, 0, streamLength); //do not delete System - avoid VB issues
                int halfBufferLength = (int)(sampleBuffer.Length / 2);             //Avoid VB issues
                //SeriesPoint[] leftChannelPoints = new SeriesPoint[halfBufferLength];
                //SeriesPoint[] rightChannelPoints = new SeriesPoint[halfBufferLength];
                this.averageChannelNormalized = new double[halfBufferLength];
                for (int i = 1, k = 0; i < sampleBuffer.Length; i += 2, k++)
                {
                    double seconds = (i / 2) * (1.0 / SamplingFrequency);
                    double normalizedValueOfLeftChannel = sampleBuffer[i] / sixteenBitSampleMaxVale;
                    double normalizedValueOfRightChannel = sampleBuffer[i - 1] / sixteenBitSampleMaxVale;
                    //leftChannelPoints[k] = new SeriesPoint(TimeSpan.FromSeconds(seconds), normalizedValueOfLeftChannel);
                    //rightChannelPoints[k] = new SeriesPoint(TimeSpan.FromSeconds(seconds), normalizedValueOfRightChannel);
                    this.averageChannelNormalized[k] = (normalizedValueOfLeftChannel + normalizedValueOfRightChannel) / 2d;
                }
                //  Series leftChannelSeries = this.chart.Series[0];
                //  leftChannelSeries.Points.AddRange(leftChannelPoints);
                //  Series rightChannelSeries = this.chart.Series[1];
                //   rightChannelSeries.Points.AddRange(rightChannelPoints);
                realSpectrum = new double[DefaultFrameLength];
                imaginarySpectrum = new double[DefaultFrameLength];
                zeroSpectrum = new double[DefaultFrameLength];
            }
            int halfOfFrame = DefaultFrameLength / 2;
            double frequencyStep = SamplingFrequency / 2d / halfOfFrame;
            SeriesPoint[] frequencyPoints = new SeriesPoint[halfOfFrame];
            for (int i = 0; i < halfOfFrame; i++)
                frequencyPoints[i] = new SeriesPoint(frequencyStep * i, 0);
            Series frequencySpectrumSeries = this.chart.Series[0];
            frequencySpectrumSeries.Points.AddRange(frequencyPoints);
            this.timer.Interval = 10;
            this.timer.Tick += Timer_Tick;
            this.timer.Start();
            this.last = DateTime.Now;
        }


        const int SamplingFrequency = 22050; //Hz
        const int DefaultFrameLength = 2048;
        const double sixteenBitSampleMaxVale = short.MaxValue;
        const double MinDb = -500d;

        int frameStartIndex = 0;
        int frameEndIndex = DefaultFrameLength - 1;
        Timer timer = new Timer();
        double[] averageChannelNormalized;
        double[] realSpectrum;
        double[] imaginarySpectrum;
        double[] zeroSpectrum;
        DateTime last;

        void Timer_Tick(object sender, EventArgs e)
        {
            DateTime current = DateTime.Now;
            double span = (current - this.last).TotalSeconds;
            this.last = current;
            MoveFrameAndStrip((int)(span * SamplingFrequency));
            RecalculateFrequencySpectrum();

        }
        void MoveFrameAndStrip(int offset)
        {
            int newEndIndex = this.frameEndIndex + offset;
            if (newEndIndex < this.averageChannelNormalized.Length)
            {
                this.frameStartIndex += offset;
                this.frameEndIndex = newEndIndex;
            }
            else
            {
                this.frameStartIndex = 0;
                this.frameEndIndex = DefaultFrameLength;
            }
            Strip currentFrameStrip = ((SwiftPlotDiagram)this.chart.Diagram).AxisX.Strips[0]; //test
            currentFrameStrip.MinLimit.AxisValue = TimeSpan.MinValue;
            currentFrameStrip.MaxLimit.AxisValue = TimeSpan.MaxValue; //avoiding errors on frame jumping to begin
            currentFrameStrip.MinLimit.AxisValue = TimeSpan.FromSeconds(1d / SamplingFrequency * this.frameStartIndex);
            currentFrameStrip.MaxLimit.AxisValue = TimeSpan.FromSeconds(1d / SamplingFrequency * this.frameEndIndex);
        }
        void RecalculateFrequencySpectrum()
        {
            Array.Copy(this.averageChannelNormalized, this.frameStartIndex, this.realSpectrum, 0, DefaultFrameLength);
            Array.Copy(this.zeroSpectrum, 0, this.imaginarySpectrum, 0, DefaultFrameLength);
            FastFourierTransformation.Transform(this.realSpectrum, this.imaginarySpectrum);
            Series frequencySpectrumSeries = this.chart.Series[0];
            SeriesPoint[] newPoints = new SeriesPoint[DefaultFrameLength / 2];
            for (int i = 0; i < DefaultFrameLength / 2; i++)
            {
                double magnitude = Math.Sqrt(this.realSpectrum[i] * this.realSpectrum[i] + this.imaginarySpectrum[i] * this.imaginarySpectrum[i]);
                double magnitudeDB = magnitude != 0 ? 20d * Math.Log10(magnitude) : MinDb;
                newPoints[i] = new SeriesPoint(frequencySpectrumSeries.Points[i].Argument, magnitudeDB);
            }
            frequencySpectrumSeries.Points.Clear();
            frequencySpectrumSeries.Points.AddRange(newPoints);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                    this.components.Dispose();
                if (this.timer != null)
                {
                    this.timer.Stop();
                    this.timer.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        static class FastFourierTransformation
        {
            public static void Transform(double[] real, double[] imaginary)
            {
                double powerOf2Double = Math.Log(real.Length, 2);
                Debug.Assert(powerOf2Double == Math.Floor(powerOf2Double));
                int powerOf2 = (int)powerOf2Double;
                int frameLength = real.Length;
                int j = 0;
                for (int i = 0; i < frameLength - 1; i++)
                {
                    if (i < j)
                    {
                        double tempReal = real[i];
                        double tempImaginary = imaginary[i];
                        real[i] = real[j];
                        imaginary[i] = imaginary[j];
                        real[j] = tempReal;
                        imaginary[j] = tempImaginary;
                    }
                    int k = frameLength / 2;
                    while (k <= j)
                    {
                        j -= k;
                        k = k / 2;
                    }
                    j += k;
                }
                double c1 = -1d;
                double c2 = 0d;
                int currentPowerOf2 = 1;
                for (int l = 0; l < powerOf2; l++)
                {
                    int previousPowerOf2 = currentPowerOf2;
                    currentPowerOf2 *= 2;
                    double u1 = 1.0;
                    double u2 = 0.0;
                    for (j = 0; j < previousPowerOf2; j++)
                    {
                        for (int i = j; i < frameLength; i += currentPowerOf2)
                        {
                            int i1 = i + previousPowerOf2;
                            double t1 = u1 * real[i1] - u2 * imaginary[i1];
                            double t2 = u1 * imaginary[i1] + u2 * real[i1];
                            real[i1] = real[i] - t1;
                            imaginary[i1] = imaginary[i] - t2;
                            real[i] += t1;
                            imaginary[i] += t2;
                        }
                        double z = u1 * c1 - u2 * c2;
                        u2 = u1 * c2 + u2 * c1;
                        u1 = z;
                    }
                    c2 = -Math.Sqrt((1d - c1) / 2d);
                    c1 = Math.Sqrt((1d + c1) / 2d);
                }
                for (int i = 0; i < frameLength; i++)
                {
                    real[i] /= frameLength;
                    imaginary[i] /= frameLength;
                }
            }
        }


        public Stream ReadFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            //  UInt32 imageSize = (UInt32)fs.Length;
            //  BinaryReader br = new BinaryReader(fs);
            return fs;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }
    }
}
