using AutoMapper;
using Microsoft.Extensions.Logging;
using Pomodoro.Core.Enums;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Services.Realizations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _tasksRepo;
        private readonly IMapper _mapper;
        private readonly IFrequencyService _freqService;
        private readonly ILogger _log;

        public TaskService(
            ITaskRepository tasksRepo,
            IFrequencyService freqService,
            IMapper mapper,
            ILogger logger)
        {
            _tasksRepo = tasksRepo;
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

            var tasksOnDate = userTasks.Where(t => IsTaskOnDate(t, date.Date)).ToList();

            return _mapper.Map<IEnumerable<TaskModel>>(tasksOnDate);
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

            FrequencyModel freq;

            try
            {
                freq = await _freqService.UpdateFrequencyAsync(taskModel.Frequency);
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
                return null;
            }

            task.Title = taskModel.Title;
            task.InitialDate = taskModel.InitialDate;
            task.AllocatedTime = taskModel.AllocatedTime;

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

        private bool IsTaskOnDate(TaskEntity task, DateTime date)
        {
            if (task.Frequency == null || task.Frequency.FrequencyType == null)
            {
                return false;
            }

            switch (task.Frequency.FrequencyType.Value)
            {
                case FrequencyValue.None:
                    return task.InitialDate.Date == date;

                case FrequencyValue.Day:
                    {
                        var every = 1;

                        if (task.Frequency.IsCustom)
                            every = task.Frequency.Every;

                        for (var d = task.InitialDate.Date; d <= date; d = d.AddDays(every))
                        {
                            if (d == date)
                                return true;
                        }
                        return false;
                    }

                case FrequencyValue.Week:
                    {
                        for (var d = task.InitialDate.Date; d <= date; d = d.AddDays(7))
                        {
                            if (d == date)
                                return true;
                        }
                        return false;
                    }

                case FrequencyValue.Month:
                    {
                        var every = 1;

                        if (task.Frequency.IsCustom)
                            every = task.Frequency.Every;

                        for (var d = task.InitialDate.Date; d <= date; d = d.AddMonths(every))
                        {
                            if (d == date)
                                return true;
                        }
                        return false;
                    }

                case FrequencyValue.Year:
                    {
                        var every = 1;

                        if (task.Frequency.IsCustom)
                            every = task.Frequency.Every;

                        for (var d = task.InitialDate.Date; d <= date; d = d.AddYears(every))
                        {
                            if (d == date)
                                return true;
                        }
                        return false;
                    }

                case FrequencyValue.Workday:
                    return !IsWeekend(date) && task.InitialDate.Date <= date;

                case FrequencyValue.Weekend:
                    return IsWeekend(date) && task.InitialDate.Date <= date;

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