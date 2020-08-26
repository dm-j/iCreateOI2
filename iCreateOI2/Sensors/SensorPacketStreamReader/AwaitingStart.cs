namespace iCreateOI2.Sensors
{
    /// <summary>
    /// The next byte 019 signals the start of a packet
    /// </summary>
    internal class AwaitingStart : IReadSensorPacketStreamData
    {
        private readonly SensorStream output;
        internal const byte START_BYTE = 019;

        internal AwaitingStart(SensorStream output) 
        {
            this.output = output;
        }

        public IReadSensorPacketStreamData Output(byte b) =>
            b == START_BYTE
                ? (IReadSensorPacketStreamData)output.ReadingLength
                : this;
    }
}
