// <copyright file="AuthService.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Pomodoro.Api.ViewModels.Auth;
using Pomodoro.Core.Exceptions;
using Pomodoro.DataAccess.Entities;

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
        private readonly SignInManager<PomoIdentityUser> signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="configuration">Set of key/value application configuration properties <see cref="IConfiguration"/>.</param>
        /// <param name="logger">Logger <see cref="ILogger"/>.</param>
        /// <param name="userManager">API for managing user in persistence store <see cref="UserManager{TUser}"/>.</param>
        /// <param name="signInManager">API for signing in user.</param>
        public AuthService(
            IConfiguration configuration,
            ILogger<AuthService> logger,
            UserManager<PomoIdentityUser> userManager,
            SignInManager<PomoIdentityUser> signInManager)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
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
                this.logger.LogCritical("Model validation at the presentation level didn't work");
                throw new ArgumentNullException($"{nameof(registrationRequest)}", " argument is null");
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
                this.logger.LogWarning("Failed registration attempt.", errors);
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
                this.logger.LogCritical("Model validation at the presentation level didn't work");
                throw new ArgumentNullException($"{nameof(loginRequest)}", " argument is null");
            }

            var user = await this.userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
            {
                this.logger.LogWarning("Authorization attempt with invalid name.");
                return new LoginResponseViewModel
                {
                    Message = "Invalid Email.",
                };
            }

            if (!await this.userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                this.logger.LogWarning("Authorization attempt with invalid password.");
                return new LoginResponseViewModel
                {
                    Message = "Invalid Password.",
                };
            }

            if (user.AppUser is null)
            {
                this.logger.LogCritical("Identity user doesn't contain AppUser property");
                throw new BrokenModelDataException("Identity user doesn't contain AppUser property");
            }

            var token = this.GetToken(user);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponseViewModel
            {
                Success = true,
                Token = jwt,
            };
        }

        /// <summary>
        /// Gets authentication properties for external provider.
        /// </summary>
        /// <param name="provider">Provider for external authentication.</param>
        /// <param name="actionUrl">URL of the callback action.</param>
        /// <param name="queryString">Query string that contains URL of the frontend resource that should be opened after successful login.</param>
        /// <returns><see cref="AuthenticationProperties"/> object used by external login service.</returns>
        /// <exception cref="ArgumentNullException">Throws if any of the arguments is NULL.</exception>
        public AuthenticationProperties GetExternalAuthProperties(
            string provider, string actionUrl, string queryString)
        {
            if (provider is null)
            {
                this.logger.LogCritical("Model validation at the presentation level didn't work");
                throw new ArgumentNullException(nameof(provider));
            }

            if (actionUrl is null)
            {
                this.logger.LogWarning($"{nameof(actionUrl)} argument is null.");
                throw new ArgumentNullException(nameof(actionUrl));
            }

            if (queryString is null)
            {
                this.logger.LogWarning($"{nameof(queryString)} argument is null.");
                throw new ArgumentNullException(nameof(queryString));
            }

            var properties = this.signInManager
                .ConfigureExternalAuthenticationProperties(
                    provider, $"{actionUrl}?{queryString}");

            properties.AllowRefresh = true;
            return properties;
        }

        /// <summary>
        /// Performs user authentication based on the data retrieved by external login service.
        /// </summary>
        /// <returns>Result of login attempt <see cref="LoginResponseViewModel"/>.</returns>
        /// <exception cref="BrokenModelDataException">Throws if identity user doesn't contain AppUser property.</exception>
        public async Task<LoginResponseViewModel> LoginViaExternalService()
        {
            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                this.logger.LogWarning("External login information is null.");
                return this.FailedLoginResponse("External login information is null.");
            }

            var signinResult = await this.signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, false);

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await this.userManager.FindByEmailAsync(email);

            if (signinResult.Succeeded)
            {
                if (user.AppUser is null)
                {
                    this.logger.LogCritical("Identity user doesn't contain AppUser property.");
                    throw new BrokenModelDataException("Identity user doesn't contain AppUser property.");
                }

                return this.SuccessfulLoginResponse(user);
            }

            if (email is null)
            {
                this.logger.LogWarning("Email retrieved by external login service is null.");
                return this.FailedLoginResponse("Email retrieved by external login service is null.");
            }

            if (user is null)
            {
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                user = await this.CreateUser(email, name);
            }

            await this.userManager.AddLoginAsync(user, info);
            await this.signInManager.SignInAsync(user, false);

            return this.SuccessfulLoginResponse(user);
        }

        /// <summary>
        /// Genererate JWT.
        /// </summary>
        /// <param name="user">Represent a user in identity system <see cref="IdentityUser{TKey}"/>.</param>
        /// <returns>JWT <see cref="JwtSecurityToken"/>.</returns>
        private JwtSecurityToken GetToken(PomoIdentityUser user)
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

        private LoginResponseViewModel FailedLoginResponse(string message)
        {
            return new LoginResponseViewModel
            {
                Message = message,
            };
        }

        private LoginResponseViewModel SuccessfulLoginResponse(PomoIdentityUser user)
        {
            var token = this.GetToken(user);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponseViewModel
            {
                Success = true,
                Token = jwt,
            };
        }

        private async Task<PomoIdentityUser> CreateUser(string email, string name)
        {
            var user = new PomoIdentityUser()
            {
                UserName = email,
                Email = email,
                AppUser = new AppUser()
                {
                    Name = name,
                    Email = email,
                },
            };
            await this.userManager.CreateAsync(user);
            return user;
        }
    }
}
