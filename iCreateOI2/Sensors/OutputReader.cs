using iCreateOI2.Modes;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace iCreateOI2.Sensors
{
    internal class OutputReader
    {
        private const float SMOOTH_PROXIMITY = 0.8f;
        private IReadOutput ParseMode;
        internal readonly Aligning Aligning;
        internal readonly AwaitingStart AwaitingStart;
        internal readonly ReadingLength ReadingLength;
        internal readonly ReadingPayload ReadingPayload;
        internal readonly ReadingComplete ReadingComplete;
        internal readonly Dictionary<SensorPacket, Subject<byte[]>> Parsers;

        internal OutputReader(IObservable<byte> outputFromRoomba)
        {
            Aligning = new Aligning(this);
            AwaitingStart = new AwaitingStart(this);
            ReadingLength = new ReadingLength(this);
            ReadingPayload = new ReadingPayload(this);
            ReadingComplete = new ReadingComplete(this);

            ParseMode = AwaitingStart;

            outputFromRoomba.Subscribe(Parse);

            Emergency               = Parsers[SensorPacket.BumpsWheelDrops].Select(value => SingleUnsigned(value) != 0).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            CliffLeft               = Parsers[SensorPacket.CliffLeft].Select(value => SingleUnsigned(value) != 0);
            CliffFrontLeft          = Parsers[SensorPacket.CliffFrontLeft].Select(value => SingleUnsigned(value) != 0);
            CliffFrontRight         = Parsers[SensorPacket.CliffFrontRight].Select(value => SingleUnsigned(value) != 0);
            CliffRight              = Parsers[SensorPacket.CliffRight].Select(value => SingleUnsigned(value) != 0);
            ButtonClean             = Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Clean)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonSpot              = Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Spot)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonDock              = Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Dock)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonMinute            = Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Minute)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonHour              = Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Hour)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonDay               = Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Day)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonSchedule          = Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Schedule)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            ButtonClock             = Parsers[SensorPacket.Buttons].Select(value => ParseButton(value, Buttons.Clock)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default);
            CliffLeftSignal         = Parsers[SensorPacket.CliffLeftSignal].Select(HighLowUnsigned);
            CliffFrontLeftSignal    = Parsers[SensorPacket.CliffFrontLeftSignal].Select(HighLowUnsigned);
            CliffFrontRightSignal   = Parsers[SensorPacket.CliffFrontRightSignal].Select(HighLowUnsigned);
            CliffRightSignal        = Parsers[SensorPacket.CliffRightSignal].Select(HighLowUnsigned);
            OIMode                  = Parsers[SensorPacket.OIMode].Select(value => (Mode)SingleUnsigned(value)).DistinctUntilChanged();
            NumberOfStreamPackets   = Parsers[SensorPacket.StreamPackets].Select(SingleUnsigned);
            RequestedRightVelocity  = Parsers[SensorPacket.RequestedRightVelocity].Select(HighLowSigned);
            RequestedLeftVelocity   = Parsers[SensorPacket.RequestedLeftVelocity].Select(HighLowSigned);
            ProximityLeftRaw        = Parsers[SensorPacket.LightBumperLeftSignal].Select(HighLowUnsigned);
            ProximityFrontLeftRaw   = Parsers[SensorPacket.LightBumperFrontLeftSignal].Select(HighLowUnsigned);
            ProximityCenterLeftRaw  = Parsers[SensorPacket.LightBumperCenterLeftSignal].Select(HighLowUnsigned);
            ProximityCenterRightRaw = Parsers[SensorPacket.LightBumperCenterRightSignal].Select(HighLowUnsigned);
            ProximityFrontRightRaw  = Parsers[SensorPacket.LightBumperFrontRightSignal].Select(HighLowUnsigned);
            ProximityRightRaw       = Parsers[SensorPacket.LightBumperRightSignal].Select(HighLowUnsigned);
            ProximityLeft           = ProximityLeftRaw.Select(SmoothProximity).Scan((acc, next) => (next * SMOOTH_PROXIMITY) + (acc * (1f - SMOOTH_PROXIMITY)));
            ProximityFrontLeft      = ProximityFrontLeftRaw.Select(SmoothProximity).Scan((acc, next) => (next * SMOOTH_PROXIMITY) + (acc * (1f - SMOOTH_PROXIMITY)));
            ProximityCenterLeft     = ProximityCenterLeftRaw.Select(SmoothProximity).Scan((acc, next) => (next * SMOOTH_PROXIMITY) + (acc * (1f - SMOOTH_PROXIMITY)));
            ProximityCenterRight    = ProximityCenterRightRaw.Select(SmoothProximity).Scan((acc, next) => (next * SMOOTH_PROXIMITY) + (acc * (1f - SMOOTH_PROXIMITY)));
            ProximityFrontRight     = ProximityFrontRightRaw.Select(SmoothProximity).Scan((acc, next) => (next * SMOOTH_PROXIMITY) + (acc * (1f - SMOOTH_PROXIMITY)));
            ProximityRight          = ProximityRightRaw.Select(SmoothProximity).Scan((acc, next) => (next * SMOOTH_PROXIMITY) + (acc * (1f - SMOOTH_PROXIMITY)));
        }

        private void Parse(byte b) =>
            ParseMode = ParseMode.Read(b);

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
        public IObservable<Mode> OIMode { get; }
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
