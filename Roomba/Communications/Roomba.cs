using iCreateOI2.Commands;
using iCreateOI2.Modes;
using iCreateOI2.Sensors;
using System;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Linq;

namespace iCreateOI2.Communications
{
    public class Roomba
    {
        private readonly Adaptor adaptor;
        private IMode CurrentMode;
        private ISensorParser SensorParser = ReadyToRead.Instance;

        public Sensors.Sensors Sensors = iCreateOI2.Sensors.Sensors.Instance;

        public Roomba(SerialPort port)
        {
            adaptor = new Adaptor(port);
            adaptor.Output.Subscribe(b => Parse(b));
            Execute(Mode.Init(this));
        }

        public void Drive(Drive drive) => 
            Execute(CurrentMode.Drive(drive));

        public void Halt() => 
            Execute(CurrentMode.Drive(Commands.Drive.Halt));

        public void Song(Song song) => 
            Execute(CurrentMode.Sing(song));

        public void Sing(Melody melody) => 
            Execute(CurrentMode.Sing(melody));

        public void Sing(params (Note note, int length64ths)[] notes) =>
            Execute(CurrentMode.Sing(Melody.Define(notes)));

        public void Sing(SongNumber number) => 
            Execute(CurrentMode.Play(number));

        public void SeekDock() => 
            Execute(CurrentMode.SeekDock());

        public void ModeOff() => 
            Execute(CurrentMode.ModeOff());

        public void ModePassive() => 
            Execute(CurrentMode.ModePassive());

        public void ModeSafe() =>
            Execute(CurrentMode.ModeSafe());

        public void ModeFull() => 
            Execute(CurrentMode.ModeFull());

        internal void Send(Command command) =>
            adaptor.Input.OnNext(command.Data.ToArray());

        public void Debug(params byte[] data) =>
            adaptor.Input.OnNext(data);

        private void Execute(IMode command) =>
            CurrentMode = command;

        private void Parse(byte b) =>
            SensorParser = SensorParser.Parse(b);
    }
}
