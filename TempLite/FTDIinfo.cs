namespace TempLite
{
    public class FTDIInfo
    {
        public FTDIInfo (string portName, uint deviceID)
        {
            PortName = portName;
            DeviceID = deviceID;
        }

        public string PortName { get; }
        public uint DeviceID { get; }

    }
}
