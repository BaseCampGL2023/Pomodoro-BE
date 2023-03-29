// <copyright file="TaskQueryModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using Pomodoro.Services.Models.Enums;

namespace Pomodoro.Services.Models.Query
{
    /// <summary>
    /// Describe available query parameters to retrive tasks.
    /// </summary>
    public class TaskQueryModel
    {
        /// <summary>
        /// Gets or sets value of task execution state.
        /// Valid values: "1" - means any task (started or pristine or finished),
        /// "2" - means not finished tasks (both started and pristine),
        /// "3" - means tasks that already in progress (have pomodoros),
        /// "4" - means tasks that don't be started,
        /// "5" - means task that already performed.
        /// </summary>
        [EnumDataType(typeof(TaskExecutionState), ErrorMessage = "Non exisiting {0}")]
        public TaskExecutionState ExecutionState { get; set; } = TaskExecutionState.All;

        /// <summary>
        /// Gets or sets task relation to schedule.
        /// Valid values: 1 - (Any) both scheduled and standalone tasks,
        /// 2 - (Routine) scheduled task (generated accomplish to existing schedule),
        /// 3 - (alone) "standalone" task.
        /// </summary>
        [EnumDataType(typeof(TaskRepeatable), ErrorMessage = "Non exisiting {0}")]
        public TaskRepeatable Repeatable { get; set; } = TaskRepeatable.Any;

        /// <summary>
        /// Gets or sets date from wich task should be queried.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets date to wich task should be queried.
        /// If null task will be quired only for start date.
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
