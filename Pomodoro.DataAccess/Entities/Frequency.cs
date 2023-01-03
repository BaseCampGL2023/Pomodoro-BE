// <copyright file="Frequency.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.DataAccess.Entities
{
    public class Frequency : BaseEntity
    {
        public int FrequencyTypeId { get; set; }
        public short Every { get; set; }
        public bool IsCustom { get; set; }

        //
        public FrequencyType? FrequencyType { get; set; }
    }
}
