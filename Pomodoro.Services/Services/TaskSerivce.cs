using AutoMapper;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Frequency;
using Pomodoro.Core.Models.Tasks;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Enums;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Services.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository tasksRepo;
        private readonly IMapper mapper;
        private readonly IFrequencyService freqService;

        public TaskService(
            ITaskRepository tasksRepo,
            IFrequencyService freqService,
            IMapper mapper)
        {
            this.tasksRepo = tasksRepo;
            this.mapper = mapper;
            this.freqService = freqService;
        }

        public TaskEntity CustomMapper(TaskModel model)
        {
            TaskEntity te = new TaskEntity();
            te.Title = model.Title;
            te.InitialDate = model.InitialDate;
            te.AllocatedTime = model.AllocatedTime;
            te.Id = model.TaskId;
            te.UserId = model.UserId;

            return te;
        } 

        public async Task<TaskModel> DeleteTask(TaskModel task)
        {
            Guid freqId = await this.GetFrequencyId(task);

            var data = CustomMapper(task);
            data.FrequencyId = freqId;

            this.tasksRepo.Remove(data);
            await this.tasksRepo.SaveChangesAsync();
            return task;
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsyncTest()
        {
            var data = await this.tasksRepo.GetAllTasks();
            var result = this.mapper.Map<IEnumerable<TaskEntity>, IEnumerable<TaskModel>>(data);
            return result;
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync(Guid userId)
        {
            var data = await this.tasksRepo.FindAsync(x => x.UserId == userId);
            var result = this.mapper.Map<IEnumerable<TaskEntity>, IEnumerable<TaskModel>>(data);
            return result;
        }

        public async Task<TaskModel?> GetTaskByIdAsync(Guid taskId)
        {
            var data = await this.tasksRepo.GetByIdAsync(taskId);
            var result = this.mapper.Map<TaskEntity?, TaskModel?>(data);
            return result;
        }

        public async Task<Guid> GetFrequencyId(TaskModel task)
        {
            var freqData = this.mapper.Map<TaskModel, FrequencyModel>(task);

            Guid freqId = await this.freqService.FindFrequencyId(freqData);

            if (freqId == Guid.Empty)
            {
                freqId = await this.freqService.AddFrequencyAsync(freqData);
            }

            return freqId;
        }

        public async Task<TaskModel> PostTask(TaskModel task)
        {
        
            Guid freqId = await this.GetFrequencyId(task);

            var data = CustomMapper(task);
            data.FrequencyId = freqId;

            Guid taskId = await this.tasksRepo.AddTask(data);
            task.TaskId = taskId;
            return task;
        }

        public async Task<TaskModel> UpdateTask(TaskModel task)
        {
            Guid freqId = await this.GetFrequencyId(task);

            var data = CustomMapper(task);
            data.FrequencyId = freqId;

            this.tasksRepo.Update(data);
            await this.tasksRepo.SaveChangesAsync();
            return task;
        }
    }
}