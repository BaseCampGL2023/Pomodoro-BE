// <copyright file="EmailConfirmed.cshtml.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pomodoro.Api.Pages
{
    /// <summary>
    /// Email confirmed result page model.
    /// </summary>
#pragma warning disable SA1649 // File name should match first type name
    public class EmailConfirmedModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        /// <summary>
        /// Gets or sets greeting name.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether operation is successfull.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Standart request handler.
        /// </summary>
        public void OnGet()
        {
            // Binding performed by attributes.
        }
    }
}
