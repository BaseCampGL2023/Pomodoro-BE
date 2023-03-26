using AutoMapper;
using Microsoft.Extensions.Logging;
using Pomodoro.Core.Enums;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models;
using Pomodoro.Core.Models.Base;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Services.Realizations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _tasksRepo;
        private readonly ICompletedRepository _pomodorosRepo;
        private readonly IMapper _mapper;
        private readonly IFrequencyService _freqService;
        private readonly ILogger _log;

        public TaskService(
            ITaskRepository tasksRepo,
            ICompletedRepository pomodorosRepo,
            IFrequencyService freqService,
            IMapper mapper,
            ILogger<TaskService> logger)
        {
            _tasksRepo = tasksRepo;
            _pomodorosRepo = pomodorosRepo;
            _mapper = mapper;
            _freqService = freqService;
            _log = logger;
        }

        public async Task<TaskModel> CreateTaskAsync(TaskModel taskModel)
        {
            taskModel.Frequency ??= new FrequencyModel
            {
                IsCustom = false,
                Every = 0
            };

            Guid freqId;

            try
            {
                freqId = await _freqService.GetFrequencyIdAsync(taskModel.Frequency);

                if (freqId == Guid.Empty)
                {
                    var newFreq = await _freqService.CreateFrequencyAsync(taskModel.Frequency);

                    freqId = newFreq.Id;
                }
            }
            catch (InvalidOperationException e)
            {
                _log.LogError(e.Message + " - occureed while getting frequency id.");
                throw;
            }
            catch (Exception e)
            {
                _log.LogError(e.Message + " - occureed while getting frequency id.");
                throw;
            }

            var task = _mapper.Map<TaskEntity>(taskModel);

            task.FrequencyId = freqId;

            try
            {
                await _tasksRepo.AddAsync(task);
                await _tasksRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogError(e.Message + " - occureed while adding task to db.");
                throw;
            }

            return _mapper.Map<TaskModel>(task);
        }

        public async Task<TaskModel> GetTaskByIdAsync(Guid taskId)
        {
            var task = await _tasksRepo.FindOneTaskAsync(taskId);

            if (task == null)
            {
                throw new InvalidOperationException("Can`t find task in db.");
            }

            return _mapper.Map<TaskModel>(task);
        }

        public async Task<IEnumerable<TaskModel>> GetTasksByDateAsync(Guid userId, DateTime date)
        {
            var userTasks = await _tasksRepo.FindAllAsync(t => t.UserId == userId);

            var tasksOnDate = userTasks.Where(t => IsTaskOnDate(t, date)).ToList();

            return _mapper.Map<IEnumerable<TaskModel>>(tasksOnDate);
        }

        public async Task<IEnumerable<TaskModel>> GetCompletedTasksByDateAsync(Guid userId, DateTime date)
        {
            var userTasks = await _tasksRepo.FindAllAsync(t => t.UserId == userId);

            var completedTasksOnDate = userTasks.Where(t => IsTaskCompletedOnDate(t, date)).ToList();

            return _mapper.Map<IEnumerable<TaskModel>>(completedTasksOnDate);
        }

        public async Task DeleteTaskAsync(TaskModel taskModel)
        {
            if (taskModel == null || taskModel.Frequency == null)
            {
                throw new ArgumentNullException(nameof(taskModel), "Can`t be Null also it`s Frequency can`t be Null.");
            }

            try
            {
                await _freqService.DeleteFrequencyAsync(taskModel.Frequency);
            }
            catch (InvalidOperationException e)
            {
                _log.LogError(e.Message + " - occureed while deleting frequency from db.");
                throw;
            }
            catch (Exception e)
            {
                _log.LogError(e.Message + " - occureed while deleting frequency from db.");
                throw;
            }

            var task = await _tasksRepo.GetByIdAsync(taskModel.Id);

            if (task == null)
            {
                throw new InvalidOperationException("Can`t find task in db.");
            }

            try
            {
                _tasksRepo.Remove(task);
                await _tasksRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogError(e.Message + " - occureed while deleting frequency from db.");
                throw;
            }
        }

        public async Task<TaskModel> UpdateTaskAsync(TaskModel taskModel)
        {
            if (taskModel == null || taskModel.Frequency == null)
            {
                throw new ArgumentNullException(nameof(taskModel), "Can`t be Null also it`s Frequency can`t be Null.");
            }

            Guid freqId;

            try
            {
                freqId = await _freqService.GetFrequencyIdAsync(taskModel.Frequency);

                if (freqId == Guid.Empty)
                {
                    var newFreq = await _freqService.CreateFrequencyAsync(taskModel.Frequency);

                    freqId = newFreq.Id;
                }
            }
            catch (InvalidOperationException e)
            {
                _log.LogError(e.Message + " - occureed while updating frequency in db.");
                throw;
            }
            catch (Exception e)
            {
                _log.LogError(e.Message + " - occureed while updating frequency in db.");
                throw;
            }

            var task = await _tasksRepo.GetByIdAsync(taskModel.Id);

            if (task == null)
            {
                throw new InvalidOperationException("Can`t find task in db.");
            }

            task.Title = taskModel.Title;
            task.InitialDate = taskModel.InitialDate;
            task.AllocatedTime = taskModel.AllocatedTime;
            task.FrequencyId = freqId;

            try
            {
                _tasksRepo.Update(task);
                await _tasksRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogError(e.Message + " - occureed while updating frequency in db.");
                throw;
            }

            return _mapper.Map<TaskModel>(task);
        }

        public async Task CompleteTaskAsync(Guid taskId, Guid pomId)
        {
            var tasks = await _tasksRepo.FindAllAsync(t => t.Id == taskId);

            var task = tasks.FirstOrDefault();

            if (task == null)
            {
                throw new InvalidOperationException("Can`t find task in db.");
            }

            if (task.CompletedTasks == null)
            {
                throw new InvalidOperationException("Can`t find any task pomodoros in db.");
            }

            var pomodoro = task.CompletedTasks.FirstOrDefault(p => p.Id == pomId);

            if (pomodoro == null)
            {
                throw new InvalidOperationException("Can`t find task pomodoro in db.");
            }

            pomodoro.IsDone = true;

            try
            {
                _pomodorosRepo.Update(pomodoro);
                await _tasksRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogError(e.Message + " - occureed while updating pomodoro in db.");
                throw;
            }

        }

        public async Task<CompletedModel> AddPomodoroToTaskAsync(CompletedModel pomodoroModel)
        {
            if (pomodoroModel == null)
            {
                throw new ArgumentNullException(nameof(pomodoroModel), "Can`t be Null.");
            }

            var taskExists = await _tasksRepo.HasByIdAsync(pomodoroModel.TaskId);

            if (!taskExists)
            {
                throw new InvalidOperationException("Can`t find task in db.");
            }

            var pomodoro = _mapper.Map<Completed>(pomodoroModel);

            try
            {
                await _pomodorosRepo.AddAsync(pomodoro);
                await _tasksRepo.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogError(e.Message + " - occureed while adding pomodoro in db.");
                throw;
            }

            return _mapper.Map<CompletedModel>(pomodoro);
        }

        private bool IsTaskCompletedOnDate(TaskEntity task, DateTime date)
        {
            if (task.CompletedTasks == null)
            {
                return false;
            }

           return task.CompletedTasks.Any(c => c.ActualDate.Date == date.Date && c.IsDone);
        }

        private bool IsTaskOnDate(TaskEntity task, DateTime date)
        {
            if (task.Frequency == null || task.Frequency.FrequencyType == null)
            {
                return false;
            }

            var dateOnly = date.Date;
            var initialDateOnly = task.InitialDate.Date;

            switch (task.Frequency.FrequencyType.Value)
            {
                case FrequencyValue.None:
                    return initialDateOnly == dateOnly;

                case FrequencyValue.Day:
                    {
                        var every = 1;

                        if (task.Frequency.IsCustom)
                            every = task.Frequency.Every;

                        for (var d = initialDateOnly; d <= dateOnly; d = d.AddDays(every))
                        {
                            if (d == dateOnly)
                                return true;
                        }
                        return false;
                    }

                case FrequencyValue.Week:
                    {
                        for (var d = initialDateOnly; d <= dateOnly; d = d.AddDays(7))
                        {
                            if (d == dateOnly)
                                return true;
                        }
                        return false;
                    }

                case FrequencyValue.Month:
                    {
                        var every = 1;

                        if (task.Frequency.IsCustom)
                            every = task.Frequency.Every;

                        for (var d = initialDateOnly; d <= dateOnly; d = d.AddMonths(every))
                        {
                            if (d == dateOnly)
                                return true;
                        }
                        return false;
                    }

                case FrequencyValue.Year:
                    {
                        var every = 1;

                        if (task.Frequency.IsCustom)
                            every = task.Frequency.Every;

                        for (var d = initialDateOnly; d <= dateOnly; d = d.AddYears(every))
                        {
                            if (d == dateOnly)
                                return true;
                        }
                        return false;
                    }

                case FrequencyValue.Workday:
                    return !IsWeekend(dateOnly) && initialDateOnly <= dateOnly;

                case FrequencyValue.Weekend:
                    return IsWeekend(dateOnly) && initialDateOnly <= dateOnly;

                default:
                    return false;
            }
        }

        private bool IsWeekend(DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;

            return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
        }
    }
}