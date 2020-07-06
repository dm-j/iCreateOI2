namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Packet payload has begun, accumulating packet data
    /// </summary>
    internal class Reading : ISensorParseMode
    {
        private int index = -1;
        private byte[] bytes;

        private Reading() { }

        public ISensorParseMode Parse(byte b)
        {
            index++;
            bytes[index] = b;
            if (index >= bytes.Length)
                return Checking.Package(bytes);
            return this;
        }

        public ISensorParseMode SetLength(int length)
        {
            index = -1;
            bytes = new byte[length];
            return this;
        }

        internal static Reading Mode { get; } = new Reading();
    }
}
