// <copyright file="BelongRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Pomodoro.Dal.Data;
using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Repositories.Base
{
    /// <inheritdoc/>
    public abstract class BelongRepository<T> : BaseRepository<T>, IBelongRepository<T>
        where T : class, IBelongEntity, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BelongRepository{T}"/> class.
        /// </summary>
        /// <param name="context">Instance of AppDbContext class <see cref="AppDbContext"/>.</param>
        protected BelongRepository(AppDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BelongRepository{T}"/> class.
        /// </summary>
        /// <param name="options">Instance of DbContextOptions to instantiate AppDbContext.</param>
        protected BelongRepository(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        public async Task<ICollection<T>> GetBelongingAll(Guid ownerId)
        {
            return await this.Table.Where(e => e.AppUserId == ownerId).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ICollection<T>> GetBelongingAllAsNoTracking(Guid ownerId)
        {
            return await this.Table.Where(e => e.AppUserId == ownerId).AsNoTracking().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<T?> GetBelongingByIdAsync(Guid id, Guid ownerId)
        {
            return await this.Table.FirstOrDefaultAsync(e => e.Id == id && e.AppUserId == ownerId);
        }
    }
}
