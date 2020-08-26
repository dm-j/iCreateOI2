namespace iCreateOI2.Sensors
{
    public struct ProximitySensors
    {
        public readonly float Left;
        public readonly float FrontLeft;
        public readonly float CenterLeft;
        public readonly float CenterRight;
        public readonly float FrontRight;
        public readonly float Right;

        public ProximitySensors(float LL, float FL, float CL, float CR, float FR, float RR)
        {
            Left = LL;
            FrontLeft = FL;
            CenterLeft = CL;
            CenterRight = CR;
            FrontRight = FR;
            Right = RR;
        }

        public override string ToString() =>
            $"Proximity: [{Left:000} {FrontLeft:000} {CenterLeft:000} {CenterRight:000} {FrontRight:000} {Right: 000}]";
    }
}
