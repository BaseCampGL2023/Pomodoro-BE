// <copyright file="BaseUserOrientedViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.ViewModels.Base
{
    /// <summary>
    /// Represents an abstract class to be inherited by view models which include data related to a certain user.
    /// </summary>
    public abstract class BaseUserOrientedViewModel
    {
        /// <summary>
        /// Gets or sets the id of the user for whom data is gathered.
        /// </summary>
        public Guid UserId { get; set; }
    }
}
