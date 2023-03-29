// <copyright file="TaskRepeatable.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Services.Models.Enums
{
    /// <summary>
    /// Repesent task relation to schedule.
    /// 1 - (Any) both scheduled and standalone tasks,
    /// 2 - (Routine) scheduled task (generated accomplish to existing schedule),
    /// 3 - (alone) "standalone" task.
    /// </summary>
    public enum TaskRepeatable
    {
        /// <summary>
        /// Both repeatable (scheduled) and non-scheduled tasks.
        /// </summary>
        Any = 1,

        /// <summary>
        /// Scheduled task (generated accomplish to existing schedule)
        /// </summary>
        Routine = 2,

        /// <summary>
        /// "Standalone" task
        /// </summary>
        Alone = 3,
    }
}
