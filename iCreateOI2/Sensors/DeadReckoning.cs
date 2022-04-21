using System;

namespace iCreateOI2.Sensors
{
    public class DeadReckoning
    {
        public const double WHEEL_BASE_DIAMETER_MM = 235d;
        public const double WHEEL_CIRCUMFERENCE_MM = 226.2d;
        public const double ENCODER_TICKS_PER_REVOLUTION = 508.8d;
        private const double MM_PER_TICK = WHEEL_CIRCUMFERENCE_MM / ENCODER_TICKS_PER_REVOLUTION;

        public double Heading { get; private set; } = 0d;
        public double X_Position { get; private set; } = 0d;
        public double Y_Position { get; private set; } = 0d;
        
        
        
        public void Update(double left, double right)
        {
            if (Math.Abs(left - right) < float.Epsilon) 
            {
                X_Position += left * MM_PER_TICK * Math.Cos(Heading);
                Y_Position += right * MM_PER_TICK * Math.Sin(Heading);
            } 
            else
            {
                double radius = WHEEL_BASE_DIAMETER_MM * (left + right) / (2 * (right - left));
                double wd = (right - left) / WHEEL_BASE_DIAMETER_MM;

                X_Position += radius * Math.Sin(wd + Heading) - radius * Math.Sin(Heading);
                Y_Position -= radius * Math.Cos(wd + Heading) + radius * Math.Cos(Heading);
                Heading = boundAngle(Heading + wd);
            }
        }

        private double boundAngle(double angle)
        {
            while (angle > Math.PI)
                angle -= Math.PI;
            while (angle < -Math.PI)
                angle += Math.PI;
            return angle;
        }
    }
}