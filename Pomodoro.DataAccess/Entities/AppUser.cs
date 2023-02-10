// <copyright file="AppUser.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Pomodoro.DataAccess.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class AppUser : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(50)]
        [Unicode(false)]
        public string Email { get; set; } = string.Empty;

        public Guid PomoIdentityUserId { get; set; }

        //
        public PomoIdentityUser? PomoIdentityUser { get; set; }
        public Settings? Settings { get; set; }
        public ICollection<TaskEntity>? Tasks { get; set; }
    }
}
