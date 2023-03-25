// <copyright file="BaseController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Dal.Entities.Base;
using Pomodoro.Dal.Repositories.Base;
using Pomodoro.Services.Base;
using Pomodoro.Services.Models.Interfaces;
using Pomodoro.Services.Models.Results;
using Swashbuckle.AspNetCore.Annotations;

namespace Pomodoro.Api.Controllers.Base
{
    /// <summary>
    /// Extended ControllerBase <see cref="ControllerBase"/> by adding UserName and
    /// UserId property and add actions for base CRUD operations.
    /// </summary>
    /// <typeparam name="TS">Service object.</typeparam>
    /// <typeparam name="TE">Entity type.</typeparam>
    /// <typeparam name="TM">DTO type.</typeparam>
    /// <typeparam name="TR">Repository type, used by service.</typeparam>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(401, "This endpoints available only for registered users")]
    public abstract class BaseController<TS, TE, TM, TR> : ControllerBase
        where TS : IBaseService<TE, TM, TR>
        where TE : IBelongEntity, new()
        where TM : IBaseModel<TE>, new()
        where TR : IBelongRepository<TE>
    {
        /// <summary>
        /// Service for business logic.
        /// </summary>
        private readonly TS service;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController{TS, TE, TM, TR}"/> class.
        /// </summary>
        /// <param name="service">Service implemented business logic.</param>
        protected BaseController(TS service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets instance of service.
        /// </summary>
        protected TS Service => this.service;

        /// <summary>
        /// Gets authenticated user id Guid.Empty
        /// if user not authenticated.
        /// </summary>
        protected Guid UserId
        {
            get
            {
                var id = this.User.FindFirst("userId");
                if (id is null)
                {
                    return Guid.Empty;
                }

                return new Guid(id.Value);
            }
        }

        /// <summary>
        /// Gets autheticated user name or "User Unknown"
        /// if user not authenticated.
        /// </summary>
        protected string UserName
        {
            get => this.User.FindFirst(ClaimTypes.Name)?.Value
                ?? "User Unknown";
        }

        /// <summary>
        /// Return all objects belonging to user.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("own")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Retrieved all user's objects.")]
        public async Task<ActionResult<ICollection<TM>>> GetOwnAll()
        {
            return this.Ok(await this.service.GetOwnAllAsync(this.UserId));
        }

        /// <summary>
        /// Return belonging object by id, or 404 if not exist, or 403 if access denied.
        /// </summary>
        /// <param name="id">Schedule id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("own/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(200, "The execution was successful")]
        [SwaggerResponse(403, "This object don't belong to current user")]
        [SwaggerResponse(404, "Object not found")]
        public async Task<ActionResult<TM>> GetById(Guid id)
        {
            var result = await this.service.GetOwnByIdAsync(id, this.UserId);

            return this.MapServiceResponse(result);
        }

        /// <summary>
        /// Persist new belonging to user object.
        /// </summary>
        /// <param name="model">New object.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(201, "Object created")]
        [SwaggerResponse(400, "The request was invalid")]
        public async Task<ActionResult<TM>> AddOne(TM model)
        {
            var result = await this.service.AddOneOwnAsync(model, this.UserId);
            if (result)
            {
                return this.CreatedAtAction(nameof(this.GetById), new { model.Id }, model);
            }

            return this.BadRequest(model);
        }

        /// <summary>
        /// Delete belongin object by id.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("own/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(204, "Delete successfully")]
        [SwaggerResponse(400, "No object with such id for this user")]
        public async Task<ActionResult> DeleteOne(Guid id)
        {
            var result = await this.service.DeleteOneOwnAsync(id, this.UserId);
            return result ? this.NoContent() : this.BadRequest(id);
        }

        /// <summary>
        /// Update existing object.
        /// </summary>
        /// <param name="id">Object id.</param>
        /// <param name="model">Exisitng object.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("own/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(204, "Update successfully")]
        [SwaggerResponse(400, "No schedule with such id for this user")]
        public async Task<ActionResult> UpdateOne(Guid id, TM model)
        {
            if (id != model.Id)
            {
                return this.BadRequest();
            }

            var result = await this.service.UpdateOneOwnAsync(model, this.UserId);
            return result ? this.NoContent() : this.BadRequest(id);
        }

        /// <summary>
        /// Map service response to ActionResult.
        /// </summary>
        /// <typeparam name="TSR">Service response data type (object, collection or plain value).</typeparam>
        /// <param name="response">Service response object.</param>
        /// <returns>ActionResult corresponding to service response.</returns>
        protected ActionResult MapServiceResponse<TSR>(ServiceResponse<TSR> response)
        {
            return response.Result switch
            {
                ResponseType.Ok => this.Ok(response.Data),
                ResponseType.NotFound => this.NotFound(),
                ResponseType.Forbid => this.Forbid(),
                ResponseType.Error => this.BadRequest(response.Message),
                _ => this.BadRequest()
            };
        }
    }
}
