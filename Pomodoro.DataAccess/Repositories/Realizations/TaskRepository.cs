using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace Pomodoro.DataAccess.Repositories.Realizations
{
    public class TaskRepository : BaseRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context)
        {
          
        }

        public async Task<IEnumerable<TaskEntity>> GetAllTasks()
        {
            return await context.Set<TaskEntity>()
                .Include(t => t.Frequency)
                .Include(t => t.Frequency.FrequencyType)
                .ToListAsync();
        }

        public async Task<Guid> AddTask(TaskEntity task)
        {
            await context.Set<TaskEntity>().AddAsync(task);
            await SaveChangesAsync();
            return task.Id;
        }
    }
}
