using System;

namespace iCreateOI2.Sensors
{
    [Flags]
    internal enum Buttons
    {
        Clean = 0,
        Spot = 1,
        Dock = 2,
        Minute = 4,
        Hour = 8,
        Day = 16,
        Schedule = 32,
        Clock = 64
    }

    public enum ChargingState
    { 
        NotCharging = 0,
        ReconditioningCharging,
        FullCharging,
        TrickleCharging,
        Waiting,
        ChargingFaultCondition
    }
}
