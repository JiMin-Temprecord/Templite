using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;

namespace TempLite.Services
{
    public class CommunicationServices
    {
        int length = 0;
        int maxlenreading = 0x40;
        byte[] recievemsg;

        public void FindLogger(SerialPort serialPort)
        {
            var msg = new StringBuilder();

            if (serialPort.IsOpen == false)
                serialPort.Open();

            while (msg.Length < 3)
            {
                WriteBytes(new WakeUpByteWritter(), serialPort);
                msg = ReadBytes(serialPort);
                System.Threading.Thread.Sleep(1000);
            }
        }
        public void GenerateHexFile(SerialPort serialPort, LoggerInformation loggerInformation)
        {
            var Hexes = new List<Hex>();
            Hexes = ReadLogger(serialPort, loggerInformation, Hexes);

            var sw = new StreamWriter(loggerInformation.SerialNumber + ".hex");
            foreach (var hex in Hexes)
            {
                sw.WriteLine(hex.ToString());
            }
            sw.Close();
        }
        List<Hex> ReadLogger(SerialPort serialPort, LoggerInformation loggerInformation, List<Hex> Hexes)
        {
            if (serialPort.IsOpen == false)
                serialPort.Open();

            WriteBytes(new WakeUpByteWritter(), serialPort);
            var currentAddress = ReadBytesWakeUp(serialPort, loggerInformation, recievemsg, Hexes);

            WriteBytes(new SetReadByteWritter(loggerInformation.LoggerType), serialPort);
            ReadBytesSetRead(serialPort, currentAddress, loggerInformation);
            
            while (currentAddress.MemoryNumber < loggerInformation.MaxMemory)
            {
                WriteBytes(new ReadLoggerByteWritter(currentAddress), serialPort);
                ReadBytesReadLogger(serialPort, currentAddress, Hexes);
                currentAddress = GetNextAddress(currentAddress, loggerInformation);
            }

            serialPort.Close();
            return Hexes;

        }
        StringBuilder ReadBytes(SerialPort serialPort)
        {
            var msg = new StringBuilder();
            recievemsg = new byte[80];
            var count = 0;

            try
            {
                var byteData = serialPort.ReadByte();

                while (byteData != 0x0d)
                {
                    recievemsg[count] = (byte)byteData;
                    count++;
                    byteData = serialPort.ReadByte();
                }
            }
            catch (TimeoutException e) { }
            recievemsg[count] = 0x0d;
            recievemsg = RemoveEscChar(recievemsg);

            for (int i = 0; i < recievemsg.Length; i++)
            {
                msg = msg.Append(recievemsg[i].ToString("x02"));
            }
            return msg;

        }
        AddressSection ReadBytesWakeUp(SerialPort serialPort, LoggerInformation loggerInformation, byte[] messageReceived, List<Hex> hexes)
        {
            SetLoggerInformation(messageReceived, loggerInformation);
            return SetCurrentAddress(serialPort, loggerInformation, hexes);
        }
        void SetLoggerInformation(byte[] messageReceived, LoggerInformation loggerInformation)
        {
            byte[] serial = { (byte)messageReceived[5], (byte)messageReceived[6], (byte)messageReceived[7], (byte)messageReceived[8] };
            loggerInformation.SerialNumber = GetSerialnumber(serial);

            switch (messageReceived[2])
            {
                case 3:
                    loggerInformation.LoggerName = "Mon T";
                    loggerInformation.LoggerType = 3;
                    loggerInformation.JsonFile = "MonT.json";
                    loggerInformation.MaxMemory = 0x02;                                                 //MON-T
                    loggerInformation.MemoryHeaderPointer = 19;                                                   //MON-T
                    loggerInformation.MemoryStart = new int[] { 0x0000, 0x0020, 0x0000, 0x0000, 0x0000 };    //MON-T
                    loggerInformation.MemoryMax = new int[] { 0x2000, 0x0100, 0x0000, 0x0000, 0x2000 };    //MON-T
                    loggerInformation.RequestMemoryStartPointer = 3;
                    loggerInformation.RequestMemoryMaxPointer = 1;
                    break;

                case 6:
                    loggerInformation.LoggerName = "G4";
                    loggerInformation.LoggerType = 6;
                    loggerInformation.JsonFile = "G4.json";
                    loggerInformation.MaxMemory = 0x04;                                                 //G4
                    loggerInformation.MemoryHeaderPointer = 13;                                                   //G4
                    loggerInformation.MemoryStart = new int[] { 0x0000, 0x0020, 0x0000, 0x0000, 0x0000 };    //G4
                    loggerInformation.MemoryMax = new int[] { 0x353C, 0x0100, 0x0000, 0x0000, 0x8000 };    //G4
                    loggerInformation.RequestMemoryStartPointer = 3;
                    loggerInformation.RequestMemoryMaxPointer = 1;
                    break;
            }
        }
        AddressSection SetCurrentAddress(SerialPort serialPort, LoggerInformation loggerInformation, List<Hex> hexes)
        {
            length = maxlenreading;
            var msg = ReadBytes(serialPort);

            var timenow = "0000000000000000";
            var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000;
            var hextime = time.ToString("x02");
            timenow = timenow.Substring(0, (timenow.Length - hextime.Length)) + hextime;
            hexes.Add(new Hex("FF0000", timenow));
            hexes.Add(new Hex("FE0000", msg.ToString(0, (msg.Length - 6))));

            var memoryAddress = (recievemsg[loggerInformation.MemoryHeaderPointer + 1] & 0xFF) << 8 | (recievemsg[loggerInformation.MemoryHeaderPointer] & 0xFF);
            var memoryAddMSB = (byte)recievemsg[loggerInformation.MemoryHeaderPointer + 1];
            var memoryAddLSB = (byte)recievemsg[loggerInformation.MemoryHeaderPointer];
            var lengthMSB = (byte)((length >> 8) & 0xFF);
            var lengthLSB = (byte)(length & 0xFF);

            return new AddressSection(lengthLSB, lengthMSB, 0, memoryAddLSB, memoryAddMSB, memoryAddress);
        }
        void ReadBytesSetRead(SerialPort serialPort, AddressSection currentAddress, LoggerInformation loggerInformation)
        {
            ReadBytes(serialPort);

            switch (loggerInformation.LoggerType)
            {
                //MonT
                case 3:
                    loggerInformation.MemoryStart[0] = 0x0000;
                    loggerInformation.MemoryMax[0] = 0x2000;
                    break;

                case 6:
                    loggerInformation.MemoryStart[4] = (recievemsg[loggerInformation.RequestMemoryStartPointer + 1] & 0xFF) << 8 | (recievemsg[loggerInformation.RequestMemoryStartPointer] & 0xFF);
                    loggerInformation.MemoryMax[4] = (recievemsg[loggerInformation.RequestMemoryMaxPointer + 1] & 0xFF) << 8 | (recievemsg[loggerInformation.RequestMemoryMaxPointer] & 0xFF);

                    if (loggerInformation.MemoryMax[4] < 80)
                    {
                        loggerInformation.MemoryMax[4] = 80;
                    }
                    break;
            }
        }
        void ReadBytesReadLogger(SerialPort serialPort, AddressSection currentAddress, List<Hex> Hexes)
        {
            var msg = ReadBytes(serialPort);
            var addressRead = "0" + currentAddress.MemoryNumber + currentAddress.MemoryAddMSB.ToString("x02") + currentAddress.MemoryAddLSB.ToString("x02");
            
            if (recievemsg[0] == 0x00)
            {
                var finalmsg = string.Empty;
                if (addressRead == "000c4a")
                {
                    finalmsg = msg.ToString(2, msg.Length - 8);
                    Hexes.Add(new Hex(addressRead, finalmsg));
                }

                else
                {
                    finalmsg = msg.ToString(2, msg.Length - 8);
                    Hexes.Add(new Hex(addressRead, finalmsg));
                }
            }

            currentAddress.LengthMSB = (byte)((length >> 8) & 0xFF);
            currentAddress.LengthLSB = (byte)(length & 0xFF);

        }
        void WriteBytes(IByteWriter byteWriter, SerialPort serialPort)
        {
            var sendMessage = new byte[11];
            sendMessage = byteWriter.WriteBytes(sendMessage);
            try
            {
                serialPort.Write(sendMessage, 0, sendMessage.Length);
            }
            catch (TimeoutException e)
            {
            }
        }
        AddressSection GetNextAddress(AddressSection currentAddress, LoggerInformation loggerInformation)
        {
            if (length >= (loggerInformation.MemoryMax[currentAddress.MemoryNumber] - currentAddress.MemoryAddress))
            {
                length = loggerInformation.MemoryMax[currentAddress.MemoryNumber] - currentAddress.MemoryAddress;
                currentAddress.MemoryAddress = loggerInformation.MemoryMax[currentAddress.MemoryNumber];
                currentAddress.LengthMSB = (byte)((length >> 8) & 0xFF);
                currentAddress.LengthLSB = (byte)(length & 0xFF);
            }

            if (currentAddress.MemoryAddress < loggerInformation.MemoryMax[currentAddress.MemoryNumber])
            {
                currentAddress.MemoryAddress = ((currentAddress.MemoryAddMSB & 0xFF) << 8) | (currentAddress.MemoryAddLSB & 0xFF);
                currentAddress.MemoryAddress += length;

                if (currentAddress.MemoryAddress >= loggerInformation.MemoryMax[currentAddress.MemoryNumber])
                {
                    length = length - currentAddress.MemoryAddress + loggerInformation.MemoryMax[0];
                    currentAddress.MemoryAddMSB = (byte)((loggerInformation.MemoryMax[currentAddress.MemoryNumber] >> 8) & 0xFF);
                    currentAddress.MemoryAddLSB = (byte)(loggerInformation.MemoryMax[currentAddress.MemoryNumber] & 0xFF);
                }
                else
                {
                    currentAddress.MemoryAddMSB = (byte)((currentAddress.MemoryAddress >> 8) & 0xFF);
                    currentAddress.MemoryAddLSB = (byte)(currentAddress.MemoryAddress & 0xFF);
                }
            }
            else
            {
                if (currentAddress.MemoryNumber < loggerInformation.MaxMemory)
                {
                    currentAddress.MemoryNumber++;

                    if (loggerInformation.MemoryMax[currentAddress.MemoryNumber] != 0)
                    {
                        if (loggerInformation.MemoryMax[currentAddress.MemoryNumber] > loggerInformation.MemoryStart[currentAddress.MemoryNumber])
                        {
                            length = maxlenreading;
                            currentAddress.MemoryAddress = loggerInformation.MemoryStart[currentAddress.MemoryNumber];
                            currentAddress.MemoryAddMSB = (byte)((currentAddress.MemoryAddress >> 8) & 0xFF);
                            currentAddress.MemoryAddLSB = (byte)(currentAddress.MemoryAddress & 0xFF);
                        }
                    }

                }
            }
            return currentAddress;
        }

