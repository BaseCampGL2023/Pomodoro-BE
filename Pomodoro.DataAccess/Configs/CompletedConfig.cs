// <copyright file="CompletedConfig.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.DataAccess.Configurations
{
    internal class CompletedConfig : IEntityTypeConfiguration<Completed>
    {
        public void Configure(EntityTypeBuilder<Completed> builder)
        {
            builder
                .Property(c => c.ActualDate)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}
