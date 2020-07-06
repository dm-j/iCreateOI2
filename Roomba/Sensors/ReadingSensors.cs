namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Sensor parser has reached payload and is accumulating data
    /// </summary>
    internal class ReadingSensors : ISensorParser
    {
        private int index = -1;
        private byte[] bytes;

        private ReadingSensors() { }

        public ISensorParser Parse(byte b)
        {
            index++;
            bytes[index] = b;
            if (index >= bytes.Length)
                return Sensors.Package(bytes);
            return this;
        }

        public ISensorParser ParseNext(int length)
        {
            index = -1;
            bytes = new byte[length];
            return this;
        }

        internal static ReadingSensors Instance { get; } = new ReadingSensors();
    }
}