        #region Byte Mainpulation 

        public static byte[] AddCRC(int len, byte[] sendMessage)
        {
            var crc16 = 0xFFFF;

            for (int i = 0; i < len; i++)
            {
                crc16 = (UInt16)(crc16 ^ (Convert.ToUInt16(sendMessage[i]) << 8));
                for (int j = 0; j < 8; j++)
                {
                    if ((crc16 & 0x8000) == 0x8000)
                    {
                        crc16 = (UInt16)((crc16 << 1) ^ 0x1021);
                    }
                    else
                    {
                        crc16 <<= 1;
                    }
                }
            }

            sendMessage[len++] = (byte)crc16;
            sendMessage[len++] = (byte)(crc16 >> 8);
            sendMessage[len++] = 0x0d;
            return AddEscChar(len - 1, sendMessage);
        }
        static byte[] AddEscChar(int Length, byte[] sendMessage)
        {
            int mx = 0;
            byte[] temp = new byte[80];

            for (int i = 0; i < Length; i++)
            {
                if (sendMessage[i] == 0x1B) // 1B = 27
                {
                    temp[i + mx] = 0x1b; // 1B = 27
                    mx++;
                    temp[i + mx] = 0x00;
                }

                else if (sendMessage[i] == 0x0D) // 1D = 29
                {
                    temp[i + mx] = 0x1b; // 1B = 27 
                    mx++;
                    temp[i + mx] = 0x01;

                }
                else if (sendMessage[i] == 0x55) // 55 = 85
                {
                    temp[i + mx] = 0x1b; // 1B = 27
                    mx++;
                    temp[i + mx] = 0x02;
                }
                else
                {
                    temp[i + mx] = sendMessage[i];
                }
            }

            temp[Length + mx] = 0x0d;
            sendMessage = new byte[Length + mx + 1];
            Array.Copy(temp, sendMessage, Length + mx + 1);
            
            return sendMessage;
        }
        byte[] RemoveEscChar(byte[] message)
        {
            int i = 0;
            int mx = 0;

            while ((i < message.Length) && (message[i] != 0x0d))
            {
                if (message[i] == 0x1B) // 1B = 27
                {
                    switch (message[i + 1])
                    {
                        case 0x00:
                            message[mx] = 0x1B; // 1B = 27
                            i++;
                            break;

                        case 0x01:
                            message[mx] = 0x0D;  // 1D = 29
                            i++;
                            break;

                        case 0x02:
                            message[mx] = 0x55; // 55 = 85
                            i++;
                            break;
                    }
                }
                else
                {
                    message[mx] = message[i];
                }

                mx++;
                i++;
            }
            message[mx] = 0x0d;

            var temp = new byte[mx + 1];
            Array.Copy(message, temp, mx + 1);

            return temp;
        }
        #endregion

