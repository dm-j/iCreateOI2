using iCreateOI2.Commands;
using iCreateOI2.Communications;
using System;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace iCreateOI2
{
    class Program
    {
        static void Main(string[] _)
        {
            Console.WriteLine("Connecting to Fenton Chassis");
            Console.WriteLine("Ports available:");
            Console.WriteLine(SerialPort.GetPortNames().Aggregate(new StringBuilder(), (acc, next) => acc.AppendLine(next)).ToString());
            Console.Write("Choose port: ");

            string portName = Console.ReadLine();
            portName = string.IsNullOrWhiteSpace(portName.Trim())
                            ? "COM5"
                            : portName;

            SerialPort serialPort = new SerialPort
            {
                PortName = portName,
                WriteTimeout = 1000,
                ReadTimeout = 1000,
                BaudRate = 115200,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None
            };

            Roomba roomba = new Roomba(serialPort);
            
            roomba.Song(Song.Define(SongNumber.Song1, Melody.Define((Note.C4, 64), (Note.D4, 32), (Note.E4, 16))));
            
            roomba.Sing(SongNumber.Song1);

            roomba.

            Console.ReadLine();

            roomba.ModeOff();
        }
    }
}
