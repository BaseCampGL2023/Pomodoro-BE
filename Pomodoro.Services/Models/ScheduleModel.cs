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
    public class ScheduleModel : IBaseModel<Schedule>, IValidatableObject
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
        [Required(ErrorMessage = "Title is required")]
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
        [Required(ErrorMessage = "Start date time is required")]
        public DateTime StartDt { get; set; }

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
        /// Gets or sets owner id.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid OwnerId { get; set; }

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
        /// <param name="isMapOwner">If TRUE add owner id to DTO.</param>
        public void Assign(Schedule entity, bool isMapOwner = false)
        {
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
            this.OwnerId = isMapOwner ? entity.AppUserId : Guid.Empty;
            this.PreviousId = entity.PreviousId;
            foreach (var task in entity.Tasks)
            {
                var model = new TaskModel();
                model.Assign(task);
                this.Tasks.Add(model);
            }
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

        /// <summary>
        /// Validate schedule.
        /// </summary>
        /// <param name="validationContext">Context in wich a validation check is performed.</param>
        /// <returns>Collection of validation errors.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new ();
            if (!Enum.GetValues<ScheduleType>().Any(e => e == this.ScheduleType))
            {
                results.Add(new ValidationResult(
                    "Non exisiting schedule type",
                    new List<string> { nameof(this.ScheduleType) }));
            }

            if (this.ScheduleType != ScheduleType.AnnualOnDate
                && !string.IsNullOrWhiteSpace(this.Template)
                && this.Template.Any(ch => ch != '0' && ch != '1'))
            {
                results.Add(new ValidationResult(
                            "Only '0' and '1' symbols expected in template",
                            new List<string> { nameof(this.Template) }));
            }

            switch (this.ScheduleType)
            {
                case ScheduleType.EveryDay:
                    if (!string.IsNullOrWhiteSpace(this.Template)
                        && this.Template != "1")
                    {
                        results.Add(new ValidationResult(
                            "Template should correspond to schedule type," +
                            " for everyday: '1', or don't add any template",
                            new List<string> { nameof(this.Template) }));
                    }

                    break;
                case ScheduleType.WorkDay:
                    if (!string.IsNullOrWhiteSpace(this.Template)
                        && this.Template.Length != 7
                        && this.Template != "0111110")
                    {
                        results.Add(new ValidationResult(
                            "Template should correspond to schedule type," +
                            " for workday: '0111110', or don't add any template",
                            new List<string> { nameof(this.Template) }));
                    }

                    break;
                case ScheduleType.WeekEnd:
                    if (!string.IsNullOrWhiteSpace(this.Template)
                        && this.Template.Length != 7
                        && this.Template != "1000001")
                    {
                        results.Add(new ValidationResult(
                           "Template should correspond to schedule type," +
                           " for weekend: '1000001', or don't add any template",
                           new List<string> { nameof(this.Template) }));
                    }

                    break;
                case ScheduleType.AnnualOnDate:
                    if (!string.IsNullOrWhiteSpace(this.Template)
                        && !DateTime.TryParse(this.Template, out _))
                    {
                        results.Add(new ValidationResult(
                           "Template should correspond to schedule type," +
                           " for annual should be equal with StartDt field, or don't add any template",
                           new List<string> { nameof(this.Template) }));
                    }

                    break;

                case ScheduleType.WeekTemplate:
                    if (string.IsNullOrWhiteSpace(this.Template)
                        || this.Template.Length != 7
                        || this.Template == "0000000")
                    {
                        results.Add(new ValidationResult(
                            "Template should correspond to schedule type," +
                            " for week template length - 7 symbols," +
                            " at least one non-zero value, something like 0100100",
                            new List<string> { nameof(this.Template) }));
                    }

                    break;

                case ScheduleType.MonthTemplate:
                case ScheduleType.MonthDayForwardTemplate:
                case ScheduleType.MonthDayBackwardTemplate:
                    if (string.IsNullOrWhiteSpace(this.Template)
                        || this.Template.Length != 31
                        || !this.Template.Any(ch => ch == '1'))
                    {
                        results.Add(new ValidationResult(
                            "Template should correspond to schedule type," +
                            " for month template length - 31 symbols," +
                            " at least one non-zero value",
                            new List<string> { nameof(this.Template) }));
                    }

                    break;
                case ScheduleType.EveryNDay:
                    if (string.IsNullOrWhiteSpace(this.Template)
                        || this.Template.Length < 2
                        || this.Template.Count(ch => ch == '1') > 1
                        || !this.Template.StartsWith('1'))
                    {
                        results.Add(new ValidationResult(
                            "Template should correspond to schedule type," +
                            " for every N day template only one '1' symbol as first character",
                            new List<string> { nameof(this.Template) }));
                    }

                    break;
                case ScheduleType.Sequence:
                    if (string.IsNullOrWhiteSpace(this.Template)
                        || this.Template.Length < 2
                        || this.Template.Count(ch => ch == '1') < 2
                        || this.Template.EndsWith('1'))
                    {
                        results.Add(new ValidationResult(
                            "Template should correspond to schedule type," +
                            " for sequance template expected more than one '1' character," +
                            "template should begins with '1' ends with '0' character," +
                            " for example '1001000100'",
                            new List<string> { nameof(this.Template) }));
                    }

                    break;
            }

            if (this.StartDt < DateTime.Now)
            {
                results.Add(new ValidationResult(
                    "Start date and time should be planned in future",
                    new List<string> { nameof(this.StartDt) }));
            }

            if (this.FinishDt != DateTime.MinValue
                && this.FinishDt < this.StartDt)
            {
                results.Add(new ValidationResult(
                    "Finish date and time should be planned after strat date and time",
                    new List<string> { nameof(this.FinishDt) }));
            }

            if (this.AllocatedDuration < 0
                || this.AllocatedDuration > 86400)
            {
                results.Add(new ValidationResult(
                    "If you provide allocated duration it should be less than one day",
                    new List<string> { nameof(this.FinishDt) }));
            }

            return results;
        }
    }
}
