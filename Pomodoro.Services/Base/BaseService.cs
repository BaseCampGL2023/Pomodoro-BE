// <copyright file="BaseService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Pomodoro.Dal.Entities.Base;
using Pomodoro.Dal.Exceptions;
using Pomodoro.Dal.Repositories.Base;
using Pomodoro.Services.Models.Interfaces;
using Pomodoro.Services.Models.Results;

namespace Pomodoro.Services.Base
{
    /// <summary>
    /// Service for implementing business logic.
    /// </summary>
    /// <typeparam name="TE">Dal entity type.</typeparam>
    /// <typeparam name="TM">Model type.</typeparam>
    /// <typeparam name="TR">Repository type.</typeparam>
    public abstract class BaseService<TE, TM, TR> : IBaseService<TE, TM, TR>
        where TE : IBelongEntity
        where TM : IBaseModel<TE>, new()
        where TR : IBelongRepository<TE>
    {
        /// <summary>
        /// Repository instance.
        /// </summary>
        private readonly TR repo;

        private readonly ILogger<IBaseService<TE, TM, TR>> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{T, TV, TR}"/> class.
        /// </summary>
        /// <param name="repo">Implementation of IBelongRepository<typeparamref name="TE"/>.</param>
        /// <param name="logger">Instance of logger.</param>
        protected BaseService(TR repo, ILogger<IBaseService<TE, TM, TR>> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        /// <summary>
        /// Gets instance of entity repository.
        /// </summary>
        protected TR Repo => this.repo;

        /// <summary>
        /// Gets instance of Logger.
        /// </summary>
        protected ILogger<IBaseService<TE, TM, TR>> Logger => this.logger;

        /// <summary>
        /// Return belonging to user object by id.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <param name="ownerId">Object owner id.</param>
        /// <returns>Return object without related objects.</returns>
        public virtual async Task<ServiceResponse<TM>> GetOwnByIdAsync(Guid id, Guid ownerId)
        {
            var result = await this.repo.GetByIdAsync(id);
            if (result is null)
            {
                return new ServiceResponse<TM> { Result = ResponseType.NotFound };
            }
            else if (result.AppUserId != ownerId)
            {
                return new ServiceResponse<TM> { Result = ResponseType.Forbid };
            }
            else
            {
                var data = new TM();
                data.Assign(result);
                return new ServiceResponse<TM>
                {
                    Result = ResponseType.Ok,
                    Data = data,
                };
            }
        }

        /// <summary>
        /// Add new object to database.
        /// </summary>
        /// <param name="model">Object for persisting, model updated after persistance.</param>
        /// <param name="ownerId">Object owner id.</param>
        /// <returns>TRUE if value persist succesfully, FALSE otherwise.</returns>
        public virtual async Task<ServiceResponse<bool>> AddOneOwnAsync(TM model, Guid ownerId)
        {
            try
            {
                var entity = model.ToDalEntity(ownerId);
                int result = await this.repo.AddAsync(entity, true);
                if (result > 0)
                {
                    model.Assign(entity);
                    return new ServiceResponse<bool> { Result = ResponseType.Ok, Data = true };
                }
                else
                {
                    this.logger.LogCritical("Unpredictable path of execution, entity shouldn't created, exception not thrown.", model);
                    return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Something went wrong" };
                }
            }
            catch (PomoDbUpdateException)
            {
                return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Wrong data, check related" };
            }
        }

        /// <summary>
        /// Retrieve all belonging to the user object from database without adding them to ChangeTracker.
        /// </summary>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>Belongin to user objects collection.</returns>
        public virtual async Task<ICollection<TM>> GetOwnAllAsync(Guid ownerId)
        {
            var collection = await this.repo.GetBelongingAllAsNoTracking(ownerId);
            var result = new List<TM>();
            foreach (var item in collection)
            {
                var model = new TM();
                model.Assign(item);
                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// Update exisiting object.
        /// </summary>
        /// <param name="model">Model object, value updated after persistance.</param>
        /// <param name="ownerId">Object owner Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public virtual async Task<ServiceResponse<bool>> UpdateOneOwnAsync(TM model, Guid ownerId)
        {
            try
            {
                var entity = model.ToDalEntity(ownerId);
                int result = await this.repo.UpdateAsync(entity, true);
                if (result > 0)
                {
                    model.Assign(entity);
                    return new ServiceResponse<bool> { Result = ResponseType.Ok, Data = true, };
                }
                else
                {
                    this.logger.LogCritical("Unpredictable path of execution, entity shouldn't update, exception not thrown.", model);
                    return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Something went wrong" };
                }
            }
            catch (PomoDbUpdateException)
            {
                return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Wrong related data" };
            }
        }

        /// <summary>
        /// Delete object from database.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <param name="ownerId">Owner id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public virtual async Task<ServiceResponse<bool>> DeleteOneOwnAsync(Guid id, Guid ownerId)
        {
            try
            {
                int result = await this.repo.DeleteOneBelongingAsync(id, ownerId, true);
                if (result > 0)
                {
                    return new ServiceResponse<bool> { Result = ResponseType.NoContent, Data = true };
                }
                else
                {
                    this.logger.LogCritical("Unpredictable path of execution, entity shouldn't delete, exception not thrown.");
                    return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Something went wrong" };
                }
            }
            catch (PomoConcurrencyException)
            {
                return new ServiceResponse<bool> { Result = ResponseType.Error, Message = "Wrong data" };
            }
        }
    }
}
