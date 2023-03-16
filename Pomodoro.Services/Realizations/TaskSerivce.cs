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
        private readonly ITaskRepository _tasksRepo;
        private readonly IMapper _mapper;
        private readonly IFrequencyService _freqService;

        public TaskService(
            ITaskRepository tasksRepo,
            IFrequencyService freqService,
            IMapper mapper)
        {
            _tasksRepo = tasksRepo;
            _mapper = mapper;
            _freqService = freqService;
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsyncTest()
        {
            var data = await _tasksRepo.GetAllTasks();
            var result = _mapper.Map<IEnumerable<TaskEntity>, IEnumerable<TaskModel>>(data);
            return result;
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync(Guid userId)
        {
            var data = await _tasksRepo.FindAllAsync(x => x.UserId == userId);
            var result = _mapper.Map<IEnumerable<TaskEntity>, IEnumerable<TaskModel>>(data);
            return result;
        }

        public async Task<TaskModel?> GetTaskByIdAsync(Guid taskId)
        {
            var data = await _tasksRepo.FindOneTaskAsync(taskId);
            var result = _mapper.Map<TaskEntity?, TaskModel?>(data);
            return result;
        }

        public async Task<TaskModel> PostTask(TaskModel task)
        {
            var freqData = _mapper.Map<TaskModel, FrequencyModel>(task);
            Guid freqId = await _freqService.GetFrequencyId(freqData);

            var data = _mapper.Map<TaskModel, TaskEntity>(task);
            data.FrequencyId = freqId;

            await _tasksRepo.AddAsync(data);
            await _tasksRepo.SaveChangesAsync();
            task.Id = data.Id;
            return task;
        }

        public async Task<TaskModel> UpdateTask(TaskModel task)
        {
            var freqData = _mapper.Map<TaskModel, FrequencyModel>(task);
            Guid freqId = await _freqService.GetFrequencyId(freqData);

            var data = _mapper.Map<TaskModel, TaskEntity>(task);
            data.FrequencyId = freqId;

            _tasksRepo.Update(data);
            await _tasksRepo.SaveChangesAsync();
            return task;
        }

        public async Task<TaskModel> DeleteTask(TaskModel task)
        {
            var freqData = _mapper.Map<TaskModel, FrequencyModel>(task);
            Guid freqId = await _freqService.GetFrequencyId(freqData);

            var data = _mapper.Map<TaskModel, TaskEntity>(task);
            data.FrequencyId = freqId;

            _tasksRepo.Remove(data);
            await _tasksRepo.SaveChangesAsync();
            return task;
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksByDate(Guid userId, DateTime startDate, DateTime endDate)
        {
            IEnumerable<TaskEntity> data = endDate == DateTime.MinValue ?
                await _tasksRepo.FindAllAsync(x => x.UserId == userId && x.InitialDate >= startDate) :
                await _tasksRepo.FindAllAsync(x => x.UserId == userId && x.InitialDate >= startDate && x.InitialDate <= endDate);
            var result = _mapper.Map<IEnumerable<TaskEntity>, IEnumerable<TaskModel>>(data);
            return result;
        }
    }
}