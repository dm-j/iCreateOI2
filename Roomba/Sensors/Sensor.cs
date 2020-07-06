using iCreateOI2.Commands;
using System;
using System.Collections.Immutable;

namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Obsolete, this is being folded into the Sensor parsing state machine
    /// </summary>
    public class Sensor : IDataBytes
    {
        internal readonly SensorPacket packet;
        public ImmutableArray<byte> Data { get; }
        private readonly SensorParser parser;
        public int ResponseLength => parser.Length;

        private Sensor(SensorPacket packet, SensorParser parser)
        {
            this.packet = packet;
            Data = new[] { (byte)packet }.ToImmutableArray();
            this.parser = parser;
        }

        internal (SensorPacket packet, int value) Read(byte[] sensorium, ref int currentHeadPosition)
        {
            var result = (packet, parser.Parse(sensorium[currentHeadPosition .. (currentHeadPosition + parser.Length)]));
            currentHeadPosition += parser.Length;
            return result;
        }

        public static Sensor BumpsWheelDrops              { get; } = new Sensor(SensorPacket.BumpsWheelDrops,              SensorParser.SingleUnsigned);
        public static Sensor Wall                         { get; } = new Sensor(SensorPacket.Wall,                         SensorParser.SingleUnsigned);
        public static Sensor CliffLeft                    { get; } = new Sensor(SensorPacket.CliffLeft,                    SensorParser.SingleUnsigned);
        public static Sensor CliffFrontLeft               { get; } = new Sensor(SensorPacket.CliffFrontLeft,               SensorParser.SingleUnsigned);
        public static Sensor CliffFrontRight              { get; } = new Sensor(SensorPacket.CliffFrontRight,              SensorParser.SingleUnsigned);
        public static Sensor CliffRight                   { get; } = new Sensor(SensorPacket.CliffRight,                   SensorParser.SingleUnsigned);
        public static Sensor VirtualWall                  { get; } = new Sensor(SensorPacket.VirtualWall,                  SensorParser.SingleUnsigned);
        public static Sensor WheelOvercurrents            { get; } = new Sensor(SensorPacket.WheelOvercurrents,            SensorParser.SingleUnsigned);
        public static Sensor DirtDetect                   { get; } = new Sensor(SensorPacket.DirtDetect,                   SensorParser.SingleUnsigned);
        public static Sensor IROmni                       { get; } = new Sensor(SensorPacket.IROmni,                       SensorParser.SingleUnsigned);
        public static Sensor IRLeft                       { get; } = new Sensor(SensorPacket.IRLeft,                       SensorParser.SingleUnsigned);
        public static Sensor IRRight                      { get; } = new Sensor(SensorPacket.IRRight,                      SensorParser.SingleUnsigned);
        public static Sensor Buttons                      { get; } = new Sensor(SensorPacket.Buttons,                      SensorParser.SingleUnsigned);
        public static Sensor Distance                     { get; } = new Sensor(SensorPacket.Distance,                     SensorParser.HighLowSigned);
        public static Sensor Angle                        { get; } = new Sensor(SensorPacket.Angle,                        SensorParser.HighLowSigned);
        public static Sensor ChargingState                { get; } = new Sensor(SensorPacket.ChargingState,                SensorParser.SingleUnsigned);
        public static Sensor Voltage                      { get; } = new Sensor(SensorPacket.Voltage,                      SensorParser.HighLowUnsigned);
        public static Sensor Current                      { get; } = new Sensor(SensorPacket.Current,                      SensorParser.HighLowSigned);
        public static Sensor Temperature                  { get; } = new Sensor(SensorPacket.Temperature,                  SensorParser.SingleSigned);
        public static Sensor BatteryCharge                { get; } = new Sensor(SensorPacket.BatteryCharge,                SensorParser.HighLowUnsigned);
        public static Sensor BatteryCapacity              { get; } = new Sensor(SensorPacket.BatteryCapacity,              SensorParser.HighLowUnsigned);
        public static Sensor WallSignal                   { get; } = new Sensor(SensorPacket.WallSignal,                   SensorParser.HighLowUnsigned);
        public static Sensor CliffLeftSignal              { get; } = new Sensor(SensorPacket.CliffLeftSignal,              SensorParser.HighLowUnsigned);
        public static Sensor CliffFrontLeftSignal         { get; } = new Sensor(SensorPacket.CliffFrontLeftSignal,         SensorParser.HighLowUnsigned);
        public static Sensor CliffFrontRightSignal        { get; } = new Sensor(SensorPacket.CliffFrontRightSignal,        SensorParser.HighLowUnsigned);
        public static Sensor CliffRightSignal             { get; } = new Sensor(SensorPacket.CliffRightSignal,             SensorParser.HighLowUnsigned);
        public static Sensor ChargingSourcesAvailable     { get; } = new Sensor(SensorPacket.ChargingSourcesAvailable,     SensorParser.SingleUnsigned);
        public static Sensor OIMode                       { get; } = new Sensor(SensorPacket.OIMode,                       SensorParser.SingleUnsigned);
        public static Sensor SongNumber                   { get; } = new Sensor(SensorPacket.SongNumber,                   SensorParser.SingleUnsigned);
        public static Sensor SongPlaying                  { get; } = new Sensor(SensorPacket.SongPlaying,                  SensorParser.SingleUnsigned);
        public static Sensor StreamPackets                { get; } = new Sensor(SensorPacket.StreamPackets,                SensorParser.SingleUnsigned);
        public static Sensor RequestedVelocity            { get; } = new Sensor(SensorPacket.RequestedVelocity,            SensorParser.HighLowSigned);
        public static Sensor RequestedRadius              { get; } = new Sensor(SensorPacket.RequestedRadius,              SensorParser.HighLowSigned);
        public static Sensor RequestedRightVelocity       { get; } = new Sensor(SensorPacket.RequestedRightVelocity,       SensorParser.HighLowSigned);
        public static Sensor RequestedLeftVelocity        { get; } = new Sensor(SensorPacket.RequestedLeftVelocity,        SensorParser.HighLowSigned);
        public static Sensor LeftEncoderCounts            { get; } = new Sensor(SensorPacket.LeftEncoderCounts,            SensorParser.HighLowUnsigned);
        public static Sensor RightEncoderCounts           { get; } = new Sensor(SensorPacket.RightEncoderCounts,           SensorParser.HighLowSigned);
        public static Sensor LightBumper                  { get; } = new Sensor(SensorPacket.LightBumper,                  SensorParser.SingleUnsigned);
        public static Sensor LightBumperLeftSignal        { get; } = new Sensor(SensorPacket.LightBumperLeftSignal,        SensorParser.HighLowUnsigned);
        public static Sensor LightBumperFrontLeftSignal   { get; } = new Sensor(SensorPacket.LightBumperFrontLeftSignal,   SensorParser.HighLowUnsigned);
        public static Sensor LightBumperCenterLeftSignal  { get; } = new Sensor(SensorPacket.LightBumperCenterLeftSignal,  SensorParser.HighLowUnsigned);
        public static Sensor LightBumperCenterRightSignal { get; } = new Sensor(SensorPacket.LightBumperCenterRightSignal, SensorParser.HighLowUnsigned);
        public static Sensor LightBumperFrontRightSignal  { get; } = new Sensor(SensorPacket.LightBumperFrontRightSignal,  SensorParser.HighLowUnsigned);
        public static Sensor LightBumperRightSignal       { get; } = new Sensor(SensorPacket.LightBumperRightSignal,       SensorParser.HighLowUnsigned);
        public static Sensor MotorCurrentLeft             { get; } = new Sensor(SensorPacket.MotorCurrentLeft,             SensorParser.HighLowSigned);
        public static Sensor MotorCurrentRight            { get; } = new Sensor(SensorPacket.MotorCurrentRight,            SensorParser.HighLowSigned);
        public static Sensor MainBrushMotorCurrent        { get; } = new Sensor(SensorPacket.MainBrushMotorCurrent,        SensorParser.HighLowSigned);
        public static Sensor Stasis                       { get; } = new Sensor(SensorPacket.Stasis,                       SensorParser.SingleUnsigned);

        private class SensorParser
        {
            internal readonly Func<byte[], int> Parse;
            internal readonly int Length;

            internal SensorParser(Func<byte[], int> parse, int length)
            {
                Parse = parse;
                Length = length;
            }

            internal static SensorParser SingleSigned    { get; } = new SensorParser(data => (sbyte)data[0], 1);
            internal static SensorParser SingleUnsigned  { get; } = new SensorParser(data => data[0], 1);
            internal static SensorParser HighLowSigned   { get; } = new SensorParser(data => BitConverter.ToInt16(data, 0), 2);
            internal static SensorParser HighLowUnsigned { get; } = new SensorParser(data => 256 * data[0] + data[1], 2);
        }
    }
}
