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
    internal class Adaptor
    {
        internal Adaptor(SerialPort port)
        {
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
            
            // Debug input and output to console
            // serialPortInput.Subscribe(data => Console.WriteLine(string.Join(' ', data.Select(b => b.ToString("{000}")))));
            // Output.Subscribe(data => Console.WriteLine(data.ToString("[000]")));
        }

        public readonly IObserver<byte[]> Input;
        public readonly IObservable<byte> Output;
    }
}
