using System;
using System.Reactive.Linq;

namespace iCreateOI2.Domain
{
    public static class Extensions
    {
        public static byte[] ToHighLow(this int value) =>
            new byte[] { (byte)(value >> 8), (byte)(value & 255) };

        public static uint FromHighLow(this byte[] bytes, int startIndex = 0) =>
            (uint)(256 * bytes[startIndex] + bytes[startIndex + 1]);

        public static int FromSignedHighLow(this byte[] bytes, int startIndex = 0)
        {
            uint u = (uint)bytes[startIndex] << 8 | bytes[startIndex + 1];
            return (int)(u >= (1u << 15) ? u - (1u << 16) : u);
        }

        public static IObservable<T> Share<T>(this IObservable<T> values) =>
            values.Publish().RefCount();
    }
}
