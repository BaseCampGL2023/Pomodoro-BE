using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.DataAccess.Repositories.Realizations
{
    public class FrequencyTypeRepository : BaseRepository<FrequencyType>, IFrequencyTypeRepository
    {
        public FrequencyTypeRepository(AppDbContext context) : base(context)
        {
        }
    }
}
