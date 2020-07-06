using iCreateOI2.Domain;
using System.Collections.Immutable;
using System.Linq;

namespace iCreateOI2.Commands
{
    public class Drive : IDataBytes
    {
        public ImmutableArray<byte> Data { get; private set; }

        private Drive(int left, int right)
        {
            Data = new[] { right, left }.SelectMany(value => value.ToHighLow()).ToImmutableArray();
        }

        public static Drive At(int left, int right) =>
            new Drive(left, right);

        public static Drive Halt { get; } = Drive.At(0, 0);
    }
}
