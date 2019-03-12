using TempLite.Services;

namespace TempLite
{
    class SetReadByteWritter : IByteWriter
    {
        public SetReadByteWritter(int loggerType)
        {
            this.loggerType = loggerType;
        }

        readonly int loggerType;

        public void WriteBytes(byte[] sendMessage)
        {
            switch (loggerType)
            {
                case 3:
                    sendMessage[0] = 0x02;
                    sendMessage[1] = 0x06;
                    sendMessage[2] = 0x00;
                    sendMessage[3] = 0x01;
                    sendMessage[4] = 0x46;
                    sendMessage[5] = 0x00;
                    sendMessage[6] = 0x00;
                    sendMessage[7] = 0x00;
                    sendMessage = CommunicationServices.AddCRC(8, sendMessage);
                    break;

                case 6:
                    sendMessage[0] = 0x02;
                    sendMessage[1] = 0x06;
                    sendMessage[2] = 0x00;
                    sendMessage[3] = 0x01;
                    sendMessage[4] = 0x63;
                    sendMessage[5] = 0x00;
                    sendMessage[6] = 0x00;
                    sendMessage[7] = 0x00;
                    sendMessage = CommunicationServices.AddCRC(8, sendMessage);
                    break;
            }
        }
    }
}
