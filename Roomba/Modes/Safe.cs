using iCreateOI2.Commands;
using iCreateOI2.Communications;
using System;

namespace iCreateOI2.Modes
{
    internal class Safe : Mode
    {
        internal Safe(Roomba robot)
            : base(robot)
        {
            Console.WriteLine("OI Mode: Safe");
        }

        public override IMode Drive(Drive drive)
        {
            robot.Send(Command.Drive(drive));
            return this;
        }

        public override IMode Sing(Song song)
        {
            robot.Send(Command.Song(song));
            return this;
        }

        public override IMode Play(SongNumber number)
        {
            robot.Send(Command.Play(number));
            return this;
        }

        public override IMode Sing(Melody melody) => 
            Sing(Song.Define(SongNumber.Immediate, melody))
                .Play(SongNumber.Immediate);

        public override IMode SeekDock()
        {
            robot.Send(Command.SeekDock());
            return new Passive(robot);
        }

        public override IMode ModeFull()
        {
            robot.Send(Command.Full());
            return new Full(robot);
        }

        public override IMode ModeOff()
        {
            robot.Send(Command.Power());
            return new Off(robot);
        }

        public override IMode ModePassive()
        {
            robot.Send(Command.Power());
            return new Passive(robot);
        }

        public override IMode ModeSafe() => 
            this;

        public override IMode Start() =>
            ModePassive();
    }
}
