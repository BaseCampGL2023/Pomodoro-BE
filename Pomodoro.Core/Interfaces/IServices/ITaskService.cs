using Pomodoro.Core.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Core.Interfaces.IServices
{
    public interface ITaskService
    {
        public Task<TaskModel?> GetTaskByIdAsync(Guid taskId);
        public Task<IEnumerable<TaskModel?>> GetTasksByDateAsync(Guid userId, DateTime date);
        public Task<TaskModel?> CreateTaskAsync(Guid userId, TaskModel taskModel);
        public Task DeleteTaskAsync(TaskModel taskModel);
        public Task<TaskModel?> UpdateTaskAsync(TaskModel taskModel);
    }
}
