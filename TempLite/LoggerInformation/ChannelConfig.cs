using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempLite
{
    public class ChannelConfig
    {
        public string SensorName { get; set; }
        public List<double> Data { get; set; }
        public double PresetUpperLimit { get; set; }
        public double PresetLowerLimit { get; set; }
        public double Mean { get; set; }
        public double MKT_C { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
        public double WithinLimits { get; set; }
        public double OutsideLimits { get; set; }
        public double AboveLimits { get; set; }
        public double BelowLimits { get; set; }
        public String BreachedAbove { get; set; }
        public String BreachedBelow { get; set; }
        public String TimeWithinLimits { get; set; }
        public String TimeOutLimits { get; set; }
        public String TimeAboveLimits { get; set; }
        public String TimeBelowLimits { get; set; }
    }
}
