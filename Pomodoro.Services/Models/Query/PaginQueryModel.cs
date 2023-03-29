// <copyright file="PaginQueryModel.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Services.Models.Query
{
    /// <summary>
    /// Incapsulate query options for pagination, sorting, filtering.
    /// </summary>
    public class PaginQueryModel
    {
        /// <summary>
        /// Gets or sets requested page number.
        /// </summary>
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// Gets or sets requested page size.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Gets or sets column by which to sort.
        /// </summary>
        public string? SortColumn { get; set; } = null;

        /// <summary>
        /// Gets or sets sorting direction ["ASC" | "DESC"].
        /// </summary>
        public string? SortOrder { get; set; } = null;

        /// <summary>
        /// Gets or sets column by which values will be filtered.
        /// </summary>
        public string? FilterColumn { get; set; } = null;

        /// <summary>
        /// Gets or sets filter query.
        /// </summary>
        public string? FilterQuery { get; set; } = null;
    }
}
