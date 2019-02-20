using System;
using System.IO.Ports;
using FTD2XX_NET;

namespace TempLite
{
    public class Reader
    {
        private string portName;
        private uint devID = 0;

        //This ONE
        public void SetupCom(SerialPort serial)
        {
            FindFTDI();

            serial.DiscardNull = false;
            serial.PortName = portName;
            serial.BaudRate = 19200;
            serial.Parity = Parity.None;
            serial.DataBits = 8;
            serial.StopBits = StopBits.One;
            serial.Handshake = Handshake.None;

            serial.ReadTimeout = 16;
            serial.WriteTimeout = 16;
        }

        private void FindFTDI()
        {
            try
            {
                FTDI.FT_STATUS stat;
                FTDI ft = new FTDI();

                uint deviceCount = 0;
                stat = ft.GetNumberOfDevices(ref deviceCount);
                FTDI.FT_DEVICE_INFO_NODE[] devices = new FTDI.FT_DEVICE_INFO_NODE[deviceCount];
                stat = ft.GetDeviceList(devices);

                foreach (FTDI.FT_DEVICE_INFO_NODE dev in devices)
                {
                    try
                    {
                        stat = ft.OpenByLocation(dev.LocId);
                        if (stat == FTDI.FT_STATUS.FT_OK)
                        {
                            ft.GetCOMPort(out portName);
                            ft.GetDeviceID(ref devID);
                            ft.Close();
                        }
                    }
                    catch
                    {
                        try { if (ft.IsOpen) { ft.Close(); } } catch { }
                    }
                }
            }
            catch { }
        }

        public string PortName { get { return portName; } set { portName = value; } }
    }
}
