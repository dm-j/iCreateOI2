using iCreateOI2.Commands;
using iCreateOI2.Communications;
using System;

namespace iCreateOI2.Modes
{
    internal class Passive : OpenInterfaceMode
    {
        internal Passive(Roomba robot)
            : base(robot)
        {
            Console.WriteLine("OI Mode: Passive");
        }

        public override IInteractionMode Start() =>
            this;

        public override IInteractionMode ModeSafe()
        {
            robot.Send(Command.Safe());
            return new Safe(robot);
        }

        public override IInteractionMode ModeFull()
        {
            robot.Send(Command.Full());
            return new Full(robot);
        }

        public override IInteractionMode ModePassive() =>
            this;

        public override IInteractionMode SeekDock()
        {
            robot.Send(Command.SeekDock());
            return this;
        }

        public override IInteractionMode ModeOff()
        {
            robot.Send(Command.Reset());
            return new Off(robot);
        }

        public override IInteractionMode Sing(Song song) =>
            ModeSafe().Sing(song);

        public override IInteractionMode Sing(Melody melody) => 
            Sing(Song.Define(SongNumber.Immediate, melody)).Play(SongNumber.Immediate);

        public override IInteractionMode Play(SongNumber number) =>
            ModeSafe().Play(number);

        public override IInteractionMode Drive(Drive drive) =>
            ModeSafe().Drive(drive);

        public override IInteractionMode Halt() =>
            ModeSafe().Halt();
    }
}
