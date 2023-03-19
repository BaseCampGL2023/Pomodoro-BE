// <copyright file="IPomoUnitRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Repositories.Base;

namespace Pomodoro.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Providing operations with AppTaskAttempt objects.
    /// </summary>
    public interface IPomoUnitRepository : IRepository<Entities.PomoUnit>
    {
    }
}
