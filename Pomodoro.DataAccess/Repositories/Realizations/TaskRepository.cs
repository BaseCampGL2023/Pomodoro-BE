using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

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
                .Include(t => t.Frequency.FrequencyType)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TaskEntity>> FindAllAsync(Expression<Func<TaskEntity, bool>> predicate)
        {
            return await context.Set<TaskEntity>()
                .Where(predicate)
                .Include(t => t.Frequency)
                .Include(t => t.Frequency.FrequencyType)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetAllTasks()
        {
            return await context.Set<TaskEntity>()
                .Include(t => t.Frequency)
                .Include(t => t.Frequency.FrequencyType)
                .ToListAsync();
        }
    }
}
