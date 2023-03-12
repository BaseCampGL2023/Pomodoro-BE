// <copyright file="AccountController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.Models;
using Pomodoro.Api.Services;
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
        private readonly AuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="authService">Create Jwt, perform registration and login operations <see cref="AuthService"/>.</param>
        public AccountController(AuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// Endpoint for user registration.
        /// </summary>
        /// <param name="newcome">Represent data for registration request <see cref="RegistrationRequestModel"/>.</param>
        /// <returns>Result of registration attempt <see cref="RegistrationResponseModel"/>.</returns>
        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Registration was succesfull")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<RegistrationResponseModel>> RegisterUser(
            [FromBody] RegistrationRequestModel newcome)
        {
            var result = await this.authService.RegistrationAsync(newcome);
            if (result.Success)
            {
                return this.Ok(result);
            }

            return this.BadRequest(result);
        }

        /// <summary>
        /// Endpoint for user authentication.
        /// </summary>
        /// <param name="loginRequest">Represent data for login request <see cref="LoginRequestModel"/>.</param>
        /// <returns>Result of login attempt <see cref="LoginResponseModel"/>.</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Registration was succesfull")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(401, "Invalid credentials")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult<LoginResponseModel>> Login(
            [FromBody] LoginRequestModel loginRequest)
        {
            var result = await this.authService.LoginAsync(loginRequest);

            if (result.Success)
            {
                return this.Ok(result);
            }

            return this.Unauthorized(result);
        }
    }
}