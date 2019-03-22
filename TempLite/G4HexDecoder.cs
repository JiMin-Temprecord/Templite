using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using TempLite.Services;
namespace TempLite
{
    public class G4HexDecoder
    {
        string loggerState;
        string batteryPercentage;
        string timeAtFirstSameple;
        string userData;

        bool loopOverwrite = false;
        bool Fahrenheit = false;

        double Kevin = 27315;
        double Kelvin_Dec = 273.15;

        long Y_2000 = 946684800000L;
        long Y_2010 = 1262304000000L;
        long samplePeriod = 0;
        long UTCreferenceTime = 0;
        long timeFirstSample = 0;
        long ticksAtLastSample = 0;
        long ticksSinceStart = 0;

        int G4MemorySize = 0x7FFF;
        int numberChannel = 0;
        int userDataLength = 0;
        int startDelay = 0;
        int totalRTCticks = 0;
        int totalSamplingEvents = 0;
        int totalUses = 0;
        int loopOverwriteStartAddress = 0;
        int memoryStartAddress = 0;
        int recordedSamples = 0;
        int tagsPlaced = 0;
        
        int[] sensorStartingValue = { 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] sensorTablePointer = { 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] sensorType = { 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] compressionTable = new int[128];

        double Delta_H = 83.14472; //Delta H   is the Activation Energy:   83.14472    kJ/mole
        double R = 8.314472; //R is the Gas Constant: 0.008314472 J/mole/degree

