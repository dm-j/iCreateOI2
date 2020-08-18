using iCreateOI2.Commands;
using iCreateOI2.Communications;
using System;

namespace iCreateOI2.Modes
{
    internal class Safe : OpenInterfaceMode
    {
        internal Safe(Roomba robot)
            : base(robot)
        {
            Console.WriteLine("OI Mode: Safe");
        }

        public override IInteractionMode Drive(Drive drive)
        {
            robot.Send(Command.Drive(drive));
            return this;
        }

        public override IInteractionMode Sing(Song song)
        {
            robot.Send(Command.Song(song));
            return this;
        }

        public override IInteractionMode Play(SongNumber number)
        {
            robot.Send(Command.Play(number));
            return this;
        }

        public override IInteractionMode Sing(Melody melody) => 
            Sing(Song.Define(SongNumber.Immediate, melody))
                .Play(SongNumber.Immediate);

        public override IInteractionMode SeekDock()
        {
            robot.Send(Command.SeekDock());
            return new Passive(robot);
        }

        public override IInteractionMode ModeFull()
        {
            robot.Send(Command.Full());
            return new Full(robot);
        }

        public override IInteractionMode ModeOff()
        {
            robot.Send(Command.Power());
            return new Off(robot);
        }

        public override IInteractionMode ModePassive()
        {
            robot.Send(Command.Power());
            return new Passive(robot);
        }

        public override IInteractionMode ModeSafe() => 
            this;

        public override IInteractionMode Start() =>
            ModePassive();

        public override IInteractionMode Halt() =>
            Drive(Commands.Drive.Halt);
    }
}
