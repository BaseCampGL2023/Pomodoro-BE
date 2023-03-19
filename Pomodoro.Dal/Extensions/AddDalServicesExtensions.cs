// <copyright file="AddDalServicesExtensions.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomodoro.Dal.Data;
using Pomodoro.Dal.Repositories;
using Pomodoro.Dal.Repositories.Interfaces;

namespace Pomodoro.Dal.Extensions
{
    /// <summary>
    /// Extensions for setting services (for DI container).
    /// </summary>
    public static class AddDalServicesExtensions
    {
        /// <summary>
        /// Add AppDbContext instance.
        /// </summary>
        /// <param name="services">Collection of service descriptors <see cref="IServiceCollection"/>.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>Collection of service descriptors.</returns>
        public static IServiceCollection AddAppDbContext(
            this IServiceCollection services,
            string connectionString)
        {
            return services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure();
                    });
            });
        }

        /// <summary>
        /// Add repositories.
        /// </summary>
        /// <param name="services">Collection of service descriptors <see cref="IServiceCollection"/>.</param>
        /// <returns>Collection of service descriptors.</returns>
        public static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<ITimerSettingRepository, TimerSettingRepository>();
            services.AddScoped<IAppTaskRepository, AppTaskRepository>();
            services.AddScoped<IPomoUnitRepository, PomoUnitsRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
