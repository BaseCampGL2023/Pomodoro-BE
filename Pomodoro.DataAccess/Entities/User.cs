// <copyright file="User.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Pomodoro.DataAccess.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [EmailAddress]
        [MaxLength(50)]
        [Unicode(false)]
        public string Email { get; set; }

        //
        public Settings? Settings { get; set; }
        public ICollection<Task>? Tasks { get; set; }

        public User(int id, string name, string email)
            : base(id)
        {
            Name = name;
            Email = email;
        }
    }
}
