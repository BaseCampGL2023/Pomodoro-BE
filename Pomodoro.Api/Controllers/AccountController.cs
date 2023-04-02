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

        /// <summary>
        /// Validate user email.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <param name="token">Emal validation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("ConfirmEmail/{userId}/{token}", Name = "ConfirmEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Registration was succesfull")]
        [SwaggerResponse(302, "Html page with email confirmation result")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult> ConfirmEmail(
            [FromRoute] string userId,
            [FromRoute]string token)
        {
            if (string.IsNullOrWhiteSpace(userId)
                || string.IsNullOrWhiteSpace(token))
            {
                return this.BadRequest();
            }

            var result = await this.authService.ConfirmEmailAsync(userId, token);

            if (result.Success)
            {
                var url = this.Url.PageLink(
                    "/EmailConfirmed",
                    null,
                    new { Name = result.Message, IsSuccess = true });
                if (url is null)
                {
                    return this.Ok("Email confirmed");
                }

                return this.Redirect(url);
            }
            else
            {
                if (result.InvalidRequest)
                {
                    return this.BadRequest(result.Message);
                }

                var url = this.Url.PageLink(
                    "/EmailConfirmed",
                    null,
                    new { Name = result.Message, IsSuccess = false });
                if (url is null)
                {
                    return this.BadRequest(result.Message);
                }

                return this.Redirect(url);
            }
        }

        /// <summary>
        /// Request reseting forgot password.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("ForgetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Registration was succesfull")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult> ForgetPassword([FromBody]string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return this.BadRequest();
            }

            var result = await this.authService.ForgetPasswordAsync(email);
            if (result.Success)
            {
                return this.Ok(result.Message);
            }

            return this.BadRequest(result.Message);
        }

        /// <summary>
        /// Reset user password.
        /// </summary>
        /// <param name="model">Reset password view model <see cref="ResetPasswordViewModel"/>.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("ResetPassword", Name = "ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Registration was succesfull")]
        [SwaggerResponse(302, "Html page with result of password reset")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<ActionResult> ResetPassword([FromForm] ResetPasswordViewModel model)
        {
            var result = await this.authService.ResetPasswordAsync(model);
            if (result.Success)
            {
                var url = this.Url.PageLink(
                    "/ResetPasswordResult",
                    null,
                    new { Name = result.Message, IsSuccess = true });
                if (url is null)
                {
                    return this.Ok("Password reseted successfully.");
                }

                return this.Redirect(url);
            }
            else
            {
                if (result.InvalidRequest)
                {
                    return this.BadRequest(result.Message);
                }

                var url = this.Url.PageLink(
                    "/ResetPasswordResult",
                    null,
                    new { Name = result.Message, IsSuccess = false });
                if (url is null)
                {
                    return this.BadRequest(result.Message);
                }

                return this.Redirect(url);
            }
        }
    }
}