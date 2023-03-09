// <copyright file="AppDbContextFactory.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pomodoro.Dal.Data
{
    /// <summary>
    /// Design time context factory.
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <summary>
        /// Create instance of DbContext class.
        /// </summary>
        /// <param name="args">Arguments from command line.</param>
        /// <returns>AppDbContext instance <see cref="AppDbContext"/>.</returns>
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=Pomodoro_Ex;Trusted_Connection=True;";
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
