using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.DataAccess.Repositories.Realizations
{
    public class UserRepository : BaseRepository<AppUser>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<AppUser?> FindByEmailAsync(string email)
        {
            return await context.AppUsers.Include(u => u.PomoIdentityUser)
                .SingleAsync(u => u.Email == email);
        }
    }
}
