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

        public double[] Data;
        public double[] presetUpperLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] presetLowerLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] Mean = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] MKT_C = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] Max = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] Min = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] withinLimits = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] outsideLimits = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] aboveLimits = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] belowLimits = { 0, 0, 0, 0, 0, 0, 0, 0 };

        public String[] breachedAbove = { "", "", "", "", "", "", "", "" };
        public String[] breachedBelow = { "", "", "", "", "", "", "", "" };
        public String[] timeWithinLimits = { "", "", "", "", "", "", "", "" };
        public String[] timeOutLimits = { "", "", "", "", "", "", "", "" };
        public String[] timeAboveLimits = { "", "", "", "", "", "", "", "" };
        public String[] timeBelowLimits = { "", "", "", "", "", "", "", "" };
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
