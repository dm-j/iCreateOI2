namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Packet has begun, first byte is packet payload length
    /// </summary>
    internal class ReadingLength : IReadSensorPacketStreamData
    {
        private readonly SensorStream output;
        internal ReadingLength(SensorStream output) 
        {
            this.output = output;
        }

        public IReadSensorPacketStreamData Output(byte b) =>
            output.ReadingPayload.SetLength(b);
    }
}
