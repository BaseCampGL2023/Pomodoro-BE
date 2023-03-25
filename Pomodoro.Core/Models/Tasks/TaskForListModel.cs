using Pomodoro.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Core.Models.Tasks
{
    /// <summary>
    /// Represents a view model for task in task list.
    /// </summary>
    public class TaskForListModel
    {
        /// <summary>
        /// Gets or sets a value of the id of the task.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value of the title of the task.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets a value of the allocated time of the task.
        /// </summary>
        public short AllocatedTime { get; set; }

        /// <summary>
        /// Gets or sets an information about the frequency used in the task.
        /// </summary>
        public FrequencyValue? Frequency { get; set; }

        /// <summary>
        /// Gets or sets task progress.
        /// </summary>
        public byte Progress { get; set; }
    }
}
