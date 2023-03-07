// <copyright file="AuthSetupExtensions.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Api.Extensions
{
    /// <summary>
    /// Setup authentication middleware.
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
            services.AddIdentity<PomoIdentityUser, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager();

            return services;
        }

        /// <summary>
        /// Add JwtBearer authentication scheme with jwt token settings.
        /// </summary>
        /// <param name="services">Collection of service descriptors <see cref="IServiceCollection"/>.</param>
        /// <param name="config">Mutable configuration object <see cref="ConfigurationManager"/>.</param>
        /// <returns>A <see cref="AuthenticationBuilder"/> that can be used to further configure authentication.</returns>
        public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services, ConfigurationManager config)
        {
            return services.AddAuthentication(options =>
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
        }

        /// <summary>
        /// Add Google authentication scheme with configuration settings.
        /// </summary>
        /// <param name="authenticationBuilder"><see cref="AuthenticationBuilder"/> to configure authentication.</param>
        /// <param name="config">Mutable configuration object <see cref="ConfigurationManager"/>.</param>
        /// <returns>A <see cref="AuthenticationBuilder"/> that can be used to further configure authentication.</returns>
        public static AuthenticationBuilder AddGoogleAuthentication(
            this AuthenticationBuilder authenticationBuilder, ConfigurationManager config)
        {
            return authenticationBuilder.AddGoogle(options =>
            {
                options.ClientId = config["GoogleSecrets:ClientId"];
                options.ClientSecret = config["GoogleSecrets:ClientSecret"];
                options.SignInScheme = IdentityConstants.ExternalScheme;

                options.Scope.Add("email");
            });
        }

        /// <summary>
        /// Configure Cookie Policy needed for external authentication.
        /// </summary>
        /// <param name="services">Collection of service descriptors <see cref="IServiceCollection"/>.</param>
        /// <returns>Collection of service descriptors.</returns>
        public static IServiceCollection AddCookiesForExternalAuth(this IServiceCollection services)
        {
            return services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            });
        }
    }
}
