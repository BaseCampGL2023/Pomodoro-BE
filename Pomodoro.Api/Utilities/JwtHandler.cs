// <copyright file="JwtHandler.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Api.Utilities
{
    /// <summary>
    /// Generate Jwt.
    /// </summary>
    public class JwtHandler
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser<Guid>> userManager;
        private readonly ILogger<JwtHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtHandler"/> class.
        /// </summary>
        /// <param name="configuration">Set of key/value application configuration properties <see cref="IConfiguration"/>.</param>
        /// <param name="userManager">API for managing user in persistence store <see cref="UserManager{TUser}"/>.</param>
        /// <param name="logger">Logger <see cref="ILogger"/>.</param>
        public JwtHandler(IConfiguration configuration, UserManager<IdentityUser<Guid>> userManager, ILogger<JwtHandler> logger)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.logger = logger;
        }

        /// <summary>
        /// Genererate JWT.
        /// </summary>
        /// <param name="user">Represent a user in identity system <see cref="IdentityUser{TKey}"/>.</param>
        /// <param name="appUser">Represent user in application <see cref="User"/>.</param>
        /// <returns>JWT <see cref="JwtSecurityToken"/>.</returns>
        public JwtSecurityToken GetToken(IdentityUser<Guid> user, User appUser)
        {
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: this.configuration["JwtSettings:Issuer"],
                audience: this.configuration["JwtSettings:Audience"],
                claims: this.GetClaims(user, appUser),
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(
                    this.configuration["JwtSettings:ExpirationTimeMinutes"])),
                signingCredentials: this.GetSigningCredentials());

            this.logger.LogInformation($"Generate JWT for user ${user.Email}");

            return token;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(this.configuration["JwtSettings:SecurityKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaims(IdentityUser<Guid> user, User appUser)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, appUser.Name),
                new Claim("userId", appUser.Id.ToString()),
            };
            return claims;
        }
    }
}
