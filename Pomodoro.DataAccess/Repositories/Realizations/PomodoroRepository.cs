using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.DataAccess.Repositories.Realizations
{
    public class PomodoroRepository : BaseRepository<PomodoroEntity>, IPomodoroRepository
    {
        public PomodoroRepository(AppDbContext context) : base(context)
        {
        }
    }
}
