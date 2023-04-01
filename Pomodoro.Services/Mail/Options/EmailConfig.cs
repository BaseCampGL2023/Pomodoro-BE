// <copyright file="EmailConfig.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Services.Mail.Options
{
    /// <summary>
    /// Mailing configuration.
    /// </summary>
    public class EmailConfig
    {
        /// <summary>
        /// Gets or sets sender.
        /// </summary>
        public string From { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets SMTP server.
        /// </summary>
        public string SmtpServer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets port on server.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets sender name.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets SMTP password.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
