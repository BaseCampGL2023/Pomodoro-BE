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
        private readonly bool disposeContext;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class when used
        /// with DI container.
        /// </summary>
        /// <param name="context">Instance of AppDbContext class <see cref="AppDbContext"/>.</param>
        protected BaseRepository(AppDbContext context)
        {
            this.Context = context;
            this.Table = this.Context.Set<T>();
            this.disposeContext = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class without using DI container.
        /// </summary>
        /// <param name="options">Instance of DbContextOptions to instantiate AppDbContext.</param>
        protected BaseRepository(DbContextOptions<AppDbContext> options)
            : this(new AppDbContext(options))
        {
            this.disposeContext = true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BaseRepository{T}"/> class. It is used to match the Dispose pattern.
        /// </summary>
        ~BaseRepository()
        {
            this.Dispose(false);
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
        /// Implementing of IDisposable interface <see cref="IDisposable"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        // TODO: Handling exception when delete no existing entity.

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
            // TODO: Exception handling. Add Iloggger as dependency.
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

        /// <summary>
        /// Disposes repository.
        /// </summary>
        /// <param name="disposing">Give TRUE if it used out of DI container scope.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.disposeContext)
                {
                    this.Context.Dispose();
                }

                this.isDisposed = true;
            }
        }
    }
}
