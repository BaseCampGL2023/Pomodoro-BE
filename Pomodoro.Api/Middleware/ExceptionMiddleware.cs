// <copyright file="ExceptionMiddleware.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Net;
using Pomodoro.Api.Contracts;

namespace Pomodoro.Api.Middleware
{
    /// <summary>
    /// Represents a class that globaly handles errors.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">Function that can process HTTP request.</param>
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Handles errors in case their occurence.
        /// </summary>
        /// <param name="context">Information about HTTP request.</param>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var error = new Error
                {
                    StatusCode = context.Response.StatusCode.ToString(),
                    Message = e.Message,
                };

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(error.ToString());
            }
        }
    }
}
