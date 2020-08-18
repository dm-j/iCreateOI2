namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Packet payload has begun, accumulating packet data
    /// </summary>
    internal class ReadingPayload : IReadOutput
    {
        private int index = 0;
        private byte[] bytes;
        private readonly OutputReader output;

        internal ReadingPayload(OutputReader output) 
        {
            this.output = output;
        }

        public IReadOutput Read(byte b)
        {
            bytes[index] = b;
            if (++index >= bytes.Length)
                return output.ReadingComplete.Package(bytes);
            return this;
        }

        public ReadingPayload SetLength(int length)
        {
            index = 0;
            bytes = new byte[length];
            return this;
        }
    }
}
