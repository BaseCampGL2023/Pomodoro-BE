// <copyright file="AuthService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Pomodoro.Api.Models;
using Pomodoro.Dal.Entities;
using Pomodoro.Services.Exceptions;
using Pomodoro.Services.Mail;
using Pomodoro.Services.Models.Mail;

namespace Pomodoro.Api.Services
{
    /// <summary>
    /// Create Jwt, perform registration and login operations.
    /// </summary>
    public class AuthService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthService> logger;
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly IHttpContextAccessor accessor;
        private readonly IEmailSender emailSender;
        private readonly LinkGenerator linkGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="configuration">Set of key/value application configuration properties <see cref="IConfiguration"/>.</param>
        /// <param name="logger">Logger <see cref="ILogger"/>.</param>
        /// <param name="userManager">API for managing user in persistence store <see cref="UserManager{TUser}"/>.</param>
        /// <param name="accessor">IHttpContextAccessor accessor.</param>
        /// <param name="emailSender">Email sender service.</param>
        /// <param name="linkGenerator">LinkGenerator instance.</param>
        public AuthService(
            IConfiguration configuration,
            ILogger<AuthService> logger,
            UserManager<AppIdentityUser> userManager,
            IHttpContextAccessor accessor,
            IEmailSender emailSender,
            LinkGenerator linkGenerator)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.userManager = userManager;
            this.accessor = accessor;
            this.emailSender = emailSender;
            this.linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Performs user registration in application.
        /// </summary>
        /// <param name="registrationRequest">Represent data for registration request <see cref="RegistrationRequestModel"/>.</param>
        /// <returns>Result of registration attempt <see cref="RegistrationResponseModel"/>.</returns>
        /// <exception cref="ArgumentNullException">Throws if registration request model is NULL.</exception>
        public async Task<RegistrationResponseModel> RegistrationAsync(RegistrationRequestModel registrationRequest)
        {
            if (registrationRequest is null)
            {
                this.logger.LogCritical("Model validation at the presentation level didn't work");
                throw new ArgumentNullException($"{nameof(registrationRequest)}", " argument is null");
            }

            var pomoIdentityUser = new AppIdentityUser()
            {
                Email = registrationRequest.Email,
                UserName = registrationRequest.Email,
                AppUser = new AppUser()
                {
                    Name = registrationRequest.UserName,
                    CreatedDt = DateTime.UtcNow,
                },
            };

            var result = await this.userManager.CreateAsync(pomoIdentityUser, registrationRequest.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                this.logger.LogWarning("Failed registration attempt.", errors);
                return new RegistrationResponseModel
                {
                    Errors = errors,
                };
            }

            var emailConfirmToken = await this.userManager.GenerateEmailConfirmationTokenAsync(pomoIdentityUser);
            var encodedConfirmToken = Encoding.UTF8.GetBytes(emailConfirmToken);
            var base64ConfirmToken = WebEncoders.Base64UrlEncode(encodedConfirmToken);

            // TODO: delete user if don't send email.
            var routeValues = new { userId = pomoIdentityUser.Id.ToString(), token = base64ConfirmToken };

            var url = this.linkGenerator.GetUriByName(
                this.accessor.HttpContext!,
                "ConfirmEmail",
                routeValues);

            await this.emailSender.SendEmailAsync(new Message(
                new string[] { pomoIdentityUser.Email },
                "Confirm email",
                $"<h1>Please confirm your email</h1><p><a href=\"{url}\">Click here</a></p>"));

            return new RegistrationResponseModel { Success = true };
        }

