using iCreateOI2.Commands;
using iCreateOI2.Communications;

namespace iCreateOI2.Modes
{
    public interface IMode
    {
        IMode ModeFull();
        IMode ModePassive();
        IMode ModeSafe();
        IMode Start();
        IMode Drive(Drive drive);
        IMode Sing(Song song);
        IMode Play(SongNumber number);
        IMode Sing(Melody melody);
        IMode ModeOff();
        IMode SeekDock();
    }

    internal abstract class Mode : IMode
    {
        protected Roomba robot;

        protected Mode(Roomba robot)
        {
            this.robot = robot;
        }

        public abstract IMode Start();
        public abstract IMode ModePassive();
        public abstract IMode ModeSafe();
        public abstract IMode ModeFull();
        public abstract IMode SeekDock();
        public abstract IMode ModeOff();
        public abstract IMode Drive(Drive drive);
        public abstract IMode Sing(Song song);
        public abstract IMode Play(SongNumber number);
        public abstract IMode Sing(Melody melody);

        public static IMode Init(Roomba robot) => 
            new Off(robot);
    }
}
