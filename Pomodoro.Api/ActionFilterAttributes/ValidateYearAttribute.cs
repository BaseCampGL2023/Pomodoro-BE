// <copyright file="ValidateYearAttribute.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pomodoro.Api.Extensions;

namespace Pomodoro.Api.ActionFilterAttributes
{
    /// <summary>
    /// Action filter attribute to be used on action methods of API controllers for the year parameter validation.
    /// </summary>
    public class ValidateYearAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Checks that the year parameter is valid and does not represent a year in the future (in UTC).<br/>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext"/>.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            const string param = "year";

            int year = context.ActionArguments.GetValueOrDefault<int>(param);
            if (year != default)
            {
                int min = DateOnly.MinValue.Year;
                int max = DateOnly.MaxValue.Year;

                if (year < min || year > max)
                {
                    context.Result = new BadRequestObjectResult(
                         $"The {param} parameter must be in the range from {min} to {max}.");
                }
                else if (year > DateTime.UtcNow.Year)
                {
                    context.Result = new BadRequestObjectResult(
                         $"The {param} parameter cannot represent a year in the future (in UTC).");
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
