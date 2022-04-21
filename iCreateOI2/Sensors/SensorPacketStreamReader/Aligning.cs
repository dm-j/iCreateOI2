using System;

namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Try to align sensor reader with the stream
    /// </summary>
    internal class Aligning : IReadSensorPacketStreamData
    {
        private static readonly Random Random = new Random();

        private readonly SensorStream _output;
        private int _skip = 1;

        internal Aligning(SensorStream output)
        {
            this._output = output;
        }

        internal Aligning Skip()
        {
            _skip = Random.Next(1, 3);
            return this;
        }

        public IReadSensorPacketStreamData Output(byte b)
        {
            if (b == AwaitingStart.START_BYTE && --_skip <= 0)
                return _output.AwaitingStart;

            return this;
        }
    }
}
