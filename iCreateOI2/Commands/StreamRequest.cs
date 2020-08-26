using iCreateOI2.Sensors;
using System.Collections.Immutable;
using System.Linq;

namespace iCreateOI2.Commands
{
    public class StreamRequest : IDataBytes
    {
        public ImmutableArray<byte> Data { get; private set; }

        private StreamRequest(params SensorPacket[] sensors)
        {
            Data = sensors.Select(o => (byte)o).ToImmutableArray();
        }

        public static StreamRequest Packets(params SensorPacket[] sensors) =>
            new StreamRequest(sensors);
    }
}
