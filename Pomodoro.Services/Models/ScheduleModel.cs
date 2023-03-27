// <copyright file="ScheduleModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pomodoro.Dal.Configs;
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
        [Required(ErrorMessage = "Schedule {0} is required.")]
        [EnumDataType(typeof(ScheduleType), ErrorMessage = "Non exisiting {0}")]
        public ScheduleType ScheduleType { get; set; }

        /// <summary>
        /// Gets or sets pattern of frequency of task execution.
        /// </summary>
        [Required(ErrorMessage = "Schedule {0} is required.")]
        [StringLength(
            PomoConstants.ScheduleTemplateMaxLength,
            ErrorMessage = "The {0} should be less or equal than {1} characters.")]
        public string Template { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets routine title.
        /// </summary>
        [Required(ErrorMessage = "Schedule {0} is required.")]
        [StringLength(
            PomoConstants.ScheduleTitleMaxLength,
            ErrorMessage = "The {0} should be less or equal than {1} characters.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets routine description, optional.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [StringLength(
            PomoConstants.ScheduleDescriptionMaxLength,
            ErrorMessage = "The {0} should be less or equal than {1} characters.")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets DateTime when routine finished.
        /// </summary>
        [Required(ErrorMessage = "Schedule {0} datetime is required")]
        public DateTime FinishAt { get; set; }

        /// <summary>
        /// Gets or sets planned start time.
        /// </summary>
        [Required(ErrorMessage = "Schedule {0} date time is required")]
        public DateTime StartDt { get; set; }

        /// <summary>
        /// Gets or sets planned duration of the routine round.
        /// </summary>
        [Required(ErrorMessage = "Schedule {0} is required")]
        [Range(60, 86400, ErrorMessage = "Allocated duration should be between one minute and 1 day.")]
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
        /// Gets or sets collection of TaskModel related to this schedule.
        /// </summary>
        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();

        /// <summary>
        /// Map from Dal entity to model object.
        /// </summary>
        /// <param name="entity">Instance of Schedule <see cref="Schedule"/>.</param>
        /// <param name="isMapOwner">If TRUE add owner id to DTO.</param>
        public void Assign(Schedule entity, bool isMapOwner = true)
        {
            this.Id = entity.Id;
            this.Title = entity.Title;
            this.Description = entity.Description;
            this.ScheduleType = entity.ScheduleType;
            this.Template = entity.Template;
            this.FinishAt = entity.FinishAtDt;
            this.StartDt = entity.StartDt;
            this.AllocatedDuration = (int)entity.AllocatedDuration.TotalSeconds;
            this.Category = entity.Category?.Name;
            this.CategoryId = entity.Category?.Id;
            this.OwnerId = isMapOwner ? entity.AppUserId : Guid.Empty;
            this.Tasks = this.Tasks.Any() ? new List<TaskModel>() : this.Tasks;
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
                StartDt = this.StartDt,
                FinishAtDt = this.FinishAt,
                AllocatedDuration = TimeSpan.FromSeconds(this.AllocatedDuration),
                CategoryId = this.CategoryId,
                AppUserId = userId,
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

            // TODO: how validate related tasks, don't create or update them;

            if (this.FinishAt < this.StartDt)
            {
                results.Add(new ValidationResult(
                    "Finish date and time should be planned after strat date and time",
                    new List<string> { nameof(this.FinishAt) }));
            }

            if (this.ScheduleType != ScheduleType.AnnualOnDate
                && this.FinishAt > this.StartDt.AddYears(1))
            {
                results.Add(new ValidationResult(
                    "You can create planned tasks only for one year",
                    new List<string> { nameof(this.FinishAt) }));
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

                    if (this.StartDt.DayOfWeek == DayOfWeek.Sunday
                        || this.StartDt.DayOfWeek == DayOfWeek.Saturday)
                    {
                        results.Add(new ValidationResult(
                            "Start datetime should be work day (from Monday to Friday)",
                            new List<string> { nameof(this.ScheduleType), nameof(this.StartDt) }));
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

                    if (this.StartDt.DayOfWeek != DayOfWeek.Sunday
                        && this.StartDt.DayOfWeek != DayOfWeek.Saturday)
                    {
                        results.Add(new ValidationResult(
                            "Start datetime should be weekend (Saturday or Sunday)",
                            new List<string> { nameof(this.ScheduleType), nameof(this.StartDt) }));
                    }

                    break;
                case ScheduleType.AnnualOnDate:
                    if (!string.IsNullOrWhiteSpace(this.Template))
                    {
                        results.Add(new ValidationResult(
                           "Template should correspond to schedule type," +
                           " for annual don't add any template",
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
                        || this.Template.Count(ch => ch == '1') < 2)
                    {
                        results.Add(new ValidationResult(
                            "Template should correspond to schedule type," +
                            " for sequance template expected more than one '1' character," +
                            "template should begins with '1'," +
                            " for example '1001000100'",
                            new List<string> { nameof(this.Template) }));
                    }

                    break;
            }

            return results;
        }
    }
}
