using iCreateOI2.Commands;
using iCreateOI2.Communications;
using iCreateOI2.Sensors;
using System;

namespace iCreateOI2.Modes
{
    internal class Full : OpenInterfaceMode
    {
        internal Full(Roomba robot) 
            : base(robot)
        {
            Console.WriteLine("OI Mode: Full");
        }

        public override IInteractionMode Drive(Drive drive)
        {
            robot.Send(Command.Drive(drive));
            return this;
        }

        public override IInteractionMode ModeFull() => 
            this;

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
            Sing(Song.Define(SongNumber.Immediate, melody)).Play(SongNumber.Immediate);

        public override IInteractionMode ModeOff()
        {
            robot.Send(Command.Reset());
            return new Off(robot);
        }

        public override IInteractionMode ModePassive()
        {
            robot.Send(Command.Passive());
            return new Passive(robot);
        }

        public override IInteractionMode ModeSafe()
        {
            robot.Send(Command.Safe());
            return new Safe(robot);
        }

        public override IInteractionMode SeekDock()
        {
            robot.Send(Command.SeekDock());
            return new Passive(robot);
        }

        public override IInteractionMode Start() =>
            ModePassive();

        public override IInteractionMode Halt() =>
            Drive(Commands.Drive.Halt);

        public override IInteractionMode Stream()
        {
            robot.Send(Command.SensorStream(
                    SensorPacket.BumpsWheelDrops,
                    SensorPacket.Buttons,
                    SensorPacket.LightBumperCenterLeftSignal,
                    SensorPacket.LightBumperCenterRightSignal,
                    SensorPacket.LightBumperFrontLeftSignal,
                    SensorPacket.LightBumperFrontRightSignal,
                    SensorPacket.LightBumperLeftSignal,
                    SensorPacket.LightBumperRightSignal,
                    SensorPacket.OIMode,
                    SensorPacket.RequestedLeftVelocity,
                    SensorPacket.RequestedRightVelocity
                ));
            return this;
        }
    }
}
