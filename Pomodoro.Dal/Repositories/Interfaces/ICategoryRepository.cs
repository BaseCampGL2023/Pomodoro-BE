// <copyright file="ICategoryRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Base;

namespace Pomodoro.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Perform operations with categories.
    /// </summary>
    public interface ICategoryRepository : IBelongRepository<Category>, IWithRelated<Category>
    {
    }
}
