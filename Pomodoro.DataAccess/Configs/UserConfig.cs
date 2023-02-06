// <copyright file="TaskConfig.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.DataAccess.Configs
{
    internal class UserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasIndex(e => e.PomoIdentityUserId, "IX_Users_AspNetUserId")
                .IsUnique();

            builder.HasOne(d => d.PomoIdentityUser)
                .WithOne(p => p.AppUser)
                .HasForeignKey<AppUser>(e => e.PomoIdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
