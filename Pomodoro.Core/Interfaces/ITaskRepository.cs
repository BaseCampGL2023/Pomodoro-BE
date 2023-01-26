using Pomodoro.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Core.Interfaces;

public interface ITaskRepository
{
    Task<TaskEntity> GetByIdAsync(int id);
    Task<IReadOnlyList<TaskEntity>> GetAllTasksByUserAsync(int userId);
    Task<TaskEntity> CreateAsync(TaskEntity item);
    Task<TaskEntity> DeleteAsync(TaskEntity item);
    Task<TaskEntity> UpdateAsync(TaskEntity item);
}
