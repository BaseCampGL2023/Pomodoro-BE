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
            if (!model.IsActive)
            {
                model.IsActive = true;
            }

            var entity = model.ToDalEntity(userId);
            var result = await this.repository.AddAsync(entity, true);
            if (result > 0)
            {
                model.Id = entity.Id;
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

        /// <summary>
        /// Retrieve current user settings, or NULL if not exists.
        /// </summary>
        /// <param name="userId">Owner Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<TimerSettingsModel?> GetBelongActiveAsync(Guid userId)
        {
            var result = await this.repository.GetCurrentTimerSettingsAsync(userId);
            return result is null ? null : TimerSettingsModel.Create(result);
        }

        /// <summary>
        /// Return settings object by id, with owning check.
        /// </summary>
        /// <param name="id">Settings id.</param>
        /// <param name="userId">Owner Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<TimerSettingsModel?> GetBelongAsync(Guid id, Guid userId)
        {
            var result = await this.repository.GetByIdAsync(id);
            if (result is null)
            {
                // TODO: result pattern
                return null;
            }

            if (result.AppUserId != userId)
            {
                // TODO: forbid
                return null;
            }

            return TimerSettingsModel.Create(result);
        }

        /// <summary>
        /// Delete timer settings belonging to user from database.
        /// </summary>
        /// <param name="id">Settings id.</param>
        /// <param name="userId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<bool> DeleteOneOwnAsync(Guid id, Guid userId)
        {
            int result = await this.repository.DeleteOneBelongingAsync(id, userId, true);

            return result > 0;
        }

        /// <summary>
        /// Update existing user tracker settings.
        /// </summary>
        /// <param name="model">Tracker settings.</param>
        /// <param name="userId">User id.</param>
        /// <returns>TRUE if value persist succesfully, FALSE otherwise.</returns>
        public async Task<bool> UpdateOneOwnAsync(TimerSettingsModel model, Guid userId)
        {
            var entity = model.ToDalEntity(userId);
            int result = await this.repository.UpdateAsync(entity, true);
            if (result > 0)
            {
                model.Id = entity.Id;
                return true;
            }

            return false;
        }
    }
}
