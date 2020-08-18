using System;
using System.Collections.Generic;

namespace iCreateOI2.Sensors
{
    internal class OutputParser
    {
        private readonly Dictionary<SensorPacket, int> Lengths = new Dictionary<SensorPacket, int> 
        {
            { SensorPacket.BumpsWheelDrops,              1 },
            { SensorPacket.Wall,                         1 },
            { SensorPacket.CliffLeft,                    1 },
            { SensorPacket.CliffFrontLeft,               1 },
            { SensorPacket.CliffFrontRight,              1 },
            { SensorPacket.CliffRight,                   1 },
            { SensorPacket.VirtualWall,                  1 },
            { SensorPacket.WheelOvercurrents,            1 },
            { SensorPacket.DirtDetect,                   1 },
            { SensorPacket.IROmni,                       1 },
            { SensorPacket.IRLeft,                       1 },
            { SensorPacket.IRRight,                      1 },
            { SensorPacket.Buttons,                      1 },
            { SensorPacket.Distance,                     2 },
            { SensorPacket.Angle,                        2 },
            { SensorPacket.ChargingState,                1 },
            { SensorPacket.Voltage,                      2 },
            { SensorPacket.Current,                      2 },
            { SensorPacket.Temperature,                  1 },
            { SensorPacket.BatteryCharge,                2 },
            { SensorPacket.BatteryCapacity,              2 },
            { SensorPacket.WallSignal,                   2 },
            { SensorPacket.CliffLeftSignal,              2 },
            { SensorPacket.CliffFrontLeftSignal,         2 },
            { SensorPacket.CliffFrontRightSignal,        2 },
            { SensorPacket.CliffRightSignal,             2 },
            { SensorPacket.ChargingSourcesAvailable,     1 },
            { SensorPacket.OIMode,                       1 },
            { SensorPacket.SongNumber,                   1 },
            { SensorPacket.SongPlaying,                  1 },
            { SensorPacket.StreamPackets,                1 },
            { SensorPacket.RequestedVelocity,            2 },
            { SensorPacket.RequestedRadius,              2 },
            { SensorPacket.RequestedRightVelocity,       2 },
            { SensorPacket.RequestedLeftVelocity,        2 },
            { SensorPacket.LeftEncoderCounts,            2 },
            { SensorPacket.RightEncoderCounts,           2 },
            { SensorPacket.LightBumper,                  2 },
            { SensorPacket.LightBumperLeftSignal,        1 },
            { SensorPacket.LightBumperFrontLeftSignal,   2 },
            { SensorPacket.LightBumperCenterLeftSignal,  2 },
            { SensorPacket.LightBumperCenterRightSignal, 2 },
            { SensorPacket.LightBumperFrontRightSignal,  2 },
            { SensorPacket.LightBumperRightSignal,       2 },
            { SensorPacket.MotorCurrentLeft,             2 },
            { SensorPacket.MotorCurrentRight,            2 },
            { SensorPacket.MainBrushMotorCurrent,        2 },
            { SensorPacket.Stasis,                       1 },
        };

        private readonly Dictionary<SensorPacket, IObserver<byte[]>> Feeds;

        internal OutputParser(Dictionary<SensorPacket, IObserver<byte[]>> feeds)
        {
            Feeds = feeds;
        }

        public void Parse(byte[] data)
        {
            int length;
            int pointer = 0;
            while (pointer < data.Length)
            {
                SensorPacket packet = (SensorPacket)data[pointer];
                length = Lengths[packet];
                byte[] payload = new byte[length];
                Array.Copy(data, ++pointer, payload, 0, length);
                pointer += length;
                Feeds[packet].OnNext(payload);
            }
        }
    }
}
