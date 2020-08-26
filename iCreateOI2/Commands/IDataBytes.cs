
using System.Collections.Immutable;

namespace iCreateOI2.Commands
{
    internal interface IDataBytes
    {
        public ImmutableArray<byte> Data { get; }
    }
}
