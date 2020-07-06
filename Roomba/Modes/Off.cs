using iCreateOI2.Commands;
using iCreateOI2.Communications;
using System;

namespace iCreateOI2.Modes
{
    internal class Off : Mode
    {
        internal Off(Roomba robot)
            : base(robot)
        {
            Console.WriteLine("OI Mode: Off");
        }

        public override IMode Start()
        {
            robot.Send(Command.Start());
            return new Passive(robot);
        }

        public override IMode ModeSafe() => 
            Start().ModeSafe();

        public override IMode ModeFull() => 
            Start().ModeFull();

        public override IMode ModePassive() => 
            Start();

        public override IMode SeekDock() => 
            Start().SeekDock();

        public override IMode ModeOff() => 
            this;

        public override IMode Sing(Song song) => 
            Start().Sing(song);

        public override IMode Play(SongNumber number) => 
            Start().Play(number);

        public override IMode Sing(Melody melody) => 
            Start().Sing(melody);

        public override IMode Drive(Drive drive) => 
            Start().Drive(drive);
    }
}
