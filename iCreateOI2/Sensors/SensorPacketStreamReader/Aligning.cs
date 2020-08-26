using System;

namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Try to align sensor reader with the stream
    /// </summary>
    internal class Aligning : IReadSensorPacketStreamData
    {
        private static readonly Random random = new Random();

        private readonly SensorStream output;
        private int skip = 1;

        internal Aligning(SensorStream output)
        {
            this.output = output;
        }

        internal Aligning Skip()
        {
            skip = random.Next(1, 3);
            return this;
        }

        public IReadSensorPacketStreamData Output(byte b)
        {
            if (b == AwaitingStart.START_BYTE && --skip <= 0)
                return output.AwaitingStart;

            return this;
        }
    }
}
