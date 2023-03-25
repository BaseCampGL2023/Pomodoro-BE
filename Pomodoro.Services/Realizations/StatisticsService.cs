﻿using Pomodoro.Core.Enums;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Statistics;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Services.Realizations
{
    public class StatisticsService:IStatisticsService
    {
    
        private readonly IPomodoroRepository pomodoroRepository;

        public StatisticsService(IPomodoroRepository pomodoroRepository)
        {
            this.pomodoroRepository = pomodoroRepository;
        }
        
        public async Task<DailyStatistics> GetDailyStatisticsAsync(Guid userId, DateOnly day)
        {
            DailyStatistics dailyStatistics = new DailyStatistics();

            dailyStatistics.UserId = userId;
            dailyStatistics.Day = day;
            
            var statistics = await pomodoroRepository.FindAsync(
                u=>u.Task != null &&
                   u.Task.UserId == userId &&
                   u.ActualDate.Year == day.Year &&
                   u.ActualDate.Month == day.Month && 
                   u.ActualDate.Day == day.Day
            );
            
            for (int i = 0; i < 24; i+=2) 
            {
                AnalyticsPerHour analytics = new AnalyticsPerHour();
                
                var hours = statistics.Where(t => t.ActualDate.Hour == i || (t.ActualDate.Hour > i && t.ActualDate.Hour < i+2));

                analytics.Hour = i;
                
                //float totalPomodorosCount = hours.Sum(c => c.PomodorosCount);

                //analytics.PomodorosDone = Convert.ToInt32(totalPomodorosCount);

                analytics.TimeSpent = hours.Sum(c=>c.TimeSpent);

                dailyStatistics.AnalyticsPerHours?.Add(analytics);

            }

            return dailyStatistics;

        }

        public async Task<MonthlyStatistics> GetMonthlyStatisticsAsync(Guid userId, int year, int month)
        {
            MonthlyStatistics monthlyStatistics = new MonthlyStatistics();
            
            monthlyStatistics.UserId = userId;
            monthlyStatistics.Year = year;
            monthlyStatistics.Month = (Month)month;

            var statistics = await pomodoroRepository.FindAsync(
                m=> m.Task != null &&
                    m.Task.UserId == userId && 
                    m.ActualDate.Year == year && 
                    m.ActualDate.Month == month );
            
            monthlyStatistics.TasksCompleted = statistics.Count();

            //float totalPomodorosCount = statistics.Sum(c=>c.PomodorosCount);

            //monthlyStatistics.PomodorosDone = Convert.ToInt32(totalPomodorosCount);

            monthlyStatistics.TimeSpent = statistics.Sum(c=>c.TimeSpent);

            return monthlyStatistics;
        }

        public async Task<AnnualStatistics> GetAnnualStatisticsAsync(Guid userId, int year)
        {
            AnnualStatistics annualStatistics = new AnnualStatistics();
            
            annualStatistics.UserId = userId;
            annualStatistics.Year = year;
            
            var statistics = await pomodoroRepository.FindAsync(
                a => a.Task != null &&
                     a.Task.UserId == userId && 
                     a.ActualDate.Year == year
            );
            
            
            foreach (int m in Enum.GetValues(typeof(Month)))
            {
                
                AnalyticsPerMonth temp = new AnalyticsPerMonth();

                var months = statistics.Where(c => c.ActualDate.Month == m);
                
                temp.Month = (Month)m;

                //float totalPomodorosCount = months.Sum(c=>c.PomodorosCount);

                //temp.PomodorosDone = Convert.ToInt32(totalPomodorosCount);

                annualStatistics.AnalyticsPerMonths?.Add(temp);
            }

            return annualStatistics;

        }
    }
}