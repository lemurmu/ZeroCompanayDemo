using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordTestConsol
{
    public class FrequencyBandTestData
    {
        public DateTime StartTime { get; set; } = new DateTime(2017, 1, 1);
        public DateTime StopTime { get; set; } = new DateTime(2017, 12, 12);
        public double Freq { get; set; }//频率
        public double Bandwidth { get; set; }//带宽
        public double Directional { get; set; }//定向


    }
}
