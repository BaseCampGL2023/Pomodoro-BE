// <copyright file="ValidateDateAttribute.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Pomodoro.Api.Extensions;

namespace Pomodoro.Api.ActionFilterAttributes
{
    /// <summary>
    /// Action filter attribute to be used on action methods of API controllers for the <see cref="DateTime"/> parameter validation.
    /// </summary>
    public class ValidateDateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Checks that the <see cref="DateTime"/> parameter does not represent a date in the future (in UTC).<br/>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext"/>.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            const string param = "day";

            var date = context.ActionArguments.GetValueOrDefault<DateTime>(param);
            if (date != default && date > DateTime.UtcNow)
            {
                context.Result = new BadRequestObjectResult(
                    $"The {param} parameter cannot represent a date in the future (in UTC).");
            }

            base.OnActionExecuting(context);
        }
    }
}
