using Pomodoro.Dal.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Services.Models.Interfaces
{
    public interface BaseModel<T>
        where T : IBelongEntity
    {
        Guid Id { get; set; }
        void Assign(T entity);
        T ToDalEntity(Guid userId);
    }
}
