using Pomodoro.Core.Models;

namespace Pomodoro.Core.Interfaces.IServices
{
    public interface ITaskService
    {
        public Task<TaskModel> GetTaskByIdAsync(Guid taskId);
        public Task<IEnumerable<TaskModel>> GetTasksByDateAsync(Guid userId, DateTime date);
        public Task<IEnumerable<TaskModel>> GetCompletedTasksByDateAsync(Guid userId, DateTime date);
        public Task<TaskModel> CreateTaskAsync(TaskModel taskModel);
        public Task DeleteTaskAsync(TaskModel taskModel);
        public Task<TaskModel> UpdateTaskAsync(TaskModel taskModel);
        public Task CompleteTaskAsync(Guid taskId);
        public Task<TaskModel> AddPomodoroToTaskAsync(PomodoroModel pomodoroModel);
    }
}
