namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Sensor parsing has failed, so we're going to skip the NEXT 019, in case we're offset from the data stream
    /// </summary>
    internal class SensorParsingError : ISensorParser
    {
        private SensorParsingError() { }

        internal static SensorParsingError Instance { get; } = new SensorParsingError();

        public ISensorParser Parse(byte b) =>
            b == ReadyToRead.START_BYTE
                ? (ISensorParser)ReadyToRead.Instance
                : this;
    }
}
