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
            await context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await context.Set<T>().AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await context.Set<T>().Where(predicate).ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync()
            => await context.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id)
            => await context.Set<T>().FindAsync(id);

        public async Task RemoveAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            context.Set<T>().RemoveRange(entities);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
