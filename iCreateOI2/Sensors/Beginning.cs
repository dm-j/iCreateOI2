namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Packet has begun, first byte is packet payload length
    /// </summary>
    internal class Beginning : ISensorParseMode
    {
        private Beginning() { }

        public ISensorParseMode Parse(byte b) =>
            Reading.Mode.SetLength(b);

        internal static Beginning Mode { get; } = new Beginning();
    }
}
