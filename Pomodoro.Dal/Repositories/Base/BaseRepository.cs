// <copyright file="BaseRepository.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Pomodoro.Dal.Data;
using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Dal.Repositories.Base
{
    /// <inheritdoc/>
    public abstract class BaseRepository<T> : IRepository<T>
        where T : class, IEntity, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class when used
        /// with DI container.
        /// </summary>
        /// <param name="context">Instance of AppDbContext class <see cref="AppDbContext"/>.</param>
        protected BaseRepository(AppDbContext context)
        {
            this.Context = context;
            this.Table = this.Context.Set<T>();
        }

        /// <summary>
        /// Gets instance of AppDbContext class <see cref="AppDbContext"/>.
        /// </summary>
        public AppDbContext Context { get; }

        /// <summary>
        /// Gets a DbSet property to perform specific query in client code.
        /// </summary>
        public DbSet<T> Table { get; }

        /// <summary>
        /// Gets IQueryable to perform request out of repository.
        /// </summary>
        public IQueryable<T> All => this.Table;

        /// <inheritdoc/>
        public async Task<int> AddAsync(T entity, bool persist = false)
        {
            await this.Table.AddAsync(entity);
            return persist ? await this.SaveChangesAsync() : 0;
        }

        /// <inheritdoc/>
        public async Task<int> AddRangeAsync(IEnumerable<T> entities, bool persist = false)
        {
            await this.Table.AddRangeAsync(entities);
            return persist ? await this.SaveChangesAsync() : 0;
        }

        /// <inheritdoc/>
        public async Task<int> DeleteAsync(T entity, bool persist = false)
        {
            this.Table.Remove(entity);
            return persist ? await this.SaveChangesAsync() : 0;
        }

        /// <inheritdoc/>
        public async Task<int> DeleteAsync(Guid id, bool persist = false)
        {
            var entity = new T { Id = id };
            this.Context.Entry(entity).State = EntityState.Deleted;
            return persist ? await this.SaveChangesAsync() : 0;
        }

        /// <inheritdoc/>
        public async Task<int> DeleteRangeAsync(IEnumerable<T> entities, bool persist = false)
        {
            this.Table.RemoveRange(entities);
            return persist ? await this.SaveChangesAsync() : 0;
        }

        /// <inheritdoc/>
        public async Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await this.Table.Where(predicate).ToListAsync();

        /// <inheritdoc/>
        public async Task<ICollection<T>> GetAllAsync()
            => await this.Table.ToListAsync();

        /// <inheritdoc/>
        public async Task<ICollection<T>> GetAllAsNoTracking()
            => await this.Table.AsNoTracking().ToListAsync();

        /// <inheritdoc/>
        public async Task<T?> GetByIdAsync(Guid id)
            => await this.Table.FindAsync(id);

        /// <inheritdoc/>
        public async Task<bool> IsExist(Guid id)
            => await this.Table.AnyAsync(e => e.Id == id);

        /// <inheritdoc/>
        public async Task<int> SaveChangesAsync()
        {
            return await this.Context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> UpdateAsync(T entity, bool persist = false)
        {
            this.Table.Update(entity);
            return persist ? await this.SaveChangesAsync() : 0;
        }

        /// <inheritdoc/>
        public async Task<int> UpdateRangeAsync(IEnumerable<T> entities, bool persist = false)
        {
            this.Table.UpdateRange(entities);
            return persist ? await this.SaveChangesAsync() : 0;
        }

        /// <inheritdoc/>
        public async Task<T?> GetByIdNoTrackingAsync(Guid id)
            => await this.Table.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

        /// <inheritdoc/>
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
            => await this.Table.Where(predicate).CountAsync();
    }
}
