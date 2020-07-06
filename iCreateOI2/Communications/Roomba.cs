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
        private IInteractionMode InteractionMode;
        private ISensorParseMode SensorParser = Aligning.Mode;

        private readonly Checking Sensors = Checking.Mode;

        public Roomba(SerialPort port)
        {
            adaptor = new Adaptor(port);
            adaptor.Output.Subscribe(Parse);
            Execute(Mode.Init(this));
        }

        public void Drive(Drive drive) => 
            Execute(InteractionMode.Drive(drive));

        public void Halt() => 
            Execute(InteractionMode.Halt());

        public void Song(Song song) => 
            Execute(InteractionMode.Sing(song));

        public void Sing(Melody melody) => 
            Execute(InteractionMode.Sing(melody));

        public void Sing(params (Note note, int length64ths)[] notes) =>
            Execute(InteractionMode.Sing(Melody.Define(notes)));

        public void Sing(SongNumber number) => 
            Execute(InteractionMode.Play(number));

        public void SeekDock() => 
            Execute(InteractionMode.SeekDock());

        public void ModeOff() => 
            Execute(InteractionMode.ModeOff());

        public void ModePassive() => 
            Execute(InteractionMode.ModePassive());

        public void ModeSafe() =>
            Execute(InteractionMode.ModeSafe());

        public void ModeFull() => 
            Execute(InteractionMode.ModeFull());

        internal void Send(Command command) =>
            adaptor.Input.OnNext(command.Data.ToArray());

        public void Debug(params byte[] data) =>
            adaptor.Input.OnNext(data);

        private void Execute(IInteractionMode command) =>
            InteractionMode = command;

        private void Parse(byte b) =>
            SensorParser = SensorParser.Parse(b);

        private void ForceMode(byte mode)
        {
            switch (mode)
            {
                case (byte)Mode.Off:
                    ModeOff();
                    return;
                case (byte)Mode.Passive:
                    ModePassive();
                    return;
                case (byte)Mode.Safe:
                    ModeSafe();
                    return;
                case (byte)Mode.Full:
                    ModeFull();
                    return;
            }
        }
    }
}
