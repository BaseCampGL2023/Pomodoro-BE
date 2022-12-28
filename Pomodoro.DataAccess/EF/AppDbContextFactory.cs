// <copyright file="AppDbContextFactory.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Pomodoro.DataAccess.EF
{
    internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private const string connectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Pomodoro;
                Integrated Security=True;Connect Timeout=30";

        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
