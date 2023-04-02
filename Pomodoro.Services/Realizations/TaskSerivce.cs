using AutoMapper;
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
        private readonly IPomodoroRepository _pomodoroRepo;
        private readonly IMapper _mapper;
        private readonly IFrequencyService _freqService;

        public TaskService(
            ITaskRepository tasksRepo,
            IPomodoroRepository pomodoroRepo,
            IFrequencyService freqService,
            IMapper mapper)
        {
            _tasksRepo = tasksRepo;
            _pomodoroRepo = pomodoroRepo;
            _mapper = mapper;
            _freqService = freqService;
        }

        public async Task<TaskModel?> CreateTaskAsync(TaskModel taskModel)
        {
            taskModel.Frequency ??= new FrequencyModel
            {
                IsCustom = false,
                Every = 0
            };

            var freqId = await _freqService.GetFrequencyIdAsync(taskModel.Frequency);

            if (freqId == Guid.Empty)
            {
                var newFreq = await _freqService.CreateFrequencyAsync(taskModel.Frequency);

                if (newFreq == null)
                {
                    return null;
                }

                freqId = newFreq.Id;
            }

            var task = _mapper.Map<TaskEntity>(taskModel);

            task.FrequencyId = freqId;

            await _tasksRepo.AddAsync(task);
            await _tasksRepo.SaveChangesAsync();

            return _mapper.Map<TaskModel>(task);
        }

        public async Task<TaskModel?> GetTaskByIdAsync(Guid taskId)
        {
            var task = await _tasksRepo.FindOneTaskAsync(taskId);

            if (task == null)
            {
                return null;
            }

            return _mapper.Map<TaskModel>(task);
        }

        public async Task<IEnumerable<TaskModel>> GetTasksByDateAsync(Guid userId, DateTime date)
        {
            var userTasks = await _tasksRepo.FindAllAsync(t => t.UserId == userId);

            var tasksOnDate = userTasks.Where(t => IsTaskOnDate(t, date)).ToList();

            foreach (var task in tasksOnDate)
            {
                if (task.Pomodoros != null)
                {
                    var pomodorosOnDate = task.Pomodoros.Where(p => p.ActualDate.Date == date.Date).ToList();
                    task.Pomodoros = pomodorosOnDate;
                }

            }

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
            if (taskModel == null)
            {
                throw new ArgumentNullException(nameof(taskModel), "Can`t be Null.");
            }

            var task = await _tasksRepo.GetByIdAsync(taskModel.Id);

            if (task == null)
            {
                throw new InvalidOperationException("Can`t find task in db.");
            }

            _tasksRepo.Remove(task);
            await _tasksRepo.SaveChangesAsync();
        }

        public async Task<TaskModel?> UpdateTaskAsync(TaskModel taskModel)
        {
            if (taskModel == null || taskModel.Frequency == null)
            {
                throw new ArgumentNullException(nameof(taskModel), "Can`t be Null also it`s Frequency can`t be Null.");
            }

            var freqId = await _freqService.GetFrequencyIdAsync(taskModel.Frequency);

            if (freqId == Guid.Empty)
            {
                var newFreq = await _freqService.CreateFrequencyAsync(taskModel.Frequency);

                if (newFreq == null)
                {
                    return null;
                }

                freqId = newFreq.Id;
            }

            var task = await _tasksRepo.GetByIdAsync(taskModel.Id);

            if (task == null)
            {
                return null;
            }

            task.Title = taskModel.Title;
            task.InitialDate = taskModel.InitialDate;
            task.AllocatedTime = taskModel.AllocatedTime;
            task.FrequencyId = freqId;

            _tasksRepo.Update(task);
            await _tasksRepo.SaveChangesAsync();

            return _mapper.Map<TaskModel>(task);
        }

        public async Task CompleteTaskAsync(Guid taskId)
        {
            var tasks = await _tasksRepo.FindAllAsync(t => t.Id == taskId);

            var task = tasks.FirstOrDefault();

            if (task == null)
            {
                throw new InvalidOperationException("Can`t find task in db.");
            }

            if (task.Pomodoros == null)
            {
                throw new InvalidOperationException("Can`t find any task pomodoros in db.");
            }

            var pomodoro = task.Pomodoros.LastOrDefault(p => p.ActualDate.Date == DateTime.Now.Date);

            if (pomodoro == null)
            {
                throw new InvalidOperationException("Can`t find task pomodoro in db.");
            }

            if (pomodoro.TaskIsDone)
            {
                return;
            }

            pomodoro.TaskIsDone = true;

            _pomodoroRepo.Update(pomodoro);
            await _tasksRepo.SaveChangesAsync();
        }

        public async Task<TaskModel?> AddPomodoroToTaskAsync(PomodoroModel pomodoroModel)
        {
            if (pomodoroModel == null)
            {
                throw new ArgumentNullException(nameof(pomodoroModel), "Can`t be Null.");
            }

            var task = await _tasksRepo.FindOneTaskAsync(pomodoroModel.TaskId);

            if (task == null)
            {
                return null;
            }

            if (task.Pomodoros != null && task.Pomodoros.Any())
            {
                var lastPomodoro = task.Pomodoros.LastOrDefault(p => p.ActualDate.Date == DateTime.Now.Date);

                if (lastPomodoro != null && lastPomodoro.TaskIsDone)
                {
                    return null;
                }
            }

            var pomodoro = _mapper.Map<PomodoroEntity>(pomodoroModel);

            await _pomodoroRepo.AddAsync(pomodoro);
            await _tasksRepo.SaveChangesAsync();

            var updatedTask = await _tasksRepo.FindOneTaskAsync(pomodoro.TaskId);

            if (updatedTask != null && updatedTask.Pomodoros != null)
            {
                updatedTask.Pomodoros = updatedTask.Pomodoros.Where(p => p.ActualDate.Date == pomodoro.ActualDate.Date).ToList();
            }

            return _mapper.Map<TaskModel>(updatedTask);
        }

        private bool IsTaskCompletedOnDate(TaskEntity task, DateTime date)
        {
            if (task.Pomodoros == null)
            {
                return false;
            }

            return task.Pomodoros.Any(c => c.ActualDate.Date == date.Date && c.TaskIsDone);
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
                    {
                        return initialDateOnly == dateOnly;
                    }

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