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
    }
}
