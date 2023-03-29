// <copyright file="TaskExecutionState.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Services.Models.Enums
{
    /// <summary>
    /// Represent task execution state.
    /// 1 - (all) any task,
    /// 2 - (pending) not finished task,
    /// 3 - (started) task alredy in progress (has related pomodoros),
    /// 4 - (pristine) task don't started,
    /// 5 - (finished) task already completed (has finish datetime).
    /// </summary>
    public enum TaskExecutionState
    {
        /// <summary>
        /// Any task.
        /// </summary>
        All = 1,

        /// <summary>
        /// Not finished task.
        /// </summary>
        Pending = 2,

        /// <summary>
        /// Task, that already in progress (has related Pomodoros)
        /// </summary>
        Started = 3,

        /// <summary>
        /// Task don't started.
        /// </summary>
        Pristine = 4,

        /// <summary>
        /// Task already performed (has finish datetime)
        /// </summary>
        Finished = 5,
    }
}
