using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;

namespace TempLite.Services
{
    public class _communicationServices
    {
        private byte[] recievemsg = new byte[80];
        public UInt16 crc16 = 0xFFFF;

        public string serialnumber = "";
        public string loggername = "";

        private int byteData = 0;
        private int loggertype = 0;

        private int maxlenreading = 0x40;
        public string jsonfile = "";
        private int maxmemory = 0;
        private int memoryheaderpointer;
        private int[] memstart;
        private int[] memmax;
        private int requestmemorystartpointer;
        private int requestmemorymaxpointer;

        private int memnumber = 0;
        private int memoryadd;
        private byte memoryaddMSB;
        private byte memoryaddLSB;

        private int length;
        private byte lengthMSB;
        private byte lengthLSB;

        /// <summary>
        /// 
        /// </summary>   
        public void ReadLogger(SerialPort serialPort)
        {
            var hexes = new List<Hex>();
            var sendMessage = new byte[11]; //{0x02,0x40,0x00,0x00,0x4a,0x0c,0x00,0x00};

            serialPort.Open();

            if (serialPort.IsOpen == true)
            {
                WriteBytes(Command.WakeUp, serialPort, sendMessage);
                ReadBytes(Command.WakeUp, serialPort, hexes);

                WriteBytes(Command.SetRead, serialPort, sendMessage);
                ReadBytes(Command.SetRead, serialPort, hexes);

                while (memnumber < maxmemory)
                {
                    WriteBytes(Command.ReadLogger, serialPort, sendMessage);
                    ReadBytes(Command.ReadLogger, serialPort, hexes);
                    getnextaddress();
                }
            }

            var sw = new StreamWriter(@serialnumber + ".hex");
            foreach (var hex in hexes)
            {
                sw.WriteLine(hex.ToString());
                //Console.WriteLine(hexinfo);
            }
            sw.Close();
            serialPort.Close();
        }
        //==========================================================//


        //=============Recieve Data=================================//
        //==========================================================//
        private void ReadBytes(Command command, SerialPort serialPort, List<Hex> hexes)
        {
            string msg = "";
            recievemsg = new byte[80];
            int count = 0;

            length = maxlenreading;

            try
            {
                byteData = serialPort.ReadByte();

                while (byteData != 13)
                {
                    recievemsg[count] = (byte)byteData;
                    msg = msg + byteData.ToString("x02");
                    count++;
                    byteData = serialPort.ReadByte();
                }
            }
            catch (TimeoutException) { }
            recievemsg[count] = 0x0d;
            recievemsg = removeEscChar(recievemsg);
            msg = msg + "0D";
            switch (command)
            {
                case Command.WakeUp:

                    switch (recievemsg[2])
                    {
                        case 3:
                            loggername = "Mon T";
                            loggertype = 3;
                            jsonfile = "MonT.json";
                            maxmemory = 0x02;                                                 //MON-T
                            memoryheaderpointer = 19;                                                   //MON-T
                            memstart = new int[] { 0x0000, 0x0020, 0x0000, 0x0000, 0x0000 };    //MON-T
                            memmax = new int[] { 0x2000, 0x0100, 0x0000, 0x0000, 0x2000 };    //MON-T
                            requestmemorystartpointer = 3;
                            requestmemorymaxpointer = 1;
                            break;

                        case 6:
                            loggername = "G4";
                            loggertype = 6;
                            jsonfile = "G4.json";
                            maxmemory = 0x04;                                                 //G4
                            memoryheaderpointer = 13;                                                   //G4
                            memstart = new int[] { 0x0000, 0x0020, 0x0000, 0x0000, 0x0000 };    //G4
                            memmax = new int[] { 0x353C, 0x0100, 0x0000, 0x0000, 0x8000 };    //G4
                            requestmemorystartpointer = 3;
                            requestmemorymaxpointer = 1;
                            break;
                    }

                    byte[] serial = { (byte)recievemsg[5], (byte)recievemsg[6], (byte)recievemsg[7], (byte)recievemsg[8] };
                    serialnumber = getSerialnumber(serial);

                    memoryadd = (recievemsg[memoryheaderpointer + 1] & 0xFF) << 8 | (recievemsg[memoryheaderpointer] & 0xFF);
                    memoryaddMSB = (byte)recievemsg[memoryheaderpointer + 1];
                    memoryaddLSB = (byte)recievemsg[memoryheaderpointer];

                    string timenow = "0000000000000000";
                    long time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000;
                    string hextime = time.ToString("x02");
                    timenow = timenow.Substring(0, (timenow.Length - hextime.Length)) + hextime;
                    hexes.Add(new Hex() { address = ("FF0000"), reply = timenow });

                    msg = msg.Substring(0, msg.Length - 6);
                    hexes.Add(new Hex() { address = ("FE0000"), reply = msg });

                    break;

                case Command.SetRead:
                    switch (loggertype)
                    {
                        case 3:
                            memstart[0] = 0x0000;
                            memmax[0] = 0x2000;

                            break;

                        case 6:     //G4
                            memstart[4] = (recievemsg[requestmemorystartpointer + 1] & 0xFF) << 8 | (recievemsg[requestmemorystartpointer] & 0xFF);
                            memmax[4] = (recievemsg[requestmemorymaxpointer + 1] & 0xFF) << 8 | (recievemsg[requestmemorymaxpointer] & 0xFF);

                            if (memmax[4] < 80)
                            {
                                memmax[4] = 80;
                            }
                            break;
                    }
                    break;

                case Command.ReadLogger:
                    if (recievemsg[0] == 0x00)
                    {
                        string finalmsg = "";
                        if (memoryaddMSB.ToString("x02") + memoryaddLSB.ToString("x02") == "0c4a")
                        {
                            finalmsg = msg.Substring(4, msg.Length - 10);
                            hexes.Add(new Hex() { address = ("0" + memnumber + memoryaddMSB.ToString("x02") + memoryaddLSB.ToString("x02")), reply = finalmsg });
                        }

                        else
                        {
                            finalmsg = msg.Substring(2, msg.Length - 8);
                            hexes.Add(new Hex() { address = ("0" + memnumber + memoryaddMSB.ToString("x02") + memoryaddLSB.ToString("x02")), reply = finalmsg });
                        }
                    }

                    break;
            }

            lengthMSB = (byte)((length >> 8) & 0xFF);
            lengthLSB = (byte)(length & 0xFF);

        }
        //==========================================================//



