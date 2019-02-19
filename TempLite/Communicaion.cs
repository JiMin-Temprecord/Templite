using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace TempLite
{
    class Communication
    {
        private static SerialPort serial = new SerialPort();

        private static byte[] sendmsg = new byte[11]; //{0x02,0x40,0x00,0x00,0x4a,0x0c,0x00,0x00};
        private static byte[] recievemsg = new byte[80];
        public static UInt16 crc16 = 0xFFFF;
        static List<Hex> loggerHex = new List<Hex>();

        public static string serialnumber = "";
        public static string loggername = "";
        
        private static int byteData = 0;
        private static int loggertype = 0;

        private static int maxlenreading = 0x40;
        static string jsonfile = "";
        private static int maxmemory = 0;
        private static int memoryheaderpointer;
        private static int[] memstart;
        private static int[] memmax;
        private static int requestmemorystartpointer;
        private static int requestmemorymaxpointer;

        private static int memnumber = 0;
        private static int memoryadd;
        private static byte memoryaddMSB;
        private static byte memoryaddLSB;

        private static int length;
        private static byte lengthMSB;
        private static byte lengthLSB;

        private static string HEXfile;


        //==========================================================//        
        public static void ReadLogger()
        {
            List<byte> msgList = sendmsg.ToList();
            loggerHex.Clear();

            Reader.SetupCom(serial);
            serial.Open();

            if (serial.IsOpen == true)
            {
                Write("wakeup");
                Read("wakeup");

                Write("setread");
                Read("setread");

                while (memnumber < maxmemory)
                {
                    Write("readdata");
                    Read("readdata");
                    getnextaddress();

                }
            }

            StreamWriter sw = new StreamWriter(@serialnumber + ".hex");
            foreach (Hex hexinfo in loggerHex)
            {
                sw.WriteLine(hexinfo.ToString());
                //Console.WriteLine(hexinfo);
            }
            sw.Close();
            serial.Close();
        }
        //==========================================================//


        //=============Recieve Data=================================//
        //==========================================================//
        static void Read(string command)
        {
            string msg = "";
            recievemsg = new byte[80];
            int count = 0;

            length = maxlenreading;

            try
            {
                byteData = serial.ReadByte();

                while (byteData != 13)
                {
                    recievemsg[count] = (byte)byteData;
                    msg = msg + byteData.ToString("x02");
                    count++;
                    byteData = serial.ReadByte();
                }
            }
            catch (TimeoutException) { }
            recievemsg[count] = 0x0d;
            //removeEscChar(recievemsg);
            msg = msg + "0D";
            switch (command)
            {
                case "wakeup":

                    switch (recievemsg[2])
                    {
                        case 3:
                            loggername = "Mon T";
                            loggertype = 3;
                            jsonfile = "MonT.json";
                            maxmemory = 2;                                                 //MON-T
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
                            maxmemory = 4;                                                 //G4
                            memoryheaderpointer = 13;                                                   //G4
                            memstart = new int[] { 0x0000, 0x0020, 0x0000, 0x0000, 0x0000 };    //G4
                            memmax = new int[] { 0x353C, 0x0100, 0x0000, 0x0000, 0x8000 };    //G4
                            requestmemorystartpointer = 3;
                            requestmemorymaxpointer = 1;
                            break;
                    }

                    byte[] serial = { (byte)recievemsg[5], (byte)recievemsg[6], (byte)recievemsg[7], (byte)recievemsg[8] };
                    serialnumber = createJSON.getSerialnumber(serial);

                    memoryadd = (recievemsg[memoryheaderpointer + 1] & 0xFF) << 8 | (recievemsg[memoryheaderpointer] & 0xFF);
                    memoryaddMSB = (byte)recievemsg[memoryheaderpointer + 1];
                    memoryaddLSB = (byte)recievemsg[memoryheaderpointer];

                    string timenow = "0000000000000000";
                    long time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()/1000;
                    string hextime = time.ToString("x02");
                    timenow = timenow.Substring(0, (timenow.Length - hextime.Length)) + hextime;
                    loggerHex.Add(new Hex() { address = ("FF0000"), reply = timenow });

                    msg = msg.Substring(0, msg.Length - 6);
                    loggerHex.Add(new Hex() { address = ("FE0000"), reply = msg });

                    break;

                case "setread":
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

                case "readdata":
                    if (recievemsg[0] == 0x00)
                    {
                        string finalmsg = "";
                        if (memnumber == 0)
                        {
                            finalmsg = msg.Substring(4, msg.Length - 10);
                            loggerHex.Add(new Hex() { address = ("0" + memnumber + memoryaddMSB.ToString("x02") + memoryaddLSB.ToString("x02")), reply = finalmsg });
                        }

                        else
                        {
                            finalmsg = msg.Substring(2, msg.Length - 8);
                            loggerHex.Add(new Hex() { address = ("0" + memnumber + memoryaddMSB.ToString("x02") + memoryaddLSB.ToString("x02")), reply = finalmsg });
                        }
                    }

                    break;
            }

            lengthMSB = (byte)((length >> 8) & 0xFF);
            lengthLSB = (byte)(length & 0xFF);

        }
        //==========================================================//



        //==========================================================//
        static void Write(string command)
        {

            switch (command)
            {
                case "wakeup":
                    sendmsg[0] = 0X00;
                    sendmsg[1] = 0X55;
                    break;

                case "setread":

                    switch (loggertype)
                    {
                        case 3:

                            sendmsg[0] = 0x02;
                            sendmsg[1] = 0x06;
                            sendmsg[2] = 0x00;
                            sendmsg[3] = 0x01;
                            sendmsg[4] = 0x46;
                            sendmsg[5] = 0x00;
                            sendmsg[6] = 0x00;
                            sendmsg[7] = 0x00;
                            addCRC(8);
                            break;

                        case 6:

                            sendmsg[0] = 0x02;
                            sendmsg[1] = 0x06;
                            sendmsg[2] = 0x00;
                            sendmsg[3] = 0x01;
                            sendmsg[4] = 0x63;
                            sendmsg[5] = 0x00;
                            sendmsg[6] = 0x00;
                            sendmsg[7] = 0x00;
                            addCRC(8);
                            break;

                    }

                    break;

                case "readdata":

                    sendmsg[0] = 0x02;
                    sendmsg[1] = lengthLSB;
                    sendmsg[2] = lengthMSB;
                    sendmsg[3] = (byte)memnumber;
                    sendmsg[4] = memoryaddLSB;
                    sendmsg[5] = memoryaddMSB;
                    sendmsg[6] = (byte)0x00;
                    sendmsg[7] = (byte)0x00;
                    addCRC(8);
                    break;

            }

            try
            {
                /*Console.Write("SENT  : ");
                for (int i = 0; i < sendmsg.Length; i++)
                    Console.Write(sendmsg[i].ToString("X02") + "-");

                Console.WriteLine("");*/

                serial.Write(sendmsg, 0, sendmsg.Length);
            }
            catch (System.TimeoutException) { }
        }
        //==========================================================//


        //==========================================================//
        private static int hextodec(string hex)
        {
            return (Convert.ToInt32(hex, 16));
        }
        //==========================================================//


        //==========================================================//
        private static string dectohex(int dec)
        {
            return (dec.ToString("x04"));
        }
        //==========================================================//


        public static void getnextaddress()
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
        private static void addCRC(int len)
        {
            crc16 = 0xFFFF;

            for (int i = 0; i < 8; i++)
            {
                crc16 = (UInt16)(crc16 ^ (Convert.ToUInt16(sendmsg[i]) << 8));
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
            
            sendmsg[len++] = (byte)crc16;
            sendmsg[len++] = (byte)(crc16 >> 8);
            sendmsg[len++] = 0x0d;

            addEscChar(len-1);
        }

        //==========================================================//


        //==========================================================//
        private static void addEscChar(int Length)
        {
            int mx = 0;
            byte[] temp = new byte[80];

            for (int i = 0; i < Length; i++)
            {
                if (sendmsg[i] == 0x1b) // 1B = 27
                {
                    temp[i + mx] = 0x1B; // 1B = 27
                    mx++;
                    temp[i + mx] = 0x00;
                }

                else if (sendmsg[i] == 0x0d) // 1D = 29
                {
                    temp[i + mx] = 0x1B; // 1B = 27 
                    mx++;
                    temp[i + mx] = 0x01;

                }
                else if (sendmsg[i] == 0x55) // 55 = 85
                {
                    temp[i + mx] = 0x1B; // 1B = 27
                    mx++;
                    temp[i + mx] = 0x02;
                }
                else
                {
                    temp[i + mx] = sendmsg[i];
                }
            }

            temp[Length + mx] = 0x0d;
            sendmsg = new byte[Length + mx + 1];
            Array.Copy(temp, sendmsg, Length + mx + 1);
        }
        //==========================================================//



        //==========================================================//
        private static void removeEscChar(byte[] message)
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
            Array.Copy(message, recievemsg,mx + 1);
            /*Console.Write("RECIEVE: ");
            for (int j = 0; j < mx + 1; j++)
                Console.Write(recievemsg[j].ToString("X02") + "-");

            Console.WriteLine("");*/
        }
        //==========================================================//

    }
}
