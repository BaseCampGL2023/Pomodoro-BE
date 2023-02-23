// <copyright file="CompletedRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
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
    /// Compled repository test class.
    /// </summary>
    public class CompletedRepositoryTests
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
            var completedRepository = new CompletedRepository(context);
            var completed = new Completed
            {
                TaskId = UnitTestHelper.Tasks[1].Id,
                ActualDate = new DateTime(2023, 1, 14),
                TimeSpent = 3000,
                PomodorosCount = 2,
                IsDone = true,
            };
            int expectedCount = UnitTestHelper.CompletedTasks.Count + 1;

            // act
            await completedRepository.AddAsync(completed);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.CompletedTasks.Count());
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
            var completedRepository = new CompletedRepository(context);
            var completed = new Completed
            {
                ActualDate = new DateTime(2023, 1, 14),
                TimeSpent = 3000,
                PomodorosCount = 2,
                IsDone = true,
            };

            // act
            var act = async () =>
            {
                await completedRepository.AddAsync(completed);
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
            var completedRepository = new CompletedRepository(context);
            var completeds = new List<Completed>
            {
                new Completed
                {
                    TaskId = UnitTestHelper.Tasks[0].Id,
                    ActualDate = new DateTime(2023, 1, 14),
                    TimeSpent = 3000,
                    PomodorosCount = 2,
                    IsDone = true,
                },
                new Completed
                {
                    TaskId = UnitTestHelper.Tasks[1].Id,
                    ActualDate = new DateTime(2023, 1, 14),
                    TimeSpent = 3000,
                    PomodorosCount = 2,
                    IsDone = true,
                },
            };
            int expectedCount = UnitTestHelper.CompletedTasks.Count + completeds.Count;

            // act
            await completedRepository.AddRangeAsync(completeds);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.CompletedTasks.Count());
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
            var completedRepository = new CompletedRepository(context);
            var completeds = new List<Completed>
            {
                new Completed
                {
                    ActualDate = new DateTime(2023, 1, 14),
                    TimeSpent = 3000,
                    PomodorosCount = 2,
                    IsDone = true,
                },
                new Completed
                {
                    ActualDate = new DateTime(2023, 1, 14),
                    TimeSpent = 3000,
                    PomodorosCount = 2,
                    IsDone = true,
                },
            };

            // act
            var act = async () =>
            {
                await completedRepository.AddRangeAsync(completeds);
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
            var completedRepository = new CompletedRepository(context);
            var expCompleteds = UnitTestHelper.CompletedTasks.Where(c => c.TaskId == UnitTestHelper.Tasks[0].Id)
                                                             .ToList();

            // act
            var actCompleteds = await completedRepository.FindAsync(x => x.TaskId == expCompleteds[0].TaskId);

            // assert
            Assert.Equal(expCompleteds, actCompleteds, new CompletedComparer());
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
            var completedRepository = new CompletedRepository(context);
            var expCompleteds = UnitTestHelper.CompletedTasks;

            // act
            var actCompleteds = await completedRepository.GetAllAsync();

            // assert
            Assert.Equal(expCompleteds, actCompleteds, new CompletedComparer());
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
            var completedRepository = new CompletedRepository(context);
            var expCompleted = UnitTestHelper.CompletedTasks[0];

            // act
            var actCompleted = await completedRepository.GetByIdAsync(expCompleted.Id);

            // assert
            Assert.Equal(expCompleted, actCompleted, new CompletedComparer());
        }

        /// <summary>
        /// Remove completed from db.
        /// </summary>
        [Fact]
        public void Remove_RemovesCompleted()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var completedRepository = new CompletedRepository(context);
            var completed = UnitTestHelper.CompletedTasks[0];
            int expectedCount = UnitTestHelper.CompletedTasks.Count - 1;

            // act
            completedRepository.Remove(completed);
            context.SaveChanges();

            // assert
            Assert.Equal(expectedCount, context.CompletedTasks.Count());
        }

        /// <summary>
        /// Removes completeds from db.
        /// </summary>
        [Fact]
        public void RemoveRange_RemovesCompleteds()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var completedRepository = new CompletedRepository(context);
            var completeds = UnitTestHelper.CompletedTasks;

            // act
            completedRepository.RemoveRange(completeds);
            context.SaveChanges();

            // assert
            Assert.Empty(context.CompletedTasks);
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
            var completedRepository = new CompletedRepository(context);
            var expCompleted = UnitTestHelper.CompletedTasks[0];
            expCompleted.TimeSpent = 5000;
            expCompleted.PomodorosCount = 3;
            expCompleted.IsDone = false;

            // act
            completedRepository.Update(expCompleted);
            context.SaveChanges();
            var actCompleted = await completedRepository.GetByIdAsync(expCompleted.Id);

            // assert
            Assert.Equal(expCompleted, actCompleted, new CompletedComparer());
        }

        /// <summary>
        /// Doesn`t update completed because task doesn`t exist.
        /// </summary>
        [Fact]
        public void Update_ThrowsDbUpdateException_TaskDoesntExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var completedRepository = new CompletedRepository(context);
            var expCompleted = UnitTestHelper.CompletedTasks[0];
            expCompleted.TaskId = Guid.Empty;
            expCompleted.TimeSpent = 5000;
            expCompleted.PomodorosCount = 3;
            expCompleted.IsDone = false;

            // act
            var act = () =>
            {
                completedRepository.Update(expCompleted);
                context.SaveChanges();
            };

            // assert
            Assert.Throws<DbUpdateException>(act);
        }
    }
}
