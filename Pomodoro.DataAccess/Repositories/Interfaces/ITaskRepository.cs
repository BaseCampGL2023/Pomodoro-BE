using Pomodoro.Core.Interfaces.IRepositories;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.DataAccess.Repositories.Interfaces
{
    public interface ITaskRepository : IBaseRepository<TaskEntity>
    {
        public Task<IEnumerable<TaskEntity>> GetAllTasks();

        public Task<Guid> AddTask(TaskEntity task);
    }
}
