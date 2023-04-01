// <copyright file="IEmailSender.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.Services.Models.Mail;

namespace Pomodoro.Services.Mail
{
    /// <summary>
    /// Describes operations with email.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Performs sending email.
        /// </summary>
        /// <param name="message">Message <see cref="Message"/>.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task SendEmailAsync(Message message);
    }
}
