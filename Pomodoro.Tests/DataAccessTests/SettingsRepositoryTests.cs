// <copyright file="SettingsRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var settingsRepository = new SettingsRepository(context);
            var identityUserId = context.Users.Add(new PomoIdentityUser()).Entity.Id;
            var userId = context.AppUsers.Add(new AppUser { PomoIdentityUserId = identityUserId }).Entity.Id;
            var settings = new Settings
            {
                UserId = userId,
                PomodoroDuration = 20,
                ShortBreak = 5,
                LongBreak = 10,
                PomodorosBeforeLongBreak = 3,
            };
            int expectedCount = UnitTestHelper.Settings.Count + 1;

            // act
            await settingsRepository.AddAsync(settings);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.Settings.Count());
        }

        /// <summary>
        /// Doesn`t add settings to database because user doesn`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_ThrowsDbUpdateException_UserDoesntExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var settingsRepository = new SettingsRepository(context);
            var settings = new Settings
            {
                PomodoroDuration = 20,
                ShortBreak = 5,
                LongBreak = 10,
                PomodorosBeforeLongBreak = 3,
            };

            // act
            var act = async () =>
            {
                await settingsRepository.AddAsync(settings);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Adds settings range to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsSettingsRangeToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var settingsRepository = new SettingsRepository(context);
            var identityUserId1 = context.Users.Add(new PomoIdentityUser()).Entity.Id;
            var identityUserId2 = context.Users.Add(new PomoIdentityUser()).Entity.Id;
            var userId1 = context.AppUsers.Add(new AppUser { PomoIdentityUserId = identityUserId1, Email = "1@i.a" }).Entity.Id;
            var userId2 = context.AppUsers.Add(new AppUser { PomoIdentityUserId = identityUserId2, Email = "2@i.a" }).Entity.Id;
            var settingsList = new List<Settings>
            {
                new Settings
                {
                    UserId = userId1,
                    PomodoroDuration = 25,
                    ShortBreak = 5,
                    LongBreak = 15,
                    PomodorosBeforeLongBreak = 4,
                },
                new Settings
                {
                    UserId = userId2,
                    PomodoroDuration = 30,
                    ShortBreak = 10,
                    LongBreak = 15,
                    PomodorosBeforeLongBreak = 2,
                },
            };
            int expectedCount = UnitTestHelper.Settings.Count + settingsList.Count;

            // act
            await settingsRepository.AddRangeAsync(settingsList);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.Settings.Count());
        }

        /// <summary>
        /// Doesn`t add settings to database because users don`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_ThrowsDbUpdateException_UsersDontExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var settingsRepository = new SettingsRepository(context);
            var settingsList = new List<Settings>
            {
                new Settings
                {
                    PomodoroDuration = 25,
                    ShortBreak = 5,
                    LongBreak = 15,
                    PomodorosBeforeLongBreak = 4,
                },
                new Settings
                {
                    PomodoroDuration = 30,
                    ShortBreak = 10,
                    LongBreak = 15,
                    PomodorosBeforeLongBreak = 2,
                },
            };

            // act
            var act = async () =>
            {
                await settingsRepository.AddRangeAsync(settingsList);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Finds settings.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task FindAsync_FindsSettings()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var settingsRepository = new SettingsRepository(context);
            var expSettingsList = UnitTestHelper.Settings.ToList();

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var settingsRepository = new SettingsRepository(context);
            var expSettingsList = UnitTestHelper.Settings.ToList();

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var settingsRepository = new SettingsRepository(context);
            var expSettingsList = UnitTestHelper.Settings[0];

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var settingsRepository = new SettingsRepository(context);
            var settings = UnitTestHelper.Settings[0];
            int expectedCount = UnitTestHelper.Settings.Count - 1;

            // act
            settingsRepository.Remove(settings);
            context.SaveChanges();

            // assert
            Assert.Equal(expectedCount, context.Settings.Count());
        }

        /// <summary>
        /// Removes settings range from db.
        /// </summary>
        [Fact]
        public void RemoveRange_RemovesSettingsRange()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var settingsRepository = new SettingsRepository(context);
            var settingsList = UnitTestHelper.Settings;

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var settingsRepository = new SettingsRepository(context);
            var expSettings = UnitTestHelper.Settings[0];
            expSettings.PomodoroDuration = 30;
            expSettings.ShortBreak = 10;
            expSettings.LongBreak = 10;
            expSettings.PomodorosBeforeLongBreak = 3;

            // act
            settingsRepository.Update(expSettings);
            context.SaveChanges();
            var actSettings = await settingsRepository.GetByIdAsync(expSettings.Id);

            // assert
            Assert.Equal(expSettings, actSettings, new SettingsComparer());
        }

        /// <summary>
        /// Doesn`t update user because user is taken.
        /// </summary>
        [Fact]
        public void Update_ThrowsDbUpdateException_UserIsTaken()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var settingsRepository = new SettingsRepository(context);
            var expSettings = UnitTestHelper.Settings[0];
            expSettings.UserId = UnitTestHelper.Settings[1].UserId;
            expSettings.PomodoroDuration = 30;
            expSettings.ShortBreak = 10;
            expSettings.LongBreak = 10;
            expSettings.PomodorosBeforeLongBreak = 3;

            // act
            var act = () =>
            {
                settingsRepository.Update(expSettings);
                context.SaveChanges();
            };

            // assert
            Assert.Throws<DbUpdateException>(act);
        }
    }
}
