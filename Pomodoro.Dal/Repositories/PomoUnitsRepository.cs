// <copyright file="PomoUnitsRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Pomodoro.Dal.Data;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Repositories.Base;
using Pomodoro.Dal.Repositories.Interfaces;

namespace Pomodoro.Dal.Repositories
{
    /// <inheritdoc/>
    public class PomoUnitsRepository : BaseRepository<PomoUnit>, IPomoUnitRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PomoUnitsRepository"/> class.
        /// </summary>
        /// <param name="context">Instance of AppDbContext class <see cref="AppDbContext"/>.</param>
        public PomoUnitsRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}
