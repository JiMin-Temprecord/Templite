using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json.Linq;
using TempLite.Services;
using System.Collections;

namespace TempLite
{
    public class HexfileDecoder
    {
        string serialnumber;
        string jsonfile;
        string batteryPercentage;

        bool loopOverwrite = false;
        bool Fahrenheit = false;

        double Kelvin_Dec = 273.15;
        double lowestTemp = 0;
        double Resolution = 0;

        long Y_2000 = 946684800000L;
        long Y_2010 = 1262304000000L;
        long timeFirstSample = 0;
        long lastSample = 0;
        long samplePeriod = 0;
        long UTCreferenceTime = 0;
        long ticksAtLastSample = 0;
        long ticksSinceStart = 0;
        long secondsTimer = 0;
        long manufactureDate = 0;

        int Kelvin = 27315;
        int recordedSamples = 0;
        int numberChannel = 0;
        int userDataLength = 0;
        int startDelay = 0;
        int totalRTCticks = 0;
        int totalSamplingEvents = 0;
        int totalUses = 0;

        int[] highestPosition = new int[8];
        int[] lowestPosition = new int[8];

        double Delta_H = 83.14472; //Delta H   is the Activation Energy:   83.14472    kJ/mole
        double R = 8.314472; //R is the Gas Constant: 0.008314472 J/mole/degree
        
        double[] m_data;
        double[] upperLimit = new double[8];
        double[] lowerLimit = new double[8];
        double[] sensorMin = new double[8];
        double[] sensorMax = new double[8];
        double[] Mean = new double[8];
        double[] MKT = new double[8];
        double[] withinLimit = new double[8];
        double[] belowLimit = new double[8];
        double[] aboveLimit = new double[8];
        
