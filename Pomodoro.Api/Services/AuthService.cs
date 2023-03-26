// <copyright file="AuthService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Pomodoro.Api.Models;
using Pomodoro.Dal.Entities;
using Pomodoro.Services.Exceptions;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="configuration">Set of key/value application configuration properties <see cref="IConfiguration"/>.</param>
        /// <param name="logger">Logger <see cref="ILogger"/>.</param>
        /// <param name="userManager">API for managing user in persistence store <see cref="UserManager{TUser}"/>.</param>
        public AuthService(
            IConfiguration configuration,
            ILogger<AuthService> logger,
            UserManager<AppIdentityUser> userManager)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.userManager = userManager;
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
        /// Genererate JWT.
        /// </summary>
        /// <param name="user">Represent a user in identity system <see cref="IdentityUser{TKey}"/>.</param>
        /// <returns>JWT <see cref="JwtSecurityToken"/>.</returns>
        private JwtSecurityToken GetToken(AppIdentityUser user)
        {
            JwtSecurityToken token = new (
                issuer: this.configuration["JwtSettings:Issuer"],
                audience: this.configuration["JwtSettings:Audience"],
                claims: this.GetClaims(user),
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

        private List<Claim> GetClaims(AppIdentityUser user)
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
    }
}