// <copyright file="TaskConfig.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.DataAccess.Configurations
{
    internal class TaskConfig : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder
                .Property(t => t.InitialDate)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}
