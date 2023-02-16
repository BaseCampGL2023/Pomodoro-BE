using Microsoft.EntityFrameworkCore;
using Pomodoro.Core.Interfaces.IRepositories;
using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using System.Linq.Expressions;

namespace Pomodoro.DataAccess.Repositories.Realizations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext context;
        public BaseRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await context.Set<T>().Where(predicate).ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync()
            => await context.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id)
            => await context.Set<T>().FindAsync(id);

        public async Task<bool> HasByIdAsync(Guid id)
            => await context.Set<T>()
            .Where(entity => entity.Id == id)
            .AnyAsync();

        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            context.Set<T>().RemoveRange(entities);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
    }
}
