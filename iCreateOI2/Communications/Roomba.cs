using iCreateOI2.Commands;
using iCreateOI2.Modes;
using System.Linq;
using System.Reactive.Linq;

namespace iCreateOI2.Communications
{
    public class Roomba
    {
        private readonly OI2Port openInterface;
        private IInteractionMode InteractionMode;
        public readonly Sensors.Sensors Sensors;

        public Roomba(string portName)
        {
            openInterface = new OI2Port(portName);
            Sensors = new Sensors.Sensors(openInterface);
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
            openInterface.Input.OnNext(command.Data.ToArray());

        public void StartStream() =>
            Execute(InteractionMode.Stream());

        public void Debug(params byte[] data) =>
            openInterface.Input.OnNext(data);

        private void Execute(IInteractionMode command) =>
            InteractionMode = command;
    }
}
