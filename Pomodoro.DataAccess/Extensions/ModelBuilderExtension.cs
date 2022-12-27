// <copyright file="ModelBuilderExtension.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Enums;

namespace Pomodoro.DataAccess.Extensions
{
    internal static class ModelBuilderExtension
    {
        internal static void Seed(this ModelBuilder builder)
        {
            builder.SeedFrequencyTypes();
            builder.SeedFrequencies();
            builder.SeedUsers();
            builder.SeedSettings();
            builder.SeedTasks();
            builder.SeedCompletedTasks();
        }

        private static void SeedFrequencyTypes(this ModelBuilder builder)
        {
            builder.Seed(
                new FrequencyType(id: 1, value: FrequencyValue.None),
                new FrequencyType(id: 2, value: FrequencyValue.Day),
                new FrequencyType(id: 3, value: FrequencyValue.Week),
                new FrequencyType(id: 4, value: FrequencyValue.Month),
                new FrequencyType(id: 5, value: FrequencyValue.Year),
                new FrequencyType(id: 6, value: FrequencyValue.Workday),
                new FrequencyType(id: 7, value: FrequencyValue.Weekend)
            );
        }

        private static void SeedFrequencies(this ModelBuilder builder)
        {
            builder.Seed(
                new Frequency(id: 1, frequencyTypeId: 1, every: 0),
                new Frequency(id: 2, frequencyTypeId: 2, every: 1),
                new Frequency(id: 3, frequencyTypeId: 3, every: 1),
                new Frequency(id: 4, frequencyTypeId: 4, every: 1),
                new Frequency(id: 5, frequencyTypeId: 5, every: 1),
                new Frequency(id: 6, frequencyTypeId: 6, every: 1),
                new Frequency(id: 7, frequencyTypeId: 7, every: 1)
            );
        }

        private static void SeedUsers(this ModelBuilder builder)
        {
            builder.Seed(
                new User(id: 1, name: "Bob", email: "bob@gmail.com"),
                new User(id: 2, name: "Jane", email: "jane@gmail.com")
            );
        }

        private static void SeedSettings(this ModelBuilder builder)
        {
            builder.Seed(
                new Settings(
                    id: 1, userId: 1,
                    pomodoroDuration: 30, shortBreak: 5, longBreak: 10,
                    pomodorosBeforeLongBreak: 2, autostartEnabled: true
                ),
                new Settings(
                    id: 2, userId: 2,
                    pomodoroDuration: 60, shortBreak: 10, longBreak: 20,
                    pomodorosBeforeLongBreak: 3, autostartEnabled: false
                )
            );
        }

        private static void SeedTasks(this ModelBuilder builder)
        {
            builder.Seed(
                new Entities.Task(
                    id: 1, userId: 1, frequencyId: 1,
                    title: "Investigate Docker", initialDate: DateTime.UtcNow,
                    allocatedTime: 100
                ),
                new Entities.Task(
                    id: 2, userId: 1, frequencyId: 3,
                    title: "Cleaning", initialDate: DateTime.UtcNow,
                    allocatedTime: 200
                ),
                new Entities.Task(
                    id: 3, userId: 2, frequencyId: 1,
                    title: "Generate DB", initialDate: DateTime.UtcNow,
                    allocatedTime: 120
                ),
                new Entities.Task(
                    id: 4, userId: 2, frequencyId: 2,
                    title: "Workout", initialDate: DateTime.UtcNow,
                    allocatedTime: 60
                )
            );
        }

        private static void SeedCompletedTasks(this ModelBuilder builder)
        {
            builder.Seed(
                new Completed(
                    id: 1, taskId: 1, actualDate: DateTime.UtcNow,
                    timeSpent: 120, pomodorosCount: 4
                ),
                new Completed(
                    id: 2, taskId: 2, actualDate: DateTime.UtcNow,
                    timeSpent: 160, pomodorosCount: 5.3F
                ),
                new Completed(
                    id: 3, taskId: 3, actualDate: DateTime.UtcNow,
                    timeSpent: 100, pomodorosCount: 1.7F
                ),
                new Completed(
                    id: 4, taskId: 4, actualDate: DateTime.UtcNow,
                    timeSpent: 60, pomodorosCount: 1
                )
            );
        }

        private static void Seed<TEntity>(
            this ModelBuilder builder, params TEntity[] data)
            where TEntity : class
        {
            builder.Entity<TEntity>().HasData(data);
        }
    }
}
