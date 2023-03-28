namespace Pomodoro.Core.Models
{
    public class PomodoroModel
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public DateTime ActualDate { get; set; }
        public int TimeSpent { get; set; }
        public bool IsDone { get; set; }
    }
}
