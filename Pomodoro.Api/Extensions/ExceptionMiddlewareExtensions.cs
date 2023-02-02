// <copyright file="ExceptionMiddlewareExtensions.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Api.Middleware;

namespace Pomodoro.Api.Extensions
{
    /// <summary>
    /// Static method extension class for the <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Adds to request pipline the <see cref="ExceptionMiddleware"/>.
        /// </summary>
        /// <param name="builder">Request pipline buider.</param>
        /// <returns><see cref="IApplicationBuilder"/> configured with <see cref="ExceptionMiddleware"/>.</returns>
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
