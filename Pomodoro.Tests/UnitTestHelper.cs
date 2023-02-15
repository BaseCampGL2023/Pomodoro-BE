// <copyright file="UnitTestHelper.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Enums;

namespace Pomodoro.Tests
{
    /// <summary>
    /// Class that provide parts for tests arranging.
    /// </summary>
    public static class UnitTestHelper
    {
        /// <summary>
        /// Create DbContext options.
        /// </summary>
        /// <returns>DbContext.</returns>
        public static DbContextOptions<AppDbContext> GetUnitTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDbContext(options))
            {
                SeedData(context);
                context.SaveChanges();
            }

            return options;
        }

        private static void SeedData(AppDbContext context)
        {
            SeedFrequencyTypes(context);
            SeedFrequencies(context);
            SeedTasks(context);
            SeedCompletedTasks(context);
            SeedAppUsers(context);
            SeedSettings(context);
        }

        public static AppDbContext Context => SqlLiteInMemoryContext();

        private static AppDbContext SqlLiteInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            var context = new AppDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            return context;
        }

        private static void SeedFrequencyTypes(AppDbContext context)
        {
            context.FrequencyTypes.AddRange(
                new FrequencyType
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.None,
                },
                new FrequencyType
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Day,
                },
                new FrequencyType
                {
                    Id = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Week,
                },
                new FrequencyType
                {
                    Id = new Guid(4, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Month,
                },
                new FrequencyType
                {
                    Id = new Guid(5, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Year,
                },
                new FrequencyType
                {
                    Id = new Guid(6, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Workday,
                },
                new FrequencyType
                {
                    Id = new Guid(7, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Weekend,
                });
        }

        private static void SeedFrequencies(AppDbContext context)
        {
            context.Frequencies.AddRange(
                new Frequency
                {
                    Id = new Guid(2, 1, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 0,
                },
                new Frequency
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 1,
                },
                new Frequency
                {
                    Id = new Guid(2, 3, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 1,
                },
                new Frequency
                {
                    Id = new Guid(2, 4, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(4, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 1,
                },
                new Frequency
                {
                    Id = new Guid(2, 5, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(5, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 1,
                },
                new Frequency
                {
                    Id = new Guid(2, 6, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(6, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 1,
                },
                new Frequency
                {
                    Id = new Guid(2, 7, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(7, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 1,
                });
        }

        private static void SeedTasks(AppDbContext context)
        {
            context.Tasks.AddRange(
                new TaskEntity
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    UserId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Title = "Cleaning",
                    InitialDate = new DateTime(2023, 1, 10),
                    AllocatedTime = 3600,
                },
                new TaskEntity
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    UserId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Title = "Play guitar",
                    InitialDate = new DateTime(2023, 1, 11),
                    AllocatedTime = 2000,
                });
        }

        private static void SeedCompletedTasks(AppDbContext context)
        {
            context.CompletedTasks.AddRange(
                new Completed
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 10),
                    TimeSpent = 3000,
                    PomodorosCount = 2,
                    IsDone = true,
                },
                new Completed
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 11),
                    TimeSpent = 4500,
                    PomodorosCount = 3,
                    IsDone = true,
                },
                new Completed
                {
                    Id = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 12),
                    TimeSpent = 3000,
                    PomodorosCount = 2,
                    IsDone = true,
                },
                new Completed
                {
                    Id = new Guid(4, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 11),
                    TimeSpent = 3000,
                    PomodorosCount = 2,
                    IsDone = true,
                },
                new Completed
                {
                    Id = new Guid(5, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 13),
                    TimeSpent = 1500,
                    PomodorosCount = 1,
                    IsDone = true,
                });
        }

        private static void SeedAppUsers(AppDbContext context)
        {
            context.AppUsers.AddRange(
                new AppUser
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Name = "Viktor",
                    Email = "vitia@gmail.com",
                },
                new AppUser
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Name = "Mia",
                    Email = "mia@gmail.com",
                });
        }

        private static void SeedSettings(AppDbContext context)
        {
            context.Settings.AddRange(
                new Settings
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    UserId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    PomodoroDuration = 25,
                    ShortBreak = 5,
                    LongBreak = 15,
                    PomodorosBeforeLongBreak = 4,
                },
                new Settings
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    UserId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    PomodoroDuration = 20,
                    ShortBreak = 5,
                    LongBreak = 10,
                    PomodorosBeforeLongBreak = 3,
                });
        }
    }
}
