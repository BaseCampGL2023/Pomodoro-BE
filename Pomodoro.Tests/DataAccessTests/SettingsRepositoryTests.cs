// <copyright file="SettingsRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Realizations;
using Pomodoro.Tests.EqualityComparers;

namespace Pomodoro.Tests.DataAccessTests
{
    /// <summary>
    /// Settings repository test class.
    /// </summary>
    public class SettingsRepositoryTests
    {
        /// <summary>
        /// Adds settings to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_AddsSettingsToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var settingsRepository = new SettingsRepository(context);
            var settings = new Settings
            {
                Id = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                PomodoroDuration = 20,
                ShortBreak = 5,
                LongBreak = 10,
                PomodorosBeforeLongBreak = 3,
            };

            // act
            await settingsRepository.AddAsync(settings);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(3, context.Settings.Count());
        }

        /// <summary>
        /// Adds settings range to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsSettingsRangeToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var settingsRepository = new SettingsRepository(context);
            var settingsList = new List<Settings>
            {
                new Settings
                {
                    Id = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    PomodoroDuration = 25,
                    ShortBreak = 5,
                    LongBreak = 15,
                    PomodorosBeforeLongBreak = 4,
                },
                new Settings
                {
                    Id = new Guid(4, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    PomodoroDuration = 30,
                    ShortBreak = 10,
                    LongBreak = 15,
                    PomodorosBeforeLongBreak = 2,
                },
            };

            // act
            await settingsRepository.AddRangeAsync(settingsList);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(4, context.Settings.Count());
        }

        /// <summary>
        /// Finds settings.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task FindAsync_FindsSettings()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var settingsRepository = new SettingsRepository(context);
            var expSettingsList = context.Settings.ToList();

            // act
            var actSettingsList = await settingsRepository.FindAsync(x => x.ShortBreak == expSettingsList[0].ShortBreak);

            // assert
            Assert.Equal(expSettingsList, actSettingsList, new SettingsComparer());
        }

        /// <summary>
        /// Returns all settings.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_ReturnsAllSettings()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var settingsRepository = new SettingsRepository(context);
            var expSettingsList = context.Settings.ToList();

            // act
            var actSettingsList = await settingsRepository.GetAllAsync();

            // assert
            Assert.Equal(expSettingsList, actSettingsList, new SettingsComparer());
        }

        /// <summary>
        /// Gets settings by id.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_ReturnsSettings()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var settingsRepository = new SettingsRepository(context);
            var expSettingsList = context.Settings.First();

            // act
            var actSettingsList = await settingsRepository.GetByIdAsync(expSettingsList.Id);

            // assert
            Assert.Equal(expSettingsList, actSettingsList, new SettingsComparer());
        }

        /// <summary>
        /// Remove settings from db.
        /// </summary>
        [Fact]
        public void Remove_RemovesSettings()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var settingsRepository = new SettingsRepository(context);
            var settings = context.Settings.First();

            // act
            settingsRepository.Remove(settings);
            context.SaveChanges();

            // assert
            Assert.Equal(1, context.Settings.Count());
        }

        /// <summary>
        /// Removes settings range from db.
        /// </summary>
        [Fact]
        public void RemoveRange_RemovesSettingsRange()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var settingsRepository = new SettingsRepository(context);
            var settingsList = context.Settings.ToList();

            // act
            settingsRepository.RemoveRange(settingsList);
            context.SaveChanges();

            // assert
            Assert.Empty(context.Settings);
        }

        /// <summary>
        /// Updates settings.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task Update_UpdatesSettings()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var settingsRepository = new SettingsRepository(context);
            var expSettings = new Settings
            {
                Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                UserId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                PomodoroDuration = 30,
                ShortBreak = 10,
                LongBreak = 20,
                PomodorosBeforeLongBreak = 3,
            };

            // act
            settingsRepository.Update(expSettings);
            context.SaveChanges();
            var actSettings = await settingsRepository.GetByIdAsync(expSettings.Id);

            // assert
            Assert.Equal(expSettings, actSettings, new SettingsComparer());
        }
    }
}
