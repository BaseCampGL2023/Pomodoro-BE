// <copyright file="ValidateModelAttribute.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Pomodoro.Api.ActionFilterAttributes
{
    /// <summary>
    /// Action filter attribute to be used globally on API controllers for model state validation.
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Checks that the model state is valid.<br/>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext"/>.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }

            base.OnActionExecuting(context);
        }
    }
}
