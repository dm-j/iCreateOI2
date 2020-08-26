using iCreateOI2.Sensors;
using System.Collections.Immutable;
using System.Linq;

namespace iCreateOI2.Commands
{
    internal enum OpCode
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

    internal class Command : IDataBytes
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

        #region Cached commands

        private static readonly Command _Start    = new Command(OpCode.Start);
        private static readonly Command _Passive  = new Command(OpCode.Start);
        private static readonly Command _Halt     = new Command(OpCode.Drive, Commands.Drive.Halt);
        private static readonly Command _Reset    = new Command(OpCode.Reset);
        private static readonly Command _Off      = new Command(OpCode.Reset);
        private static readonly Command _Safe     = new Command(OpCode.Safe);
        private static readonly Command _Full     = new Command(OpCode.Full);
        private static readonly Command _SeekDock = new Command(OpCode.SeekDock);
        private static readonly Command _Power    = new Command(OpCode.Power);

        #endregion
    }
}
