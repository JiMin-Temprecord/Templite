using System;
using System.Collections;

namespace TempLite
{
    public class PDFvariables
    {
        ArrayList time = new ArrayList();
       
        public bool[] EnabledChannels { get; set; }
        public int RecordedSamples { get; set; }
        public string SerialNumber { get; set; }
        public string LoggerState { get; set; }
        public string BatteryPercentage { get; set; }
        public string SameplePeriod { get; set; }
        public string StartDelay { get; set; }
        public string FirstSample { get; set; }
        public string LastSample { get; set; }
        public string TagsPlaced { get; set; }
        public string TotalTrip { get; set; }
        public string UserData { get; set; }
        public ArrayList Time { get { return time; } set { time = value; } }
        
        ChannelConfig channelOne;
        ChannelConfig channelTwo;
        public ChannelConfig ChannelOne => channelOne ?? (channelOne = new ChannelConfig());
        public ChannelConfig ChannelTwo => channelTwo ?? (channelTwo = new ChannelConfig());
        public Boolean IsChannelTwoEnabled { get; set; }
    }
}
