using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TempLite
{
    class createJSON
    {

        static string serialnumber = "";
        static string add = "";
        static string len = "";
        static string code = "";
        static string hide = "";

        private class DEVICE
        {
            public  SENSOR sensor { get; set; }
            public STATISTICS stat { get; set; }
        }

        private class SENSOR
        {
            public List<sensor> sensor { get; set; }
        }

        private class STATISTICS
        {
            public List<stat> stat { get; set; }
        }

        private class stat
        {
            public string Min{ get; set; }
            public string Max { get; set; }
            public string LowestSamplePosition { get; set; }
            public string HighestSamplePosition { get; set; }
            public string WithinLimit { get; set; }
            public string OutsideLimit { get; set; }
            public string BelowLimit { get; set; }
            public string AboveLimit { get; set; }
            public string Mean { get; set; }
            public string MKT_K { get; set; }
            public string MKT_C { get; set; }
        }

        private class sensor
        {
            public List<limits> limits { get; set; }
            public string FirstSampleAt { get; set; }
            public string SamplingPeriod { get; set; }
            public string SensorType { get; set; }
            public string SensorResolution { get; set; }
            public string SensorMinimum { get; set; }
            public string[] values { get; set; }
            public string SensorNumber { get; set; }
            public string FirstSampleAtUNIX { get; set; }
        }

        private class limits
        {
            public string LowerLimit { get; set; }
            public string UpperLimit { get; set; }
        }


        public static string getSerialnumber(byte[] msg)
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

            string number = (((msg[2] & 0xFF) << 16) | ((msg[1] & 0xFF) << 8) | (msg[0]) & 0xFF).ToString();
            while (number.Length != 6)
                number = "0" + number;
            serialnumber += number.ToString();

            return serialnumber;
        }

        public static string readJson(string info)
        {
            var jsonobject = new JObject();
            string[] decodeinfo;
            string fromreader="";

            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader("MonT.json"))
                {
                    //come back and change the part where is read
                    var json = sr.ReadToEnd();
                    jsonobject = JObject.Parse(json);

                    add = jsonobject.GetValue(info).First.Last.ToString();
                    len = jsonobject.GetValue(info).First.Next.Last.ToString();
                    code = jsonobject.GetValue(info).First.Next.Next.Last.ToString();
                    hide = jsonobject.GetValue(info).Last.Last.ToString();

                    //Console.WriteLine(add, len, code, hide);

                    decodeinfo = new string[] { add, len, code, hide };
                    fromreader = decodeHEX.decodehex(decodeinfo,serialnumber);
                    
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

    }
}
