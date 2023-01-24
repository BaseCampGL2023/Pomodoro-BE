// <copyright file="ValidateMonthAttribute.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pomodoro.Api.Extensions;

namespace Pomodoro.Api.ActionFilterAttributes
{
    /// <summary>
    /// Action filter attribute to be used on action methods of API controllers for the month parameter validation.
    /// </summary>
    public class ValidateMonthAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Checks that the month parameter is valid and according to the year parameter does not represent a month in the future (in UTC).<br/>
        /// Called before the action executes, after model binding is complete and the year parameter is checked.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext"/>.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            const string monthParam = "month";
            const string yearParam = "year";

            int month = context.ActionArguments.GetValueOrDefault<int>(monthParam);
            int year = context.ActionArguments.GetValueOrDefault<int>(yearParam);

            if (month != default && year != default)
            {
                const int min = 1;
                const int max = 12;

                if (month < min || month > max)
                {
                    context.Result = new BadRequestObjectResult(
                         $"The {monthParam} parameter must be in the range " +
                         $"from {min} to {max}.");
                }
                else if (year == DateTime.UtcNow.Year && month > DateTime.UtcNow.Month)
                {
                    context.Result = new BadRequestObjectResult(
                         $"The {monthParam} parameter according to the " +
                         $"{yearParam} parameter cannot represent a month " +
                         $"in the future (in UTC).");
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
