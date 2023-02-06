using Microsoft.AspNetCore.Identity;
using Pomodoro.DataAccess.Entities.Interfaces;

namespace Pomodoro.DataAccess.Entities
{
    public class PomoIdentityUser : IdentityUser<Guid>, IBaseEntity
    {
        public AppUser? AppUser { get; set; }
    }
}
