// <copyright file="TaskConfig.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.DataAccess.Configs
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(e => e.IdentityUserId, "IX_Users_AspNetUserId")
                .IsUnique();

            builder.HasOne(d => d.IdentityUser)
                .WithOne()
                .HasForeignKey<User>(e => e.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
