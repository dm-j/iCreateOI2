using iCreateOI2.Commands;
using iCreateOI2.Modes;
using iCreateOI2.Sensors;
using System;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace iCreateOI2.Communications
{
    public class Roomba
    {
        private const float SMOOTH_PROXIMITY = 0.8f;

        private readonly Communications adaptor;
        private IInteractionMode InteractionMode;
        private readonly OutputReader Output;

        public Roomba(SerialPort port)
        {
            adaptor = new Communications(port);
            Output = new OutputReader(adaptor.Output);
            Execute(OpenInterfaceMode.Init(this));

            Emergency = Output.Parsers[SensorPacket.BumpsWheelDrops].Select(value => SingleUnsigned(value) != 0).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            CliffLeft = Output.Parsers[SensorPacket.CliffLeft].Select(value => SingleUnsigned(value) != 0);
            CliffFrontLeft = Output.Parsers[SensorPacket.CliffFrontLeft].Select(value => SingleUnsigned(value) != 0);
            CliffFrontRight = Output.Parsers[SensorPacket.CliffFrontRight].Select(value => SingleUnsigned(value) != 0);
            CliffRight = Output.Parsers[SensorPacket.CliffRight].Select(value => SingleUnsigned(value) != 0);
            ButtonClean = Output.Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Clean)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonSpot = Output.Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Spot)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonDock = Output.Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Dock)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonMinute = Output.Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Minute)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonHour = Output.Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Hour)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonDay = Output.Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Day)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonSchedule = Output.Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Schedule)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonClock = Output.Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Clock)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            CliffLeftSignal = Output.Parsers[SensorPacket.CliffLeftSignal].Select(HighLowUnsigned);
            CliffFrontLeftSignal = Output.Parsers[SensorPacket.CliffFrontLeftSignal].Select(HighLowUnsigned);
            CliffFrontRightSignal = Output.Parsers[SensorPacket.CliffFrontRightSignal].Select(HighLowUnsigned);
            CliffRightSignal = Output.Parsers[SensorPacket.CliffRightSignal].Select(HighLowUnsigned);
            RealOIMode = Output.Parsers[SensorPacket.OIMode].Select(value => (Mode)SingleUnsigned(value)).DistinctUntilChanged();
            NumberOfStreamPackets = Output.Parsers[SensorPacket.StreamPackets].Select(SingleUnsigned);
            RequestedRightVelocity = Output.Parsers[SensorPacket.RequestedRightVelocity].Select(HighLowSigned);
            RequestedLeftVelocity = Output.Parsers[SensorPacket.RequestedLeftVelocity].Select(HighLowSigned);
            ProximityLeftRaw = Output.Parsers[SensorPacket.LightBumperLeftSignal].Select(HighLowUnsigned);
            ProximityFrontLeftRaw = Output.Parsers[SensorPacket.LightBumperFrontLeftSignal].Select(HighLowUnsigned);
            ProximityCenterLeftRaw = Output.Parsers[SensorPacket.LightBumperCenterLeftSignal].Select(HighLowUnsigned);
            ProximityCenterRightRaw = Output.Parsers[SensorPacket.LightBumperCenterRightSignal].Select(HighLowUnsigned);
            ProximityFrontRightRaw = Output.Parsers[SensorPacket.LightBumperFrontRightSignal].Select(HighLowUnsigned);
            ProximityRightRaw = Output.Parsers[SensorPacket.LightBumperRightSignal].Select(HighLowUnsigned);
            ProximityLeft = ProximityLeftRaw.Select(SmoothProximity).Scan((acc, next) => (next * SMOOTH_PROXIMITY) + (acc * (1f - SMOOTH_PROXIMITY)));
            ProximityFrontLeft = ProximityFrontLeftRaw.Select(SmoothProximity).Scan((acc, next) => (next * SMOOTH_PROXIMITY) + (acc * (1f - SMOOTH_PROXIMITY)));
            ProximityCenterLeft = ProximityCenterLeftRaw.Select(SmoothProximity).Scan((acc, next) => (next * SMOOTH_PROXIMITY) + (acc * (1f - SMOOTH_PROXIMITY)));
            ProximityCenterRight = ProximityCenterRightRaw.Select(SmoothProximity).Scan((acc, next) => (next * SMOOTH_PROXIMITY) + (acc * (1f - SMOOTH_PROXIMITY)));
            ProximityFrontRight = ProximityFrontRightRaw.Select(SmoothProximity).Scan((acc, next) => (next * SMOOTH_PROXIMITY) + (acc * (1f - SMOOTH_PROXIMITY)));
            ProximityRight = ProximityRightRaw.Select(SmoothProximity).Scan((acc, next) => (next * SMOOTH_PROXIMITY) + (acc * (1f - SMOOTH_PROXIMITY)));
        }

        public void Drive(Drive drive) => 
            Execute(InteractionMode.Drive(drive));

        public void Halt() => 
            Execute(InteractionMode.Halt());

        public void Song(Song song) => 
            Execute(InteractionMode.Sing(song));

        public void Sing(Melody melody) => 
            Execute(InteractionMode.Sing(melody));

        public void Sing(params (Note note, int length64ths)[] notes) =>
            Execute(InteractionMode.Sing(Melody.Define(notes)));

        public void Sing(SongNumber number) => 
            Execute(InteractionMode.Play(number));

        public void SeekDock() => 
            Execute(InteractionMode.SeekDock());

        public void ModeOff() => 
            Execute(InteractionMode.ModeOff());

        public void ModePassive() => 
            Execute(InteractionMode.ModePassive());

        public void ModeSafe() =>
            Execute(InteractionMode.ModeSafe());

        public void ModeFull() => 
            Execute(InteractionMode.ModeFull());

        internal void Send(Command command) =>
            adaptor.Input.OnNext(command.Data.ToArray());

        public void Debug(params byte[] data) =>
            adaptor.Input.OnNext(data);

        private void Execute(IInteractionMode command) =>
            InteractionMode = command;

        private void ForceMode(byte mode)
        {
            switch (mode)
            {
                case (byte)Mode.Off:
                    InteractionMode = new Off(this);
                    return;
                case (byte)Mode.Passive:
                    InteractionMode = new Passive(this);
                    return;
                case (byte)Mode.Safe:
                    InteractionMode = new Safe(this);
                    return;
                case (byte)Mode.Full:
                    InteractionMode = new Full(this);
                    return;
            }
        }

        public IObservable<Unit> Emergency { get; }
        public IObservable<bool> CliffLeft { get; }
        public IObservable<bool> CliffFrontLeft { get; }
        public IObservable<bool> CliffFrontRight { get; }
        public IObservable<bool> CliffRight { get; }
        public IObservable<Unit> ButtonClean { get; }
        public IObservable<Unit> ButtonSpot { get; }
        public IObservable<Unit> ButtonDock { get; }
        public IObservable<Unit> ButtonMinute { get; }
        public IObservable<Unit> ButtonHour { get; }
        public IObservable<Unit> ButtonDay { get; }
        public IObservable<Unit> ButtonSchedule { get; }
        public IObservable<Unit> ButtonClock { get; }
        public IObservable<int> BatteryCharge { get; }
        public IObservable<int> CliffLeftSignal { get; }
        public IObservable<int> CliffFrontLeftSignal { get; }
        public IObservable<int> CliffFrontRightSignal { get; }
        public IObservable<int> CliffRightSignal { get; }
        internal IObservable<Mode> RealOIMode { get; }
        public IObservable<int> NumberOfStreamPackets { get; }
        public IObservable<int> RequestedRightVelocity { get; }
        public IObservable<int> RequestedLeftVelocity { get; }
        public IObservable<int> ProximityLeftRaw { get; }
        public IObservable<int> ProximityFrontLeftRaw { get; }
        public IObservable<int> ProximityFrontRightRaw { get; }
        public IObservable<int> ProximityCenterLeftRaw { get; }
        public IObservable<int> ProximityCenterRightRaw { get; }
        public IObservable<int> ProximityRightRaw { get; }
        public IObservable<float> ProximityLeft { get; }
        public IObservable<float> ProximityFrontLeft { get; }
        public IObservable<float> ProximityCenterLeft { get; }
        public IObservable<float> ProximityCenterRight { get; }
        public IObservable<float> ProximityFrontRight { get; }
        public IObservable<float> ProximityRight { get; }

        private readonly Func<byte[], int> SingleSigned = data => (sbyte)data[0];
        private readonly Func<byte[], int> SingleUnsigned = data => data[0];
        private readonly Func<byte[], int> HighLowSigned = data => BitConverter.ToInt16(data, 0);
        private readonly Func<byte[], int> HighLowUnsigned = data => 256 * data[0] + data[1];
        private readonly Func<byte[], Buttons, bool> ParseButton = (data, button) => (data[0] & (int)button) > 0;
        private readonly Func<int, float> SmoothProximity = data => data / 4095f;
    }
}
