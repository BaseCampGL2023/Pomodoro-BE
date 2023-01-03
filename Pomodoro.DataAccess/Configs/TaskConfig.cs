// <copyright file="TaskConfig.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pomodoro.DataAccess.Configurations
{
    internal class TaskConfig : IEntityTypeConfiguration<Entities.Task>
    {
        public void Configure(EntityTypeBuilder<Entities.Task> builder)
        {
            builder
                .Property(t => t.InitialDate)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}
