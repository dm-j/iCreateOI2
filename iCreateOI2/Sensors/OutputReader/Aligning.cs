using System;

namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Try to align sensor reader with the stream
    /// </summary>
    internal class Aligning : IReadOutput
    {
        private static readonly Random random = new Random();

        private readonly OutputReader output;
        private int skip = 1;

        internal Aligning(OutputReader output)
        {
            this.output = output;
        }

        public Aligning Skip()
        {
            skip = random.Next(1, 3);
            return this;
        }

        public IReadOutput Read(byte b)
        {
            if (b == AwaitingStart.START_BYTE && --skip <= 0)
                return output.AwaitingStart;

            return this;
        }
    }
}
