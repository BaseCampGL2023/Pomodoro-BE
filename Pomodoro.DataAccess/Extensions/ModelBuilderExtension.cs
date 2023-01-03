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
        }

        private static void SeedFrequencyTypes(this ModelBuilder builder)
        {
            builder.Seed(
                new FrequencyType
                {
                    Id = 1,
                    Value = FrequencyValue.None
                },
                new FrequencyType
                {
                    Id = 2,
                    Value = FrequencyValue.Day
                },
                new FrequencyType
                {
                    Id = 3,
                    Value = FrequencyValue.Week
                },
                new FrequencyType
                {
                    Id = 4,
                    Value = FrequencyValue.Month
                },
                new FrequencyType
                {
                    Id = 5,
                    Value = FrequencyValue.Year
                },
                new FrequencyType
                {
                    Id = 6,
                    Value = FrequencyValue.Workday
                },
                new FrequencyType
                {
                    Id = 7,
                    Value = FrequencyValue.Weekend
                }
            );
        }

        private static void SeedFrequencies(this ModelBuilder builder)
        {
            builder.Seed(
                new Frequency
                {
                    Id = 1,
                    FrequencyTypeId = 1,
                    Every = 0
                },
                new Frequency
                {
                    Id = 2,
                    FrequencyTypeId = 2,
                    Every = 1
                },
                new Frequency
                {
                    Id = 3,
                    FrequencyTypeId = 3,
                    Every = 1
                },
                new Frequency
                {
                    Id = 4,
                    FrequencyTypeId = 4,
                    Every = 1
                },
                new Frequency
                {
                    Id = 5,
                    FrequencyTypeId = 5,
                    Every = 1
                },
                new Frequency
                {
                    Id = 6,
                    FrequencyTypeId = 6,
                    Every = 1
                },
                new Frequency
                {
                    Id = 7,
                    FrequencyTypeId = 7,
                    Every = 1
                }
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
