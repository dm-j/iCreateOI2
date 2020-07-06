namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Sensor parser that works out how long the next read will be
    /// </summary>
    internal class ParsingLength : ISensorParser
    {
        private ParsingLength() { }

        public ISensorParser Parse(byte b) =>
            ReadingSensors.Instance.ParseNext(b);

        internal static ParsingLength Instance { get; } = new ParsingLength();
    }
}
