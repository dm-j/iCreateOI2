namespace iCreateOI2.Sensors
{
    /// <summary>
    /// Try to align sensor data which may start in the middle
    /// </summary>
    internal class Aligning : ISensorParseMode
    {
        private Aligning() { }

        internal static Aligning Mode { get; } = new Aligning();

        public ISensorParseMode Parse(byte b) =>
            b == Ready.START_BYTE
                ? (ISensorParseMode)Ready.Mode
                : this;
    }
}
