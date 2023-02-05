// <copyright file="AuthSetupExtensions.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Pomodoro.DataAccess.EF;

namespace Pomodoro.Api.Extensions
{
    /// <summary>
    /// Setup Identity store and password settings.
    /// </summary>
    public static class AuthSetupExtensions
    {
        /// <summary>
        /// Add Identity with password and store settings.
        /// </summary>
        /// <param name="services">Collection of service descriptors <see cref="IServiceCollection"/>.</param>
        /// <returns>Collection of service descriptors.</returns>
        public static IServiceCollection AddIdentityEF(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<AppDbContext>();

            return services;
        }

        /// <summary>
        /// Add JwtBearer authentication scheme with jwt token settings.
        /// </summary>
        /// <param name="services">Collection of service descriptors <see cref="IServiceCollection"/>.</param>
        /// <param name="config">Mutable configuration object <see cref="ConfigurationManager"/>.</param>
        /// <returns>Collection of service descriptors.</returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["JwtSettings:Issuer"],
                    ValidAudience = config["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        config["JwtSettings:SecurityKey"])),
                };
            });

            return services;
        }
    }
}
