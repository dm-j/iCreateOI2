using iCreateOI2.Commands;
using iCreateOI2.Communications;

namespace iCreateOI2.Modes
{
    internal enum Mode
    {
        Off,
        Passive,
        Safe,
        Full
    }

    public interface IInteractionMode
    {
        IInteractionMode ModeFull();
        IInteractionMode ModePassive();
        IInteractionMode ModeSafe();
        IInteractionMode Start();
        IInteractionMode Drive(Drive drive);
        IInteractionMode Halt();
        IInteractionMode Sing(Song song);
        IInteractionMode Play(SongNumber number);
        IInteractionMode Sing(Melody melody);
        IInteractionMode ModeOff();
        IInteractionMode SeekDock();
    }

    internal abstract class OpenInterfaceMode : IInteractionMode
    {
        protected Roomba robot;

        protected OpenInterfaceMode(Roomba robot)
        {
            this.robot = robot;
        }

        public abstract IInteractionMode Start();
        public abstract IInteractionMode ModePassive();
        public abstract IInteractionMode ModeSafe();
        public abstract IInteractionMode ModeFull();
        public abstract IInteractionMode SeekDock();
        public abstract IInteractionMode ModeOff();
        public abstract IInteractionMode Drive(Drive drive);
        public abstract IInteractionMode Halt();
        public abstract IInteractionMode Sing(Song song);
        public abstract IInteractionMode Play(SongNumber number);
        public abstract IInteractionMode Sing(Melody melody);

        public static IInteractionMode Init(Roomba robot) => 
            new Off(robot);
    }
}
