// <copyright file="IEmailSender.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Services.Email.Models;

namespace Pomodoro.Services.Email
{
    /// <summary>
    /// Describes email service methods.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Performs sending email.
        /// </summary>
        /// <param name="message">Message <see cref="Message"/>.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task SendEnailAsync(Message message);
    }
}
