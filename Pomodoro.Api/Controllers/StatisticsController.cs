// <copyright file="StatisticsController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.ActionFilterAttributes;
using Pomodoro.Api.Controllers.Base;
using Pomodoro.Api.ViewModels.Statistics;
using Pomodoro.Core.Interfaces.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace Pomodoro.Api.Controllers
{
    /// <summary>
    /// API controller which provides daily, monthly, and annual statistics for the current user.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatisticsController : BaseController
    {
        private readonly IMapper mapper;
        private readonly IStatisticsService statisticsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsController"/> class.
        /// </summary>
        /// <param name="mapper">Interface for mapping 2 objects.</param>
        /// <param name="statisticsService">Service for obtaining statistics data.</param>
        public StatisticsController(
            IMapper mapper,
            IStatisticsService statisticsService)
        {
            this.mapper = mapper;
            this.statisticsService = statisticsService;
        }

        /// <summary>
        /// Gets daily statistics for the current user.
        /// </summary>
        /// <param name="day">The day for which statistics should be returned.</param>
        /// <returns>A <see cref="DailyStatisticsViewModel"/> object.</returns>
        [HttpGet("daily/{day}")]
        [ValidateDate]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Returns an object containing daily user statistics.")]
        [SwaggerResponse(400, "The model state is invalid or validation error occured.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(404, "No statistics found for the required day.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult<DailyStatisticsViewModel>> GetDailyStatistics(DateTime day)
        {
            var result = await this.statisticsService
                .GetDailyStatisticsAsync(this.UserId, day);

            if (result is null)
            {
                return this.NotFound("No statistics found for the required day.");
            }

            var resultViewModel = this.mapper.Map<DailyStatisticsViewModel>(result);
            return this.Ok(resultViewModel);
        }

        /// <summary>
        /// Gets monthly statistics for the current user.
        /// </summary>
        /// <param name="year">The year of the certain month for which statistics should be returned.</param>
        /// <param name="month">The month for which statistics should be returned.</param>
        /// <returns>A <see cref="MonthlyStatisticsViewModel"/> object.</returns>
        [HttpGet("monthly/{year}/{month}")]
        [ValidateYear]
        [ValidateMonth]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Returns an object containing monthly user statistics.")]
        [SwaggerResponse(400, "The model state is invalid or validation error occured.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(404, "No statistics found for the required month.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult<MonthlyStatisticsViewModel>> GetMonthlyStatistics(int year, int month)
        {
            var result = await this.statisticsService
                .GetMonthlyStatisticsAsync(this.UserId, year, month);

            if (result is null)
            {
                return this.NotFound("No statistics found for the required month.");
            }

            var resultViewModel = this.mapper.Map<MonthlyStatisticsViewModel>(result);
            return this.Ok(resultViewModel);
        }

        /// <summary>
        /// Gets annual statistics for the current user.
        /// </summary>
        /// <param name="year">The year for which statistics should be returned.</param>
        /// <returns>An <see cref="AnnualStatisticsViewModel"/> object.</returns>
        [HttpGet("annual/{year}")]
        [ValidateYear]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Returns an object containing annual user statistics.")]
        [SwaggerResponse(400, "The model state is invalid or validation error occured.")]
        [SwaggerResponse(401, "An unauthorized request cannot be processed.")]
        [SwaggerResponse(404, "No statistics found for the required year.")]
        [SwaggerResponse(500, "An unhandled exception occurred on the server while executing the request.")]
        public async Task<ActionResult<AnnualStatisticsViewModel>> GetAnnualStatistics(int year)
        {
            var result = await this.statisticsService
                .GetAnnualStatisticsAsync(this.UserId, year);

            if (result is null)
            {
                return this.NotFound("No statistics found for the required year.");
            }

            var resultViewModel = this.mapper.Map<AnnualStatisticsViewModel>(result);
            return this.Ok(resultViewModel);
        }
    }
}
