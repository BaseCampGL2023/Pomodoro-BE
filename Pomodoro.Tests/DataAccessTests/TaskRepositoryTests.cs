// <copyright file="TaskRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Realizations;
using Pomodoro.Tests.EqualityComparers;

namespace Pomodoro.Tests.DataAccessTests
{
    /// <summary>
    /// Task repository test class.
    /// </summary>
    public class TaskRepositoryTests
    {
        /// <summary>
        /// Adds task to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_AddsTaskToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var taskRepository = new TaskRepository(context);
            var task = new TaskEntity
            {
                Id = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                UserId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                FrequencyId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                Title = "Reading the book",
                InitialDate = new DateTime(2023, 1, 11),
                AllocatedTime = 4200,
            };
            int expectedCount = context.Tasks.Count() + 1;

            // act
            await taskRepository.AddAsync(task);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.Tasks.Count());
        }

        /// <summary>
        /// Adds tasks to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsTasksToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var taskRepository = new TaskRepository(context);
            var tasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    Id = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    UserId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Title = "Reading the book",
                    InitialDate = new DateTime(2023, 1, 11),
                    AllocatedTime = 4200,
                },
                new TaskEntity
                {
                    Id = new Guid(4, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    UserId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Title = "Do exercise",
                    InitialDate = new DateTime(2023, 1, 12),
                    AllocatedTime = 3000,
                },
            };
            int expectedCount = context.Tasks.Count() + tasks.Count;

            // act
            await taskRepository.AddRangeAsync(tasks);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.Tasks.Count());
        }

        /// <summary>
        /// Finds tasks.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task FindAsync_FindsTasks()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var taskRepository = new TaskRepository(context);
            var expTasks = context.Tasks.ToList();

            // act
            var actTasks = await taskRepository.FindAsync(x => x.UserId == expTasks[0].UserId);

            // assert
            Assert.Equal(expTasks, actTasks, new TaskComparer());
        }

        /// <summary>
        /// Returns all tasks.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_ReturnsAllTasks()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var taskRepository = new TaskRepository(context);
            var expTasks = context.Tasks.ToList();

            // act
            var actTasks = await taskRepository.GetAllAsync();

            // assert
            Assert.Equal(expTasks, actTasks, new TaskComparer());
        }

        /// <summary>
        /// Gets task by id.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_ReturnsTask()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var taskRepository = new TaskRepository(context);
            var expTask = context.Tasks.First();

            // act
            var actTask = await taskRepository.GetByIdAsync(expTask.Id);

            // assert
            Assert.Equal(expTask, actTask, new TaskComparer());
        }

        /// <summary>
        /// Remove task from db.
        /// </summary>
        [Fact]
        public void Remove_RemovesTask()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var taskRepository = new TaskRepository(context);
            var task = context.Tasks.First();
            int expectedCount = context.Tasks.Count() - 1;

            // act
            taskRepository.Remove(task);
            context.SaveChanges();

            // assert
            Assert.Equal(expectedCount, context.Tasks.Count());
        }

        /// <summary>
        /// Removes tasks from db.
        /// </summary>
        [Fact]
        public void RemoveRange_RemovesTasks()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var taskRepository = new TaskRepository(context);
            var tasks = context.Tasks.ToList();

            // act
            taskRepository.RemoveRange(tasks);
            context.SaveChanges();

            // assert
            Assert.Empty(context.Tasks);
        }

        /// <summary>
        /// Updates task.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task Update_UpdatesTask()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var taskRepository = new TaskRepository(context);
            var expTask = new TaskEntity
            {
                Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                UserId = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                FrequencyId = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                Title = "New Cleaning",
                InitialDate = new DateTime(2023, 1, 10),
                AllocatedTime = 3600,
            };

            // act
            taskRepository.Update(expTask);
            context.SaveChanges();
            var actTask = await taskRepository.GetByIdAsync(expTask.Id);

            // assert
            Assert.Equal(expTask, actTask, new TaskComparer());
        }
    }
}
