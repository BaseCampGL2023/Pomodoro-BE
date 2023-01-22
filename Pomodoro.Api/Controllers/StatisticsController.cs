// <copyright file="StatisticsController.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Api.ViewModels.Statistics;
using Pomodoro.Core.Interfaces.IServices;

namespace Pomodoro.Api.Controllers
{
    /// <summary>
    /// API controller which provides daily, monthly, and annual statistics for the current user.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IMapper mapper;

        private readonly IStatisticsService statisticsService;
        private readonly ISecurityContextService securityContextService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsController"/> class.
        /// </summary>
        /// <param name="mapper">Interface for mapping 2 objects.</param>
        /// <param name="statisticsService">Service for obtaining statistics data.</param>
        /// <param name="securityContextService">Service for obtaining data about the current user.</param>
        public StatisticsController(
            IMapper mapper,
            IStatisticsService statisticsService,
            ISecurityContextService securityContextService)
        {
            this.mapper = mapper;
            this.statisticsService = statisticsService;
            this.securityContextService = securityContextService;
        }

        /// <summary>
        /// Gets daily statistics for the current user.
        /// </summary>
        /// <param name="day">The day for which statistics should be returned.</param>
        /// <returns>A <see cref="DailyStatisticsViewModel"/> object.</returns>
        /// <response code="200">Returns an object containing daily user statistics.</response>
        [HttpGet("daily")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DailyStatisticsViewModel>> GetDailyStatistics([FromQuery] DateOnly day)
        {
            int userId = this.securityContextService.GetCurrentUserId();
            var result = await this.statisticsService.GetDailyStatisticsAsync(userId, day);
            var resultViewModel = this.mapper.Map<DailyStatisticsViewModel>(result);

            return this.Ok(resultViewModel);
        }

        /// <summary>
        /// Gets monthly statistics for the current user.
        /// </summary>
        /// <param name="year">The year of the certain month for which statistics should be returned.</param>
        /// <param name="month">The month for which statistics should be returned.</param>
        /// <returns>A <see cref="MonthlyStatisticsViewModel"/> object.</returns>
        /// <response code="200">Returns an object containing monthly user statistics.</response>
        [HttpGet("monthly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<MonthlyStatisticsViewModel>> GetMonthlyStatistics(int year, int month)
        {
            int userId = this.securityContextService.GetCurrentUserId();
            var result = await this.statisticsService.GetMonthlyStatisticsAsync(userId, year, month);
            var resultViewModel = this.mapper.Map<MonthlyStatisticsViewModel>(result);

            return this.Ok(resultViewModel);
        }

        /// <summary>
        /// Gets annual statistics for the current user.
        /// </summary>
        /// <param name="year">The year for which statistics should be returned.</param>
        /// <returns>An <see cref="AnnualStatisticsViewModel"/> object.</returns>
        /// <response code="200">Returns an object containing annual user statistics.</response>
        [HttpGet("annual")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AnnualStatisticsViewModel>> GetAnnualStatistics(int year)
        {
            int userId = this.securityContextService.GetCurrentUserId();
            var result = await this.statisticsService.GetAnnualStatisticsAsync(userId, year);
            var resultViewModel = this.mapper.Map<AnnualStatisticsViewModel>(result);

            return this.Ok(resultViewModel);
        }
    }
}
