using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json.Linq;
using TempLite.Services;

namespace TempLite
{
    public class decodeHEX
    {
        string fromreader = "";
        string addtoread = "";
        byte[] decodebyte;
        bool bitbool = false;

        long UTCreference = 0;
        long secondtimer = 0;

        private string serialnumber;
        private string jsonfile;

        private int[] StartValue = new int[8];
        private int memory_start = 0;
        private int[] m_sensor_starting_value = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private int[] m_sensor_table_pointer = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private int[] m_sensor_type = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private int[] m_compression_table = new int[128];
        private long m_starting_time;
        private int m_sampling_period = 0;
        private int m_sample_number = 0;
        private int m_sensor_number = 0;
        private int m_user_data_len = 0;
        private long m_UTC_reference_time = 0;

        private double m_pedestal = 0;
        private double m_resolution = 0;
        private long m_ticks_at_last_sample = 0;
        private long m_ticks_since_start = 0;
        private int m_holdoff = 0;
        private bool m_overwriting = false;
        private long m_seconds_timer = 0;
        private long m_manufacture_date = 0;
        private int m_total_rtc_ticks = 0;
        private int m_total_sampling_events = 0;
        private int m_total_uses = 0;

        public decodeHEX(_communicationServices communicationServies)
        {
            serialnumber = communicationServies.serialnumber;
            jsonfile = communicationServies.jsonfile;
        }


        public string readJson(string info)
        {
            var jsonobject = new JObject();
            string[] decodeinfo;
            string fromreader = "";

            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(jsonfile))
                {
                    //come back and change the part where is read
                    var json = sr.ReadToEnd();
                    jsonobject = JObject.Parse(json);

                    var add = jsonobject.GetValue(info).First.Last.ToString();
                    var len = jsonobject.GetValue(info).First.Next.Last.ToString();
                    var code = jsonobject.GetValue(info).First.Next.Next.Last.ToString();
                    var hide = jsonobject.GetValue(info).Last.Last.ToString();

                    decodeinfo = new string[] { add, len, code, hide };
                    fromreader = decodehex(decodeinfo);

                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return fromreader;
        }

        //stringarrayinfo from JSON file [add,len,code,hide]
        public string decodehex(string[] stringarrayinfo)
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
                    fromreader = littleEndian();

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
                    var one = readJson("DATA_INFO,Overwritten");
                    Console.WriteLine("m_overwriting : " + one);
                    var two = readJson("USER_SETTINGS,StartDelay");
                    Console.WriteLine("m_holdoff : " + two);
                    var three = readJson("USER_SETTINGS,SamplingPeriod");
                    Console.WriteLine("m_sampling_period : " + three);
                    m_ticks_at_last_sample = Convert.ToInt32(readJson("DATA_INFO, LastSampleAt"), 16);

                    if (m_overwriting)
                        fromreader = "4681";
                    else
                    {
                        int samplenumber = (((int)(m_ticks_at_last_sample - m_holdoff) / m_sampling_period) + 1);
                        Console.WriteLine("SAMPLE NUMBER : " + samplenumber);
                        fromreader = m_sample_number.ToString();
                    }

                    break;
                
                

                case "Serial_Number_Decoding":
                        string serialNumber = "";
                        if ((decodebyte[3] & 0xF0) == 0x50)
                        {
                        serialNumber = "L";

                            switch (decodebyte[3] & 0x0F)
                            {
                                case 0x00:
                                serialNumber += "0";
                                    break;
                                case 0x07:
                                serialNumber += "T";
                                    break;
                                case 0x08:
                                serialNumber += "G";
                                    break;
                                case 0x09:
                                serialNumber += "H";
                                    break;
                                case 0x0A:
                                serialNumber += "P";
                                    break;
                                case 0x0C:
                                serialNumber += "M";
                                    break;
                                case 0x0D:
                                serialNumber += "S";
                                    break;
                                case 0x0E:
                                serialNumber += "X";
                                    break;
                                case 0x0F:
                                serialNumber += "C";
                                    break;
                                default:
                                serialNumber = "L-------";
                                    break;
                            }
                        }
                        else if ((decodebyte[3] & 0xF0) == 0x60)//For MonT
                        {
                            serialNumber = "R0";
                        }
                        else
                        {
                            serialNumber = "--------";
                        }
                        serialNumber += ((((decodebyte[2] & 0xFF) << 16) | ((decodebyte[1] & 0xFF) << 8) | (decodebyte[0]) & 0xFF));
                    
                    fromreader = serialNumber;
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

        private void writeJSON()
        {

        }

        static int GetBit(int Value, int bit)
        {
            return (Value >> bit) & 1;
        }

        public byte[] ReadHex(string[] currentinfo)
        {
            byte[] bytes = { };

            try
            {
                //currentinfo = [add, len, code, hide];

                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(serialnumber + ".hex"))
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

        private string littleEndian ()
        {
            string littleendian = "";
            for (int i = decodebyte.Length - 1; i >= 0; i--)
                littleendian += decodebyte[i].ToString("x02");

            return littleendian;
        }

        private string bigEndian()
        {
            string bigendian = "";
            for (int i = 0; i < decodebyte.Length; i++)
                bigendian += decodebyte[i].ToString("x02");
            return bigendian;
        }

        private void Logger_State()
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
        

        private void Time_LastSample_MonT()
        {
            string temp = readJson("SecondsTimer");
            secondtimer = long.Parse(temp, NumberStyles.HexNumber);
            temp = readJson("UTCReferenceTime");
            UTCreference = long.Parse(temp, NumberStyles.HexNumber);
            String STOPPED_TIME = UNIXtoUTC((UTCreference) - (4294967040L - secondtimer));
            Console.WriteLine("STOPPED TIME : " + STOPPED_TIME);
            fromreader = STOPPED_TIME;
        }

        public String UNIXtoUTC(long now)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(now * 1000);
            date = date.ToUniversalTime();
            string simpledate = date.ToString("yyyy-MM-dd HH:mm:ss");
            return simpledate;
        }
    }
}
