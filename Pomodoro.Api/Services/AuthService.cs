// <copyright file="AuthService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Pomodoro.Api.ViewModels.Auth;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Api.Services
{
    /// <summary>
    /// Create Jwt, perform registration and login operations.
    /// </summary>
    public class AuthService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthService> logger;
        private readonly UserManager<PomoIdentityUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="configuration">Set of key/value application configuration properties <see cref="IConfiguration"/>.</param>
        /// <param name="logger">Logger <see cref="ILogger"/>.</param>
        /// <param name="userManager">API for managing user in persistence store <see cref="UserManager{TUser}"/>.</param>
        public AuthService(
            IConfiguration configuration,
            ILogger<AuthService> logger,
            UserManager<PomoIdentityUser> userManager)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.userManager = userManager;
        }

        /// <summary>
        /// Performs user registration in application.
        /// </summary>
        /// <param name="registrationRequest">Represent data for registration request <see cref="RegistrationRequestViewModel"/>.</param>
        /// <returns>Result of registration attempt <see cref="RegistrationResponseViewModel"/>.</returns>
        /// <exception cref="ArgumentNullException">Throws if registration request model is NULL.</exception>
        public async Task<RegistrationResponseViewModel> RegistrationAsync(RegistrationRequestViewModel registrationRequest)
        {
            if (registrationRequest is null)
            {
                throw new ArgumentNullException($"{nameof(registrationRequest)} is null");
            }

            var pomoIdentityUser = new PomoIdentityUser()
            {
                Email = registrationRequest.Email,
                UserName = registrationRequest.Email,
                AppUser = new AppUser()
                {
                    Name = registrationRequest.UserName,
                    Email = registrationRequest.Email,
                },
            };

            var result = await this.userManager.CreateAsync(pomoIdentityUser, registrationRequest.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return new RegistrationResponseViewModel
                {
                    Errors = errors,
                };
            }

            return new RegistrationResponseViewModel { Success = true };
        }

        /// <summary>
        /// Performs user authentication.
        /// </summary>
        /// <param name="loginRequest">Represent data for login request <see cref="LoginRequestViewModel"/>.</param>
        /// <returns>Result of login attempt <see cref="LoginResponseViewModel"/>.</returns>
        /// <exception cref="ArgumentNullException">Throws if login request model is NULL.</exception>
        public async Task<LoginResponseViewModel> LoginAsync(LoginRequestViewModel loginRequest)
        {
            if (loginRequest is null)
            {
                throw new ArgumentNullException($"{nameof(loginRequest)} is null");
            }

            var user = await this.userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
            {
                return new LoginResponseViewModel
                {
                    Message = "Invalid Email.",
                };
            }

            if (!await this.userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                return new LoginResponseViewModel
                {
                    Message = "Invalid Password.",
                };
            }

            /*if (user.AppUser is null)
            {
                throw new
            }*/

            var token = this.GetToken(user);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponseViewModel
            {
                Success = true,
                Token = jwt,
            };
        }

        /// <summary>
        /// Genererate JWT.
        /// </summary>
        /// <param name="user">Represent a user in identity system <see cref="IdentityUser{TKey}"/>.</param>
        /// <returns>JWT <see cref="JwtSecurityToken"/>.</returns>
        private JwtSecurityToken GetToken(PomoIdentityUser user)
        {
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: this.configuration["JwtSettings:Issuer"],
                audience: this.configuration["JwtSettings:Audience"],
                claims: this.GetClaims(user),
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(
                    this.configuration["JwtSettings:ExpirationTimeMinutes"])),
                signingCredentials: this.GetSigningCredentials());

            this.logger.LogInformation("Generate JWT for user {userEmail}", user.Email);

            return token;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(this.configuration["JwtSettings:SecurityKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaims(PomoIdentityUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.AppUser!.Name),
                new Claim("userId", user.AppUser!.Id.ToString()),
            };
            return claims;
        }
    }
}
