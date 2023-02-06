// <copyright file="AccountController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Utilities;
using Pomodoro.Api.ViewModels.Auth;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

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
        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Registration was succesfull")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<RegistrationResponseViewModel>> RegisterUser(
            [FromBody] RegistrationRequestViewModel newcome)
        {
            if (newcome == null || !this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var identityUser = new IdentityUser<Guid>
            {
                Email = newcome.Email,
                UserName = newcome.Email,
            };

            var result = await this.userManager.CreateAsync(identityUser, newcome.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return this.BadRequest(new RegistrationResponseViewModel { Errors = errors });
            }

            var appUser = new User
            {
                Email = newcome.Email,
                Name = newcome.UserName,
                IdentityUserId = identityUser.Id,
            };

            await this.userRepository.AddAsync(appUser);
            await this.userRepository.SaveChangesAsync();

            return this.Ok(new RegistrationResponseViewModel() { Success = true });
        }

        /// <summary>
        /// Performs user authentication.
        /// </summary>
        /// <param name="loginRequest">Represent data for login request <see cref="LoginRequestViewModel"/>.</param>
        /// <returns>Result of login attempt <see cref="LoginResponseViewModel"/>.</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Registration was succesfull")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(401, "Invalid credentials")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<LoginResponseViewModel>> Login(
            [FromBody] LoginRequestViewModel loginRequest)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = await this.userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
            {
                return this.Unauthorized(new LoginResponseViewModel
                {
                    Message = "Invalid Email.",
                });
            }

            if (!await this.userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                return this.Unauthorized(new LoginResponseViewModel()
                {
                    Message = "Invalid Password.",
                });
            }

            var appUser = (await this.userRepository.FindAsync(u => u.Email == loginRequest.Email)).First();

            var token = this.jwtHandler.GetToken(user, appUser);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return this.Ok(new LoginResponseViewModel()
            {
                Success = true,
                Message = "Login successful",
                Token = jwt,
            });
        }
    }
}
