// <copyright file="ModelBuilderExtension.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.Entities;
using Pomodoro.Core.Enums;

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
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.None
                },
                new FrequencyType
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Day
                },
                new FrequencyType
                {
                    Id = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Week
                },
                new FrequencyType
                {
                    Id = new Guid(4, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Month
                },
                new FrequencyType
                {
                    Id = new Guid(5, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Year
                },
                new FrequencyType
                {
                    Id = new Guid(6, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Workday
                },
                new FrequencyType
                {
                    Id = new Guid(7, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Weekend
                }
            );
        }

        private static void SeedFrequencies(this ModelBuilder builder)
        {
            builder.Seed(
                new Frequency
                {
                    Id = new Guid(2, 1, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 0
                },
                new Frequency
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 1
                },
                new Frequency
                {
                    Id = new Guid(2, 3, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 1
                },
                new Frequency
                {
                    Id = new Guid(2, 4, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(4, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 1
                },
                new Frequency
                {
                    Id = new Guid(2, 5, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(5, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 1
                },
                new Frequency
                {
                    Id = new Guid(2, 6, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(6, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 1
                },
                new Frequency
                {
                    Id = new Guid(2, 7, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(7, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
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
