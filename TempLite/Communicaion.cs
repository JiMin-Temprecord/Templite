using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace TempLite
{
    class Communication
    {
        private static SerialPort serial = new SerialPort();

        //private static byte[] wakeup = new byte[80]; //{0x00,0x55};
        private static byte[] sendmsg = new byte[11]; //{0x02,0x40,0x00,0x00,0x4a,0x0c,0x00,0x00};
        public static UInt16 crc16 = 0xFFFF;
        static List<Hex> loggerHex = new List<Hex>();


        private static string msg = "";
        private static string nextadd = "";
        private static string addLO = "";
        private static string addHI = "";

        private static int count = 0;
        private static int byteData = 0;
        private static int loggertype = 0;
        
        public static void ReadLogger()
        {
            List<byte> msgList = sendmsg.ToList();

            sendmsg[4] = 0x4a;
            sendmsg[5] = 0x0c;
            loggerHex.Clear();

            Reader.SetupCom(serial);
            serial.Open();

            if (serial.IsOpen == true)
            {
                Write("wakeup");
                Read("wakeup");

                while (sendmsg[5] <= 31)
                {
                    msgList = sendmsg.ToList();
                    
                    byte[] readbyte = msgList.ToArray();
                    
                    nextadd = sendmsg[5].ToString("x02") + sendmsg[4].ToString("x02");
                    int nextint = hextodec(nextadd);
                    string next = dectohex(nextint + 64);

                    addLO = next[0].ToString() + next[1].ToString();
                    addHI = next[2].ToString() + next[3].ToString();
                    
                    byte[] c = BitConverter.GetBytes(hextodec(addLO));
                    byte[] d = BitConverter.GetBytes(hextodec(addHI));
                    sendmsg[5] = c[0];
                    sendmsg[4] = d[0];
                    
                    Write("readdata");
                    Read("readdata");
                    
                }
            }

            foreach (Hex hexinfo in loggerHex)
            {
                Console.WriteLine(hexinfo);
            }
            serial.Close();
        }


        static void Read(string command)
        {
            byte[] recievemsg = new byte[60];
            int count = 0;

            try
            {

                byteData = serial.ReadByte();

                while (byteData != 13)
                {
                    recievemsg[count] = (byte)byteData;
                    count++;
                    byteData = serial.ReadByte();
                }
            }
            catch (TimeoutException) { }
            
            for(int i = 0; i <count; i++)
                Console.Write((recievemsg[i]).ToString("x02"));

            switch (command)
            {
                case "wakeup":
                    switch (recievemsg[2])
                    {
                        case 3:
                            loggertype = 3;
                            break;

                        case 6:
                            loggertype = 6;
                            break;
                    }
                    break;

                case "readdata":
                    loggerHex.Add(new Hex() { address = nextadd, reply = msg });
                    break;
            }

        }

        static void Write(string command)
        {
            byte[] crc; 

            switch (command)
            {
                case "wakeup":
                    sendmsg[0] = 0X00;
                    sendmsg[1] = 0X55;
                    break;

                case "readdata":

                    switch(loggertype)
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
                            crc = addCRC(sendmsg);
                            sendmsg[8] = crc[0];
                            sendmsg[9] = crc[1];
                            sendmsg[10] = 0x0D;
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
                            crc = addCRC(sendmsg);
                            sendmsg[8] = crc[0];
                            sendmsg[9] = crc[1];
                            sendmsg[10] = 0x0D;
                            break;
                            
                    }

                    break;
            }


            serial.Write(sendmsg, 0, sendmsg.Length);
        }

        private static int hextodec (string hex)
        {
            return (Convert.ToInt32(hex, 16));
        }

        private static string dectohex (int dec)
        {
            return (dec.ToString("x04"));
        }

        public static byte[] addCRC(byte[] b)
        {
            crc16 = 0xFFFF;

                for (int i = 0; i < 8; i++)
                {
                    crc16 = (UInt16)(crc16 ^ (Convert.ToUInt16(b[i]) << 8));
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

            byte[] crcarray = new byte[2];
            crcarray[0] = (byte)crc16;
            crcarray[1] = (byte)(crc16 >> 8);

            return crcarray;
        }

        static private void addEscChar (int Length)
        {
            int mx = 0;
            byte[] sendtemp = new byte[80];

            for (int i = 0; i < Length; i++)
            {
                if (sendmsg[i] == 0x1B)
                {
                    sendtemp[i + mx] = 0x1B;
                    mx++;
                    sendtemp[i + mx] = 0x00;
                }

                else if (sendmsg[i] == 0x0D)
                {
                    sendtemp[i + mx] = 0x1B;
                    mx++;
                    sendtemp[i + mx] = 0x01;

                }
                else if (sendmsg[i] == 0x55)
                {
                    sendtemp[i + mx] = 0x1B;
                    mx++;
                    sendtemp[i + mx] = 0x02;
                }
                else
                {
                    sendtemp[i + mx] = sendmsg[i];
                }
            }

            sendtemp[Length + mx] = 0x0D;
            sendmsg = new byte[80];
            Array.Copy(sendtemp, sendmsg, Length + mx + 1);
        }

        static byte[] removeEscChar (byte[] msg)
        {
            int i = 0;
            int mx = 0;

            while ((i < msg.Length) && (msg[i] != 0x00))
            {
                if(msg[i] == 0x0B)
                {
                     switch (msg[i+1])
                    {
                        case 0x00:
                            msg[mx] = 0x1B;
                            i++;
                            break;

                        case 0x01:
                            msg[mx] = 0x1D;
                            i++;
                            break;

                        case 0x02:
                            msg[mx] = 0x55;
                            i++;
                            break;
                    }
                }
                else
                {
                    msg[mx] = msg[i];
                }

                mx++;
                i++;
            }
            msg[mx] = 0x0D;

            byte[] removeEsc = new byte[mx + 1];
            Array.Copy(msg, removeEsc, mx + 1);

            return removeEsc;
            
        }
        
    }
}
