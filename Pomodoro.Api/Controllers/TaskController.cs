// <copyright file="TaskController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Controllers.Base;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services;
using Pomodoro.Services.Models;

namespace Pomodoro.Api.Controllers
{
    /// <summary>
    /// Manage tasks.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : BaseCrudController<TaskService, AppTask, TaskModel, IAppTaskRepository>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskController"/> class.
        /// </summary>
        /// <param name="service">Instance of Task service.</param>
        public TaskController(TaskService service)
            : base(service)
        {
        }
    }
}
