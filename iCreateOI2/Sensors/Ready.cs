namespace iCreateOI2.Sensors
{
    /// <summary>
    /// The next byte 019 signals the start of a packet
    /// </summary>
    internal class Ready : ISensorParseMode
    {
        internal const byte START_BYTE = 019;

        private Ready() { }

        public ISensorParseMode Parse(byte b) =>
            b == START_BYTE
                ? (ISensorParseMode)Beginning.Mode
                : this;

        internal static Ready Mode { get; } = new Ready();
    }
}
