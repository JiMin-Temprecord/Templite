using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TempLite
{
    public class createJSON
    {
        private class DEVICE
        {
            public  SENSOR sensor { get; set; }
            public STATISTICS stat { get; set; }
        }

        private class SENSOR
        {
            public List<sensor> sensor { get; set; }
        }

        private class STATISTICS
        {
            public List<stat> stat { get; set; }
        }

        private class stat
        {
            public string Min{ get; set; }
            public string Max { get; set; }
            public string LowestSamplePosition { get; set; }
            public string HighestSamplePosition { get; set; }
            public string WithinLimit { get; set; }
            public string OutsideLimit { get; set; }
            public string BelowLimit { get; set; }
            public string AboveLimit { get; set; }
            public string Mean { get; set; }
            public string MKT_K { get; set; }
            public string MKT_C { get; set; }
        }

        private class sensor
        {
            public List<limits> limits { get; set; }
            public string FirstSampleAt { get; set; }
            public string SamplingPeriod { get; set; }
            public string SensorType { get; set; }
            public string SensorResolution { get; set; }
            public string SensorMinimum { get; set; }
            public string[] values { get; set; }
            public string SensorNumber { get; set; }
            public string FirstSampleAtUNIX { get; set; }
        }

        private class limits
        {
            public string LowerLimit { get; set; }
            public string UpperLimit { get; set; }
        }

    }
}
