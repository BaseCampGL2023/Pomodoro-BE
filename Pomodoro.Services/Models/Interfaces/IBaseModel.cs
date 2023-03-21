// <copyright file="IBaseModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities.Base;

namespace Pomodoro.Services.Models.Interfaces
{
    /// <summary>
    /// Defines type of DTO with mapping methods.
    /// </summary>
    /// <typeparam name="T">Underlying entity type.</typeparam>
    public interface IBaseModel<T>
        where T : IBelongEntity
    {
        /// <summary>
        /// Gets or sets entity Id.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Map properties from Dal object to DTO instance.
        /// </summary>
        /// <param name="entity">Object of IBelongEntity.</param>
        /// <param name="isMapOwner">If TRUE add owner id to DTO.</param>
        void Assign(T entity, bool isMapOwner = false);

        /// <summary>
        /// Map properties from DTO to Dal instance.
        /// </summary>
        /// <param name="userId">Owner id.</param>
        /// <returns>Corresponding entity object.</returns>
        T ToDalEntity(Guid userId);
    }
}
