using iCreateOI2.Commands;
using iCreateOI2.Communications;
using System;
using System.IO.Ports;

namespace Program
{
    class Program
    {
        static void Main(string[] _)
        {
            Console.WriteLine("Connecting to Fenton Chassis");
            Console.WriteLine("Ports available:");
            Console.WriteLine(string.Join(" ", SerialPort.GetPortNames()));
            Console.Write("Choose port: ");

            string portName = Console.ReadLine();
            portName = string.IsNullOrWhiteSpace(portName.Trim())
                            ? "COM5"
                            : portName.ToUpper().Trim();

            Roomba roomba = new Roomba(portName);

            roomba.Sensors.ButtonSpot.Subscribe(o => Console.WriteLine("Spot Button Pressed"));
            
            roomba.Song(Song.Define(SongNumber.Song1, Melody.Define((Note.C4, 64), (Note.D4, 32), (Note.E4, 16))));
            
            roomba.Sing(SongNumber.Song1);

            Console.ReadLine();

            roomba.ModeOff();
        }
    }
}
