using Pomodoro.Core.Enums;

namespace Pomodoro.Core.Models
{
    public class FrequencyModel
    {
        public Guid Id { get; set; }

        public FrequencyValue FrequencyValue { get; set; } = FrequencyValue.None;

        public bool IsCustom { get; set; }

        public short Every { get; set; }
    }
}
