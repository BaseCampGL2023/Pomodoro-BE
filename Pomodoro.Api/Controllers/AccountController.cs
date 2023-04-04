// <copyright file="AccountController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pomodoro.Api.Services;
using Pomodoro.Api.ViewModels.Auth;
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
        private const string ReturnUrlQueryParam = "returnUrl";
        private const string ExternalLoginResponseKey = "ExternalLoginResponse";
        private const string JwtSettingsAudienceKey = "JwtSettings:Audience";

        private readonly AuthService authService;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="authService">Create Jwt, perform registration and login operations <see cref="AuthService"/>.</param>
        /// <param name="configuration">Provides access to configuration settings.</param>
        public AccountController(AuthService authService, IConfiguration configuration)
        {
            this.authService = authService;
            this.configuration = configuration;
        }

        /// <summary>
        /// Endpoint for user registration.
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
            if (loginRequest is null || !this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this.authService.LoginAsync(loginRequest);

            if (result.Success)
            {
                return this.Ok(result);
            }

            return this.Unauthorized(result);
        }

        /// <summary>
        /// Endpoint for calling an external login service.
        /// </summary>
        /// <param name="provider">Provider for external authentication.</param>
        /// <param name="returnUrl">URL of the frontend resource that should be opened after successful login.</param>
        /// <returns>A <see cref="ChallengeResult"/> with the specified authentication scheme and properties.</returns>
        [HttpGet("external-login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "External login service was successfully called.")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(500, "Something went wrong")]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var actionUrl = this.Url.Action(nameof(this.ExternalLoginCallback));
            var queryString = ExternalLoginQueryString(returnUrl);

            var properties = this.authService.GetExternalAuthProperties(
                provider, actionUrl!, queryString);

            return this.Challenge(properties, provider);
        }

        /// <summary>
        /// Callback endpoint to complete an external login.
        /// </summary>
        /// <param name="returnUrl">URL of the frontend resource that should be opened after successful login.</param>
        /// <returns>A <see cref="RedirectResult"/> object that redirects to the specified <paramref name="returnUrl"/>.</returns>
        [HttpGet("external-login-callback")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(302, "Authentication was successful.")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var result = await this.authService.LoginViaExternalService();
            var options = new CookieOptions()
            {
                Expires = DateTime.UtcNow.AddMinutes(5),
            };

            this.Response.Cookies.Append(
                ExternalLoginResponseKey,
                JsonConvert.SerializeObject(result, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy(),
                    },
                    Formatting = Formatting.Indented,
                }), options);

            return this.Redirect($"{this.configuration[JwtSettingsAudienceKey]}?" +
                ExternalLoginQueryString(returnUrl));
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
        [SwaggerResponse(200, "Confirmation was succesfull")]
        [SwaggerResponse(302, "Html page with email confirmation result")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<IActionResult> ConfirmEmail(
            [FromRoute] string userId,
            [FromRoute] string token)
        {
            if (string.IsNullOrWhiteSpace(userId)
                || string.IsNullOrWhiteSpace(token))
            {
                return this.BadRequest();
            }

            var result = await this.authService.ConfirmEmailAsync(userId, token);
            if (result.InvalidRequest)
            {
                return this.BadRequest(result.Message);
            }

            var url = this.Url.PageLink(
                "/EmailConfirmed",
                null,
                new { Name = result.Message, IsSuccess = result.Success });

            if (result.Success)
            {
                return url is null
                    ? this.Ok("Email confirmed")
                    : this.Redirect(url);
            }

            return url is null
                ? this.BadRequest(result.Message
                    + ", something went wrong, try again later")
                : this.Redirect(url);
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
        [SwaggerResponse(200, "Request successful, check mailbox")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
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
        [SwaggerResponse(200, "Password reseted succesfully")]
        [SwaggerResponse(302, "Html page with result of password reset")]
        [SwaggerResponse(400, "Invalid data")]
        [SwaggerResponse(500, "Something went wrong")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordViewModel model)
        {
            var result = await this.authService.ResetPasswordAsync(model);
            if (result.InvalidRequest)
            {
                return this.BadRequest(result.Message);
            }

            var url = this.Url.PageLink(
                "/ResetPasswordResult",
                null,
                new { Name = result.Message, IsSuccess = result.Success });

            if (result.Success)
            {
                return url is null
                    ? this.Ok("Password reseted")
                    : this.Redirect(url);
            }

            return url is null
                ? this.BadRequest(result.Message
                    + ", something went wrong")
                : this.Redirect(url);
        }

        private static string ExternalLoginQueryString(string returnUrl)
         => $"{ReturnUrlQueryParam}={returnUrl}";
    }
}