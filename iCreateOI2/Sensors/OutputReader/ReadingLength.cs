namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Packet has begun, first byte is packet payload length
    /// </summary>
    internal class ReadingLength : IReadOutput
    {
        private readonly OutputReader output;
        internal ReadingLength(OutputReader output) 
        {
            this.output = output;
        }

        public IReadOutput Read(byte b) =>
            output.ReadingPayload.SetLength(b);
    }
}