        private void AssignValues (PDFvariables pdfVariables)
        {
            pdfVariables.recordedSample = Convert.ToInt32(readJson("DATA_INFO,SamplesNumber"));
            pdfVariables.serialNumber = readJson("HEADER,SerialNumber");
            pdfVariables.loggerState = readJson("HEADER,State");
            pdfVariables.batteryPercentage = readJson("BATTERY_INFO,Battery") + "%";
            pdfVariables.sameplePeriod = HHMMSS(samplePeriod);
            pdfVariables.startDelay = readJson("USER_SETTINGS,StartDelay");
            pdfVariables.firstSample = readJson("DATA_INFO,Time_FirstSample_MonT");
            pdfVariables.tagsPlaced = "0";
            pdfVariables.userData = readJson("USER_DATA,UserData");
            
            if (Fahrenheit)
            {
                pdfVariables.tempUnit = " °F";
            }

            else
            {
                pdfVariables.tempUnit = " °C";

            }
            for (int i = 0; i < numberChannel; i++)
            {
                pdfVariables.enabledChannels[i] = true;
                pdfVariables.presetLowerLimit[i] = lowerLimit[i];
                pdfVariables.presetUpperLimit[i] = upperLimit[i];
                pdfVariables.Mean[i] = Mean[i];
                pdfVariables.MKT_C[i] = MKT[i] - Kelvin_Dec;
                pdfVariables.Max[i] = sensorMax[i];
                pdfVariables.Min[i] = sensorMin[i];
                pdfVariables.withinLimits[i] = withinLimit[i];
                pdfVariables.outsideLimits[i] = aboveLimit[i] + belowLimit[i];
                pdfVariables.aboveLimits[i] = aboveLimit[i];
                pdfVariables.belowLimits[i] = belowLimit[i];
                pdfVariables.timeWithinLimits[i] = HHMMSS(pdfVariables.withinLimits[i] * pdfVariables.recordedSample);
                pdfVariables.timeOutLimits[i] = HHMMSS(pdfVariables.outsideLimits[i] * pdfVariables.recordedSample);
                pdfVariables.timeAboveLimits[i] = HHMMSS(pdfVariables.aboveLimits[i] * pdfVariables.recordedSample);
                pdfVariables.timeBelowLimits[i] = HHMMSS(pdfVariables.belowLimits[i] * pdfVariables.recordedSample);


                if (pdfVariables.aboveLimits[i] > 0)
                    pdfVariables.breachedAbove[i] = " (breached)";

                if (pdfVariables.belowLimits[i] > 0)
                    pdfVariables.breachedBelow[i] = " (breached)";

            }


            for (int i = 0; i < pdfVariables.recordedSample; i++)
            {
                pdfVariables.Time.Add(timeFirstSample);
                timeFirstSample = timeFirstSample + samplePeriod;
            }
            long timeLastSample = Convert.ToInt32(pdfVariables.Time[(pdfVariables.Time.Count - 1)]);
            pdfVariables.lastSample = UNIXtoUTC(timeLastSample);

        }
        public HexfileDecoder(_communicationServices communicationServies, PDFvariables pdfVariables)
        {
            jsonfile = communicationServies.jsonfile;
            numberChannel = Convert.ToInt32(readJson("SENSOR,SensorNumber"), 16);
            Fahrenheit = Convert.ToBoolean(readJson("USER_SETTINGS,Fahrenheit"));
            UTCreferenceTime = Convert.ToInt32(readJson("UTCReferenceTime"), 16);
            totalRTCticks = Convert.ToInt32(readJson("HEADER,TotalRTCTicks"), 16);
            readJson("HEADER,ManufactureDate");
            totalSamplingEvents = Convert.ToInt32(readJson("HEADER,TotalSamplingEvents"), 16);
            totalUses = Convert.ToInt32(readJson("HEADER,TotalUses"), 16);
            loopOverwrite = Convert.ToBoolean(readJson("DATA_INFO,Overwritten"));
            startDelay = Convert.ToInt32(readJson("USER_SETTINGS,StartDelay"), 16);
            samplePeriod = Convert.ToInt32(readJson("USER_SETTINGS,SamplingPeriod"), 16);
            secondsTimer = Convert.ToInt32(readJson("SecondsTimer"), 16);
            ticksSinceStart = Convert.ToInt32(readJson("DATA_INFO,TicksSinceArousal"), 16);
            ticksAtLastSample = Convert.ToInt32(readJson("DATA_INFO,TicksAtLastSample"), 16);
            recordedSamples = Convert.ToInt32(readJson("DATA_INFO,SamplesNumber"));
            lowestTemp = Convert.ToDouble(readJson("USER_SETTINGS,LowestTemp"));
            Resolution = Convert.ToDouble(readJson("USER_SETTINGS,ResolutionRatio")) / 100;
            readJson("SENSOR,Decode_MonT_Data");
            upperLimit[0] = Convert.ToDouble(readJson("CHANNEL_INFO,UpperLimit"));
            lowerLimit[0] = Convert.ToDouble(readJson("CHANNEL_INFO,LowerLimit"));
        }