        /// <summary>
        /// Performs user authentication.
        /// </summary>
        /// <param name="loginRequest">Represent data for login request <see cref="LoginRequestModel"/>.</param>
        /// <returns>Result of login attempt <see cref="LoginResponseModel"/>.</returns>
        /// <exception cref="PomoBrokenModelException">Throws if login request model is NULL.</exception>
        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel loginRequest)
        {
            if (loginRequest is null)
            {
                this.logger.LogCritical("Model validation at the presentation level didn't work");
                throw new ArgumentNullException($"{nameof(loginRequest)}", " argument is null");
            }

            var user = await this.userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
            {
                this.logger.LogWarning("Authorization attempt with invalid name.");
                return new LoginResponseModel
                {
                    Message = "Invalid Email.",
                };
            }

            if (!user.EmailConfirmed)
            {
                this.logger.LogWarning("Authorization attempt with non-confirmed email.");
                return new LoginResponseModel
                {
                    Message = "Non confirm Email.",
                };
            }

            if (!await this.userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                this.logger.LogWarning("Authorization attempt with invalid password.");
                return new LoginResponseModel
                {
                    Message = "Invalid Password.",
                };
            }

            if (user.AppUser is null)
            {
                this.logger.LogCritical("Identity user doesn't contain AppUser property");
                throw new PomoBrokenModelException("AppUser is null");
            }

            var token = this.GetToken(user);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponseModel
            {
                Success = true,
                Token = jwt,
                UserName = user.AppUser.Name,
            };
        }

        /// <summary>
        /// Confirm user email.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="token">Confirmation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return false;
            }

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string confirmToken = Encoding.UTF8.GetString(decodedToken);

            var result = await this.userManager.ConfirmEmailAsync(user, confirmToken);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reset forgot password.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<bool> ForgetPasswordAsync(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return false;
            }

            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = EncodeToken(token);

            var routeValues = new { email = user.Email, token = encodedToken };

            var url = this.linkGenerator.GetUriByPage(
                this.accessor.HttpContext!,
                "/ResetPassword",
                null,
                routeValues);

            var messageBody = "<h1>Follow this instructions on this page to reset your password</h1>" +
                $"<p><a href=\"{url}\">Click here</a></p>";

            await this.emailSender.SendEmailAsync(
                new Message(
                    new string[] { email },
                    "Reset password",
                    messageBody));

            return true;
        }

        /// <summary>
        /// Reset user password.
        /// </summary>
        /// <param name="model">Reset password model <see cref="ResetPasswordViewModel"/>.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<AuthResponseModel> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await this.userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return new AuthResponseModel
                {
                    Success = false,
                    Message = "No user with those id.",
                };
            }

            var result = await this.userManager.ResetPasswordAsync(
                user,
                DecodeToken(model.Token),
                model.NewPassword);

            if (result.Succeeded)
            {
                return new AuthResponseModel
                {
                    Success = true,
                    Message = user.AppUser!.Name,
                };
            }

            return new AuthResponseModel { Success = false, Message = user.AppUser!.Name };
        }

        private static List<Claim> GetClaims(AppIdentityUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.AppUser!.Name),
                new Claim("userId", user.AppUser!.Id.ToString()),
                new Claim("signUpAt", user.AppUser!.CreatedDt.ToString()),
            };
            return claims;
        }

        private static string EncodeToken(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            return WebEncoders.Base64UrlEncode(bytes);
        }

        private static string DecodeToken(string token)
        {
            var bytes = WebEncoders.Base64UrlDecode(token);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Genererate JWT.
        /// </summary>
        /// <param name="user">Represent a user in identity system <see cref="IdentityUser{TKey}"/>.</param>
        /// <returns>JWT <see cref="JwtSecurityToken"/>.</returns>
        private JwtSecurityToken GetToken(AppIdentityUser user)
        {
            JwtSecurityToken token = new (
                issuer: this.configuration["JwtSettings:Issuer"],
                audience: this.configuration["JwtSettings:Audience"],
                claims: GetClaims(user),
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(
                    this.configuration["JwtSettings:ExpirationTimeMinutes"])),
                signingCredentials: this.GetSigningCredentials());

            this.logger.LogInformation("Generate token for user {userEmail}", user.Email);

            return token;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(this.configuration["JwtSettings:SecurityKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
    }
}