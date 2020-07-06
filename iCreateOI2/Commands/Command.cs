using iCreateOI2.Sensors;
using System.Collections.Immutable;
using System.Linq;

namespace iCreateOI2.Commands
{
    public enum OpCode
    {
        Start = 128,
        Reset = 7,
        Safe = 131,
        Full = 132,
        SeekDock = 143,
        Power = 173,
        Drive = 145,
        Song = 140,
        Play = 141,
        SensorStream = 148
    }

    public class Command : IDataBytes
    {
        public ImmutableArray<byte> Data { get; private set; }

        private Command(OpCode code)
        {
            Data = new[] { (byte)code }.ToImmutableArray();
        }

        private Command(OpCode code, IDataBytes data)
            : this(code)
        {
            Data = Data.Concat(data.Data).ToImmutableArray();
        }

        public static Command Start() =>
            _Start;

        public static Command Passive() =>
            _Passive;

        public static Command Reset() =>
            _Reset;

        public static Command Safe() =>
            _Safe;

        public static Command Full() =>
            _Full;

        public static Command SeekDock() =>
            _SeekDock;

        public static Command Power() =>
            _Power;

        public static Command Drive(Drive drive) =>
            new Command(OpCode.Drive, drive);

        public static Command Halt() =>
            _Halt;

        public static Command Song(Song song) =>
            new Command(OpCode.Song, song);

        public static Command Play(SongNumber song) =>
            new Command(OpCode.Play, song);

        public static Command SensorStream(params SensorPacket[] packets) =>
            new Command(OpCode.SensorStream, StreamRequest.Packets(packets));

        #region Cached 0-Data commands

        private static Command _Start { get; } = new Command(OpCode.Start);
        private static Command _Passive { get; } = new Command(OpCode.Start);
        private static Command _Halt { get; } = new Command(OpCode.Drive, Commands.Drive.Halt);
        private static Command _Reset { get; } = new Command(OpCode.Reset);
        private static Command _Off { get; } = new Command(OpCode.Reset);
        private static Command _Safe { get; } = new Command(OpCode.Safe);
        private static Command _Full { get; } = new Command(OpCode.Full);
        private static Command _SeekDock { get; } = new Command(OpCode.SeekDock);
        private static Command _Power { get; } = new Command(OpCode.Power);

        #endregion
    }
}
