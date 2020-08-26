namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Packet payload has begun, accumulating packet data
    /// </summary>
    internal class ReadingPayload : IReadSensorPacketStreamData
    {
        private int index = 0;
        private byte[] bytes;
        private readonly SensorStream output;

        internal ReadingPayload(SensorStream output) 
        {
            this.output = output;
        }

        public IReadSensorPacketStreamData Output(byte b)
        {
            bytes[index] = b;
            if (++index >= bytes.Length)
                return output.ReadingComplete.Package(bytes);
            return this;
        }

        public ReadingPayload SetLength(int length)
        {
            index = 0;
            bytes = new byte[length];
            return this;
        }
    }
}
