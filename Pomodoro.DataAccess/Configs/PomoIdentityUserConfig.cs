// <copyright file="TaskConfig.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.DataAccess.Configs
{
    internal class PomoIdentityUserConfig : IEntityTypeConfiguration<PomoIdentityUser>
    {
        public void Configure(EntityTypeBuilder<PomoIdentityUser> builder)
        {
            builder.Navigation(e => e.AppUser).AutoInclude();
        }
    }
}
