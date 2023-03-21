// <copyright file="BaseService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities.Base;
using Pomodoro.Dal.Repositories.Base;
using Pomodoro.Services.Models.Interfaces;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services.Base
{
    /// <summary>
    /// Service for implementing business logic.
    /// </summary>
    /// <typeparam name="T">Dal entity type.</typeparam>
    /// <typeparam name="TV">Model type.</typeparam>
    public abstract class BaseService<T, TV>
        where T : IBelongEntity
        where TV : IBaseModel<T>, new()
    {
        /// <summary>
        /// Repository instance.
        /// </summary>
        protected readonly IBelongRepository<T> repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{T, V}"/> class.
        /// </summary>
        /// <param name="repo">Implementation of IBelongRepository.</param>
        protected BaseService(IBelongRepository<T> repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// Return belonging to user object by id.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <param name="ownerId">Object owner id.</param>
        /// <returns>Return object without related objects.</returns>
        public async Task<ServiceResponse<TV>> GetOwnByIdAsync(Guid id, Guid ownerId)
        {
            var result = await this.repo.GetByIdAsync(id);
            if (result is null)
            {
                return new ServiceResponse<TV> { Result = ResponseType.NotFound };
            }
            else if (result.AppUserId != ownerId)
            {
                return new ServiceResponse<TV> { Result = ResponseType.Forbid };
            }
            else
            {
                var data = new TV();
                data.Assign(result);
                return new ServiceResponse<TV>
                {
                    Result = ResponseType.Ok,
                    Data = data,
                };
            }
        }

        /// <summary>
        /// Add new object to database.
        /// </summary>
        /// <param name="model">Object for persisting.</param>
        /// <param name="ownerId">Object owner id.</param>
        /// <returns>TRUE if value persist succesfully, FALSE otherwise.</returns>
        public async Task<bool> AddOneOwnAsync(TV model, Guid ownerId)
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

        /// <summary>
        /// Retrieve all belonging to the user object from database without adding them to ChangeTracker.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>Belongin to user objects collection.</returns>
        public async Task<ICollection<TV>> GetOwnAllAsync(Guid ownerId)
        {
            var collection = await this.repo.GetBelongingAllAsNoTracking(ownerId);
            var result = new List<TV>();
            foreach (var item in collection)
            {
                var model = new TV();
                model.Assign(item);
                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// Update exisiting object.
        /// </summary>
        /// <param name="model">Model object.</param>
        /// <param name="ownerId">Object owner Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<bool> UpdateOneOwnAsync(TV model, Guid ownerId)
        {
            var entity = model.ToDalEntity(ownerId);
            int result = await this.repo.UpdateAsync(entity, true);
            return result > 0;
        }

        /// <summary>
        /// Delete object from database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<bool> DeleteOneOwnAsync(Guid id, Guid ownerId)
        {
            int result = await this.repo.DeleteOneBelongingAsync(id, ownerId, true);
            return result > 0;
        }
    }
}
