using iCreateOI2.Commands;
using iCreateOI2.Communications;
using System;

namespace iCreateOI2.Modes
{
    internal class Passive : Mode
    {
        internal Passive(Roomba robot)
            : base(robot)
        {
            Console.WriteLine("OI Mode: Passive");
        }

        public override IMode Start() => 
            this;

        public override IMode ModeSafe()
        {
            robot.Send(Command.Safe());
            return new Safe(robot);
        }

        public override IMode ModeFull()
        {
            robot.Send(Command.Full());
            return new Full(robot);
        }

        public override IMode ModePassive() => 
            this;

        public override IMode SeekDock()
        {
            robot.Send(Command.SeekDock());
            return this;
        }

        public override IMode ModeOff()
        {
            robot.Send(Command.Reset());
            return new Off(robot);
        }

        public override IMode Sing(Song song) => 
            ModeSafe().Sing(song);

        public override IMode Sing(Melody melody) => 
            Sing(Song.Define(SongNumber.Immediate, melody)).Play(SongNumber.Immediate);

        public override IMode Play(SongNumber number) => 
            ModeSafe().Play(number);

        public override IMode Drive(Drive drive) => 
            ModeSafe().Drive(drive);
    }
}
