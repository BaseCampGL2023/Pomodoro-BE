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
        public string Title { get; set; } = string.Empty;
        public DateTime InitialDate { get; set; }
        public short AllocatedTime { get; set; }
        public string Frequency { get; set; } = "None";
        public short Every { get; set; }
        public bool Custom { get; set; }

    }
}
