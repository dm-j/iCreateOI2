using System;
using System.Linq;
using System.Reactive.Linq;

namespace iCreateOI2.Sensors
{
    public class Sensors : ISensorParser
    {
        private byte[] data;

        private Sensors() { }

        public ISensorParser Parse(byte b)
        {
            if (Checksum(b))
            {
                // Send data here
                return ReadyToRead.Instance;
            }
            else
            {
                Console.WriteLine("Sensor read error");
                return SensorParsingError.Instance;
            }
        }

        private bool Checksum(byte b) =>
            BitConverter.GetBytes(data.Aggregate(0, (acc, next) => acc + next) + b)[0] == 0;

        public ISensorParser SetBytes(byte[] bytes)
        {
            data = bytes;
            return this;
        }

        internal static Sensors Instance { get; } = new Sensors();

        internal static ISensorParser Package(byte[] bytes) => Instance.SetBytes(bytes);

        private class SensorParser
        {
            internal readonly Func<byte[], int> Parse;
            internal readonly int Length;

            internal SensorParser(Func<byte[], int> parse, int length)
            {
                Parse = parse;
                Length = length;
            }

            internal static SensorParser SingleSigned { get; } = new SensorParser(data => (sbyte)data[0], 1);
            internal static SensorParser SingleUnsigned { get; } = new SensorParser(data => data[0], 1);
            internal static SensorParser HighLowSigned { get; } = new SensorParser(data => BitConverter.ToInt16(data, 0), 2);
            internal static SensorParser HighLowUnsigned { get; } = new SensorParser(data => 256 * data[0] + data[1], 2);
        }
    }
}
