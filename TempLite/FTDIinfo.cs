using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempLite
{
    public class FTDIInfo
    {
        public string PortName { get; }
        public uint DeviceID { get; }

        public FTDIInfo (string portName, uint deviceID)
        {
            PortName = portName;
            DeviceID = deviceID;
        }
    }
}
