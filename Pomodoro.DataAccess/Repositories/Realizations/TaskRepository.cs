using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Pomodoro.DataAccess.Repositories.Realizations
{
    public class TaskRepository : BaseRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context)
        {
          
        }

        public async Task<TaskEntity?> FindOneTaskAsync(Guid id)
        {
            return await context.Set<TaskEntity>()
                .Where(t => t.Id == id)
                .Include(t => t.Frequency)
                    .ThenInclude(f => f.FrequencyType)
                .Include(t => t.CompletedTasks)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TaskEntity>> FindAllAsync(Expression<Func<TaskEntity, bool>> predicate)
        {
            return await context.Set<TaskEntity>()
                .Where(predicate)
                .Include(t => t.Frequency)
                    .ThenInclude(f => f.FrequencyType)
                .Include(t => t.CompletedTasks)
                .ToListAsync();
        }
    }
}
