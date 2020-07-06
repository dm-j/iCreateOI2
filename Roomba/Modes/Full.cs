using iCreateOI2.Commands;
using iCreateOI2.Communications;
using System;

namespace iCreateOI2.Modes
{
    internal class Full : Mode
    {
        internal Full(Roomba robot) 
            : base(robot)
        {
            Console.WriteLine("OI Mode: Full");
        }

        public override IMode Drive(Drive drive)
        {
            robot.Send(Command.Drive(drive));
            return this;
        }

        public override IMode ModeFull() => 
            this;

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
            Sing(Song.Define(SongNumber.Immediate, melody)).Play(SongNumber.Immediate);

        public override IMode ModeOff()
        {
            robot.Send(Command.Reset());
            return new Off(robot);
        }

        public override IMode ModePassive()
        {
            robot.Send(Command.Passive());
            return new Passive(robot);
        }

        public override IMode ModeSafe()
        {
            robot.Send(Command.Safe());
            return new Safe(robot);
        }

        public override IMode SeekDock()
        {
            robot.Send(Command.SeekDock());
            return new Passive(robot);
        }

        public override IMode Start() =>
            ModePassive();
    }
}
