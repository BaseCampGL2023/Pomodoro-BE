using Pomodoro.Core.Interfaces.IRepositories;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<User?> FindByEmailAsync(string email);
    }
}
