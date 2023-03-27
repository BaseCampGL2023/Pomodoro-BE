using AutoMapper;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Frequency;
using Pomodoro.Core.Models.Tasks;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Services.Realizations
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

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsyncTest()
        {
            var data = await tasksRepo.GetAllTasks();
            var result = mapper.Map<IEnumerable<TaskEntity>, IEnumerable<TaskModel>>(data);
            return result;
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync(Guid userId)
        {
            var data = await tasksRepo.FindAllAsync(x => x.UserId == userId);
            var result = mapper.Map<IEnumerable<TaskEntity>, IEnumerable<TaskModel>>(data);
            return result;
        }

        public async Task<TaskModel?> GetTaskByIdAsync(Guid taskId)
        {
            var data = await tasksRepo.FindOneTaskAsync(taskId);
            var result = mapper.Map<TaskEntity?, TaskModel?>(data);
            return result;
        }

        public async Task<TaskModel> PostTask(TaskModel task)
        {
            var freqData = mapper.Map<TaskModel, FrequencyModel>(task);
            Guid freqId = await freqService.GetFrequencyId(freqData);

            var data = mapper.Map<TaskModel, TaskEntity>(task);
            data.FrequencyId = freqId;

            await tasksRepo.AddAsync(data);
            await tasksRepo.SaveChangesAsync();
            task.TaskId = data.Id;
            return task;
        }

        public async Task<TaskModel> UpdateTask(TaskModel task)
        {
            var freqData = mapper.Map<TaskModel, FrequencyModel>(task);
            Guid freqId = await freqService.GetFrequencyId(freqData);

            var data = mapper.Map<TaskModel, TaskEntity>(task);
            data.FrequencyId = freqId;

            tasksRepo.Update(data);
            await tasksRepo.SaveChangesAsync();
            return task;
        }

        public async Task<TaskModel> DeleteTask(TaskModel task)
        {
            var freqData = mapper.Map<TaskModel, FrequencyModel>(task);
            Guid freqId = await freqService.GetFrequencyId(freqData);

            var data = mapper.Map<TaskModel, TaskEntity>(task);
            data.FrequencyId = freqId;

            tasksRepo.Remove(data);
            await tasksRepo.SaveChangesAsync();
            return task;
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksByDate(Guid userId, DateTime startDate, DateTime endDate)
        {
            IEnumerable<TaskEntity> data = endDate == DateTime.MinValue ?
                await tasksRepo.FindAllAsync(x => x.UserId == userId && x.InitialDate >= startDate) :
                await tasksRepo.FindAllAsync(x => x.UserId == userId && x.InitialDate >= startDate && x.InitialDate <= endDate);
            var result = mapper.Map<IEnumerable<TaskEntity>, IEnumerable<TaskModel>>(data);
            return result;
        }
    }
}