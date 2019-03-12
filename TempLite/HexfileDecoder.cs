using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using TempLite.Services;

namespace TempLite
{
    public class HexfileDecoder
    {
        string loggerState;
        string batteryPercentage;
        string timeAtFirstSameple;
        string userData;

        bool loopOverwrite = false;
        bool Fahrenheit = false;

        double Kelvin_Dec = 273.15;
        double lowestTemp = 0;
        double Resolution = 0;

        long Y_2000 = 946684800000L;
        long timeFirstSample = 0;
        long samplePeriod = 0;
        long UTCreferenceTime = 0;
        long ticksAtLastSample = 0;
        long ticksSinceStart = 0;
        long secondsTimer = 0;
        long manufactureDate = 0;
        
        int recordedSamples = 0;
        int numberChannel = 0;
        int userDataLength = 0;
        int startDelay = 0;
        int totalRTCticks = 0;
        int totalSamplingEvents = 0;
        int totalUses = 0;

        int[] highestPosition = { 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] lowestPosition = { 0, 0, 0, 0, 0, 0, 0, 0 };

        double Delta_H = 83.14472; //Delta H   is the Activation Energy:   83.14472    kJ/mole
        double R = 8.314472; //R is the Gas Constant: 0.008314472 J/mole/degree
        
        double[] Data;
        double[] upperLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] lowerLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] sensorMin = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] sensorMax = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] Mean = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] MKT = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] withinLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] belowLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] aboveLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };

        public HexfileDecoder(LoggerInformation loggerInformation)
        {
            this.loggerInformation = loggerInformation;
            this.serialNumber = loggerInformation.SerialNumber;
            this.jsonFile = loggerInformation.JsonFile;
        }

        public void ReadIntoJsonFileAndSetupDecoder()
        {
            var jsonObject = GetJsonObject();
            numberChannel = Convert.ToInt32(ReadFromJObject(jsonObject, "SENSOR,SensorNumber"), 16);
            userData = ReadFromJObject(jsonObject, "USER_DATA,UserData");
            loggerState = ReadFromJObject(jsonObject, "HEADER,State");
            batteryPercentage = ReadFromJObject(jsonObject, "BATTERY_INFO,Battery") + "%";
            Fahrenheit = Convert.ToBoolean(ReadFromJObject(jsonObject, "USER_SETTINGS,Fahrenheit"));
            UTCreferenceTime = Convert.ToInt32(ReadFromJObject(jsonObject, "UTCReferenceTime"), 16);
            totalRTCticks = Convert.ToInt32(ReadFromJObject(jsonObject, "HEADER,TotalRTCTicks"), 16);
            manufactureDate = Convert.ToInt32(ReadFromJObject(jsonObject, "HEADER,ManufactureDate"));
            totalSamplingEvents = Convert.ToInt32(ReadFromJObject(jsonObject, "HEADER,TotalSamplingEvents"), 16);
            totalUses = Convert.ToInt32(ReadFromJObject(jsonObject, "HEADER,TotalUses"), 16);
            loopOverwrite = Convert.ToBoolean(ReadFromJObject(jsonObject, "DATA_INFO,Overwritten"));
            startDelay = Convert.ToInt32(ReadFromJObject(jsonObject, "USER_SETTINGS,StartDelay"), 16);
            samplePeriod = Convert.ToInt32(ReadFromJObject(jsonObject, "USER_SETTINGS,SamplingPeriod"), 16);
            secondsTimer = Convert.ToInt32(ReadFromJObject(jsonObject, "SecondsTimer"), 16);
            ticksSinceStart = Convert.ToInt32(ReadFromJObject(jsonObject, "DATA_INFO,TicksSinceArousal"), 16);
            timeAtFirstSameple = ReadFromJObject(jsonObject, "DATA_INFO,Time_FirstSample_MonT");
            ticksAtLastSample = Convert.ToInt32(ReadFromJObject(jsonObject, "DATA_INFO,TicksAtLastSample"), 16);
            recordedSamples = Convert.ToInt32(ReadFromJObject(jsonObject, "DATA_INFO,SamplesNumber"));
            lowestTemp = Convert.ToDouble(ReadFromJObject(jsonObject, "USER_SETTINGS,LowestTemp"));
            Resolution = Convert.ToDouble(ReadFromJObject(jsonObject, "USER_SETTINGS,ResolutionRatio")) / 100;
            ReadFromJObject(jsonObject, "SENSOR,Decode_MonT_Data");
            upperLimit[0] = Convert.ToDouble(ReadFromJObject(jsonObject, "CHANNEL_INFO,UpperLimit"));
            lowerLimit[0] = Convert.ToDouble(ReadFromJObject(jsonObject, "CHANNEL_INFO,LowerLimit"));
        }

        readonly LoggerInformation loggerInformation;
        readonly string serialNumber;
        readonly string jsonFile;

        public PDFvariables AssignPDFValue ()
        {
            var pdfVariables = new PDFvariables();

            pdfVariables.RecordedSamples = recordedSamples;
            pdfVariables.SerialNumber = serialNumber;
            pdfVariables.LoggerState = loggerState;
            pdfVariables.BatteryPercentage = batteryPercentage;
            pdfVariables.SameplePeriod = HHMMSS(samplePeriod);
            pdfVariables.StartDelay = HHMMSS(startDelay);
            pdfVariables.FirstSample = timeAtFirstSameple;
            pdfVariables.TagsPlaced = "0";
            pdfVariables.UserData = userData;

            for (int i = 0; i < pdfVariables.RecordedSamples; i++)
            {
                pdfVariables.Time.Add(timeFirstSample);
                timeFirstSample = timeFirstSample + samplePeriod;
            }

            var timeLastSample = Convert.ToInt32(pdfVariables.Time[(pdfVariables.Time.Count - 1)]);
            pdfVariables.LastSample = UNIXtoUTC(timeLastSample);

            if (Fahrenheit)
                pdfVariables.TempUnit = " °F";

            else
                pdfVariables.TempUnit = " °C";
            
            AssignChannelValues(pdfVariables.ChannelOne, 0);

            if (numberChannel > 1)
            {
                pdfVariables.IsChannelTwoEnabled = true;
                AssignChannelValues(pdfVariables.ChannelTwo, 1);
            }

            return pdfVariables;
        }

        private void AssignChannelValues (ChannelConfig Channel, int i)
        {
            Channel.PresetLowerLimit = lowerLimit[i];
            Channel.PresetUpperLimit = upperLimit[i];
            Channel.Mean = Mean[i];
            Channel.MKT_C = MKT[i] - Kelvin_Dec;
            Channel.Max = sensorMax[i];
            Channel.Min = sensorMin[i];
            Channel.WithinLimits = withinLimit[i];
            Channel.OutsideLimits = aboveLimit[i] + belowLimit[i];
            Channel.AboveLimits = aboveLimit[i];
            Channel.BelowLimits = belowLimit[i];
            Channel.TimeWithinLimits = HHMMSS(withinLimit[i] * samplePeriod);
            Channel.TimeOutLimits = HHMMSS((aboveLimit[i] + belowLimit[i]) * samplePeriod);
            Channel.TimeAboveLimits = HHMMSS(aboveLimit[i] * samplePeriod);
            Channel.TimeBelowLimits = HHMMSS(belowLimit[i] * samplePeriod);

            if (Channel.AboveLimits > 0)
                Channel.BreachedAbove = " (breached)";

            if (Channel.BelowLimits > 0)
                Channel.BreachedBelow = " (breached)";

            Channel.Data = Data;
        }

        private byte[] ReadHex(string[] currentinfo)
        {
            byte[] bytes = { };
            string addtoread = "";

            try
            {
                using (var sr = new StreamReader(serialNumber + ".hex"))
                {
                    string line;
                    int diff = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string address = line.Substring(0, 6);
                        string data = line.Substring(7, line.Length - 7);
                        string temp = "";

                        if (Convert.ToInt32(currentinfo[0], 16) >= Convert.ToInt32(address, 16))
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
                                    return bytes;
                                }
                                else
                                {
                                    temp = data.Substring(diff * 2, infolength * 2);
                                    int totallength = temp.Length;
                                    bytes = new byte[totallength / 2];
                                    for (int i = 0; i < totallength; i += 2)
                                        bytes[i / 2] = (byte)(Convert.ToByte(temp.Substring(i, 2), 16));
                                    return bytes;
                                }
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

        #region Reading Json File
        private string ReadFromJObject(JObject jsonObject, string info)
        {
            var decodeInfo = JsontoString(jsonObject, info);
            return CallDecodeFunctions(decodeInfo);
        }

        private JObject GetJsonObject()
        {
            using (var sr = new StreamReader(jsonFile))
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
        #endregion

        #region Decoding Hex Functions 
        private string CallDecodeFunctions(string[] stringArrayInfo)
        {
            bool bitbool = false;
            var decodeByte = ReadHex(stringArrayInfo);

            switch (stringArrayInfo[2])
            {

                case "_1_Byte_to_Decimal":
                    return _1ByteToDecimal(stringArrayInfo, decodeByte);

                case "_2_Byte_to_Decimal":
                    return (((decodeByte[1] & 0xFF) << 8) | (decodeByte[0] & 0xFF)).ToString();

                case "_2_Byte_to_Temperature_MonT":
                    return _2ByteToTemperatureMonT(decodeByte);

                case "_4_Byte_to_Decimal":
                    return decodeByte[3].ToString("X02") + decodeByte[2].ToString("X02") + decodeByte[1].ToString("X02") + decodeByte[0].ToString("X02");

                case "_4_Byte_to_UNIX":
                    return _4ByteToUNIX(decodeByte);

                case "_4_Byte_Sec_to_Date":
                    return _4ByteSectoDec(decodeByte);

                case "_8_Byte_to_Unix_UTC":
                    return ToBigEndian(decodeByte);

                case "b0":
                    bitbool = false;
                    if (GetBit(decodeByte[0], 0) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "b1":
                    bitbool = false;
                    if (GetBit(decodeByte[0], 1) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "b2":
                    bitbool = false;
                    if (GetBit(decodeByte[0], 2) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "b3":
                    bitbool = false;
                    if (GetBit(decodeByte[0], 3) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "b4":
                    bitbool = false;
                    if (GetBit(decodeByte[0], 4) != 0)
                        bitbool = true;
                    return bitbool.ToString();


                case "b5":
                    bitbool = false;
                    if (GetBit(decodeByte[0], 5) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "b6":
                    bitbool = false;
                    if (GetBit(decodeByte[0], 6) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "b7":
                    bitbool = false;
                    if (GetBit(decodeByte[0], 7) != 0)
                        bitbool = true;
                    return bitbool.ToString();

                case "Battery_MonT":
                    return BatteryMonT(decodeByte);

                case "Channel_1_MonT":
                    return "1";

                case "Decode_MonT_Data":
                    DecodeMonTData(decodeByte);
                    break;

                case "DJNZ_2_Byte_Type_1":
                    decodeByte[1]--;
                    return ToLittleEndian(decodeByte);

                case "DJNZ_4_Byte_Type_2":
                    return DJNZ4ByteType_2(decodeByte);

                case "*Logger_State":
                    return LoggerState(decodeByte);

                case "SampleNumber_logged_MonT":
                    return SampleNumberLoggedMonT(decodeByte);

                case "String":
                    return String(decodeByte);

                case "Time_FirstSample_MonT":
                    return TimeFirstSampleMonT(decodeByte);
            }

            return string.Empty;
        }

        private string _1ByteToDecimal(string[] stringArrayInfo, byte[] decodeByte)
        {
            if (decodeByte.Length > 1)
            {
                for (int i = 0; i < decodeByte.Length; i++)
                    decodeByte[i] = (byte)(decodeByte[i] & 0xff);

                if (stringArrayInfo[3] == "0")
                    return ToBigEndian(decodeByte);
                else
                    return string.Empty;
            }
            else
            {
                decodeByte[0] = (byte)(decodeByte[0] & 0xff);
                return ToBigEndian(decodeByte);
            }
        }

        private string _2ByteToTemperatureMonT(byte[] decodeByte)
        {
            var value = (((decodeByte[1]) & 0xFF) << 8) | (decodeByte[0] & 0xFF);
            value -= 4000;
            var V = ((double)value / 100);
            return V.ToString("F");
        }

        private string _4ByteToUNIX(byte[] decodeByte)
        {
            long _4byteUnix = (Convert.ToInt32(ToLittleEndian(decodeByte), 16) * (long)1000);
            manufactureDate = ((long)_4byteUnix + Y_2000) / (long)1000;
            return manufactureDate.ToString();
        }

        private string _4ByteSectoDec(byte[] decodeByte)
        {
            long _4sectobyte = (decodeByte[3] + decodeByte[2] + decodeByte[1] + decodeByte[0]) * 1000;

            if (_4sectobyte > 0)
            {
                _4sectobyte += Y_2000;
                DateTime date = new DateTime(_4sectobyte);
                return date.ToString("dd/MM/yyyy HH:mm:sss");
            }
            else
                return string.Empty;
        }

        private int GetBit(int Value, int bit)
        {
            return (Value >> bit) & 1;
        }

        private string BatteryMonT(byte[] decodeByte)
        {
            var RCLBatteryMargin = 0.25;    // i.e. leave 25% in reserve
            var RCLSelfDischargeCurrentuA = 0.630;
            var RCLQuiescentDischargeCurrentuA = 9.000;
            var RCLConversionDischargeCurrentuA = 1300.0;
            var RCLDownloadDischargeCurrentuA = 2900.00;
            var RCLConversionDurationS = 1.0;
            var RCLDownloadDurationS = 3.5;
            var RCLAssumedDownloadsPerTrip = 2.0;

            var INITAL_BATTERY = (int)((1.0 - RCLBatteryMargin) * 0.56 * 60 * 60 * 1000000); //mAHr
            var BatteryUsed =
            (((UTCreferenceTime - manufactureDate) * RCLSelfDischargeCurrentuA) +
            (totalRTCticks * RCLQuiescentDischargeCurrentuA) +
            ((totalSamplingEvents + 4681) * RCLConversionDurationS * RCLConversionDischargeCurrentuA) +
            (RCLAssumedDownloadsPerTrip * totalUses * RCLDownloadDurationS * RCLDownloadDischargeCurrentuA));
            var batteryPercentage = (int)(((INITAL_BATTERY - BatteryUsed) / INITAL_BATTERY) * 100);
            return batteryPercentage.ToString();
        }

        private void DecodeMonTData(byte[] decodeByte)
        {
            int a = 0;
            int b = 0;
            int array_pointer = 0;
            bool Flag_End = false;

            if (recordedSamples > 0)
            {
                var Data = new double[recordedSamples];
                Init_Sensor_Statistics_Field(0);

                if (loopOverwrite)
                {
                    while ((b < decodeByte.Length))
                    {

                        if (decodeByte[b] == 0xFF) //0xFF = 255 Two's complement
                        {
                            break;
                        }
                        b++;
                    }
                    b++;  //Go to the index after the 0xFF
                    while ((b < decodeByte.Length)) //reads the data after the 0xFF
                    {
                        Temperature_Statistics(0, lowestTemp + ((decodeByte[b] & 0xFF) * Resolution), array_pointer);
                        Data[array_pointer++] = (lowestTemp + ((decodeByte[b] & 0xFF) * Resolution));
                        b++;
                    }

                }

                while ((a < decodeByte.Length) && (!Flag_End))
                {
                    if (decodeByte[a] == 0xFE || decodeByte[a] == 0xFF) //0xFE = 254 Two's complement || 0xFF = 255 Two's complement
                    {
                        Flag_End = true;
                    }
                    else
                    {
                        Temperature_Statistics(0, lowestTemp + ((decodeByte[a] & 0xFF) * Resolution), array_pointer);
                        Data[array_pointer++] = (lowestTemp + ((decodeByte[a] & 0xFF) * Resolution));
                    }
                    a++;
                }
                this.Data = Data;
            }
            Finalize_Statistics(0);
        }

        private string DJNZ4ByteType_2(byte[] decodeByte)
        {
            for (int i = 0; i < 4; i++)
            {
                int z = (0x100) - (decodeByte[i] & 0xFF);
                decodeByte[i] = (byte)z;
            }
            return ToLittleEndian(decodeByte);
        }

        private string SampleNumberLoggedMonT(byte[] decodeByte)
        {
            if (loopOverwrite)
                return "4681";
            else
            {
                long samplenumber = (((long)(ticksAtLastSample - startDelay) / samplePeriod) + 1);
                return samplenumber.ToString();
            }
        }

        private string String(byte[] decodeByte)
        {
            var userdatastring = string.Empty;
            for (int i = 0; i < decodeByte.Length; i++)
            {
                userdatastring += (char)(decodeByte[i] & 0xFF);
            }

            return userdatastring.Substring(0, userDataLength);
        }

        private string TimeFirstSampleMonT(byte[] decodeByte)
        {
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
        }

        public string HHMMSS(double mseconds)
        {
            int hours = (int)(mseconds / 3600);
            int minutes = (int)((mseconds % 3600) / 60);
            int seconds = (int)(mseconds % 60);

            return $"{hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}";
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

        private string ToBigEndian(byte[] decodebyte)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < decodebyte.Length; i++)
            {
                sb.Append(decodebyte[i].ToString("x02"));
            }
            return sb.ToString();
        }

        private string LoggerState(byte[] decodebyte)
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

        private string UNIXtoUTC(long now)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(now * 1000);
            date = date.ToUniversalTime();
            var simpledate = date.ToString("yyyy-MM-dd HH:mm:sss UTC");
            return simpledate;
        }

        public string UNIXtoUTCDate (long now)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(now * 1000);
            date = date.ToUniversalTime();
            var simpledate = date.ToString("yyyy-MM-dd");
            return simpledate;
        }

        public string UNIXtoUTCTime(long now)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(now * 1000);
            date = date.ToUniversalTime();
            var simpledate = date.ToString("HH:mm:sss UTC");
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
            
            MKT[currentChannel] += Math.Exp((-Delta_H) / ((Value + Kelvin_Dec) * R));
            Mean[currentChannel] += Value;
            recordedSamples++;
        }

        private void Finalize_Statistics(int currentChannel)
        {
            Mean[currentChannel] /= recordedSamples;
            MKT[currentChannel] = (Delta_H / R) / (-Math.Log(MKT[currentChannel] / recordedSamples));
        }
        #endregion
        
    }
}
