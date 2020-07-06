namespace iCreateOI2.Sensors
{
    /// <summary>
    /// SensorParser that waits for byte 19 before starting parsing
    /// </summary>
    internal class ReadyToRead : ISensorParser
    {
        internal const byte START_BYTE = 19;

        private ReadyToRead() { }

        public ISensorParser Parse(byte b) =>
            b == START_BYTE
                ? (ISensorParser)ParsingLength.Instance
                : this;

        internal static ReadyToRead Instance { get; } = new ReadyToRead();
    }
}