        //==========================================================//
        private void WriteBytes(Command command, SerialPort serialPort, byte[] sendMessage)
        {

            switch (command)
            {
                case Command.WakeUp:
                    sendMessage[0] = 0X00;
                    sendMessage[1] = 0X55;
                    break;

                case Command.SetRead:
                    switch (loggertype)
                    {
                        case 3:
                            sendMessage[0] = 0x02;
                            sendMessage[1] = 0x06;
                            sendMessage[2] = 0x00;
                            sendMessage[3] = 0x01;
                            sendMessage[4] = 0x46;
                            sendMessage[5] = 0x00;
                            sendMessage[6] = 0x00;
                            sendMessage[7] = 0x00;
                            sendMessage = AddCRC(8, sendMessage);
                            break;

                        case 6:
                            sendMessage[0] = 0x02;
                            sendMessage[1] = 0x06;
                            sendMessage[2] = 0x00;
                            sendMessage[3] = 0x01;
                            sendMessage[4] = 0x63;
                            sendMessage[5] = 0x00;
                            sendMessage[6] = 0x00;
                            sendMessage[7] = 0x00;
                            sendMessage = AddCRC(8, sendMessage);
                            break;
                        default:
                            break;
                    }
                    break;

                case Command.ReadLogger:
                    sendMessage[0] = 0x02;
                    sendMessage[1] = lengthLSB;
                    sendMessage[2] = lengthMSB;
                    sendMessage[3] = (byte)memnumber;
                    sendMessage[4] = memoryaddLSB;
                    sendMessage[5] = memoryaddMSB;
                    sendMessage[6] = (byte)0x00;
                    sendMessage[7] = (byte)0x00;
                    sendMessage = AddCRC(8, sendMessage);
                    break;
                default:
                    break;
            }
            
            try
            {
                serialPort.Write(sendMessage, 0, sendMessage.Length);
            }
            catch (TimeoutException e) { }
        }

        public void getnextaddress()
        {
            if (length >= (memmax[memnumber] - memoryadd))
            {
                length = memmax[memnumber] - memoryadd;
                memoryadd = memmax[memnumber];
                lengthMSB = (byte)((length >> 8) & 0xFF);
                lengthLSB = (byte)(length & 0xFF);
            }

            if (memoryadd < memmax[memnumber])
            {
                //Inc address
                memoryadd = ((memoryaddMSB & 0xFF) << 8) | (memoryaddLSB & 0xFF);
                memoryadd += length;

                //Check if Max address
                if (memoryadd >= memmax[memnumber])
                {
                    //CALC NEW LEN
                    length = length - memoryadd + memmax[0];
                    memoryaddMSB = (byte)((memmax[memnumber] >> 8) & 0xFF);
                    memoryaddLSB = (byte)(memmax[memnumber] & 0xFF);
                }
                else
                {
                    memoryaddMSB = (byte)((memoryadd >> 8) & 0xFF);
                    memoryaddLSB = (byte)(memoryadd & 0xFF);
                }
            }

            else
            {
                if (memnumber < maxmemory)
                {
                    memnumber++;

                    if (memmax[memnumber] != 0)
                    {
                        if (memmax[memnumber] > memstart[memnumber])
                        {
                            length = maxlenreading;
                            memoryadd = memstart[memnumber];
                            memoryaddMSB = (byte)((memoryadd >> 8) & 0xFF);
                            memoryaddLSB = (byte)(memoryadd & 0xFF);
                        }
                    }

                }
            }
        }

        //==========================================================//
        private byte[] AddCRC(int len, byte[] sendMessage)
        {
            crc16 = 0xFFFF;

            for (int i = 0; i < 8; i++)
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

        //==========================================================//


        //==========================================================//
        private byte[] AddEscChar(int Length, byte[] sendMessage)
        {
            int mx = 0;
            byte[] temp = new byte[80];

            for (int i = 0; i < Length; i++)
            {
                if (sendMessage[i] == 0x1b) // 1B = 27
                {
                    temp[i + mx] = 0x1b; // 1B = 27
                    mx++;
                    temp[i + mx] = 0x00;
                }

                else if (sendMessage[i] == 0x0d) // 1D = 29
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
        //==========================================================//



        //==========================================================//
        private byte[] removeEscChar(byte[] message)
        {
            int i = 0;
            int mx = 0;

            while ((i < message.Length) && (message[i] != 13))
            {
                if (message[i] == 27) // 1B = 27
                {
                    switch (message[i + 1])
                    {
                        case 0:
                            message[mx] = 27; // 1B = 27
                            i++;
                            break;

                        case 1:
                            message[mx] = 29;  // 1D = 29
                            i++;
                            break;

                        case 2:
                            message[mx] = 85; // 55 = 85
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

            recievemsg = new byte[mx + 1];
            Array.Copy(message, recievemsg, mx + 1);
            return recievemsg;
        }
        //==========================================================//

        public string getSerialnumber(byte[] msg)
        {
            serialnumber = "";

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
