// <copyright file="ServicesExtension.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomodoro.DataAccess.EF;

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
    }
}
