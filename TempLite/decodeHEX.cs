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

        private long Y_2000 = 946684800000L;
        private long Y_2010 = 1262304000000L;
        private int Kelvin = 27315;
        private double Kelvin_Dec = 273.15;
        private int Data_Address = 0x7FFF;
        private int NUM_DATA_LOGGED = 0;
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

                                                                         //    private static  final   double      R                       = 0.008314472;                      //R         is the Gas Constant:        0.008314472 kJ/mole/degree
        private double Delta_H = 83.14472;                         //Delta H   is the Activation Energy:   83.14472    kJ/mole
        private double R = 8.314472;                      //R         is the Gas Constant:        0.008314472 J/mole/degree

        private int[] Sample_Number = new int[8];
        private double[] m_upper_limit = new double[8];
        private double[] m_lower_limit = new double[8];

        private double[] m_sensor_min = new double[8];
        private double[] m_sensor_max = new double[8];

        private int[] m_highest_position = new int[8];
        private int[] m_lowest_position = new int[8];
        private double[] m_mean = new double[8];
        private double[] m_MKT = new double[8];

        private double[] m_nb_within_limit = new double[8];
        private double[] m_nb_below_limit = new double[8];
        private double[] m_nb_above_limit = new double[8];

        public decodeHEX(_communicationServices communicationServies)
        {
            serialnumber = communicationServies.serialnumber;
            jsonfile = communicationServies.jsonfile;
            m_overwriting = Convert.ToBoolean(readJson("DATA_INFO,Overwritten"));
            m_holdoff = Convert.ToInt32(readJson("USER_SETTINGS,StartDelay"), 16);
            m_sampling_period = Convert.ToInt32(readJson("USER_SETTINGS,SamplingPeriod"), 16);
            m_seconds_timer = Convert.ToInt32(readJson("SecondsTimer"), 16);
            m_ticks_since_start = Convert.ToInt32(readJson("DATA_INFO,TicksSinceArousal"), 16);
            m_ticks_at_last_sample = Convert.ToInt32(readJson("DATA_INFO,TicksAtLastSample"), 16);
            m_sample_number = Convert.ToInt32(readJson("DATA_INFO,SamplesNumber"));
            m_pedestal = Convert.ToDouble(readJson("USER_SETTINGS,LowestTemp"));
            m_resolution = Convert.ToDouble(readJson("USER_SETTINGS,ResolutionRatio"))/100;
            readJson("SENSOR,Decode_MonT_Data");

            Console.WriteLine("m_upper_limit : " + m_upper_limit[0]);
            Console.WriteLine("m_lower_limit : " + m_lower_limit[0]);
            Console.WriteLine("m_sensor_min : " + m_sensor_min[0]);
            Console.WriteLine("m_sensor_max : " + m_sensor_max[0]);
            Console.WriteLine("m_mean : " + m_mean[0]);
            Console.WriteLine("m_MKT : " + m_MKT[0]);
            Console.WriteLine("m_nb_within_limit : " + m_nb_within_limit[0]);
            Console.WriteLine("m_nb_below_limit : " + m_nb_below_limit[0]);
            Console.WriteLine("m_nb_above_limit : " + m_nb_above_limit[0]);
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
            Console.WriteLine("Length : " + decodebyte.Length);
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
                        fromreader = date.ToString("dd/MM/yyyy HH:mm:sss");
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
                    int a = 0;
                    int b = 0;
                    int array_pointer = 0;
                    bool Flag_End = false;
                    
                    Console.WriteLine("m_sample_number: " + m_sample_number);

                    if (m_sample_number > 0)
                    {
                        var VALUE = new double[m_sample_number];
                        Init_Sensor_Statistics_Field(0);

                        //==================================If loop overwriting=====================================
                        if (m_overwriting)
                        {
                            while ((b < decodebyte.Length))
                            {

                                if (decodebyte[b] == 0xFF)                                                                //0xFF = 255 Two's complement
                                {
                                    break;
                                }
                                b++;
                            }
                            b++;                                                                                        //Go to the index after the 0xFF
                            while ((b < decodebyte.Length))                                                           //reads the data after the 0xFF
                            {
                                NUM_DATA_LOGGED++;

                                Temperature_Statistics(0, m_pedestal + ((decodebyte[b] & 0xFF) * m_resolution), array_pointer);

                                VALUE[array_pointer++] = (m_pedestal + ((decodebyte[b] & 0xFF) * m_resolution));


                                b++;
                            }

                        }
                        //==========================================================================================

                        //=========================Used when Loop overwrite is enabled or disabled==================
                        while ((a < decodebyte.Length) && (!Flag_End))
                        {
                            if (decodebyte[a] == 0xFE || decodebyte[a] == 0xFF)                                               //0xFE = 254 Two's complement || 0xFF = 255 Two's complement
                            {
                                Flag_End = true;
                            }
                            else
                            {

                                Temperature_Statistics(0, m_pedestal + ((decodebyte[a] & 0xFF) * m_resolution), array_pointer);

                                VALUE[array_pointer++] = (m_pedestal + ((decodebyte[a] & 0xFF) * m_resolution));


                                NUM_DATA_LOGGED++;

                            }
                            a++;
                        }

                        foreach (double val in VALUE)
                        {
                            Console.WriteLine("DATA : " + val);
                        }

                        Console.WriteLine("DATA LENGTH " + VALUE.Length);
                    }
                    Finalize_Statistics(0);
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
                    if (m_overwriting)
                        fromreader = "4681";
                    else
                    {
                        int samplenumber = (((int)(m_ticks_at_last_sample - m_holdoff) / m_sampling_period) + 1);
                        fromreader = samplenumber.ToString();
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
                    if (m_overwriting)
                    {
                        long STARTED_TIME = ((m_UTC_reference_time) - (4680 * m_sampling_period) - (m_ticks_since_start - m_ticks_at_last_sample));
                        fromreader = UNIXtoUTC(STARTED_TIME);
                    }
                    else
                    {
                        long STARTED_TIME = ((m_UTC_reference_time - m_ticks_since_start) + m_holdoff);
                        fromreader = UNIXtoUTC(STARTED_TIME);
                    }

                    break;

                case "Time_LastSample_MonT":
                    string temp = readJson("SecondsTimer");
                    secondtimer = long.Parse(temp, NumberStyles.HexNumber);
                    temp = readJson("UTCReferenceTime");
                    UTCreference = long.Parse(temp, NumberStyles.HexNumber);
                    String STOPPED_TIME = UNIXtoUTC((UTCreference) - (4294967040L - secondtimer));
                    fromreader = STOPPED_TIME;
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
                        
                        if (int.Parse(currentinfo[0], NumberStyles.HexNumber) >= int.Parse(address, NumberStyles.HexNumber))
                            addtoread = address;

                        if (addtoread == address)
                        {
                            diff = int.Parse(currentinfo[0], NumberStyles.HexNumber) - int.Parse(address, NumberStyles.HexNumber);

                            if (diff>=0 && diff < 65) // reader can only send 64bytes at a time
                            {
                                int infolength = Convert.ToUInt16(currentinfo[1]);
                                if (infolength > 65)
                                {
                                    int readinfo = 64 - diff;
                                    while (infolength > 0)
                                    {
                                        temp += data.Substring(diff * 2, readinfo * 2);
                                        line = sr.ReadLine();
                                        data = line.Substring(7, line.Length - 7);
                                        infolength = infolength - readinfo;

                                        if (infolength > 65)
                                        {
                                            diff = 0;
                                            readinfo = 64;
                                        }
                                        else
                                        {
                                            readinfo = infolength;
                                        }
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

        public String UNIXtoUTC(long now)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(now * 1000);
            date = date.ToUniversalTime();
            string simpledate = date.ToString("dd/MM/yyyy HH:mm:sss");
            return simpledate;
        }

        private void Init_Sensor_Statistics_Field(int m_current_sensor)
        {
            Sample_Number[m_current_sensor] = 0;

            m_sensor_min[m_current_sensor] = 0xFFFFFFFFFFL;
            m_sensor_max[m_current_sensor] = -274;

            m_highest_position[m_current_sensor] = 0;
            m_lowest_position[m_current_sensor] = 0;

            m_mean[m_current_sensor] = 0;
            m_MKT[m_current_sensor] = 0;

            m_nb_within_limit[m_current_sensor] = 0;
            m_nb_below_limit[m_current_sensor] = 0;
            m_nb_above_limit[m_current_sensor] = 0;
        }
        //==========================================================//

        //==========================================================//
        private void Temperature_Statistics(int m_current_sensor, double Value, int index)
        {
            if (Value < m_sensor_min[m_current_sensor])
            {
                m_lowest_position[m_current_sensor] = index + 1;// Cause it starts from zero
                m_sensor_min[m_current_sensor] = Value;
            }

            if (Value > m_sensor_max[m_current_sensor])
            {
                m_highest_position[m_current_sensor] = index + 1;
                m_sensor_max[m_current_sensor] = Value;
            }


            if (Value > m_upper_limit[m_current_sensor])
            {
                m_nb_above_limit[m_current_sensor]++;
            }
            else if (Value < m_lower_limit[m_current_sensor])
            {
                m_nb_below_limit[m_current_sensor]++;
            }
            else
            {
                m_nb_within_limit[m_current_sensor]++;
            }

            //exp(-Delta_H/R x Tn)
            m_MKT[m_current_sensor] += Math.Exp((-Delta_H) / ((Value + Kelvin_Dec) * R));
            m_mean[m_current_sensor] += Value;
            Sample_Number[m_current_sensor]++;
        }
        //==========================================================//

        //==========================================================//
        private void Finalize_Statistics(int m_current_sensor)
        {
            String[] m_path = { "STATISTICS", "Sensor_" + m_current_sensor };

            m_mean[m_current_sensor] /= Sample_Number[m_current_sensor];
            m_MKT[m_current_sensor] = (Delta_H / R) / (-Math.Log(m_MKT[m_current_sensor] / Sample_Number[m_current_sensor]));
        }
    }
}
