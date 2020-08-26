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

        internal ReadingComplete Package(byte[] data)
        {
            this.data = data;
            return this;
        }
    }
}
