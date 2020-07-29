using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using DevExpress.Utils;

namespace DevExpress.XtraCharts.Demos {

    static class CsvReader {
        internal static List<FinancialDataPoint> ReadFinancialData(string fileName) {
            string longFileName = string.Empty;
            StreamReader reader;
            var dataSource = new List<FinancialDataPoint>();
            Stream stream = AssemblyHelper.GetEmbeddedResourceStream(typeof(CsvReader).Assembly, fileName, false);
            try {
                reader = new StreamReader(stream);
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    var values = line.Split(',');
                    var point = new FinancialDataPoint();
                    point.DateTimeStamp = DateTime.ParseExact(values[0], "yyyy.MM.dd", null);
                    point.Open = double.Parse(values[1], CultureInfo.InvariantCulture);
                    point.High = double.Parse(values[2], CultureInfo.InvariantCulture);
                    point.Low = double.Parse(values[3], CultureInfo.InvariantCulture);
                    point.Close = double.Parse(values[4], CultureInfo.InvariantCulture);
                    dataSource.Add(point);
                }
            }
            catch {
                throw new Exception("It's impossible to load " + fileName);
            }
            return dataSource;
        }
    }

    public class FinancialDataPoint
    {
        public DateTime DateTimeStamp { get; set; }
        public double Low { get; set; }
        public double High { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public bool IsEmpty { get { return DateTimeStamp.Equals(new DateTime()); } }

        public FinancialDataPoint() { }
        public FinancialDataPoint(DateTime date, double open, double high, double low, double close, double volume)
        {
            this.DateTimeStamp = date;
            this.Low = low;
            this.High = high;
            this.Open = open;
            this.Close = close;
            this.Volume = volume;
        }
    }
}
