// <copyright file="CategoryModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;
using Pomodoro.Dal.Entities;
using Pomodoro.Services.Models.Interfaces;

namespace Pomodoro.Services.Models
{
    /// <summary>
    /// Represent category for client.
    /// </summary>
    public class CategoryModel : IBaseModel<Category>
    {
        /// <summary>
        /// Gets or sets category id.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets category name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets category description.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets foreign key to AppUser Entity.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid AppUserId { get; set; }

        /// <summary>
        /// Gets or sets collection of schedules related to this category.
        /// </summary>
        public ICollection<ScheduleModel> Schedules { get; set; } = new List<ScheduleModel>();

        /// <summary>
        /// Gets or sets collection of tasks related to this category.
        /// </summary>
        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();

        /// <inheritdoc/>
        public void Assign(Category entity, bool isMapOwner = false)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.Description = entity.Description;
            this.AppUserId = isMapOwner ? entity.AppUserId : Guid.Empty;

            foreach (var task in entity.Tasks)
            {
                var model = new TaskModel();
                model.Assign(task);
                this.Tasks.Add(model);
            }

            foreach (var schedule in entity.Schedules)
            {
                var model = new ScheduleModel();
                model.Assign(schedule);
                this.Schedules.Add(model);
            }
        }

        /// <inheritdoc/>
        public Category ToDalEntity(Guid userId)
        {
            return new Category
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                AppUserId = userId,
                Tasks = this.Tasks.Any() ?
                    this.Tasks.Select(e => e.ToDalEntity(userId)).ToList() : new List<AppTask>(),
                Schedules = this.Schedules.Any() ?
                    this.Schedules.Select(e => e.ToDalEntity(userId)).ToList() : new List<Schedule>(),
            };
        }
    }
}
