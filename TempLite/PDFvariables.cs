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
        ArrayList time = new ArrayList();
        ChannelConfig channelOne;
        ChannelConfig channelTwo;

        public bool[] EnabledChannels { get; set; }
        public int RecordedSamples { get; set; }
        public String SerialNumber { get; set; }
        public String LoggerState { get; set; }
        public String BatteryPercentage { get; set; }
        public String SameplePeriod { get; set; }
        public String StartDelay { get; set; }
        public String FirstSample { get; set; }
        public String LastSample { get; set; }
        public String TagsPlaced { get; set; }
        public String TempUnit { get; set; }
        public String UserData { get; set; }
        public ArrayList Time { get { return time; } set { time = value; } }

        public ChannelConfig ChannelOne
        {
            get
            {
                if (channelOne == null)
                {
                    channelOne = new ChannelConfig();
                }
                return channelOne;
            }
        }

        public ChannelConfig ChannelTwo
        {
            get
            {
                if (channelTwo == null)
                {
                    channelTwo = new ChannelConfig();
                }
                return channelTwo;
            }
        }
        public Boolean IsChannelTwoEnabled { get; set; }
    }
}
