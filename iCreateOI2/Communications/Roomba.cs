using iCreateOI2.Commands;
using iCreateOI2.Modes;
using iCreateOI2.Sensors;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Linq;

namespace iCreateOI2.Communications
{
    public class Roomba
    {
        private readonly Communications adaptor;
        private IInteractionMode InteractionMode;
        private readonly OutputReader Output;

        public Roomba(SerialPort port)
        {
            adaptor = new Communications(port);
            Output = new OutputReader(adaptor.Output);
            Execute(OpenInterfaceMode.Init(this));
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

        private void ForceMode(byte mode)
        {
            switch (mode)
            {
                case (byte)Mode.Off:
                    InteractionMode = new Off(this);
                    return;
                case (byte)Mode.Passive:
                    InteractionMode = new Passive(this);
                    return;
                case (byte)Mode.Safe:
                    InteractionMode = new Safe(this);
                    return;
                case (byte)Mode.Full:
                    InteractionMode = new Full(this);
                    return;
            }
        }
    }
}
