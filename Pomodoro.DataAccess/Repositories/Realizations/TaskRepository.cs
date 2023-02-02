using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.DataAccess.Repositories.Realizations
{
    public class TaskRepository : BaseRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context)
        {
        }
    }
}
