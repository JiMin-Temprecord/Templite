using System;
using System.Collections;

namespace TempLite
{
    public class PDFvariables
    {
        ArrayList time = new ArrayList();
       
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


        ChannelConfig channelOne;
        public ChannelConfig ChannelOne => channelOne ?? (channelOne = new ChannelConfig());
        ChannelConfig channelTwo;
        public ChannelConfig ChannelTwo => channelTwo ?? (channelTwo = new ChannelConfig());
        public Boolean IsChannelTwoEnabled { get; set; }
    }
}
