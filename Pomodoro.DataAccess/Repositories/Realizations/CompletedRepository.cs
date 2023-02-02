using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.DataAccess.Repositories.Realizations
{
    public class CompletedRepository : BaseRepository<Completed>, ICompletedRepository
    {
        public CompletedRepository(AppDbContext context) : base(context)
        {
        }
    }
}
