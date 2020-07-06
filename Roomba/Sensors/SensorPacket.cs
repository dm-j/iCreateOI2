﻿namespace iCreateOI2.Sensors
{
    public enum SensorPacket
    {
        BumpsWheelDrops = 7,
        Wall = 8,
        CliffLeft = 9,
        CliffFrontLeft = 10,
        CliffFrontRight = 11,
        CliffRight = 12,
        VirtualWall = 13,
        WheelOvercurrents = 14,
        DirtDetect = 15,
        IROmni = 17,
        IRLeft = 52,
        IRRight = 53,
        Buttons = 18,
        Distance = 19, // Warning: Packet 019 can be mistaken for the start sensor data stream marker!
        Angle = 20,
        ChargingState = 21,
        Voltage = 22,
        Current = 23,
        Temperature = 24,
        BatteryCharge = 25,
        BatteryCapacity = 26,
        WallSignal = 27,
        CliffLeftSignal = 28,
        CliffFrontLeftSignal = 28,
        CliffFrontRightSignal = 30,
        CliffRightSignal = 31,
        ChargingSourcesAvailable = 34,
        OIMode = 35,
        SongNumber = 36,
        SongPlaying = 37,
        StreamPackets = 38,
        RequestedVelocity = 39,
        RequestedRadius = 40,
        RequestedRightVelocity = 41,
        RequestedLeftVelocity = 42,
        LeftEncoderCounts = 43,
        RightEncoderCounts = 44,
        LightBumper = 45,
        LightBumperLeftSignal = 46,
        LightBumperFrontLeftSignal = 47,
        LightBumperCenterLeftSignal = 48,
        LightBumperCenterRightSignal = 49,
        LightBumperFrontRightSignal = 50,
        LightBumperRightSignal = 51,
        MotorCurrentLeft = 54,
        MotorCurrentRight = 55,
        MainBrushMotorCurrent = 56,
        Stasis = 58
    }
}
