﻿using Pomodoro.Core.Models.Frequency;
using Pomodoro.Core.Models.Tasks;
using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.DataAccess.Repositories.Realizations
{
    public class FrequencyRepository : BaseRepository<Frequency>, IFrequencyRepository
    {
        public FrequencyRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Guid> AddFrequencyAsync(Frequency freq)
        {
            await context.Set<Frequency>().AddAsync(freq);
            await SaveChangesAsync();
            return freq.Id;
        }

    }
}
