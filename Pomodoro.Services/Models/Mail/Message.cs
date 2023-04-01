// <copyright file="Message.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using MimeKit;

namespace Pomodoro.Services.Models.Mail
{
    /// <summary>
    /// Represent Email message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="to">List of recipients.</param>
        /// <param name="subject">Message subject.</param>
        /// <param name="content">Message content.</param>
        public Message(IEnumerable<string> to, string subject, string content)
        {
            this.To = new List<MailboxAddress>();

            this.To.AddRange(to.Select(s => MailboxAddress.Parse(s)));
            this.Subject = subject;
            this.Content = content;
        }

        /// <summary>
        /// Gets or sets list of recipients.
        /// </summary>
        public List<MailboxAddress> To { get; set; }

        /// <summary>
        /// Gets or sets message subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets message content.
        /// </summary>
        public string Content { get; set; }
    }
}
