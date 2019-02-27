using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempLite
{
    public class PDFvariables
    {
        public int recordedSample = 0;

        public bool[] enabledChannels = { false, false, false, false, false, false, false, false };

        public double[] presetUpperLimit = new double[8];
        public double[] presetLowerLimit = new double[8];
        public double[] Mean = new double[8];
        public double[] MKT_C = new double[8];
        public double[] Max = new double[8];
        public double[] Min = new double[8];
        public double[] withinLimits = new double[8];
        public double[] outsideLimits = new double[8];
        public double[] aboveLimits = new double[8];
        public double[] belowLimits = new double[8];
        public double[] Data;

        public String serialNumber = "";
        public String loggerState = "";
        public String batteryPercentage = "";
        public String sameplePeriod = "";
        public String startDelay = "";
        public String firstSample = "";
        public String lastSample = "";
        public String tagsPlaced = "";
        public String tempUnit = "";
        public String userData = "";
        public String[] breachedAbove = { "", "", "", "", "", "", "", "" };
        public String[] breachedBelow = { "", "", "", "", "", "", "", "" };
        public String[] timeWithinLimits = { "", "", "", "", "", "", "", "" };
        public String[] timeOutLimits = { "", "", "", "", "", "", "", "" };
        public String[] timeAboveLimits = { "", "", "", "", "", "", "", "" };
        public String[] timeBelowLimits = { "", "", "", "", "", "", "", "" };

        public ArrayList Time = new ArrayList();

        public String UNIXtoUTCDate(long now)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(now * 1000);
            date = date.ToUniversalTime();
            string simpledate = date.ToString("yyyy-MM-dd");
            return simpledate;
        }

        public String UNIXtoUTCTime(long now)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(now * 1000);
            date = date.ToUniversalTime();
            string simpledate = date.ToString("HH:mm:sss UTC");
            return simpledate;
        }
    }
}
