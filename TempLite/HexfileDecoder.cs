using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TempLite
{
    public class HexFileDecoder
    {
        string loggerName = string.Empty;
        string loggerState = string.Empty;
        string batteryPercentage = string.Empty;
        string userData = string.Empty;
        string emailID =  string.Empty;

        bool loopOverwrite = false;
        bool fahrenheit = false;
        bool notOverflown = false;
        bool ch1Enabled = false;
        bool ch2Enabled = false;

        long samplePeriod = 0;
        long secondsTimer = 0;
        long utcReferenceTime = 0;
        long timeAtFirstSameple = 0;
        long ticksAtLastSample = 0;
        long ticksSinceStart = 0;
        long manufactureDate = 0;

        int numberChannel = 0;
        int userDataLength = 0;
        int startDelay = 0;
        int totalRTCticks = 0;
        int totalSamplingEvents = 0;
        int totalUses = 0;
        int loopOverwriteStartAddress = 0;
        int dataAddress = 32767;
        int recordedSamples = 0;
        int mont2Samples = 0;
        int tagNumbers = 0;
        int samplePointer = 0;
        int sensorNumber = 0;

        int[] compressionTable = new int[128];

        double lowestTemp = 0;
        double resolution = 0;

        readonly int[] highestPosition = new int[8];
        readonly int[] lowestPosition = new int[8];
        readonly int[] sensorStartingValue = { 0, 0, 0, 0, 0, 0, 0, 0 };
        readonly int[] sensorTablePointer = { 0, 0, 0, 0, 0, 0, 0, 0 };
        readonly int[] sensorType = { 0, 0, 0, 0, 0, 0, 0, 0 };

        double[] upperLimit = new double[8];
        double[] lowerLimit = new double[8];
        readonly double[] sensorMin = new double[8];
        readonly double[] sensorMax = new double[8];
        readonly double[] mean = new double[8];
        readonly double[] mkt = new double[8];
        readonly double[] withinLimit = new double[8];
        readonly double[] belowLimit = new double[8];
        readonly double[] aboveLimit = new double[8];

        List<List<double>> Data = new List<List<double>>();
        List<int> Tag = new List<int>();

        readonly int Kelvin = 27315;
        readonly double KelvinDec = 273.15;
        readonly double Delta_H = 83.14472;     //Delta H   is the Activation Energy:   83.14472    kJ/mole
        readonly double R = 8.314472;           //R is the Gas Constant: 0.008314472 J/mole/degree
        readonly long Year2000 = 946684800000;
        readonly long Year2010 = 1262304000000;
        readonly int G4MemorySize = 32767;

        readonly LoggerInformation loggerInformation;
        readonly string serialNumber;
        readonly string jsonFile;
        readonly string path = AppDomain.CurrentDomain.BaseDirectory;

        public HexFileDecoder(LoggerInformation loggerInformation)
        {
            this.loggerInformation = loggerInformation;
            this.serialNumber = loggerInformation.SerialNumber;
            this.jsonFile = loggerInformation.JsonFile;
        }

        public void ReadIntoJsonFileAndSetupDecoder()
        {
            var jsonObject = GetJsonObject();
            Console.WriteLine("==========================================================================================================================================================");
            
            if (loggerInformation.LoggerName == DecodeConstant.G4)
            {
                loggerName = DecodeConstant.G4;
                dataAddress = ReadIntFromJObject(jsonObject, DecodeConstant.DataEndPointer);
                numberChannel = ReadIntFromJObject(jsonObject, DecodeConstant.NumberOfChannels);
                userDataLength = ReadIntFromJObject(jsonObject, DecodeConstant.UserDataLength);
                emailID = ReadStringFromJObject(jsonObject, DecodeConstant.EmailID);
                userData = ReadStringFromJObject(jsonObject, DecodeConstant.UserData);
                loggerState = ReadStringFromJObject(jsonObject, DecodeConstant.LoggerState);
                batteryPercentage = ReadIntFromJObject(jsonObject, DecodeConstant.BatteryPercentage) + DecodeConstant.Percentage;
                loopOverwriteStartAddress = ReadIntFromJObject(jsonObject, DecodeConstant.LoopOverwriteAddress);
                fahrenheit = ReadBoolFromJObject(jsonObject, DecodeConstant.IsFahrenhiet);
                utcReferenceTime = ReadIntFromJObject(jsonObject, DecodeConstant.UTCReferenceTime);
                totalRTCticks = ReadIntFromJObject(jsonObject, DecodeConstant.TotalRTCTicks);
                totalSamplingEvents = ReadIntFromJObject(jsonObject, DecodeConstant.TotalSamplingEvents);
                totalUses = ReadIntFromJObject(jsonObject, DecodeConstant.TotalUses);
                startDelay = ReadIntFromJObject(jsonObject, DecodeConstant.StartDelay);
                samplePeriod = ReadIntFromJObject(jsonObject, DecodeConstant.SamplePeriod);
                ticksSinceStart = ReadIntFromJObject(jsonObject, DecodeConstant.TicksSinceStart);
                ticksAtLastSample = ReadIntFromJObject(jsonObject, DecodeConstant.TicksSinceLastSample);
                recordedSamples = ReadIntFromJObject(jsonObject, DecodeConstant.TotalRecordedSamples);
                timeAtFirstSameple = ReadLongFromJObject(jsonObject, DecodeConstant.TimeAtFirstSample);
                ReadStringFromJObject(jsonObject, DecodeConstant.CompressionTable);
                ReadStringFromJObject(jsonObject, DecodeConstant.Sensor);
                lowerLimit = ReadArrayFromJObject(jsonObject, DecodeConstant.LowerLimit);
                upperLimit = ReadArrayFromJObject(jsonObject, DecodeConstant.UpperLimit);
                ReadStringFromJObject(jsonObject, DecodeConstant.Data);
            }

            else if (loggerInformation.LoggerName == DecodeConstant.MonT)
            {
                loggerName = DecodeConstant.MonT;
                numberChannel = ReadIntFromJObject(jsonObject, DecodeConstant.NumberOfChannels);
                userDataLength = ReadIntFromJObject(jsonObject, DecodeConstant.UserDataLength);
                userData = ReadStringFromJObject(jsonObject, DecodeConstant.UserData);
                loggerState = ReadStringFromJObject(jsonObject, DecodeConstant.LoggerState);
                fahrenheit = ReadBoolFromJObject(jsonObject, DecodeConstant.IsFahrenhiet);
                utcReferenceTime = ReadIntFromJObject(jsonObject, DecodeConstant.UTCReferenceTime);
                totalRTCticks = ReadIntFromJObject(jsonObject, DecodeConstant.TotalRTCTicks);
                manufactureDate = ReadLongFromJObject(jsonObject, DecodeConstant.ManufactureDate);
                totalSamplingEvents = ReadIntFromJObject(jsonObject, DecodeConstant.TotalSamplingEvents);
                totalUses = ReadIntFromJObject(jsonObject, DecodeConstant.TotalUses);
                batteryPercentage = ReadIntFromJObject(jsonObject, DecodeConstant.BatteryPercentage) + DecodeConstant.Percentage;
                loopOverwrite = ReadBoolFromJObject(jsonObject, DecodeConstant.IsLoopOverwrite);
                startDelay = ReadIntFromJObject(jsonObject, DecodeConstant.StartDelay);
                samplePeriod = ReadIntFromJObject(jsonObject, DecodeConstant.SamplePeriod);
                secondsTimer = ReadIntFromJObject(jsonObject, DecodeConstant.SecondTimer);
                ticksSinceStart = ReadIntFromJObject(jsonObject, DecodeConstant.TicksSinceStart);
                ticksAtLastSample = ReadIntFromJObject(jsonObject, DecodeConstant.TicksSinceLastSample);
                timeAtFirstSameple = ReadLongFromJObject(jsonObject, DecodeConstant.MonTTimeAtFirstSample);
                lowestTemp = Convert.ToDouble(ReadStringFromJObject(jsonObject, DecodeConstant.LowestTemp));
                resolution = (double)ReadIntFromJObject(jsonObject, DecodeConstant.ResolutionRatio) / 100;
                recordedSamples = ReadIntFromJObject(jsonObject, DecodeConstant.TotalRecordedSamples);
                lowerLimit = ReadArrayFromJObject(jsonObject, DecodeConstant.LowerLimit);
                upperLimit = ReadArrayFromJObject(jsonObject, DecodeConstant.UpperLimit);
                ReadStringFromJObject(jsonObject, DecodeConstant.MonTData);
            }

            else if (loggerInformation.LoggerName.Substring(0,6) == DecodeConstant.MonT2)
            {
                loggerName = ReadStringFromJObject(jsonObject, DecodeConstant.MonT2Model);
                numberChannel = ReadIntFromJObject(jsonObject, DecodeConstant.NumberOfChannels);
                ch1Enabled = ReadBoolFromJObject(jsonObject, DecodeConstant.ChannelOneEnable);
                if(ch1Enabled) sensorNumber++;
                ch2Enabled = ReadBoolFromJObject(jsonObject, DecodeConstant.ChannelTwoEnable);
                if (ch2Enabled) sensorNumber++;
                userDataLength = 104;
                userData = ReadStringFromJObject(jsonObject, DecodeConstant.UserData);
                loggerState = ReadStringFromJObject(jsonObject, DecodeConstant.LoggerState);
                fahrenheit = ReadBoolFromJObject(jsonObject, DecodeConstant.IsFahrenhiet);
                utcReferenceTime = ReadIntFromJObject(jsonObject, DecodeConstant.UTCReferenceTime);
                totalSamplingEvents = ReadIntFromJObject(jsonObject, DecodeConstant.TotalSamplingEvents);
                totalUses = ReadIntFromJObject(jsonObject, DecodeConstant.TotalUses);
                batteryPercentage = ReadIntFromJObject(jsonObject, DecodeConstant.BatteryPercentage) + DecodeConstant.Percentage;
                loopOverwrite = ReadBoolFromJObject(jsonObject, DecodeConstant.LoopOverwrite);
                notOverflown = ReadBoolFromJObject(jsonObject, DecodeConstant.NotOverflown);
                startDelay = ReadIntFromJObject(jsonObject, DecodeConstant.StartDelay);
                samplePeriod = ReadIntFromJObject(jsonObject, DecodeConstant.SamplePeriod);
                ticksSinceStart = ReadIntFromJObject(jsonObject, DecodeConstant.TicksSinceStart);
                ticksAtLastSample = ReadIntFromJObject(jsonObject, DecodeConstant.TicksSinceLastSample);
                timeAtFirstSameple = ReadLongFromJObject(jsonObject, DecodeConstant.MonT2TimeAtFirstSample);
                recordedSamples = ReadIntFromJObject(jsonObject, DecodeConstant.TotalRecordedSamples);
                samplePointer = ReadIntFromJObject(jsonObject, DecodeConstant.SamplePointer);
                lowerLimit[0] = Convert.ToDouble(ReadStringFromJObject(jsonObject, DecodeConstant.MonT2LowerLimitCh1));
                lowerLimit[1] = Convert.ToDouble(ReadStringFromJObject(jsonObject, DecodeConstant.MonT2LowerLimitCh2));
                upperLimit[0] = Convert.ToDouble(ReadStringFromJObject(jsonObject, DecodeConstant.MonT2UpperLimitCh1));
                upperLimit[1] = Convert.ToDouble(ReadStringFromJObject(jsonObject, DecodeConstant.MonT2UpperLimitCh2));
                ReadStringFromJObject(jsonObject, DecodeConstant.MonT2DecodeValues);
            }
        }

        public LoggerVariables AssignLoggerValue()
        {
            ReadIntoJsonFileAndSetupDecoder();

            var loggerVariable = new LoggerVariables();
            loggerInformation.LoggerName = loggerName;
            loggerVariable.RecordedSamples = recordedSamples;
            loggerVariable.SerialNumber = serialNumber;
            loggerVariable.LoggerState = loggerState;
            loggerVariable.BatteryPercentage = batteryPercentage;
            loggerVariable.SameplePeriod = HHMMSS(samplePeriod);
            loggerVariable.StartDelay = HHMMSS(startDelay);
            loggerVariable.FirstSample = UNIXtoUTC(timeAtFirstSameple);
            loggerVariable.LastSample = UNIXtoUTC(timeAtFirstSameple);
            loggerVariable.TagsPlaced = tagNumbers;
            loggerVariable.TotalTrip = totalUses;
            loggerVariable.UserData = userData.Substring(0,userDataLength);
            loggerVariable.Tag = Tag;

            for(int i = 0; i < Tag.Count; i++)
            {
                Console.WriteLine("Tag : " + Tag[i]);
            }

            loggerInformation.EmailId = emailID;

            if (batteryPercentage == "255%")
            {
                loggerVariable.BatteryPercentage = "100%";
            }

            for (int i = 0; i < loggerVariable.RecordedSamples; i++)
            {
                loggerVariable.Time.Add(timeAtFirstSameple);
                timeAtFirstSameple = timeAtFirstSameple + samplePeriod;
            }

            if (recordedSamples > 0)
            {
                var timeLastSample = Convert.ToInt32(loggerVariable.Time[(loggerVariable.Time.Count - 1)]);
                loggerVariable.LastSample = UNIXtoUTC(timeLastSample);
            }

            AssignChannelValues(loggerVariable.ChannelOne, 0);

            if (numberChannel > 1 || ch2Enabled)
            {
                loggerVariable.IsChannelTwoEnabled = true;
                AssignChannelValues(loggerVariable.ChannelTwo, 1);
            }
            
            return loggerVariable;
        }

        void AssignChannelValues(ChannelConfig Channel, int i)
        {
            Channel.PresetLowerLimit = lowerLimit[i];
            Channel.PresetUpperLimit = upperLimit[i];
            Channel.Mean = mean[i];
            Channel.MKT_C = mkt[i] - KelvinDec;
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
                Channel.BreachedAbove = DecodeConstant.Exceeded;

            if (Channel.BelowLimits > 0)
                Channel.BreachedBelow = DecodeConstant.Exceeded;

            if (Data.Count > 0)
                Channel.Data = Data[i];

            if (loggerInformation.LoggerType == 1 && i == 1)
            {
                Channel.Unit = DecodeConstant.Percentage;
            }
            else if (sensorType[i] == 0 || sensorType[i] == 6)
            {
                if (fahrenheit) Channel.Unit = DecodeConstant.Farenhiet;
                else Channel.Unit = DecodeConstant.Celcius;
            }
            else
            {
                Channel.Unit = DecodeConstant.Percentage;
            }
        }

        byte[] ReadHex(string[] currentinfo)
        {
            byte[] bytes = { };
            var addtoread = string.Empty;
            var hexPath = Path.GetTempPath() + "\\" + serialNumber + ".hex";

            try
            {
                if (File.Exists(hexPath))
                {
                    using (var sr = new StreamReader(hexPath))
                    {
                        string line;
                        int dataFromAddress = -99;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string address = line.Substring(0, 6);
                            string data = line.Substring(7, line.Length - 7);
                            string temp = string.Empty;

                            if (Convert.ToInt32(currentinfo[0], 16) >= Convert.ToInt32(address, 16))
                                dataFromAddress = Convert.ToInt32(currentinfo[0], 16) - Convert.ToInt32(address, 16);

                            if (dataFromAddress >= 0 && dataFromAddress < 58) // reader can only send 64bytes at a time
                            {
                                int lengthToRead = Convert.ToInt32(currentinfo[1]);

                                if (lengthToRead == 32768) // if we are reading DATA
                                {
                                    lengthToRead = dataAddress;

                                    if (loopOverwriteStartAddress > 0)
                                        lengthToRead = G4MemorySize + 1;
                                }

                                if (lengthToRead > 58)
                                {
                                    int readinfo = data.Length/2 - dataFromAddress;

                                    while (lengthToRead > 0)
                                    {
                                        Console.WriteLine("lengthToRead  : " + lengthToRead);
                                        Console.WriteLine("readinfo  : " + readinfo);
                                        Console.WriteLine("temp  : " + temp.Length);

                                        temp += data.Substring(dataFromAddress * 2, readinfo * 2);
                                        line = sr.ReadLine();
                                        if (line != null)
                                        {
                                            data = line.Substring(7, line.Length - 7);
                                            Console.WriteLine("data  : " + data);
                                            lengthToRead = lengthToRead - readinfo;
                                            dataFromAddress = 0;

                                            if (lengthToRead > (data.Length / 2))
                                            {
                                                readinfo = data.Length / 2;
                                            }
                                            else
                                            {
                                                readinfo = lengthToRead;
                                            }
                                        }
                                        else { break; }
                                    }

                                    int totallength = temp.Length;
                                    bytes = new byte[totallength / 2];
                                    for (int i = 0; i < totallength; i += 2)
                                        bytes[i / 2] = (byte)(Convert.ToByte(temp.Substring(i, 2), 16));
                                    return bytes;
                                }
                                else
                                {
                                    if (data.Length < (dataFromAddress + lengthToRead)*2)
                                    {
                                        var readNextLine = (dataFromAddress + lengthToRead - data.Length/2);
                                        temp = data.Substring(dataFromAddress * 2, (lengthToRead - readNextLine)*2 );
                                        line = sr.ReadLine();
                                        data = line.Substring(7, line.Length - 7);
                                        temp += data.Substring(0, readNextLine*2);
                                    }
                                    else
                                        temp = data.Substring(dataFromAddress * 2, lengthToRead * 2);
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
        string ReadStringFromJObject(JObject jsonObject, string info)
        {
            var decodeInfo = JsontoString(jsonObject, info);
            return CallDecodeFunctions(decodeInfo);
        }

        int ReadIntFromJObject(JObject jsonObject, string info)
        {
            var decodeInfo = JsontoString(jsonObject, info);
            return Convert.ToInt32(CallDecodeFunctions(decodeInfo), 16);
        }

        long ReadLongFromJObject(JObject jsonObject, string info)
        {
            var decodeInfo = JsontoString(jsonObject, info);
            return Convert.ToInt32(CallDecodeFunctions(decodeInfo));
        }

        double[] ReadArrayFromJObject(JObject jsonObject, string info)
        {
            var decodeInfo = JsontoString(jsonObject, info);
            var limit = CallDecodeFunctions(decodeInfo).Split(','); ;
            return Array.ConvertAll<string, double>(limit, Double.Parse);
        }

        bool ReadBoolFromJObject(JObject jsonObject, string info)
        {
            var decodeInfo = JsontoString(jsonObject, info);
            return Convert.ToBoolean(CallDecodeFunctions(decodeInfo));
        }

        JObject GetJsonObject()
        {
            using (var sr = new StreamReader(path + "\\Json\\" + jsonFile))
            {
                return JObject.Parse(sr.ReadToEnd());
            }
        }

        string[] JsontoString(JObject jsonObject, string info)
        {
            var add = jsonObject.GetValue(info).First.Last.ToString();
            var len = jsonObject.GetValue(info).First.Next.Last.ToString();
            var code = jsonObject.GetValue(info).First.Next.Next.Last.ToString();
            var hide = jsonObject.GetValue(info).Last.Last.ToString();

            return new string[] { add, len, code, hide };

        }
        #endregion

        #region Decoding Hex Functions 
        string CallDecodeFunctions(string[] stringArrayInfo)
        {
            bool bitbool = false;
            var decodeByte = ReadHex(stringArrayInfo);
            switch (stringArrayInfo[2])
            {
                case "_1_Byte_to_Boolean":
                    return _1ByteToBoolean(decodeByte);

                case "_1_Byte_to_Decimal":
                    return _1ByteToDecimal(decodeByte);

                case "_2_Byte_to_Decimal":
                    return _2ByteToDecimal(decodeByte);

                case "_2_Byte_to_Decimal_Big_Endian":
                    return ToBigEndian(decodeByte);

                case "_2_Byte_to_Temperature_MonT":
                    return _2ByteToTemperatureMonT(decodeByte);

                case "_3_Byte_to_Decimal":
                    return ToLittleEndian(decodeByte);

                case "_3_Byte_to_Temperature_ARRAY":
                    return _3BytetoTemperatureArray(decodeByte);

                case "_4_Byte_Built":
                    return _4ByteBuilt(decodeByte);

                case "_4_Byte_to_Decimal":
                    return ToLittleEndian(decodeByte);

                case "_4_Byte_to_Decimal_2":
                    return ToLittleEndian(decodeByte);

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

                case "CompressionTable":
                    compressionTable = CompressionTable(decodeByte);
                    break;

                case "Channel_1_MonT":
                    return "1";

                case "Decode_Delta_Data":
                    DecodeDeltaData(decodeByte);
                    break;

                case "Decode_MonT_Data":
                    DecodeMonTData(decodeByte);
                    break;

                case "DJNZ_2_Byte_Type_1":
                    decodeByte[1]--;
                    return ToLittleEndian(decodeByte);

                case "DJNZ_2_Byte_Type_2":
                    return DJNZ2ByteType2(decodeByte);

                case "DJNZ_4_Byte_Type_1":
                    return DJNZ4ByteType1(decodeByte);

                case "DJNZ_4_Byte_Type_2":
                    return DJNZ4ByteType_2(decodeByte);

                case "LoggingSelection":
                    return LoggingSelection(decodeByte);

                case "*Logger_State":
                    return LoggerState(decodeByte);

                case "MonT2_Model":
                    return MonT2Model(decodeByte);

                case "MonT2_Limit_Convert":
                    return MonT2LimitConvert(decodeByte);

                case "MonT2_SampleNum":
                    return MonT2SampleNum(decodeByte);

                case "MonT2_SensorNumber":
                    return decodeByte[0].ToString();

                case "MonT2_Value_Decode":
                    MonT2ValueDecode(decodeByte);
                    break;

                case "SamplePointer":
                    return decodeByte[0].ToString();

                case "SampleNumber_logged_MonT":
                    return SampleNumberLoggedMonT(decodeByte);

                case "SENSOR_Decoding":
                    SensorDecoding(decodeByte);
                    break;

                case "String":
                    return String(decodeByte);

                case "Time_FirstSample_MonT":
                    return TimeFirstSampleMonT(decodeByte);

                case "Time_FirstSample_MonT2":
                    return TimeFirstSampleMonT2(decodeByte);

                case "Time_LastSample_MonT":
                    return TimeLastSampleMonT(decodeByte);

                default:
                    break;
            }

            return string.Empty;
        }
        string _1ByteToBoolean(byte[] decodeByte)
        {
            if ((decodeByte[0] & 0xff) == 0)
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }
        string _1ByteToDecimal(byte[] decodeByte)
        {
            return decodeByte[0].ToString("x02");
        }

        string _2ByteToDecimal(byte[] decodeByte)
        {
            return (((decodeByte[1] & 0xFF) << 8) | (decodeByte[0] & 0xFF)).ToString("x02");
        }

        string _2ByteToTemperatureMonT(byte[] decodeByte)
        {
            var value = ((((decodeByte[1]) & 0xFF) << 8) | (decodeByte[0] & 0xFF));
            value -= 4000;
            var V = ((double)value / 100);
            return V.ToString("N2");
        }

        string _3BytetoTemperatureArray(byte[] decodeByte)
        {
            var offset = 9;
            var limitArray = new double[numberChannel];
            var limitString = string.Empty;

            for (int i = 0; i < numberChannel; i++)
            {
                var element = ((((decodeByte[(offset * i) + 2]) & 0xFF) << 16) | (((decodeByte[(offset * i) + 1]) & 0xFF) << 8) | (decodeByte[(offset * i)] & 0xFF));
                if (sensorType[i] == 0 || sensorType[i] == 6)
                {
                    element -= Kelvin;
                }

                limitArray[i] = element / 100;
            }

            for (int i = 0; i < limitArray.Length; i++)
            {
                if ((i + 1) == limitArray.Length)
                    limitString += limitArray[i].ToString();
                else limitString += limitArray[i].ToString() + ',';
            }

            return limitString;
        }
        string _4ByteBuilt(byte[] decodeByte)
        {
            long value = Convert.ToInt32(ToLittleEndian(decodeByte)) * 1000;

            if (value > 0)
            {
                value += Year2000;
                value -= Year2010;
                value /= 21600000;
                return value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        string _4ByteToUNIX(byte[] decodeByte)
        {
            long _4byteUnix = (Convert.ToInt32(ToLittleEndian(decodeByte), 16) * 1000);
            return CalculateTimeStarted(_4byteUnix).ToString();
        }
        string _4ByteSectoDec(byte[] decodeByte)
        {
            long _4sectobyte = (decodeByte[3] + decodeByte[2] + decodeByte[1] + decodeByte[0]) * 1000;

            if (_4sectobyte > 0)
            {
                _4sectobyte += Year2000;
                DateTime date = new DateTime(_4sectobyte);
                return date.ToString("dd/MM/yyyy HH:mm:sss");
            }
            else
                return string.Empty;
        }
        int GetBit(int Value, int bit)
        {
            return (Value >> bit) & 1;
        }
        string BatteryMonT(byte[] decodeByte)
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
            (((utcReferenceTime - manufactureDate) * RCLSelfDischargeCurrentuA) +
            (totalRTCticks * RCLQuiescentDischargeCurrentuA) +
            ((totalSamplingEvents + 4681) * RCLConversionDurationS * RCLConversionDischargeCurrentuA) +
            (RCLAssumedDownloadsPerTrip * totalUses * RCLDownloadDurationS * RCLDownloadDischargeCurrentuA));

            var batteryPercentage = (int)(((INITAL_BATTERY - BatteryUsed) / INITAL_BATTERY) * 100);
            return batteryPercentage.ToString("x02");
        }
        long CalculateTimeStarted(long timeFirstSample)
        {
            var timeStarted = ((timeFirstSample + Year2000) / 1000) + startDelay;
            var ticksSinceStop = ticksSinceStart - ticksAtLastSample;
            var totalNumberofTicks = ((G4MemorySize - 9) - (6 * numberChannel - 1)) / numberChannel; // what are these numbers 
            var totalLoggingTime = totalNumberofTicks * samplePeriod;

            if (loopOverwriteStartAddress == 0)
                timeStarted = utcReferenceTime - ticksSinceStart + startDelay;
            else
                timeStarted = utcReferenceTime - totalLoggingTime - ticksSinceStop;

            return timeStarted;
        }
        Boolean CheckStartSentinel(byte[] decodeByte, int memoryStart)
        {
            var startValue = new int[8];
            var currentSensor = 0;
            var check = true;

            while ((currentSensor < numberChannel) && check)
            {
                var addMSB = (memoryStart + (2 * currentSensor) + 1) & G4MemorySize;
                var addLSB = (memoryStart + (2 * currentSensor)) & G4MemorySize;

                var VaddMSB = (memoryStart + (2 * currentSensor) + (2 * numberChannel) + 1) & G4MemorySize;
                var VaddLSB = (memoryStart + (2 * currentSensor) + (2 * numberChannel)) & G4MemorySize;

                startValue[currentSensor] = (((decodeByte[addMSB]) & 0xff) << 8) | (decodeByte[addLSB] & 0xff);
                var verifyValue = ((decodeByte[VaddMSB] & 0xff) << 8) | (decodeByte[VaddLSB] & 0xff);
                if (startValue[currentSensor] != verifyValue)
                {
                    check = false;
                }
                sensorStartingValue[currentSensor] -= verifyValue;
                sensorStartingValue[currentSensor] *= -1;

                currentSensor++;
            }

            return check;
        }
        int[] CompressionTable(byte[] decodeByte)
        {
            var value = new int[decodeByte.Length / 2];

            for (int i = 0; i < decodeByte.Length; i += 2)
            {
                compressionTable[i / 2] = (((decodeByte[i] & 0xff) << 8) | (decodeByte[i + 1] & 0xff));
                value[i / 2] = compressionTable[i / 2];
            }

            return value;
        }
        void DecodeDeltaData(byte[] decodeByte)
        {
            var memoryStart = loopOverwriteStartAddress;
            if (memoryStart > 0)
            {
                memoryStart = FindStartSentinel(memoryStart - 1, 16, decodeByte);
                memoryStart++;
            }
            if (decodeByte.Length > 0)
            {
                if (CheckStartSentinel(decodeByte, memoryStart))
                {
                    memoryStart += ((6 * numberChannel) + 1);
                    memoryStart &= G4MemorySize;
                    memoryStart = FindStartSentinel(memoryStart, (8 * numberChannel), decodeByte);
                    if (memoryStart != 0xFFFF)
                    {
                        memoryStart++;
                        ReadStoreData(decodeByte, memoryStart);
                    }
                }
            }
        }
        void DecodeMonTData(byte[] decodeByte)
        {
            int a = 0;
            int b = 0;
            int array_pointer = 0;
            bool Flag_End = false;

            if (recordedSamples > 0)
            {
                var channelList = new List<double>();
                InitSensorStatisticsField(0);

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
                        TemperatureStatistics(0, lowestTemp + ((decodeByte[b] & 0xFF) * resolution), array_pointer);
                        channelList.Add(lowestTemp + ((decodeByte[b] & 0xFF) * resolution));
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
                        TemperatureStatistics(0, lowestTemp + ((decodeByte[a] & 0xFF) * resolution), array_pointer);
                        channelList.Add(lowestTemp + ((decodeByte[a] & 0xFF) * resolution));
                    }
                    
                    a++;
                }
                Data.Add(channelList);
            }
            FinalizeStatistics(0);
        }
        string DJNZ2ByteType2(byte[] decodeByte)
        {
            for (int i = 0; i < decodeByte.Length; i++)
            {
                decodeByte[i] = (byte)(0x100 - (decodeByte[i] & 0xFF));
            }
            return ToLittleEndian(decodeByte);
        }
        string DJNZ4ByteType1(byte[] decodeByte)
        {
            for (int i = 1; i < decodeByte.Length; i++)
            {
                decodeByte[i]--;
            }
            return ToLittleEndian(decodeByte);
        }
        string DJNZ4ByteType_2(byte[] decodeByte)
        {
            for (int i = 0; i < 4; i++)
            {
                int z = (0x100) - (decodeByte[i] & 0xFF);
                decodeByte[i] = (byte)z;
            }
            return ToLittleEndian(decodeByte);
        }
        int FindStartSentinel(int memoryStart, int max, byte[] decodeByte)
        {
            var maxI = (memoryStart + max);
            if (maxI > memoryStart)
            {
                while (maxI > memoryStart)
                {
                    if ((decodeByte[memoryStart] == 0x7F) || (decodeByte[memoryStart] == 0xff)) 
                    {
                        return memoryStart;
                    }
                    memoryStart++;
                    memoryStart &= G4MemorySize; // 0x007fff is to allow buffer rotation in case of Header in middle of end buffer?????????????
                }
            }
            else
            {
                memoryStart = 0;
                while (memoryStart <= maxI)
                {
                    if ((decodeByte[memoryStart] == 0x7f) || (decodeByte[memoryStart] & 0xff) == 0xff) // ????????
                    {
                        return memoryStart;
                    }
                    memoryStart++;
                    memoryStart &= G4MemorySize;
                }
            }
            return 0xffff;
        }
        public string HHMMSS(double mseconds)
        {
            int hours = (int)(mseconds / 3600);
            int minutes = (int)((mseconds % 3600) / 60);
            int seconds = (int)(mseconds % 60);

            return $"{hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}";
        }
        string LoggingSelection(byte[] decodeByte)
        {
            switch (decodeByte[0])
            {
                case 1:
                    return "Temperature";
                case 2:
                    return "Humidity";
                case 3:
                    return "Temperature-Humidity";
                default:
                    return "Undefined";
            }
        }
        string LoggerState(byte[] decodeByte)
        {
            if (loggerInformation.LoggerType == 1) // MonT2
            {
                switch (decodeByte[0])
                {
                    case 0:
                        return "Sleep";
                    case 1:
                        return "No Config";
                    case 2:
                        return "Ready";
                    case 3:
                        return "Start Delay";
                    case 4:
                        return "Logging";
                    case 5:
                        return "Stopped";
                    case 6:
                        return "Reuse";
                    case 7:
                        return "Error";
                    default:
                        return "Undefined";
                }
            }
            else
            {
                switch (decodeByte[0])
                {
                    case 0:
                        return "Ready";
                    case 1:
                        return "Delay";
                    case 2:
                        return "Logging";
                    case 3:
                        return "Stopped";
                    default:
                        return "Undefined";
                }
            }
        }
        string MonT2LimitConvert(byte[] decodeByte)
        {
            double Limit = (double)((short) (((decodeByte[1] & 0xFF) << 8) | (decodeByte[0] & 0xFF)) / 10);
            return Limit.ToString();
        }

        string MonT2Model(byte[] decodeByte)
        {
            switch (decodeByte[0])
            {
                case 1:
                    return "Mon T2 Plain";
                case 2:
                    return"Mon T2 Plain USB";
                case 3:
                    return "Mon T2 Plain RH USB";
                case 4:
                    return "Mon T2 LCD";
                case 5:
                    return "Mon T2 LCD USB";
                case 6:
                    return "Mon T2 LCD RH USB";
                case 7:
                    return "Mon T2 PDF";
                case 8:
                    return "Mon T2 RH PDF";
                case 9:
                    return "Mon T2 LCD PDF";
                case 10:
                    return "Mon T2 LCD RH PDF";
                case 11:
                    return "Mon T2 Plain RH";
                case 12:
                    return "Mon T2 LCD RH";
                default:
                    return "Unknown";
            }
        }

        string MonT2SampleNum(byte[] decodeByte)
        {
            int sampleNumber = ((((decodeByte[3]) & 0xFFFFFF) << 24) | (((decodeByte[2]) & 0xFFFF) << 16) | (((decodeByte[1]) & 0xFF) << 8) | (decodeByte[0] & 0xFF));

            if (sensorNumber == 2)
                sampleNumber *= 2;

            mont2Samples = sampleNumber;

            if (!notOverflown)
                sampleNumber =  16384;
            
            return sampleNumber.ToString("x02");
        }

        void MonT2ValueDecode(byte[] decodeByte)
        {
            var ch1List = new List<double>();
            var ch2List = new List<double>();
            var tagList = new List<int>();
            bool hasFlag;
            short sample1;
            short sample2;
            int index = 0;


            //When MonT2 is loop over writing
            if (!notOverflown && loopOverwrite)
            {
                var bytePointer = samplePointer * 2;
                var position = 0;
                var pageOffset = bytePointer % 512;
                var address = ((bytePointer / 512) + 1) * 512;
                address += pageOffset;

                var copyArrayList = new List<double>();
                //Also problem with MonT2
                //Creating a temporary array - 33280 is MAX memory size with an extra page of 512 bytes
                for (int i = 0; i < 33280; i++)
                {
                    copyArrayList.Add(decodeByte[i]);
                }
                //Copying the top half back in the array
                for (int i = address; i < 33280; i++)
                {
                    decodeByte[position] = (byte)(copyArrayList[i]);
                    position++;
                }
                //Copying the bottom half into the array
                for (int i = 0; i < samplePointer * 2; i++)
                {
                    decodeByte[position] = (byte)(copyArrayList[i]);
                    position++;
                }

            }
            
            if (ch1Enabled && ch2Enabled)
            {
                InitSensorStatisticsField(0);
                InitSensorStatisticsField(1);

                for (int i = 0; i < mont2Samples * 2; i++)
                {
                    sample1 = (short)(decodeByte[i] & 0xff);
                    i++;
                    sample1 |= (short)(decodeByte[i] << 8);
                    i++;
                    sample2 = (short)(decodeByte[i] & 0xff);
                    i++;
                    sample2 |= (short)(decodeByte[i] << 8);
                    hasFlag = ((sample1 & (0x0002)) != 0);

                    ch1List.Add(((double)(sample1 >> 2)) / 10);
                    ch2List.Add(((double)(sample2 >> 2)) / 10);
                    if (hasFlag)
                    {
                        tagList.Add(index);
                    }
                    
                    TemperatureStatistics(0, ((double)(sample1 >> 2)) / 10, index);
                    TemperatureStatistics(1, ((double)(sample2 >> 2)) / 10, index);
                    index++;
                }

                Data.Add(ch1List);
                Data.Add(ch2List);
                Tag = tagList;
                recordedSamples /= 2;

                FinalizeStatistics(0);
                FinalizeStatistics(1);
            }
            else if (ch1Enabled)
            {
                InitSensorStatisticsField(0);
                for (int i = 0; i < mont2Samples * 2; i++)
                {
                    sample1 = (short)(decodeByte[i] & 0xff);
                    i++;
                    sample1 |= (short)(decodeByte[i] << 8);
                    hasFlag = ((sample1 & (0x0002)) != 0);

                    ch1List.Add(((double)(sample1 >> 2)) / 10);
                    if (hasFlag)
                    {
                        tagList.Add(index);
                    }
                    
                    TemperatureStatistics(0, ((double)(sample1 >> 2)) / 10, index);
                    index++;
                }

                Data.Add(ch1List);
                Tag = tagList;


                FinalizeStatistics(0);
            }
            else if (ch2Enabled)
            {
                InitSensorStatisticsField(1);

                for (int i = 0; i < mont2Samples * 2; i++)
                {
                    sample2 = (short)(decodeByte[i] & 0xff);
                    i++;
                    sample2 |= (short)(decodeByte[i] << 8);
                    hasFlag = ((sample2 & (0x0002)) != 0);

                    ch2List.Add(((double)(sample2 >> 2)) / 10);
                    if (hasFlag)
                    {
                        tagList.Add(index);
                    }
                    
                    TemperatureStatistics(1, ((double)(sample2 >> 2)) / 10, index);
                    index++;
                }
                Data.Add(ch1List);
                Data.Add(ch2List);
                Tag = tagList;
                
                FinalizeStatistics(1);
            }
        }
        void ReadStoreData(byte[] decodeByte, int memoryStart)
        {
            var MaxReadingLength = (G4MemorySize - (6 * numberChannel) - 2) / numberChannel;

            for (int i = 0; i < numberChannel; i++)
            {
                var tagList = new List<int>();
                var channelList = new List<double>();
                var arrayPointer = 0;
                var dataPointer = (memoryStart + i) & G4MemorySize;                  //& 0x007FFF is to allow buffer rotation
                var totalSameples = 0;

                InitSensorStatisticsField(i);

                while ((dataPointer < decodeByte.Length) && (decodeByte[dataPointer] != 0x7F) && ((decodeByte[dataPointer] & 0xFF) != 0xFF) && (totalSameples < MaxReadingLength))
                {
                    if ((decodeByte[dataPointer] & 0xFF) == 0x80)
                    {
                        if (i == 0)
                        {
                            tagList.Add(dataPointer);
                            tagNumbers++;
                        }
                    }
                    else
                    {
                        if (decodeByte[dataPointer] > 0x7f)//> 0x7f
                        {
                            //Console.WriteLine("Value 0 : " + (double)sensorStartingValue[i]);
                            sensorStartingValue[i] -= decodeByte[dataPointer] & 0x7F;// & 0x7F;
                            //Console.WriteLine("channelList 0 : " + decodeByte[dataPointer].ToString("X02"));
                            //Console.WriteLine("Value 0 : " + (double)sensorStartingValue[i] / 100);
                        }
                        else
                        {
                            //Console.WriteLine("Value 1 : " + (double)sensorStartingValue[i] / 100);
                            sensorStartingValue[i] += decodeByte[dataPointer];
                            //Console.WriteLine("channelList 1 : " + decodeByte[dataPointer].ToString("X02"));
                            //Console.WriteLine("Value 1 : " + (double)sensorStartingValue[i] / 100);
                        }
                        
                        channelList.Add((double)sensorStartingValue[i] / 100);
                        TemperatureStatistics(i, ((double)sensorStartingValue[i] / 100), arrayPointer);
                        totalSameples++;
                    }

                    dataPointer += numberChannel;
                    dataPointer &= G4MemorySize;                                                           //& 0x007FFF is to allow buffer rotation
                }

                FinalizeStatistics(i);

                Data.Add(channelList);
                Tag = tagList;
            }
        }
        string SampleNumberLoggedMonT(byte[] decodeByte)
        {
            if (loopOverwrite)
                return "4681";
            else
            {
                long samplenumber = (((long)(ticksAtLastSample - startDelay) / samplePeriod) + 1);
                return samplenumber.ToString();
            }
        }
        void SensorDecoding(byte[] decodeByte)
        {
            var offset = 11;
            var sensorAddressArray = new string[2];

            for (int i = 0; i < numberChannel; i++)
            {
                var pointer = i * offset;
                sensorType[i] = decodeByte[pointer + 7]; // byte 7 is where the sensorType is stored
                sensorAddressArray[0] = decodeByte[pointer + 10].ToString("x02") + decodeByte[pointer + 9].ToString("x02");
                sensorAddressArray[1] = "21"; // size of the sensor information 

                var sensorInfoArray = ReadHex(sensorAddressArray);

                if (sensorInfoArray.Length != 0)
                {
                    var sensorData = (sensorInfoArray[20] & 0xFF) << 16 | (sensorInfoArray[19] & 0xFF) << 8 | (sensorInfoArray[18] & 0xFF);
                    if (sensorType[i] == 0 || sensorType[i] == 6 || sensorType[i] == 2) 
                    {
                        sensorStartingValue[i] = Kelvin - sensorData;
                    }
                    else
                    {
                        sensorStartingValue[i] = 0x1000000 - sensorData;
                    }
                }
            }
        }
        string String(byte[] decodeByte)
        {
            string UserDataString = string.Empty;

            for (int i = 0; i < decodeByte.Length; i++)
            {
                if (decodeByte[i] > 12 && decodeByte[i] < 127)
                {
                    if (decodeByte[i] == 13)
                        decodeByte[i] = 32;
                    UserDataString += Convert.ToChar(decodeByte[i]);
                }
            }
            return UserDataString;
        }
        string TimeFirstSampleMonT(byte[] decodeByte)
        {
            if (loopOverwrite)
            {
                timeAtFirstSameple = ((utcReferenceTime) - (4680 * samplePeriod) - (ticksSinceStart - ticksAtLastSample));
                return timeAtFirstSameple.ToString();
            }
            else
            {
                timeAtFirstSameple = ((utcReferenceTime - ticksSinceStart) + startDelay);
                return timeAtFirstSameple.ToString();
            }
        }
        string TimeFirstSampleMonT2(byte[] decodeByte)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            if (decodeByte[0] == (byte)0xFF && decodeByte[1] == (byte)0xFF && decodeByte[2] == (byte)0xFF && decodeByte[3] == (byte)0xFF && decodeByte[4] == (byte)0xFF && decodeByte[5] == (byte)0xFF)
            {
                return "0";
            }
            else
            {
                int year = (decodeByte[0] >> 1) + 2000;
                int month = (((decodeByte[1] & 0xff) >> 4) - 1);
                var dateTime = new DateTime(year, month, decodeByte[2], decodeByte[3], decodeByte[4], decodeByte[5]);
                
                if (!notOverflown && loopOverwrite)
                {
                    long val;
                    if (loggerState == "Logging")
                    {
                        val = ((utcReferenceTime) - ((16384 / sensorNumber) * samplePeriod) - (ticksSinceStart - ticksAtLastSample));
                        mont2Samples = 16384;
                        return val.ToString();
                    }
                    else
                    {
                        int seconds = (int)samplePeriod;
                        int maxMem = 16384;
                        if (sensorNumber == 2)
                        {
                            maxMem = maxMem / 2;
                        }

                        maxMem = ((int)mont2Samples / sensorNumber) - maxMem;
                        seconds = seconds * maxMem;

                        var date = dateTime.AddSeconds(seconds);
                        date = date.AddSeconds(startDelay);
                        mont2Samples = 16384;
                        return (date - epoch).TotalSeconds.ToString();
                    }
                }
                else
                {
                    dateTime = dateTime.AddSeconds(startDelay);
                    return (dateTime - epoch).TotalSeconds.ToString();
                }

            }
        }
        string TimeLastSampleMonT(byte[] decodeByte)
        {
            var LastSample = (utcReferenceTime - (4294967040 - secondsTimer));
            return LastSample.ToString();
        }
        string ToLittleEndian(byte[] decodebyte)
        {
            var sb = new StringBuilder();
            for (int i = decodebyte.Length - 1; i >= 0; i--)
            {
                sb.Append(decodebyte[i].ToString("x02"));
            }

            return sb.ToString();
        }
        string ToBigEndian(byte[] decodebyte)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < decodebyte.Length; i++)
            {
                sb.Append(decodebyte[i].ToString("x02"));
            }
            return sb.ToString();
        }
        public string UNIXtoUTC(long now)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(now * 1000);
            return date.ToUniversalTime().ToString("dd/MM/yyyy HH:mm:sss UTC");
        }
        public string UNIXtoUTCDate(long now)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(now * 1000);
            return date.ToUniversalTime().ToString("dd/MM/yyyy");
        }
        public string UNIXtoUTCTime(long now)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(now * 1000);
            return date.ToUniversalTime().ToString("HH:mm:sss");
        }
        void InitSensorStatisticsField(int currentChannel)
        {
            recordedSamples = 0;

            sensorMin[currentChannel] = 0xFFFFFFFFFFL;
            sensorMax[currentChannel] = -274;

            highestPosition[currentChannel] = 0;
            lowestPosition[currentChannel] = 0;

            mean[currentChannel] = 0;
            mkt[currentChannel] = 0;

            withinLimit[currentChannel] = 0;
            belowLimit[currentChannel] = 0;
            aboveLimit[currentChannel] = 0;
        }
        void TemperatureStatistics(int currentChannel, double Value, int index)
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

            mkt[currentChannel] += Math.Exp((-Delta_H) / ((Value + KelvinDec) * R));
            mean[currentChannel] += Value;
            
            recordedSamples++;
        }
        void FinalizeStatistics(int currentChannel)
        {
            mean[currentChannel] /= recordedSamples;
            mkt[currentChannel] = (Delta_H / R) / (-Math.Log(mkt[currentChannel] / recordedSamples));
        }
        #endregion

    }
}

