using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TempLite
{
    class decodeHEX
    {
        static string fromreader = "";
        static string addtoread = "";
        static byte[] decodebyte;
        static bool bitbool = false;

        static long secondtimer = 0;
        static long UTCreference = 0;

        //stringarrayinfo from JSON file [add,len,code,hide]
        public static string decodehex(string[] stringarrayinfo)
        {
            //returned byte[] from the hex file
            fromreader = "";
            decodebyte = ReadHex(stringarrayinfo);
            switch (stringarrayinfo[2])
            {

                case "_1_Byte_to_Decimal":
                    if (decodebyte.Length > 1)
                    {
                        for (int i = 0; i < decodebyte.Length; i++)
                            decodebyte[i] = (byte)(decodebyte[i] & 0xff);

                        if (stringarrayinfo[3] == "0")
                            fromreader = bigEndian();
                    }
                    else
                    {
                        decodebyte[0] = (byte)(decodebyte[0] & 0xff);
                        fromreader = bigEndian();
                    }
                    break;

                case "_2_Byte_to_Decimal":
                    fromreader = decodebyte[1].ToString("X02") + decodebyte[0].ToString("X02");
                    break;

                case "_2_Byte_to_Temperature_MonT":
                    int value = (((decodebyte[1]) & 0xFF) << 8) | (decodebyte[0] & 0xFF);
                    value -= 4000;
                    double V = ((double)value / 100);
                    fromreader = V.ToString("F");
                    break;

                case "_4_Byte_to_Decimal":
                    fromreader = decodebyte[3].ToString("X02") + decodebyte[2].ToString("X02") + decodebyte[1].ToString("X02") + decodebyte[0].ToString("X02");
                    break;

                case "_4_Byte_Sec_to_Date":
                    long _4sectobyte = (decodebyte[3] + decodebyte[2] + decodebyte[1] + decodebyte[0]) * 1000;

                    if (_4sectobyte > 0)
                    {
                        _4sectobyte += 946684800000L;
                        DateTime date = new DateTime(_4sectobyte);
                        fromreader = date.ToString("dd-MM-yyyy HH:mm:sss");
                    }
                    else
                        fromreader = "";
                    break;

                case "_8_Byte_to_Unix_UTC":
                    string _8btyetounix = bigEndian();
                    fromreader = _8btyetounix.ToString();
                    break;

                case "b0":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 0) != 0)
                        bitbool = true;
                    fromreader = bitbool.ToString();
                    break;

                case "b1":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 1) != 0)
                        bitbool = true;
                    fromreader = bitbool.ToString();
                    break;

                case "b2":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 2) != 0)
                        bitbool = true;
                    fromreader = bitbool.ToString();
                    break;

                case "b3":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 3) != 0)
                        bitbool = true;
                    fromreader = bitbool.ToString();
                    break;

                case "b4":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 4) != 0)
                        bitbool = true;
                    fromreader = bitbool.ToString();
                    break;

                case "b5":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 5) != 0)
                        bitbool = true;
                    fromreader = bitbool.ToString();
                    break;

                case "b6":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 6) != 0)
                        bitbool = true;
                    fromreader = bitbool.ToString();
                    break;

                case "b7":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 7) != 0)
                        bitbool = true;
                    fromreader = bitbool.ToString();
                    break;
                

                case "Channel_1_MonT":
                    fromreader = "1";
                    break;

                case "Decode_MonT_Data":
                    break;

                case "DJNZ_2_Byte_Type_1":
                    decodebyte[1]--;
                    fromreader = HHMMSS(decodebyte[1] + decodebyte[0]) + " (hh:mm:ss)";

                    break;

                case "DJNZ_4_Byte_Type_2":
                    for (int i =0; i< 4; i++)
                    {
                        int z = (0x100) - (decodebyte[i] & 0xFF);
                        decodebyte[i] = (byte) z;
                    }
                    fromreader = littleEndian();
                    break;

                case "*Logger_State":
                    Logger_State();
                    break;

                case "Logging_selection_MonT":
                    break;

                case "SampleNumber_logged_MonT":

                    break;
                
                

                case "Serial_Number_Decoding":
                        string serialnumber = "";
                        if ((decodebyte[3] & 0xF0) == 0x50)
                        {
                            serialnumber = "L";

                            switch (decodebyte[3] & 0x0F)
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
                        else if ((decodebyte[3] & 0xF0) == 0x60)//For MonT
                        {
                            serialnumber = "R0";
                        }
                        else
                        {
                            serialnumber = "--------";
                        }
                        serialnumber += ((((decodebyte[2] & 0xFF) << 16) | ((decodebyte[1] & 0xFF) << 8) | (decodebyte[0]) & 0xFF));
                    
                    fromreader = serialnumber;
                    break;

                case "String":
                    for (int i = 0; i < decodebyte.Length; i++)
                    {
                        decodebyte[i] = (byte)(decodebyte[i] & 0xFF);
                        if ((decodebyte[i] == 0x01))
                            decodebyte[i] = 0x20;
                    }

                    fromreader += Encoding.ASCII.GetString(decodebyte);
                    break;

                case "Time_FirstSample_MonT":

                    break;

                case "Time_LastSample_MonT":
                    Time_LastSample_MonT();
                    break;

                case "XXX":
                    break;
            }

            return fromreader;
        }

        static int GetBit(int Value, int bit)
        {
            return (Value >> bit) & 1;
        }

        public static byte[] ReadHex(string[] currentinfo)
        {
            byte[] bytes = { };

            try
            {
                //currentinfo = [add, len, code, hide];

                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(Communication.serialnumber.ToString() + ".hex"))
                {
                    string line;
                    int diff = 0;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        string address = line.Substring(0, 6);
                        string data = line.Substring(7, line.Length - 7);
                        string temp = "";
                        
                        if (int.Parse(currentinfo[0], System.Globalization.NumberStyles.HexNumber) >= int.Parse(address, System.Globalization.NumberStyles.HexNumber))
                            addtoread = address;

                        if (addtoread == address)
                        {
                            diff = int.Parse(currentinfo[0], System.Globalization.NumberStyles.HexNumber) - int.Parse(address, System.Globalization.NumberStyles.HexNumber);

                            if (diff>=0 && diff < 65)
                            {
                                /*Console.WriteLine("AIM ADDRESS : " + currentinfo[0]);
                                Console.WriteLine("FROM ADDRESS: " + address);
                                Console.WriteLine("THE DIFFERENCE :" + diff);
                                Console.WriteLine("LENGTH : " + currentinfo[1]);
                                Console.WriteLine("DATA : " + data);*/

                                int infolength = Convert.ToUInt16(currentinfo[1]);

                                if (infolength > 65)
                                {
                                    int readinfo = 64 - diff * 2;
                                    while (infolength > 0)
                                    {
                                        temp += data.Substring(diff * 2, readinfo * 2);
                                        line = sr.ReadLine();
                                        data = line.Substring(7, line.Length - 7);
                                        infolength = infolength - readinfo;
                                        if (infolength > 65)
                                            readinfo = 64;
                                        else
                                            readinfo = infolength;
                                    }

                                    int totallength = temp.Length;
                                    bytes = new byte[totallength / 2];
                                    for (int i = 0; i < totallength; i += 2)
                                        bytes[i / 2] = (byte)(Convert.ToByte(temp.Substring(i, 2), 16));
                                }

                                else
                                {
                                    temp = data.Substring(diff * 2, Convert.ToInt16(currentinfo[1]) * 2);
                                    int totallength = temp.Length;
                                    bytes = new byte[totallength / 2];
                                    for (int i = 0; i < totallength; i += 2)
                                        bytes[i / 2] = (byte)(Convert.ToByte(temp.Substring(i, 2), 16));
                                }

                                return bytes;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return bytes;
        }

        private static string littleEndian ()
        {
            string littleendian = "";
            for (int i = decodebyte.Length - 1; i >= 0; i--)
                littleendian += decodebyte[i].ToString("x02");

            return littleendian;
        }

        private static string bigEndian()
        {
            string bigendian = "";
            for (int i = 0; i < decodebyte.Length; i++)
                bigendian += decodebyte[i].ToString("x02");
            return bigendian;
        }

        private static void Logger_State()
        {
            String VALUE = "UNDEFINED";
            Console.WriteLine("LOGGER STATE: " + decodebyte[0].ToString("X02"));

            switch (decodebyte[0])
            {
                case 0:
                    VALUE = "READY";
                    break;
                case 1:
                    VALUE = "DELAY";
                    break;
                case 2:
                    VALUE = "RUNNING";
                    break;
                case 3:
                    VALUE = "STOPPED";
                    break;
                case 4:
                    VALUE = "UNDEFINED";
                    break;
            }

            fromreader = VALUE;
        }

        public static String HHMMSS(double mseconds)
        {
            int hours = (int)mseconds / 3600;
            int minutes = (int)(mseconds % 3600) / 60;
            int seconds = (int)mseconds % 60;

            String timeString = (hours.ToString("00") +":"+ minutes.ToString("00") +":"+ seconds.ToString("00"));
            return timeString;
        }

        private static void Time_LastSample_MonT()
        {
            string temp = createJSON.readJson("SecondsTimer");
            secondtimer = long.Parse(temp, NumberStyles.HexNumber);
            temp = createJSON.readJson("UTCReferenceTime");
            UTCreference = long.Parse(temp, NumberStyles.HexNumber);
            String STOPPED_TIME = UNIXtoUTC((UTCreference) - (4294967040L - secondtimer));
            Console.WriteLine("================================ STOPPED_TIME : " + STOPPED_TIME);
            fromreader = STOPPED_TIME;
        }

        public static String UNIXtoUTC(long now)
        {
            //Date format to DATETIME
            DateTime date = new DateTime (now * 100);
            string simpledate = date.ToString("yyyy-MM-dd HH:mm:ss zzz");
            return simpledate;
        }
    }
}
