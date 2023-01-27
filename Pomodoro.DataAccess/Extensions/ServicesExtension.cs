// <copyright file="ServicesExtension.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Repositories.Interfaces;
using Pomodoro.DataAccess.Repositories.Realizations;

namespace Pomodoro.DataAccess.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddAppDbContext(
            this IServiceCollection services,
            string connectionString)
        {
            return services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(connectionString);
            });
        }

        public static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<ICompletedRepository, CompletedRepository>();
            services.AddScoped<IFrequencyRepository, FrequencyRepository>();
            services.AddScoped<IFrequencyTypeRepository, FrequencyTypeRepository>();
            services.AddScoped<ISettingsRepository, SettingsRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
