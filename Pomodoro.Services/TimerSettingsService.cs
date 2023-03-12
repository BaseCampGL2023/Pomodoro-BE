// <copyright file="TimerSettingsService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Repositories.Interfaces;
using Pomodoro.Services.Models;

namespace Pomodoro.Services
{
    /// <summary>
    /// Perform operations with timer settings.
    /// </summary>
    public class TimerSettingsService
    {
        private readonly ITimerSettingsRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerSettingsService"/> class.
        /// </summary>
        /// <param name="repository">Instance of ITimerSettingsRepository <see cref="ITimerSettingsRepository"/>.</param>
        public TimerSettingsService(ITimerSettingsRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Persist new user tracker settings.
        /// </summary>
        /// <param name="model">Tracker settings.</param>
        /// <param name="userId">User id.</param>
        /// <returns>TRUE if value persist succesfully, FALSE otherwise.</returns>
        public async Task<bool> AddSettingsAsync(TimerSettingsModel model, Guid userId)
        {
            var result = await this.repository.AddAsync(
                model.ToDalEntity(userId), true);
            if (result > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Retrieve all belonging to the user settings from database without adding them to ChangeTracker.
        /// </summary>
        /// <param name="userId">Owner id.</param>
        /// <returns>Settings collection.</returns>
        public async Task<ICollection<TimerSettingsModel>> GetBelongAllAsync(Guid userId)
        {
            var settings = await this.repository.GetBelongingAllAsNoTracking(userId);
            return settings.Select(e => TimerSettingsModel.Create(e)).ToList();
        }

        // TODO: add triger in DB for update or maybe no?
    }
}
