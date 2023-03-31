using Pomodoro.Core.Models.Base;

namespace Pomodoro.Core.Models
{
    public class TaskModel : BaseUserOrientedModel
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public DateTime InitialDate { get; set; }

        public short AllocatedTime { get; set; }

        public FrequencyModel? Frequency { get; set; }

        public float Progress { get; set; }
    }
}
