// <copyright file="TaskViewModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace Pomodoro.Api.ViewModels;

/// <summary>
/// Represents a view model for task entity.
/// </summary>
public class TaskViewModel : BaseUserOrientedViewModel
{
    /// <summary>
    /// Gets or sets the frequency the task starts with.
    /// </summary>
    public int FrequencyId { get; set; }

    /// <summary>
    /// Gets or sets the title of task.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the initial date of start for the task.
    /// </summary>
    public DateTime InitialDate { get; set; }

    /// <summary>
    /// Gets or sets the allocated time for the task.
    /// </summary>
    public short AllocatedTime { get; set; }
}
