﻿using FTD2XX_NET;
using System.IO.Ports;

namespace TempLite
{
    public class Reader
    {
        public void SetUpCom(SerialPort serial, FTDIInfo ftdiInfo)
        {

            serial.DiscardNull = false;
            serial.PortName = ftdiInfo.PortName;
            serial.BaudRate = 19200;
            serial.Parity = Parity.None;
            serial.DataBits = 8;
            serial.StopBits = StopBits.One;
            serial.Handshake = Handshake.None;

            serial.ReadTimeout = 100;
            serial.WriteTimeout = 100;
        }

        public FTDIInfo FindFTDI()
        {
            try
            {
                var ft = new FTDI();

                uint deviceCount = 0;
                uint deviceID = 0;

                var stat = ft.GetNumberOfDevices(ref deviceCount);
                FTDI.FT_DEVICE_INFO_NODE[] devices = new FTDI.FT_DEVICE_INFO_NODE[deviceCount];
                stat = ft.GetDeviceList(devices);

                foreach (var dev in devices)
                {
                    try
                    {
                        stat = ft.OpenByLocation(dev.LocId);
                        if (stat == FTDI.FT_STATUS.FT_OK)
                        {
                            ft.GetCOMPort(out var portName);
                            ft.GetDeviceID(ref deviceID);
                            ft.Close();

                            return new FTDIInfo(portName, deviceID);
                        }
                    }
                    catch
                    {
                        try
                        {
                            if (ft.IsOpen)
                            {
                                ft.Close();
                            }
                        } 
                        finally
                        {
                            if(ft.IsOpen)
                            {
                                ft.Close();
                            }
                        }
                    }
                }
            }
            catch { }

            return null;
        }
    }
}
