using iCreateOI2.Communications;
using iCreateOI2.Domain;
using iCreateOI2.Modes;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace iCreateOI2.Sensors
{
    public class Sensors
    {
        private readonly SensorStream stream;

        internal Sensors(OI2Port port)
        {
            stream = new SensorStream(port.Output);

            Emergency = stream.Parsers[SensorPacket.BumpsWheelDrops].DistinctUntilChanged().Select(value => SingleUnsigned(value) != 0).Where(o => o).Select(_ => Unit.Default).Share();

            var ButtonsRaw = stream.Parsers[SensorPacket.Buttons].DistinctUntilChanged().Select(SingleUnsigned).Where(o => o > 0).Share();
            ButtonClean = ButtonsRaw.Select(value => ParseButton(value, Buttons.Clean)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default).Share();
            ButtonSpot = ButtonsRaw.Select(value => ParseButton(value, Buttons.Spot)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default).Share();
            ButtonDock = ButtonsRaw.Select(value => ParseButton(value, Buttons.Dock)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default).Share();
            ButtonMinute = ButtonsRaw.Select(value => ParseButton(value, Buttons.Minute)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default).Share();
            ButtonHour = ButtonsRaw.Select(value => ParseButton(value, Buttons.Hour)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default).Share();
            ButtonDay = ButtonsRaw.Select(value => ParseButton(value, Buttons.Day)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default).Share();
            ButtonSchedule = ButtonsRaw.Select(value => ParseButton(value, Buttons.Schedule)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default).Share();
            ButtonClock = ButtonsRaw.Select(value => ParseButton(value, Buttons.Clock)).DistinctUntilChanged().Where(o => o).Select(_ => Unit.Default).Share();

            RealOIMode = stream.Parsers[SensorPacket.OIMode].DistinctUntilChanged().Select(value => (Mode)SingleUnsigned(value)).Share();

            RequestedRightVelocity = stream.Parsers[SensorPacket.RequestedRightVelocity].Select(HighLowSigned).Share();
            RequestedLeftVelocity = stream.Parsers[SensorPacket.RequestedLeftVelocity].Select(HighLowSigned).Share();

            var ProximityLeft = stream.Parsers[SensorPacket.LightBumperLeftSignal].Scan(0f, SmoothProximity);
            var ProximityFrontLeft = stream.Parsers[SensorPacket.LightBumperFrontLeftSignal].Scan(0f, SmoothProximity);
            var ProximityCenterLeft = stream.Parsers[SensorPacket.LightBumperCenterLeftSignal].Scan(0f, SmoothProximity);
            var ProximityCenterRight = stream.Parsers[SensorPacket.LightBumperCenterRightSignal].Scan(0f, SmoothProximity);
            var ProximityFrontRight = stream.Parsers[SensorPacket.LightBumperFrontRightSignal].Scan(0f, SmoothProximity);
            var ProximityRight = stream.Parsers[SensorPacket.LightBumperRightSignal].Scan(0f, SmoothProximity);

            var prox = ProximityLeft.And(ProximityFrontLeft).And(ProximityCenterLeft).And(ProximityCenterRight).And(ProximityFrontRight).And(ProximityRight);
            Proximity = Observable.When(prox.Then((LL, FL, CL, CR, FR, RR) => new ProximitySensors(LL, FL, CL, CR, FR, RR))).Share();
        }

        public IObservable<Unit> Emergency { get; private set; }
        public IObservable<Unit> ButtonClean { get; private set; }
        public IObservable<Unit> ButtonSpot { get; private set; }
        public IObservable<Unit> ButtonDock { get; private set; }
        public IObservable<Unit> ButtonMinute { get; private set; }
        public IObservable<Unit> ButtonHour { get; private set; }
        public IObservable<Unit> ButtonDay { get; private set; }
        public IObservable<Unit> ButtonSchedule { get; private set; }
        public IObservable<Unit> ButtonClock { get; private set; }
        internal IObservable<Mode> RealOIMode { get; private set; }
        public IObservable<int> RequestedRightVelocity { get; private set; }
        public IObservable<int> RequestedLeftVelocity { get; private set; }
        public IObservable<ProximitySensors> Proximity { get; private set; }


        private readonly Func<byte[], int> SingleUnsigned = data => data[0];
        private readonly Func<byte[], int> HighLowSigned = data => BitConverter.ToInt16(data, 0);
        private readonly Func<int, Buttons, bool> ParseButton = (data, button) => (data & (int)button) > 0;
        private readonly Func<float, byte[], float> SmoothProximity = (acc, next) => (acc * 0.8f) + (((256 * next[0] + next[1]) / 4095f) * (1f - 0.8f));
    }
}