using System;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace iCreateOI2.Communications
{
    internal class OI2Port
    {
        internal OI2Port(string portName)
        {
            SerialPort port = new SerialPort
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

            Subject<byte[]> serialPortInput = new Subject<byte[]>();
            Input = serialPortInput.AsObserver();
            port.Open();
            port.DiscardOutBuffer();
            port.DiscardInBuffer();
            StreamReader reader = new StreamReader(port.BaseStream);
            serialPortInput.Subscribe(bytes => port.Write(bytes, 0, bytes.Length));
            Output = Observable.FromEventPattern<
                SerialDataReceivedEventHandler,
                SerialDataReceivedEventArgs>
                (
                    handler => port.DataReceived += handler,
                    handler => port.DataReceived -= handler
                ).SelectMany(_ =>
                {
                    char[] buffer = new char[16];
                    reader.ReadBlock(buffer, 0, 16);
                    return Encoding.UTF8.GetBytes(buffer);
                });
        }

        internal readonly IObserver<byte[]> Input;
        internal readonly IObservable<byte> Output;
    }
}