        string GetSerialnumber(byte[] msg)
        {
            var serialnumber = "";

            if ((msg[3] & 0xF0) == 0x50)
            {
                serialnumber = "L";

                switch (msg[3] & 0x0F)
                {
                    case 0x00:
                        serialnumber += "0";
                        break;
                    case 0x07:
                        serialnumber += "T";
                        break;
                    case 0x08:
                        serialnumber += "G";
                        break;
                    case 0x09:
                        serialnumber += "H";
                        break;
                    case 0x0A:
                        serialnumber += "P";
                        break;
                    case 0x0C:
                        serialnumber += "M";
                        break;
                    case 0x0D:
                        serialnumber += "S";
                        break;
                    case 0x0E:
                        serialnumber += "X";
                        break;
                    case 0x0F:
                        serialnumber += "C";
                        break;
                    default:
                        serialnumber = "L-------";
                        break;
                }
            }
            else if ((msg[3] & 0xF0) == 0x60)//For MonT
            {
                serialnumber = "R0";
            }
            else
            {
                serialnumber = "--------";
            }

            var number = (((msg[2] & 0xFF) << 16) | ((msg[1] & 0xFF) << 8) | (msg[0]) & 0xFF).ToString();
            while (number.Length != 6)
                number = "0" + number;
            serialnumber += number.ToString();

            return serialnumber;
        }


    }
}
