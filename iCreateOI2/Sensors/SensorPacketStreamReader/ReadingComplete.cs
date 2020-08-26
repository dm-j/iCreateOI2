using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace iCreateOI2.Sensors

{
    /// <summary>
    /// Sensor packet data received. If it passes checksum, interpret the data and fire the sensor parser
    /// </summary>
    public class ReadingComplete : IReadSensorPacketStreamData
    {
        private readonly SensorStream output;
        private byte[] data;
        private int checksum;

        internal ReadingComplete(SensorStream output)
        {
            this.output = output;
        }

        public IReadSensorPacketStreamData Output(byte b)
        {
            checksum = Checksum(b);
            if (checksum == 0)
            {
                return output.AwaitingStart;
            }
            else
            {
                Console.WriteLine($"Sensor stream: [ 019, {data.Length:000}, {data.Aggregate(new StringBuilder(), (acc, next) => acc.Append($"{next:000}, "))}{b:000} ]");
                Console.WriteLine($"Sensor read error. Expected checksum [000], received [{checksum:000}]");
                return output.Aligning.Skip();
            }
        }

        private int Checksum(byte b) =>
            BitConverter.GetBytes(data.Aggregate(0, (acc, next) => acc + next) + b)[0];

        public ReadingComplete Package(byte[] data)
        {
            this.data = data;
            return this;
        }

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
