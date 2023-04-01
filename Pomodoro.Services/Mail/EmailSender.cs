// <copyright file="EmailSender.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using Pomodoro.Services.Exceptions;
using Pomodoro.Services.Mail.Options;
using Pomodoro.Services.Models.Mail;

namespace Pomodoro.Services.Mail
{
    /// <inheritdoc/>
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfig emailConfig;
        private readonly ILogger<EmailSender> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSender"/> class.
        /// </summary>
        /// <param name="emailConfig">Instance of email configuration <see cref="EmailConfig"/>.</param>
        /// <param name="logger">Instance of ILogger.</param>
        public EmailSender(EmailConfig emailConfig, ILogger<EmailSender> logger)
        {
            this.emailConfig = emailConfig;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task SendEmailAsync(Message message)
        {
            var email = this.CreateEmailMessage(message);

            await this.Send(email);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(this.emailConfig.From));
            email.To.AddRange(message.To);
            email.Subject = message.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = message.Content,
            };

            return email;
        }

        private async Task Send(MimeMessage email)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(
                    this.emailConfig.SmtpServer,
                    this.emailConfig.Port,
                    SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(
                    this.emailConfig.UserName,
                    this.emailConfig.Password);
                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred sending email");
                throw new PomoMailException("An error occurred sending email", ex);
            }
        }
    }
}
