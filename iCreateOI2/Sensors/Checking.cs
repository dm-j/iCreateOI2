using System;
using System.Linq;
using System.Reactive.Linq;

namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Sensor packet data received. If it passes checksum, interpret the data and fire the 
    /// </summary>
    public class Checking : ISensorParseMode
    {
        private byte[] data;

        private Checking() { }

        public ISensorParseMode Parse(byte b)
        {
            if (Checksum(b))
            {
                // Send data here
                return Ready.Mode;
            }
            else
            {
                Console.WriteLine("Sensor read error");
                return Aligning.Mode;
            }
        }

        private bool Checksum(byte b) =>
            BitConverter.GetBytes(data.Aggregate(0, (acc, next) => acc + next) + b)[0] == 0;

        public ISensorParseMode SetBytes(byte[] bytes)
        {
            data = bytes;
            return this;
        }

        internal static Checking Mode { get; } = new Checking();

        internal static ISensorParseMode Package(byte[] bytes) => Mode.SetBytes(bytes);

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
