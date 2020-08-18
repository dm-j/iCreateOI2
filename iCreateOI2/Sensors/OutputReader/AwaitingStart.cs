namespace iCreateOI2.Sensors
{
    /// <summary>
    /// The next byte 019 signals the start of a packet
    /// </summary>
    internal class AwaitingStart : IReadOutput
    {
        private readonly OutputReader output;
        internal const byte START_BYTE = 019;

        internal AwaitingStart(OutputReader output) 
        {
            this.output = output;
        }

        public IReadOutput Read(byte b) =>
            b == START_BYTE
                ? (IReadOutput)output.ReadingLength
                : this;
    }
}
