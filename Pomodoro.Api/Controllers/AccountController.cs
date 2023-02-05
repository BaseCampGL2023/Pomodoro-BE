// <copyright file="AccountController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Utilities;
using Pomodoro.Api.ViewModels.Auth;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Api.Controllers
{
    /// <summary>
    /// Manage login and registration.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser<Guid>> userManager;
        private readonly IUserRepository userRepository;
        private readonly ILogger<AccountController> logger;
        private readonly JwtHandler jwtHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">API for managing identity user in persistence store <see cref="UserManager{TUser}"/>.</param>
        /// <param name="userRepository">API for CRUD operations with application user <see cref="IUserRepository"/>.</param>
        /// <param name="logger">Logger <see cref="ILogger"/>.</param>
        /// <param name="jwtHandler">Generate Jwt <see cref="JwtHandler"/>.</param>
        public AccountController(
            UserManager<IdentityUser<Guid>> userManager,
            IUserRepository userRepository,
            ILogger<AccountController> logger,
            JwtHandler jwtHandler)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.logger = logger;
            this.jwtHandler = jwtHandler;
        }

        /// <summary>
        /// Performs user registration in application.
        /// </summary>
        /// <param name="newcome">Represent data for registration request <see cref="RegistrationRequestViewModel"/>.</param>
        /// <returns>Result of registration attempt <see cref="RegistrationResponseViewModel"/>.</returns>
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationRequestViewModel newcome)
        {
            if (newcome == null || !this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var appUser = new User
            {
                Email = newcome.Email,
                Name = newcome.UserName,
            };
            var identityUser = new IdentityUser<Guid>
            {
                Email = newcome.Email,
                UserName = newcome.UserName,
            };

            var result = await this.userManager.CreateAsync(identityUser, newcome.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return this.BadRequest(new RegistrationResponseViewModel { Errors = errors });
            }

            await this.userRepository.AddAsync(appUser);

            return this.Ok(new RegistrationResponseViewModel() { Success = true });
        }
    }
}
