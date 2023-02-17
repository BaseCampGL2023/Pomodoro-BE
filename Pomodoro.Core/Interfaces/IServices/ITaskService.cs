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
        public Task<IEnumerable<TaskModel>> GetAllTasksAsync(Guid userId);
        public Task<TaskModel?> GetTaskByIdAsync(Guid taskId);
        public Task<TaskModel> PostTask(TaskModel task);
        public Task<TaskModel> DeleteTask(TaskModel task);
        public Task<TaskModel> UpdateTask(TaskModel task);
        public Task<IEnumerable<TaskModel>> GetAllTasksAsyncTest();
    }
}
