// <copyright file="TaskModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pomodoro.Dal.Configs;
using Pomodoro.Dal.Entities;
using Pomodoro.Services.Models.Interfaces;

namespace Pomodoro.Services.Models
{
    /// <summary>
    /// Represent task for client.
    /// </summary>
    public class TaskModel : IBaseModel<AppTask>
    {
        /// <summary>
        /// Gets or sets task id.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets task title.
        /// </summary>
        [Required(ErrorMessage = "Task {0} is required.")]
        [StringLength(
            PomoConstants.TaskTitleMaxLength,
            ErrorMessage = "The {0} should be less than {1} characters.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets task description, optional.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [StringLength(
            PomoConstants.TaskDescriptionMaxLength,
            ErrorMessage = "The {0} should be less or equal than {1} characters.")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets task number in sequence for tasks with a schedule.
        /// Sets 1 for non-periodic tasks.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets planned start time.
        /// </summary>
        public DateTime StartDt { get; set; }

        /// <summary>
        /// Gets or sets DateTime when task performing completed.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime? FinishDt { get; set; }

        /// <summary>
        /// Gets or sets planned duration of the task.
        /// </summary>
        [Required(ErrorMessage = "The {0} is required, if you no allocate time - set 0.")]
        [Range(0, int.MaxValue, ErrorMessage = "Allocated duration shouldn't be less than 0 second.")]
        public int AllocatedDuration { get; set; }

        /// <summary>
        /// Gets or sets Category name.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Category { get; set; }

        /// <summary>
        /// Gets or sets CategoryId.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets owner id.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Gets or sets Schedule title.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Schedule { get; set; }

        /// <summary>
        /// Gets or sets ScheduleId.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid? ScheduleId { get; set; }

        /// <summary>
        /// Gets or sets collection of pomodoros spent to task.
        /// </summary>
        public ICollection<PomoModel> Pomodoros { get; set; } = new List<PomoModel>();

        /// <summary>
        /// Map from Dal entity to model object.
        /// </summary>
        /// <param name="entity">Instance of AppTask <see cref="AppTask"/>.</param>
        /// <param name="isMapOwner">If TRUE add owner id to DTO.</param>
        public void Assign(AppTask entity, bool isMapOwner = true)
        {
            this.Id = entity.Id;
            this.Title = entity.Title;
            this.Description = entity.Description;
            this.SequenceNumber = entity.ScheduleId != null ? entity.SequenceNumber : 0;
            this.StartDt = entity.StartDt;
            this.FinishDt = entity.FinishDt;
            this.AllocatedDuration = (int)entity.AllocatedDuration.TotalSeconds;
            this.Category = entity.Category?.Name;
            this.OwnerId = isMapOwner ? entity.AppUserId : Guid.Empty;
            this.ScheduleId = entity.ScheduleId;
            this.Schedule = entity.Schedule?.Title;
            this.Pomodoros = entity.Pomodoros.Any() ?
                entity.Pomodoros.Select(e => PomoModel.Create(e)).ToList() : new List<PomoModel>();
        }

        /// <summary>
        /// Map from model to Dal entity.
        /// </summary>
        /// <param name="userId">Owner id.</param>
        /// <returns>Dal entity.</returns>
        public AppTask ToDalEntity(Guid userId)
        {
            return new AppTask
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description,
                SequenceNumber = this.SequenceNumber > 0 ?
                    this.SequenceNumber : 0,
                StartDt = this.StartDt,
                FinishDt = this.FinishDt,
                AllocatedDuration = TimeSpan.FromSeconds(this.AllocatedDuration),
                AppUserId = userId,
                ScheduleId = this.ScheduleId,
                CategoryId = this.CategoryId,
                Pomodoros = this.Pomodoros.Any() ?
                    this.Pomodoros.Select(p => p.ToDalEntity()).ToList() : new List<PomoUnit>(),
            };
        }
    }
}
