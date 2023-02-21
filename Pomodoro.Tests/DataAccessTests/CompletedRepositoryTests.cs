// <copyright file="CompletedRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

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
                Id = new Guid(6, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                TaskId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                ActualDate = new DateTime(2023, 1, 14),
                TimeSpent = 3000,
                PomodorosCount = 2,
                IsDone = true,
            };
            int expectedCount = context.CompletedTasks.Count() + 1;

            // act
            await completedRepository.AddAsync(completed);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.CompletedTasks.Count());
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
                    Id = new Guid(6, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 14),
                    TimeSpent = 3000,
                    PomodorosCount = 2,
                    IsDone = true,
                },
                new Completed
                {
                    Id = new Guid(7, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 14),
                    TimeSpent = 3000,
                    PomodorosCount = 2,
                    IsDone = true,
                },
            };
            int expectedCount = context.CompletedTasks.Count() + completeds.Count;

            // act
            await completedRepository.AddRangeAsync(completeds);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.CompletedTasks.Count());
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
            var expCompleteds = new List<Completed>
            {
                new Completed
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 10),
                    TimeSpent = 3000,
                    PomodorosCount = 2,
                    IsDone = true,
                },
                new Completed
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 11),
                    TimeSpent = 4500,
                    PomodorosCount = 3,
                    IsDone = true,
                },
                new Completed
                {
                    Id = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    TaskId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    ActualDate = new DateTime(2023, 1, 12),
                    TimeSpent = 3000,
                    PomodorosCount = 2,
                    IsDone = true,
                },
            };

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
            var expCompleteds = context.CompletedTasks.ToList();

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
            var expCompleted = context.CompletedTasks.First();

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
            var completed = context.CompletedTasks.First();
            int expectedCount = context.CompletedTasks.Count() - 1;

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
            var completeds = context.CompletedTasks.ToList();

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
            var expCompleted = new Completed
            {
                Id = new Guid(5, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                TaskId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                ActualDate = new DateTime(2023, 1, 13),
                TimeSpent = 3000,
                PomodorosCount = 2,
                IsDone = true,
            };

            // act
            completedRepository.Update(expCompleted);
            context.SaveChanges();
            var actCompleted = await completedRepository.GetByIdAsync(expCompleted.Id);

            // assert
            Assert.Equal(expCompleted, actCompleted, new CompletedComparer());
        }
    }
}
