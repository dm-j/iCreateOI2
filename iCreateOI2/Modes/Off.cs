using iCreateOI2.Commands;
using iCreateOI2.Communications;
using System;

namespace iCreateOI2.Modes
{
    internal class Off : OpenInterfaceMode
    {
        internal Off(Roomba robot)
            : base(robot)
        {
            Console.WriteLine("OI Mode: Off");
        }

        public override IInteractionMode Start()
        {
            robot.Send(Command.Start());
            return new Passive(robot);
        }

        public override IInteractionMode ModeSafe() => 
            Start().ModeSafe();

        public override IInteractionMode ModeFull() => 
            Start().ModeFull();

        public override IInteractionMode ModePassive() => 
            Start();

        public override IInteractionMode SeekDock() => 
            Start().SeekDock();

        public override IInteractionMode ModeOff() => 
            this;

        public override IInteractionMode Sing(Song song) => 
            Start().Sing(song);

        public override IInteractionMode Play(SongNumber number) => 
            Start().Play(number);

        public override IInteractionMode Sing(Melody melody) => 
            Start().Sing(melody);

        public override IInteractionMode Drive(Drive drive) => 
            Start().Drive(drive);

        public override IInteractionMode Halt() =>
            this;
    }
}
