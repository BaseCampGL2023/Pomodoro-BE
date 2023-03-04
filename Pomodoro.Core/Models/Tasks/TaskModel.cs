using Pomodoro.Core.Enums;
using Pomodoro.Core.Models.Frequency;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Core.Models.Tasks
{
    public class TaskModel : BaseUserOrientedModel
    {        
        public Guid TaskId { get; set; }
        public string? Title { get; set; }
        public DateTime InitialDate { get; set; }
        public short AllocatedTime { get; set; }
        public FrequencyModel FrequencyData { get; set; }

    }
}
