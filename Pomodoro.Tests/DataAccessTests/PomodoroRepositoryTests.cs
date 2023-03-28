// <copyright file="PomodoroRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Realizations;
using Pomodoro.Tests.EqualityComparers;

namespace Pomodoro.Tests.DataAccessTests
{
    /// <summary>
    /// Pomodoro repository test class.
    /// </summary>
    public class PomodoroRepositoryTests
    {
        /// <summary>
        /// Adds completed to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_AddsCompletedToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var pomodoroRepository = new PomodoroRepository(context);
            var pomodoro = new PomodoroEntity
            {
                TaskId = UnitTestHelper.Tasks[1].Id,
                ActualDate = new DateTime(2023, 1, 14),
                TimeSpent = 3000,
                TaskIsDone = true,
            };
            int expectedCount = UnitTestHelper.Pomodoros.Count + 1;

            // act
            await pomodoroRepository.AddAsync(pomodoro);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.Pomodoros.Count());
        }

        /// <summary>
        /// Doesn`t add completed to database because task doesn`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_ThrowsDbUpdateException_TaskDoesntExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var pomodoroRepository = new PomodoroRepository(context);
            var pomodoro = new PomodoroEntity
            {
                ActualDate = new DateTime(2023, 1, 14),
                TimeSpent = 3000,
                TaskIsDone = true,
            };

            // act
            var act = async () =>
            {
                await pomodoroRepository.AddAsync(pomodoro);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Adds completeds to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsCompletedsToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var pomodoroRepository = new PomodoroRepository(context);
            var pomodoros = new List<PomodoroEntity>
            {
                new PomodoroEntity
                {
                    TaskId = UnitTestHelper.Tasks[0].Id,
                    ActualDate = new DateTime(2023, 1, 14),
                    TimeSpent = 3000,
                    TaskIsDone = true,
                },
                new PomodoroEntity
                {
                    TaskId = UnitTestHelper.Tasks[1].Id,
                    ActualDate = new DateTime(2023, 1, 14),
                    TimeSpent = 3000,
                    TaskIsDone = true,
                },
            };
            int expectedCount = UnitTestHelper.Pomodoros.Count + pomodoros.Count;

            // act
            await pomodoroRepository.AddRangeAsync(pomodoros);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.Pomodoros.Count());
        }

        /// <summary>
        /// Doesn`t add completeds to database because tasks don`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_ThrowsDbUpdateException_TasksDontExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var pomodoroRepository = new PomodoroRepository(context);
            var pomodoros = new List<PomodoroEntity>
            {
                new PomodoroEntity
                {
                    ActualDate = new DateTime(2023, 1, 14),
                    TimeSpent = 3000,
                    TaskIsDone = true,
                },
                new PomodoroEntity
                {
                    ActualDate = new DateTime(2023, 1, 14),
                    TimeSpent = 3000,
                    TaskIsDone = true,
                },
            };

            // act
            var act = async () =>
            {
                await pomodoroRepository.AddRangeAsync(pomodoros);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Finds completeds.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task FindAsync_FindsCompleteds()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var pomodoroRepository = new PomodoroRepository(context);
            var expPomodoros = UnitTestHelper.Pomodoros.Where(c => c.TaskId == UnitTestHelper.Tasks[0].Id)
                                                             .ToList();

            // act
            var actPomodoros = await pomodoroRepository.FindAsync(x => x.TaskId == expPomodoros[0].TaskId);

            // assert
            Assert.Equal(expPomodoros, actPomodoros, new PomodoroComparer());
        }

        /// <summary>
        /// Returns all completeds.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_ReturnsAllCompleteds()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var pomodoroRepository = new PomodoroRepository(context);
            var expPomodoros = UnitTestHelper.Pomodoros;

            // act
            var actPomodoros = await pomodoroRepository.GetAllAsync();

            // assert
            Assert.Equal(expPomodoros, actPomodoros, new PomodoroComparer());
        }

        /// <summary>
        /// Gets completed by id.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_ReturnsCompleted()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var pomodoroRepository = new PomodoroRepository(context);
            var expPomodoro = UnitTestHelper.Pomodoros[0];

            // act
            var actPomodoro = await pomodoroRepository.GetByIdAsync(expPomodoro.Id);

            // assert
            Assert.Equal(expPomodoro, actPomodoro, new PomodoroComparer());
        }

        /// <summary>
        /// Remove completed from db.
        /// </summary>
        [Fact]
        public void Remove_RemovesCompleted()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var pomodoroRepository = new PomodoroRepository(context);
            var pomodoro = UnitTestHelper.Pomodoros[0];
            int expectedCount = UnitTestHelper.Pomodoros.Count - 1;

            // act
            pomodoroRepository.Remove(pomodoro);
            context.SaveChanges();

            // assert
            Assert.Equal(expectedCount, context.Pomodoros.Count());
        }

        /// <summary>
        /// Removes completeds from db.
        /// </summary>
        [Fact]
        public void RemoveRange_RemovesCompleteds()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var pomodoroRepository = new PomodoroRepository(context);
            var pomodoros = UnitTestHelper.Pomodoros;

            // act
            pomodoroRepository.RemoveRange(pomodoros);
            context.SaveChanges();

            // assert
            Assert.Empty(context.Pomodoros);
        }

        /// <summary>
        /// Updates completed.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task Update_UpdatesCompleted()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var pomodoroRepository = new PomodoroRepository(context);
            var expPomodoro = UnitTestHelper.Pomodoros[0];
            expPomodoro.TimeSpent = 5000;
            expPomodoro.TaskIsDone = false;

            // act
            pomodoroRepository.Update(expPomodoro);
            context.SaveChanges();
            var actPomodoro = await pomodoroRepository.GetByIdAsync(expPomodoro.Id);

            // assert
            Assert.Equal(expPomodoro, actPomodoro, new PomodoroComparer());
        }

        /// <summary>
        /// Doesn`t update completed because task doesn`t exist.
        /// </summary>
        [Fact]
        public void Update_ThrowsDbUpdateException_TaskDoesntExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var pomodoroRepository = new PomodoroRepository(context);
            var expPomodoro = UnitTestHelper.Pomodoros[0];
            expPomodoro.TaskId = Guid.Empty;
            expPomodoro.TimeSpent = 5000;
            expPomodoro.TaskIsDone = false;

            // act
            var act = () =>
            {
                pomodoroRepository.Update(expPomodoro);
                context.SaveChanges();
            };

            // assert
            Assert.Throws<DbUpdateException>(act);
        }
    }
}
