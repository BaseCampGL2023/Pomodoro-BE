using Pomodoro.Core.Models.Base;

namespace Pomodoro.Core.Models
{
    public class SettingsModel : BaseModel
    {
        public Guid UserId { get; set; }
        public byte PomodoroDuration { get; set; }
        public byte ShortBreak { get; set; }
        public byte LongBreak { get; set; }
        public byte PomodorosBeforeLongBreak { get; set; }
        public bool AutostartEnabled { get; set; }
    }
}
