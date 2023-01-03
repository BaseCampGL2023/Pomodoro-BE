// <copyright file="FrequencyType.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pomodoro.DataAccess.Entities
{
    [Index(nameof(Value), IsUnique = true)]
    public class FrequencyType : BaseEntity
    {
        [Column(TypeName = "varchar(7)")]
        public FrequencyValue Value { get; set; }

        //
        public ICollection<Frequency>? Frequencies { get; set; }
    }
}
