// <copyright file="ScheduleModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pomodoro.Dal.Entities;
using Pomodoro.Dal.Enums;
using Pomodoro.Services.Models.Interfaces;

namespace Pomodoro.Services.Models
{
    /// <summary>
    /// Represents the schedule.
    /// </summary>
    public class ScheduleModel : BaseModel<Schedule>
        
        //BaseModel, IBelongModel<ScheduleModel, Schedule>
    {
        /// <summary>
        /// Gets or sets task id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets schedule type.
        /// </summary>
        [Required(ErrorMessage = "Schedule type is required")]
        public ScheduleType ScheduleType { get; set; }

        /// <summary>
        /// Gets or sets pattern of frequency of task execution.
        /// </summary>
        public string Template { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets routine title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the schedule is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets routine description, optional.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets DateTime when routine created.
        /// </summary>
        public DateTime CreatedDt { get; set; }

        /// <summary>
        /// Gets or sets DateTime when routine finished.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime? FinishDt { get; set; }

        /// <summary>
        /// Gets or sets planned start time.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime? StartDt { get; set; }

        /// <summary>
        /// Gets or sets planned duration of the routine round.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int AllocatedDuration { get; set; }

        /// <summary>
        /// Gets or sets CategoryId.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets Category name.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Category { get; set; }

        /// <summary>
        /// Gets or sets foreign key to Schedule entity.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid? PreviousId { get; set; }

        /// <summary>
        /// Gets or sets collection of TaskModel related to this schedule.
        /// </summary>
        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();

        /// <summary>
        /// Map from Dal entity to model object.
        /// </summary>
        /// <param name="entity">Instance of Schedule <see cref="Schedule"/>.</param>
        /// <returns>Model object.</returns>
        public void Assign(Schedule entity)
        {
            //return new ScheduleModel
            //{
            this.Id = entity.Id;
                this.Title = entity.Title;
                this.Description = entity.Description;
                this.ScheduleType = entity.ScheduleType;
                this.Template = entity.Template;
                this.CreatedDt = entity.CreatedDt;
                this.FinishDt = entity.FinishDt;
                this.StartDt = entity.StartDt;
                this.IsActive = entity.IsActive;
                this.AllocatedDuration = entity.AllocatedDuration.HasValue ?
                    (int)entity.AllocatedDuration.Value.TotalSeconds : 0;
                this.Category = entity.Category?.Name;
                this.CategoryId = entity.Category?.Id;
                this.PreviousId = entity.PreviousId;
                this.Tasks = entity.Tasks.Any() ?
                    entity.Tasks.Select(e => TaskModel.Create(e)).ToList() : new List<TaskModel>();
            //};
        }

        /// <summary>
        /// Map from model to Dal entity.
        /// </summary>
        /// <param name="userId">Owner id.</param>
        /// <returns>Dal entity.</returns>
        public Schedule ToDalEntity(Guid userId)
        {
            return new Schedule
            {
                Id = this.Id,
                Title = this.Title,
                ScheduleType = this.ScheduleType,
                Template = this.Template,
                Description = this.Description,
                CreatedDt = this.CreatedDt == DateTime.MinValue
                    ? DateTime.UtcNow : this.CreatedDt,
                StartDt = this.StartDt,
                FinishDt = this.FinishDt,
                AllocatedDuration = this.AllocatedDuration > 0 ?
                    TimeSpan.FromSeconds(this.AllocatedDuration) : null,
                CategoryId = this.CategoryId,
                AppUserId = userId,
                IsActive = this.IsActive,
                PreviousId = this.PreviousId,
                Tasks = this.Tasks.Any() ?
                    this.Tasks.Select(e => e.ToDalEntity(userId)).ToList() : new List<AppTask>(),
            };
        }

        // TODO: Add validation schedule, ModifiedAt
    }
}
