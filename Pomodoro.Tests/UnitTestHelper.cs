// <copyright file="UnitTestHelper.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Enums;

namespace Pomodoro.Tests
{
    /// <summary>
    /// Class that provides data for testing.
    /// </summary>
    public static class UnitTestHelper
    {
        /// <summary>
        /// Gets frequencyTypes for testing.
        /// </summary>
        public static List<FrequencyType> FrequencyTypes => GetFrequencyTypes();

        /// <summary>
        /// Gets frequencies for testing.
        /// </summary>
        public static List<Frequency> Frequencies => GetFrequencies();

        /// <summary>
        /// Gets tasks for testing.
        /// </summary>
        public static List<TaskEntity> Tasks => GetTasks();

        /// <summary>
        /// Gets completedTasks for testing.
        /// </summary>
        public static List<PomodoroEntity> Pomodoros => GetPomodoros();

        /// <summary>
        /// Gets appUsers for testing.
        /// </summary>
        public static List<AppUser> AppUsers => GetAppUsers();

        /// <summary>
        /// Gets identityUsers for testing.
        /// </summary>
        public static List<PomoIdentityUser> IdentityUsers => GetIdentityUsers();

        /// <summary>
        /// Gets settings for testing.
        /// </summary>
        public static List<Settings> Settings => GetSettings();

        /// <summary>
        /// Gets dbContext options for testing.
        /// </summary>
        public static DbContextOptions<AppDbContext> DbOptions => GetUnitTestDbOptions();

        private static DbContextOptions<AppDbContext> GetUnitTestDbOptions()
        {
            var connection = new SqliteConnection("DataSource=:memory:");

            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();

                SeedData(context);

                context.SaveChanges();
            }

            return options;
        }

        private static void SeedData(AppDbContext context)
        {
            context.Users.AddRange(IdentityUsers);
            context.AppUsers.AddRange(AppUsers);
            context.Settings.AddRange(Settings);
            context.Tasks.AddRange(Tasks);
            context.Pomodoros.AddRange(Pomodoros);
        }

        private static List<FrequencyType> GetFrequencyTypes()
        {
            return new List<FrequencyType>
            {
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
                },
            };
        }

        private static List<Frequency> GetFrequencies()
        {
            return new List<Frequency>
            {
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
                },
            };
        }

        private static List<TaskEntity> GetTasks()
        {
            return new List<TaskEntity>
            {
                new TaskEntity
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    UserId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyId = new Guid(2, 1, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
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
                },
            };
        }

        private static List<PomodoroEntity> GetPomodoros()
        {
            return new List<PomodoroEntity>
            {
                new PomodoroEntity
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 10),
                    TimeSpent = 3000,
                    TaskIsDone = true,
                },
                new PomodoroEntity
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 11),
                    TimeSpent = 4500,
                    TaskIsDone = true,
                },
                new PomodoroEntity
                {
                    Id = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 12),
                    TimeSpent = 3000,
                    TaskIsDone = true,
                },
                new PomodoroEntity
                {
                    Id = new Guid(4, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 11),
                    TimeSpent = 3000,
                    TaskIsDone = true,
                },
                new PomodoroEntity
                {
                    Id = new Guid(5, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 13),
                    TimeSpent = 1500,
                    TaskIsDone = true,
                },
            };
        }

        private static List<PomoIdentityUser> GetIdentityUsers()
        {
            return new List<PomoIdentityUser>
            {
                new PomoIdentityUser
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                },
                new PomoIdentityUser
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                },
            };
        }

        private static List<AppUser> GetAppUsers()
        {
            return new List<AppUser>
            {
                new AppUser
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    PomoIdentityUserId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Name = "Viktor",
                    Email = "vitia@gmail.com",
                },
                new AppUser
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    PomoIdentityUserId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Name = "Mia",
                    Email = "mia@gmail.com",
                },
            };
        }

        private static List<Settings> GetSettings()
        {
            return new List<Settings>
            {
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
                },
            };
        }
    }
}
