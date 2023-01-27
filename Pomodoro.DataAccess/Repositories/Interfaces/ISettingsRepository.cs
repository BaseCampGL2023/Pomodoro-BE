﻿using Pomodoro.Core.Interfaces.IRepositories;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.DataAccess.Repositories.Interfaces
{
    public interface ISettingsRepository : IBaseRepository<Settings>
    {
    }
}
