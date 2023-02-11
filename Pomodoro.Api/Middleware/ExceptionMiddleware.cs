// <copyright file="ExceptionMiddleware.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Net;
using Pomodoro.Api.ViewModels;

namespace Pomodoro.Api.Middleware
{
    /// <summary>
    /// Represents a class that globaly handles errors.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment environment;
        private readonly ILogger<ExceptionMiddleware> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next middleware in pipe.</param>
        /// <param name="environment">Object provides information about host environment.</param>
        /// <param name="logger">Logger <see cref="ILogger"/>.</param>
        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.environment = environment;
            this.logger = logger;
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
                await this.next(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var message = this.environment.IsDevelopment()
                    ? e.Message : "Internal error";
                var stackTrace = e?.StackTrace ?? string.Empty;

                this.logger.LogError(e, "Process failed with {stackTrace}", stackTrace);

                var error = new Error
                {
                    StatusCode = context.Response.StatusCode.ToString(),
                    Message = message,
                };

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(error.ToString());
            }
        }
    }
}
