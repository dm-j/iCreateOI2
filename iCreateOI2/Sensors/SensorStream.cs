using iCreateOI2.Modes;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace iCreateOI2.Sensors
{
    internal class SensorStream
    {
        private IReadSensorPacketStreamData ReadMode;
        internal readonly Aligning Aligning;
        internal readonly AwaitingStart AwaitingStart;
        internal readonly ReadingLength ReadingLength;
        internal readonly ReadingPayload ReadingPayload;
        internal readonly ReadingComplete ReadingComplete;
        internal readonly Dictionary<SensorPacket, Subject<byte[]>> Parsers;

        internal SensorStream(IObservable<byte> outputFromRoomba)
        {
            Aligning = new Aligning(this);
            AwaitingStart = new AwaitingStart(this);
            ReadingLength = new ReadingLength(this);
            ReadingPayload = new ReadingPayload(this);
            ReadingComplete = new ReadingComplete(this);

            ReadMode = AwaitingStart;

            outputFromRoomba.Subscribe(Output);
        }

        private void Output(byte b) =>
            ReadMode = ReadMode.Output(b);
    }
}
