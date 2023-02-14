// <copyright file="BaseViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.ViewModels.Base
{
    /// <summary>
    /// Represents an abstract class to be inherited by view models which hold an id for DB.
    /// </summary>
    public abstract class BaseViewModel
    {
        /// <summary>
        /// Gets or sets the id of the view model.
        /// </summary>
        public Guid Id { get; set; }
    }
}
