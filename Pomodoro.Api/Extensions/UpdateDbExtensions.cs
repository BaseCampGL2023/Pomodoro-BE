// <copyright file="UpdateDbExtensions.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.EF;

namespace Pomodoro.Api.Extensions
{
    /// <summary>
    /// Apply migrations, if any available.
    /// </summary>
    public static class UpdateDbExtensions
    {
        /// <summary>
        /// Adds to request pipline check for pending migrations.
        /// </summary>
        /// <param name="app">Request pipline buider <see cref="IApplicationBuilder"/>.</param>
        /// <param name="isDrop">If TRUE, database will be deleted and recreated, by default FALSE,
        /// what means applying migration without recreating.</param>
        /// <returns>Request pipline buider.</returns>
        public static IApplicationBuilder UseUpdateDb(this IApplicationBuilder app, bool isDrop = false)
        {
            AppDbContext context = app.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                if (isDrop)
                {
                    context.Database.EnsureDeleted();
                }

                context.Database.Migrate();
            }

            return app;
        }
    }
}