        public string readJson(string info)
        {
            var jsonObject = new JObject();
            var decodeInfo = new string[4];

            try
            {
                jsonObject = GetJsonObject();
                decodeInfo = JsontoString(jsonObject, info);
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return decodeHex(decodeInfo);
        }

        private JObject GetJsonObject()
        {
            using (var sr = new StreamReader(jsonfile))
            {
                return JObject.Parse(sr.ReadToEnd());
            }
        }

        private string[] JsontoString(JObject jsonObject, string info)
        {
            var add = jsonObject.GetValue(info).First.Last.ToString();
            var len = jsonObject.GetValue(info).First.Next.Last.ToString();
            var code = jsonObject.GetValue(info).First.Next.Next.Last.ToString();
            var hide = jsonObject.GetValue(info).Last.Last.ToString();

            return new string[] { add, len, code, hide };

        }

        //stringarrayinfo from JSON file [add,len,code,hide]
        public string decodeHex(string[] stringarrayinfo)
        {
            //returned byte[] from the hex file
            bool bitbool = false;
            var decodebyte = ReadHex(stringarrayinfo);
            switch (stringarrayinfo[2])
            {

                case "_1_Byte_to_Decimal":
                    if (decodebyte.Length > 1)
                    {
                        for (int i = 0; i < decodebyte.Length; i++)
                            decodebyte[i] = (byte)(decodebyte[i] & 0xff);

                        if (stringarrayinfo[3] == "0")
                            return bigEndian(decodebyte);
                    }
                    else
                    {
                        decodebyte[0] = (byte)(decodebyte[0] & 0xff);
                        return bigEndian(decodebyte);
                    }
                    break;

                case "_2_Byte_to_Decimal":
                    return (((decodebyte[1] & 0xFF) << 8) | (decodebyte[0] & 0xFF)).ToString();

                case "_2_Byte_to_Temperature_MonT":
                    int value = (((decodebyte[1]) & 0xFF) << 8) | (decodebyte[0] & 0xFF);
                    value -= 4000;
                    double V = ((double)value / 100);
                    return V.ToString("F");

                case "_4_Byte_to_Decimal":
                    return decodebyte[3].ToString("X02") + decodebyte[2].ToString("X02") + decodebyte[1].ToString("X02") + decodebyte[0].ToString("X02");

                case "_4_Byte_to_UNIX":
                    long _4byteUnix = (Convert.ToInt32(ToLittleEndian(decodebyte), 16) * (long)1000);
                    manufactureDate = ((long)_4byteUnix + Y_2000) / (long)1000;
                    return manufactureDate.ToString();

                case "_4_Byte_Sec_to_Date":
                    long _4sectobyte = (decodebyte[3] + decodebyte[2] + decodebyte[1] + decodebyte[0]) * 1000;

                    if (_4sectobyte > 0)
                    {
                        _4sectobyte += Y_2000;
                        DateTime date = new DateTime(_4sectobyte);
                        return date.ToString("dd/MM/yyyy HH:mm:sss");
                    }
                    else
                        return "";

                case "_8_Byte_to_Unix_UTC":
                    return bigEndian(decodebyte);

                case "b0":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 0) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "b1":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 1) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "b2":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 2) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "b3":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 3) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "b4":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 4) != 0)
                        bitbool = true;
                    return bitbool.ToString();


                case "b5":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 5) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "b6":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 6) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "b7":
                    bitbool = false;
                    if (GetBit(decodebyte[0], 7) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "Battery_MonT":
                    double RCLBatteryMargin = 0.25;    // i.e. leave 25% in reserve
                    double RCLSelfDischargeCurrentuA = 0.630;
                    double RCLQuiescentDischargeCurrentuA = 9.000;
                    double RCLConversionDischargeCurrentuA = 1300.0;
                    double RCLDownloadDischargeCurrentuA = 2900.00;
                    double RCLConversionDurationS = 1.0;
                    double RCLDownloadDurationS = 3.5;
                    double RCLAssumedDownloadsPerTrip = 2.0;

                    int batteryPercentage;
                    int INITAL_BATTERY = (int)((1.0 - RCLBatteryMargin) * 0.56 * 60 * 60 * 1000000); //mAHr
                    double BatteryUsed =
                    (((UTCreferenceTime - manufactureDate) * RCLSelfDischargeCurrentuA) +
                    (totalRTCticks * RCLQuiescentDischargeCurrentuA) +
                    ((totalSamplingEvents + 4681) * RCLConversionDurationS * RCLConversionDischargeCurrentuA) +
                    (RCLAssumedDownloadsPerTrip * totalUses * RCLDownloadDurationS * RCLDownloadDischargeCurrentuA));
                    batteryPercentage = (int)(((INITAL_BATTERY - BatteryUsed) / INITAL_BATTERY) * 100);
                    return batteryPercentage.ToString();

                case "Channel_1_MonT":
                    return "1";

                case "Decode_MonT_Data":
                    int a = 0;
                    int b = 0;
                    int array_pointer = 0;
                    bool Flag_End = false;

                    if (recordedSamples > 0)
                    {
                        var VALUE = new double[recordedSamples];
                        Init_Sensor_Statistics_Field(0);

                        //==================================If loop overwriting=====================================
                        if (loopOverwrite)
                        {
                            while ((b < decodebyte.Length))
                            {

                                if (decodebyte[b] == 0xFF) //0xFF = 255 Two's complement
                                {
                                    break;
                                }
                                b++;
                            }
                            b++;  //Go to the index after the 0xFF
                            while ((b < decodebyte.Length)) //reads the data after the 0xFF
                            {
                                Temperature_Statistics(0, lowestTemp + ((decodebyte[b] & 0xFF) * Resolution), array_pointer);
                                VALUE[array_pointer++] = (lowestTemp + ((decodebyte[b] & 0xFF) * Resolution));
                                b++;
                            }

                        }
                        //==========================================================================================

                        //=========================Used when Loop overwrite is enabled or disabled==================
                        while ((a < decodebyte.Length) && (!Flag_End))
                        {
                            if (decodebyte[a] == 0xFE || decodebyte[a] == 0xFF) //0xFE = 254 Two's complement || 0xFF = 255 Two's complement
                            {
                                Flag_End = true;
                            }
                            else
                            {
                                Temperature_Statistics(0, lowestTemp + ((decodebyte[a] & 0xFF) * Resolution), array_pointer);
                                VALUE[array_pointer++] = (lowestTemp + ((decodebyte[a] & 0xFF) * Resolution));
                            }
                            a++;
                        }

                        m_data = VALUE;
                    }
                    Finalize_Statistics(0);
                    break;

                case "DJNZ_2_Byte_Type_1":
                    decodebyte[1]--;
                    return ToLittleEndian(decodebyte);

                case "DJNZ_4_Byte_Type_2":
                    for (int i = 0; i < 4; i++)
                    {
                        int z = (0x100) - (decodebyte[i] & 0xFF);
                        decodebyte[i] = (byte)z;
                    }
                    return ToLittleEndian(decodebyte);

                case "*Logger_State":
                    return Logger_State(decodebyte);

                case "Logging_selection_MonT":
                    break;

                case "SampleNumber_logged_MonT":
                    if (loopOverwrite)
                        return "4681";
                    else
                    {
                        long samplenumber = (((long)(ticksAtLastSample - startDelay) / samplePeriod) + 1);
                        return samplenumber.ToString();
                    }



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

                    return serialNumber;

                case "String":
                    var userdatastring = string.Empty; 
                    for (int i = 0; i < decodebyte.Length; i++)
                    {
                        userdatastring += (char)(decodebyte[i] & 0xFF);
                    }

                    return userdatastring.Substring(0, userDataLength);

                case "Time_FirstSample_MonT":
                    if (loopOverwrite)
                    {
                        timeFirstSample = ((UTCreferenceTime) - (4680 * samplePeriod) - (ticksSinceStart - ticksAtLastSample));
                        return UNIXtoUTC(timeFirstSample);
                    }
                    else
                    {
                        timeFirstSample = ((UTCreferenceTime - ticksSinceStart) + startDelay);
                        return UNIXtoUTC(timeFirstSample);
                    }

                case "Time_LastSample_MonT":
                    var UTCreference = long.Parse(readJson("UTCReferenceTime"), NumberStyles.HexNumber);
                    String STOPPED_TIME = UNIXtoUTC((UTCreference) - (4294967040L - secondsTimer));
                    return STOPPED_TIME;
            }

            return string.Empty;
        }

        static int GetBit(int Value, int bit)
        {
            return (Value >> bit) & 1;
        }

        public byte[] ReadHex(string[] currentinfo)
        {
            byte[] bytes = { };
            string addtoread = "";

            try
            {
                //currentinfo = [add, len, code, hide];
                using (StreamReader sr = new StreamReader(serialnumber + ".hex"))
                {
                    string line;
                    int diff = 0;
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

                            if (diff >= 0 && diff < 64) // reader can only send 64bytes at a time
                            {
                                int infolength = Convert.ToInt32(currentinfo[1]);
                                if (infolength > 64)
                                {
                                    int readinfo = 64 - diff;
                                    while (infolength > 0)
                                    {
                                        temp += data.Substring(diff * 2, readinfo * 2);
                                        line = sr.ReadLine();
                                        data = line.Substring(7, line.Length - 7);
                                        infolength = infolength - readinfo;

                                        if (infolength > 64)
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
                                    temp = data.Substring(diff * 2, infolength * 2);
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

        private string ToLittleEndian(byte[] decodebyte)
        {
            var sb = new StringBuilder();
            for (int i = decodebyte.Length - 1; i >= 0; i--)
            {
                sb.Append(decodebyte[i].ToString("x02"));
            }
            return sb.ToString();
        }

        private string bigEndian(byte[] decodebyte)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < decodebyte.Length; i++)
            {
                sb.Append(decodebyte[i].ToString("x02"));
            }
            return sb.ToString();
        }

        private string Logger_State(byte[] decodebyte)
        {
            String VALUE = "UNDEFINED";

            switch (decodebyte[0])
            {
                case 0:
                    VALUE = "Ready";
                    break;
                case 1:
                    VALUE = "Delay";
                    break;
                case 2:
                    VALUE = "Running";
                    break;
                case 3:
                    VALUE = "Stopped";
                    break;
                case 4:
                    VALUE = "Undefined";
                    break;
            }

            return VALUE;
        }

        public String UNIXtoUTC(long now)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(now * 1000);
            date = date.ToUniversalTime();
            var simpledate = date.ToString("yyyy-MM-dd HH:mm:sss UTC");
            return simpledate;
        }

        private void Init_Sensor_Statistics_Field(int currentChannel)
        {
            recordedSamples = 0;

            sensorMin[currentChannel] = 0xFFFFFFFFFFL;
            sensorMax[currentChannel] = -274;

            highestPosition[currentChannel] = 0;
            lowestPosition[currentChannel] = 0;

            Mean[currentChannel] = 0;
            MKT[currentChannel] = 0;

            withinLimit[currentChannel] = 0;
            belowLimit[currentChannel] = 0;
            aboveLimit[currentChannel] = 0;
        }

        private void Temperature_Statistics(int currentChannel, double Value, int index)
        {
            if (Value < sensorMin[currentChannel])
            {
                lowestPosition[currentChannel] = index + 1;// Cause it starts from zero
                sensorMin[currentChannel] = Value;
            }

            if (Value > sensorMax[currentChannel])
            {
                highestPosition[currentChannel] = index + 1;
                sensorMax[currentChannel] = Value;
            }


            if (Value > upperLimit[currentChannel])
            {
                aboveLimit[currentChannel]++;
            }
            else if (Value < lowerLimit[currentChannel])
            {
                belowLimit[currentChannel]++;
            }
            else
            {
                withinLimit[currentChannel]++;
            }

            //exp(-Delta_H/R x Tn)
            MKT[currentChannel] += Math.Exp((-Delta_H) / ((Value + Kelvin_Dec) * R));
            Mean[currentChannel] += Value;
            recordedSamples++;
        }

        private void Finalize_Statistics(int currentChannel)
        {
            Mean[currentChannel] /= recordedSamples;
            MKT[currentChannel] = (Delta_H / R) / (-Math.Log(MKT[currentChannel] / recordedSamples));
        }

        public String HHMMSS(double mseconds)
        {
            int hours = (int)(mseconds / 3600);
            int minutes = (int)((mseconds % 3600) / 60);
            int seconds = (int)(mseconds % 60);
            
            return $"{hours}:{minutes}:{seconds}"; 
        }
    }
}
