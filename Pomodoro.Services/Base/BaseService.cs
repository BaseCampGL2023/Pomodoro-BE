// <copyright file="BaseService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities.Base;
using Pomodoro.Dal.Repositories.Base;
using Pomodoro.Services.Models;
using Pomodoro.Services.Models.Interfaces;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services.Base
{
    /// <summary>
    /// Base service class.
    /// </summary>
    /// <typeparam name="T">IBelongEntity type.</typeparam>
    public abstract class BaseService<T, V>
        where T : IBelongEntity
        //where V : IBelongModel<BaseModel, IBelongEntity>
        where V : BaseModel<T>, new()
    {
        protected readonly IBelongRepository<T> repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{T}"/> class.
        /// </summary>
        /// <param name="repo">Implementation of IBelongRepository.</param>
        protected BaseService(IBelongRepository<T> repo)
        {
            this.repo = repo;
        }

        public async Task<ServiceResponse<V>> GetOwnByIdAsync(Guid id, Guid ownerId)
        {
            var result = await this.repo.GetByIdAsync(id);
            if (result is null)
            {
                return new ServiceResponse<V> { Result = ResponseType.NotFound };
            }
            else if (result.AppUserId != ownerId)
            {
                return new ServiceResponse<V> { Result = ResponseType.Forbid };
            }
            else
            {
                var data = new V();
                data.Assign(result);
                return new ServiceResponse<V>
                {
                    Result = ResponseType.Ok,
                    Data = data,
                };
            }
        }

        public async Task<bool> AddOneOwnAsync(V model, Guid ownerId)
        {
            var entity = model.ToDalEntity(ownerId);
            int result = await this.repo.AddAsync(entity, true);
            if (result > 0)
            {
                model.Id = entity.Id;
                return true;
            }

            return false;
        }

        public async Task<ICollection<V>> GetOwnAllAsync(Guid ownerId)
        {
            var collection = await this.repo.GetBelongingAllAsNoTracking(ownerId);
            var result = new List<V>();
            foreach (var item in collection)
            {
                var model = new V();
                model.Assign(item);
                result.Add(model);
            }
            return result;
            //return collection.Select(e => new V().Assign(e)).ToList();
        }
    }
}
