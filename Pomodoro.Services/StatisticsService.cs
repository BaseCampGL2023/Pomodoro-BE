using Pomodoro.Core.Enums;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Statistics;
using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Services
{
	public  class StatisticsService : IStatisticsService
	{
		public async Task<DailyStatistics> GetDailyStatisticsAsync(Guid userId, DateOnly day)
		{
			DailyStatistics dailyStatistics = new DailyStatistics();

			using (AppDbContext db = new AppDbContext())
			{
				IQueryable<Completed> tasks = db.CompletedTasks;
				
				tasks = tasks.Where(t => t.Id == userId);
				tasks = tasks.Where(t => t.ActualDate.Year == day.Year);
				tasks = tasks.Where(t => t.ActualDate.Month == day.Month);
				tasks = tasks.Where(t => t.ActualDate.Day == day.Day);


				for (int i = 0; i < 24; i++)
				{
					int hour = i;
					
					AnalyticsPerHour analytics = new AnalyticsPerHour();

					IQueryable<Completed> hourstatistic = tasks.Where(t => t.ActualDate.Hour == hour);

					analytics.Hour = hour;

					foreach (var h in hourstatistic)
					{
						analytics.TimeSpent += h.TimeSpent;
						analytics.PomodorosDone += h.PomodorosCount;
					}

					dailyStatistics.AnalyticsPerHours?.Add(analytics);
				}
			}
			return await Task.FromResult(dailyStatistics);

		}

		public async Task<MonthlyStatistics> GetMonthlyStatisticsAsync(Guid userId, int year, int month)
		{
			MonthlyStatistics monthlyStatistics = new MonthlyStatistics();

			monthlyStatistics.UserId = userId;

			using (AppDbContext db = new AppDbContext())
			{
				IQueryable<Completed> tasks = db.CompletedTasks;
				
				tasks = tasks.Where(t => t.Id == userId);
				tasks = tasks.Where(t => t.ActualDate.Year == year);
				tasks = tasks.Where(t => t.ActualDate.Month == month);

				monthlyStatistics.TasksCompleted = tasks.Count();

				foreach (var pomo in tasks)
				{
					monthlyStatistics.PomodorosDone += pomo.PomodorosCount;
				}
				foreach (var time in tasks)
				{
					monthlyStatistics.TimeSpent += time.TimeSpent;
				}
			}

			return await Task.FromResult(monthlyStatistics);

		}

		public async Task<AnnualStatistics> GetAnnualStatisticsAsync(Guid userId, int year)
		{
			AnnualStatistics annualStatistics = new AnnualStatistics();

			annualStatistics.UserId = userId;
			annualStatistics.Year = year;

			using (AppDbContext db = new AppDbContext())
			{
				IQueryable<Completed> monthsTasks = db.CompletedTasks;

				foreach (int m in Enum.GetValues(typeof(Month)))
				{
					monthsTasks = monthsTasks.Where(t => t.ActualDate.Year == annualStatistics.Year);
					monthsTasks = monthsTasks.Where(t => t.ActualDate.Month == m);

					AnalyticsPerMonth temp = new AnalyticsPerMonth();
					
					temp.Month = (Month)m;
					foreach (var d in monthsTasks)
					{
						temp.PomodorosDone += d.PomodorosCount;
					}
					annualStatistics.AnalyticsPerMonths?.Add(temp);
				}

			}

			return await Task.FromResult(annualStatistics);

		}
	}
}