        List<double> Data;
        double[] upperLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] lowerLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] sensorMin = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] sensorMax = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] Mean = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] MKT = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] withinLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] belowLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] aboveLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };

        public G4HexDecoder(LoggerInformation loggerInformation)
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
            loopOverwriteStartAddress = Convert.ToInt32(ReadFromJObject(jsonObject, "LoopOverWriteAddress"),16);
            Fahrenheit = Convert.ToBoolean(ReadFromJObject(jsonObject, "USER_SETTINGS,Fahrenheit"));
            UTCreferenceTime = Convert.ToInt32(ReadFromJObject(jsonObject, "UTCReferenceTime"), 16);
            totalRTCticks = Convert.ToInt32(ReadFromJObject(jsonObject, "HEADER,TotalRTCTicks"), 16);
            totalSamplingEvents = Convert.ToInt32(ReadFromJObject(jsonObject, "HEADER,TotalSamplingEvents"), 16);
            totalUses = Convert.ToInt32(ReadFromJObject(jsonObject, "HEADER,TotalUses"), 16);
            batteryPercentage = ReadFromJObject(jsonObject, "BATTERY_INFO,Battery") + "%";
            startDelay = Convert.ToInt32(ReadFromJObject(jsonObject, "USER_SETTINGS,StartDelay"), 16);
            samplePeriod = Convert.ToInt32(ReadFromJObject(jsonObject, "USER_SETTINGS,SamplingPeriod"), 16);
            ticksSinceStart = Convert.ToInt32(ReadFromJObject(jsonObject, "DATA_INFO,TicksSinceArousal"), 16);
            ticksAtLastSample = Convert.ToInt32(ReadFromJObject(jsonObject, "DATA_INFO,TicksAtLastSample"), 16);
            recordedSamples = Convert.ToInt32(ReadFromJObject(jsonObject, "DATA_INFO,SamplesNumber"),16);
            timeFirstSample = Convert.ToInt32(ReadFromJObject(jsonObject, "DATA_INFO,TimeStarted"));
            ReadFromJObject(jsonObject, "SENSOR,Decode_Delta_Data");
            //upperLimit = ReadFromJObject(jsonObject, "CHANNEL_INFO,UpperLimit");
            //lowerLimit = ReadFromJObject(jsonObject, "CHANNEL_INFO,LowerLimit");
            //ReadFromJObject(jsonObject, "SENSOR,Decode_MonT_Data");
        }

        readonly LoggerInformation loggerInformation;
        readonly string serialNumber;
        readonly string jsonFile;

        public PDFvariables AssignPDFValue()
        {
            var pdfVariables = new PDFvariables();

            pdfVariables.RecordedSamples = recordedSamples;
            pdfVariables.SerialNumber = serialNumber;
            pdfVariables.LoggerState = loggerState;
            pdfVariables.BatteryPercentage = batteryPercentage;
            pdfVariables.SameplePeriod = HHMMSS(samplePeriod);
            pdfVariables.StartDelay = HHMMSS(startDelay);
            pdfVariables.FirstSample = UNIXtoUTC(timeFirstSample);
            pdfVariables.LastSample = UNIXtoUTC(timeFirstSample);
            pdfVariables.TagsPlaced = tagsPlaced.ToString();
            pdfVariables.UserData = userData;

            for (int i = 0; i < pdfVariables.RecordedSamples; i++)
            {
                pdfVariables.Time.Add(timeFirstSample);
                timeFirstSample = timeFirstSample + samplePeriod;
            }

            if (recordedSamples > 0)
            {
                var timeLastSample = Convert.ToInt32(pdfVariables.Time[(pdfVariables.Time.Count - 1)]);
                pdfVariables.LastSample = UNIXtoUTC(timeLastSample);
            }

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

        void AssignChannelValues(ChannelConfig Channel, int i)
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

        byte[] ReadHex(string[] currentinfo)
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
                        Console.WriteLine("Length : " + currentinfo[1]);
                        if (addtoread == address)
                        {
                            diff = int.Parse(currentinfo[0], NumberStyles.HexNumber) - int.Parse(address, NumberStyles.HexNumber);
                            Console.WriteLine("DIFF : " + diff);
                            if (diff >= 0 && diff < 58) // reader can only send 64bytes at a time
                            {
                                int infolength = Convert.ToInt32(currentinfo[1]);
                                if (infolength > 58)
                                {
                                    int readinfo = 58;
                                    while (infolength > 0)
                                    {
                                        temp += data.Substring(diff * 2, readinfo * 2);
                                        line = sr.ReadLine();
                                        data = line.Substring(7, line.Length - 7);
                                        infolength = infolength - readinfo;

                                        if (infolength > 58)
                                        {
                                            diff = 0;
                                            readinfo = 58;
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
        string ReadFromJObject(JObject jsonObject, string info)
        {
            var decodeInfo = JsontoString(jsonObject, info);
            return CallDecodeFunctions(decodeInfo);
        }

        JObject GetJsonObject()
        {
            using (var sr = new StreamReader(jsonFile))
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
                    return _1ByteToDecimal(stringArrayInfo, decodeByte);

                case "_2_Byte_to_Decimal":
                    return (((decodeByte[1] & 0xFF) << 8) | (decodeByte[0] & 0xFF)).ToString();

                case "_2_Byte_to_Decimal_Big_Endian":
                    return ToBigEndian(decodeByte);
                   
                case "_3_Byte_to_Decimal":
                    return ToLittleEndian(decodeByte);

                case "_3_Byte_to_Temperature_ARRAY":
                    _3BytetoTemperatureArray(decodeByte);
                    break;

                case "_4_Byte_Built":
                    return _4ByteBuilt(decodeByte);

                case "_4_Byte_to_Decimal":
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
                    
                case "Battery_Type":
                    break;
                    
                case "CompressionTable":
                    CompressionTable(decodeByte);
                    break;
                    
                case "Decode_Delta_Data":
                    DecodeDeltaData(decodeByte);
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

                case "String":
                    return String(decodeByte);

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
        string _1ByteToDecimal(string[] stringArrayInfo, byte[] decodeByte)
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
                decodeByte[0] = (byte)(decodeByte[0]);
                return ToBigEndian(decodeByte);
            }
        }
        double[] _3BytetoTemperatureArray(byte[] decodeByte)
        {
            var offset = 9;
            var value = new double[numberChannel];

            for (int i = 0; i < numberChannel; i++)
            {
                double element = ((((decodeByte[(offset * i) + 2]) & 0xFF) << 16) | (((decodeByte[(offset * i) + 1]) & 0xFF) << 8) | (decodeByte[(offset * i)] & 0xFF));
                element += Kevin;
                element /= 100;
                value[i] += element;
            }

            return value;
        }
        string _4ByteBuilt(byte[] decodeByte)
        {
            long value = Convert.ToInt32(ToLittleEndian(decodeByte)) * 1000;

            if (value > 0)
            {
                value += Y_2000;
                value -= Y_2010;
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
            return _4byteUnix.ToString();
        }
        string _4ByteSectoDec(byte[] decodeByte)
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
        int GetBit(int Value, int bit)
        {
            return (Value >> bit) & 1;
        }
        int[] CompressionTable(byte[] decodeByte)
        {
            var value = new int[decodeByte.Length / 2];

            for (int i = 0; i < decodeByte.Length; i += 2)
            {
                compressionTable[i / 2] = (((decodeByte[i / 2] & 0xff) << 8) | (decodeByte[i + 1] & 0xff));
                value[i / 2] = compressionTable[i / 2];
            }

            return value;
        }
        void DecodeDeltaData(byte[] decodeByte)
        {
            var memoryStart = loopOverwriteStartAddress;
            Console.WriteLine("DATA LENGTH : " + decodeByte.Length);

            if (memoryStart > 0)
            {
                memoryStart = FindStartSentinel(memoryStart - 1, 16, decodeByte);
                memoryStart++;
            }
            if (decodeByte.Length > 0)
            {
                if (CheckStartSentinel(decodeByte))
                {
                    memoryStart += ((6 * numberChannel) + 1);
                    memoryStart &= G4MemorySize;
                    memoryStart = FindStartSentinel(memoryStart, (8 * numberChannel), decodeByte); 


                    if (memoryStart != 0xFFFF)
                    {
                        memoryStart++;
                        ReadStoreData(decodeByte);
                    }
                }
            }
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
        int FindStartSentinel(int memoryStart, int Max, byte[] decodeByte)
        {
            var maxI = ((memoryStart + Max) & G4MemorySize);

            if (maxI > memoryStart)
            {
                while (memoryStart < maxI)
                {
                    if ((decodeByte[memoryStart] == 0x7F) || (decodeByte[memoryStart] & 0xff) == 0xff) //???????????????????????
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
        Boolean CheckStartSentinel ( byte[] decodeByte)
        {
            var startValue = new int[8];
            var sensorNumber = 0;
            var check = true;

            while ((sensorNumber < 2) && check)
            {
                var addMSB = (memoryStartAddress + (2 * sensorNumber) + 1) & G4MemorySize;
                var addLSB = memoryStartAddress + (2 * sensorNumber) & G4MemorySize;

                var VaddMSB = (memoryStartAddress + (2 * sensorNumber) + 1) & G4MemorySize;
                var VaddLSB = memoryStartAddress + (2 * sensorNumber) & G4MemorySize;

                startValue[sensorNumber] = (((decodeByte[addMSB]) & 0xff) << 8) | (decodeByte[addLSB] & 0xff);
                var verifyValue = (((decodeByte[VaddMSB] & 0xff) << 8) | (decodeByte[VaddLSB] & 0xff));

                if(startValue[sensorNumber] != verifyValue)
                {
                    check = false;
                }

                sensorStartingValue[sensorNumber] -= verifyValue;
                sensorStartingValue[sensorNumber] *= -1;

                sensorNumber++;
            }

            return check;
        }
        string String(byte[] decodeByte)
        {
            var userdatastring = string.Empty;
            for (int i = 0; i < decodeByte.Length; i++)
            {
                userdatastring += (char)(decodeByte[i] & 0xFF);
            }

            return userdatastring.Substring(0, userDataLength);
        }
        public string HHMMSS(double mseconds)
        {
            int hours = (int)(mseconds / 3600);
            int minutes = (int)((mseconds % 3600) / 60);
            int seconds = (int)(mseconds % 60);

            return $"{hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}";
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
        string LoggerState(byte[] decodebyte)
        {
            switch (decodebyte[0])
            {
                case 0:
                    return "Ready";
                case 1:
                    return "Delay";
                case 2:
                    return "Running";
                case 3:
                    return "Stopped";
                default:
                    return "Undefined";
            }
        }
        void ReadStoreData(byte[] decodebyte)
        {
            var VALUE = new double[numberChannel][G4MemorySize];
            Object[] MARK = new Object[G4MemorySize + 1];
            int tagNumbers = 0;

            //int MAX_TO_READ = ((G4_memory_size +1) - (( 4 * m_sensor_number) + (2 * (m_sensor_number + 1)) + 1))/(m_sensor_number);
            //if Simplified =>
            int MAX_TO_READ = (G4MemorySize - (6 * numberChannel) - 2) / numberChannel;

            for (int m_current_sensor = 0; m_current_sensor < numberChannel; m_current_sensor++)
            {
                int array_pointer = 0;
                int data_pointer = (memoryStartAddress + m_current_sensor) & G4MemorySize;                  //& 0x007FFF is to allow buffer rotation
                recordedSamples = 0;
                
                InitSensorStatisticsField(m_current_sensor);
  
                while (
                        (data_pointer < decodebyte.Length)                                                 //Must be first
                                &&
                                (decodebyte[data_pointer] != 0x7F)
                                &&
                                ((decodebyte[data_pointer] & 0xFF) != 0xFF)
                                &&
                                (recordedSamples < MAX_TO_READ)                                         //This is just in case I cant find any FF or 7F
                )
                {
                    if ((decodebyte[data_pointer] & 0xFF) == 0x80)
                    {
                        if (m_current_sensor == 0)
                        {
                            MARK[tagNumbers++] = array_pointer;
                        }
                    }
                    else
                    {
                        if (decodebyte[data_pointer] < 0x00)
                        {
                            sensorStartingValue[m_current_sensor] -= compressionTable[(decodebyte[data_pointer] & 0x7F)];
                        }
                        else
                        {
                            sensorStartingValue[m_current_sensor] += compressionTable[(decodebyte[data_pointer])];
                        }

                        VALUE[m_current_sensor][array_pointer++] = ((double)sensorStartingValue[m_current_sensor] / 100);
                        TemperatureStatistics(m_current_sensor, ((double)sensorStartingValue[m_current_sensor] / 100), array_pointer);
                        recordedSamples++;
                    }

                    data_pointer += recordedSamples;
                    data_pointer &= G4MemorySize;                                                           //& 0x007FFF is to allow buffer rotation
                }
                
                FinalizeStatistics(m_current_sensor);

                Array.Copy(VALUE[m_current_sensor], this.Data, recordedSamples);
            }

            Array.Copy(MARK, tagNumbers, MARK.Length);
        }
        public string UNIXtoUTC(long now)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(now * 1000);
            date = date.ToUniversalTime();
            var simpledate = date.ToString("yyyy-MM-dd HH:mm:sss UTC");
            return simpledate;
        }
        public string UNIXtoUTCDate(long now)
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
        void InitSensorStatisticsField(int currentChannel)
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

            MKT[currentChannel] += Math.Exp((-Delta_H) / ((Value + Kelvin_Dec) * R));
            Mean[currentChannel] += Value;
            recordedSamples++;
        }
        void FinalizeStatistics(int currentChannel)
        {
            Mean[currentChannel] /= recordedSamples;
            MKT[currentChannel] = (Delta_H / R) / (-Math.Log(MKT[currentChannel] / recordedSamples));
        }
        #endregion

    }
}

