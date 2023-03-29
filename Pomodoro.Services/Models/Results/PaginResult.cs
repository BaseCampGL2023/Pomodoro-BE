// <copyright file="PaginResult.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Dal.Entities.Base;
using Pomodoro.Services.Models.Interfaces;

namespace Pomodoro.Services.Models.Results
{
    /// <summary>
    /// Represents a request response object with pagination, sorting, and filtering.
    /// </summary>
    /// <typeparam name="T">Represent model object.</typeparam>
    public class PaginResult<T>
        where T : class
    {
        /// <summary>
        /// Gets or sets collection of return values.
        /// </summary>
        public ICollection<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// Gets a value indicating whether Data properties empty collection.
        /// </summary>
        public bool IsDataEmpty
        {
            get
            {
                return !this.Data.Any();
            }
        }

        /// <summary>
        /// Gets or sets current page number.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets total number of available objects.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets total pages available.
        /// </summary>
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(this.TotalCount / (double)this.PageSize);
            }
        }

        /// <summary>
        /// Gets a value indicating whether current page isn't first.
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return this.PageIndex > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether isn't last.
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return (this.PageIndex + 1) < this.TotalPages;
            }
        }

        /// <summary>
        /// Gets or sets sorting Column name (or null if none set).
        /// </summary>
        public string? SortColumn { get; set; }

        /// <summary>
        /// Gets or sets sorting order ("ASC", "DESC" or null if none set).
        /// </summary>
        public string? SortOrder { get; set; }

        /// <summary>
        /// Gets or sets filter column name (or null if none set).
        /// </summary>
        public string? FilterColumn { get; set; }

        /// <summary>
        /// Gets or sets filter Query string
        /// (to be used within the given FilterColumn).
        /// </summary>
        public string? FilterQuery { get; set; }
    }
}
