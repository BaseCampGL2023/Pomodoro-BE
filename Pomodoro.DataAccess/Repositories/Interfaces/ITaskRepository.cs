using Pomodoro.Core.Interfaces.IRepositories;
using Pomodoro.DataAccess.Entities;
using System.Linq.Expressions;

namespace Pomodoro.DataAccess.Repositories.Interfaces
{
    public interface ITaskRepository : IBaseRepository<TaskEntity>
    {
        public Task<IEnumerable<TaskEntity>> FindAllAsync(Expression<Func<TaskEntity, bool>> predicate);
        public Task<TaskEntity?> FindOneTaskAsync(Guid id);
    }
}
