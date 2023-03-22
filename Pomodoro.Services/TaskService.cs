// <copyright file="TaskService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Base;
using Pomodoro.Services.Models;

namespace Pomodoro.Services
{
    /// <summary>
    /// Perform operations with tasks.
    /// </summary>
    public class TaskService : BaseService<AppTask, TaskModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="repo">Implementation of IAppTaskRepository <see cref="IAppTaskRepository"/>.</param>
        public TaskService(IAppTaskRepository repo)
            : base(repo)
        {
        }
    }
}
