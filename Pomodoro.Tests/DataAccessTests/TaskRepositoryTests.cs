// <copyright file="TaskRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var task = new TaskEntity
            {
                UserId = UnitTestHelper.AppUsers[0].Id,
                FrequencyId = UnitTestHelper.Frequencies[0].Id,
                Title = "Reading the book",
                InitialDate = new DateTime(2023, 1, 11),
                AllocatedTime = 4200,
            };
            int expectedCount = UnitTestHelper.Tasks.Count + 1;

            // act
            await taskRepository.AddAsync(task);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.Tasks.Count());
        }

        /// <summary>
        /// Doesn`t add task to database because user doesn`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_ThrowsDbUpdateException_UserDoesntExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var task = new TaskEntity
            {
                FrequencyId = UnitTestHelper.Frequencies[0].Id,
                Title = "Reading the book",
                InitialDate = new DateTime(2023, 1, 11),
                AllocatedTime = 4200,
            };

            // act
            var act = async () =>
            {
                await taskRepository.AddAsync(task);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Doesn`t add task to database because frequency doesn`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_ThrowsDbUpdateException_FrequencyDoesntExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var task = new TaskEntity
            {
                UserId = UnitTestHelper.AppUsers[0].Id,
                Title = "Reading the book",
                InitialDate = new DateTime(2023, 1, 11),
                AllocatedTime = 4200,
            };

            // act
            var act = async () =>
            {
                await taskRepository.AddAsync(task);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Adds tasks to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsTasksToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var tasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    UserId = UnitTestHelper.AppUsers[0].Id,
                    FrequencyId = UnitTestHelper.Frequencies[0].Id,
                    Title = "Reading the book",
                    InitialDate = new DateTime(2023, 1, 11),
                    AllocatedTime = 4200,
                },
                new TaskEntity
                {
                    UserId = UnitTestHelper.AppUsers[1].Id,
                    FrequencyId = UnitTestHelper.Frequencies[1].Id,
                    Title = "Do exercise",
                    InitialDate = new DateTime(2023, 1, 12),
                    AllocatedTime = 3000,
                },
            };
            int expectedCount = UnitTestHelper.Tasks.Count + tasks.Count;

            // act
            await taskRepository.AddRangeAsync(tasks);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.Tasks.Count());
        }

        /// <summary>
        /// Doesn`t add tasks to database because users don`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_ThrowsDbUpdateException_UsersDontExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var tasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    FrequencyId = UnitTestHelper.Frequencies[0].Id,
                    Title = "Reading the book",
                    InitialDate = new DateTime(2023, 1, 11),
                    AllocatedTime = 4200,
                },
                new TaskEntity
                {
                    FrequencyId = UnitTestHelper.Frequencies[1].Id,
                    Title = "Do exercise",
                    InitialDate = new DateTime(2023, 1, 12),
                    AllocatedTime = 3000,
                },
            };

            // act
            var act = async () =>
            {
                await taskRepository.AddRangeAsync(tasks);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Doesn`t add tasks to database because frequencies don`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_ThrowsDbUpdateException_FrequenciesDontExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var tasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    UserId = UnitTestHelper.AppUsers[0].Id,
                    Title = "Reading the book",
                    InitialDate = new DateTime(2023, 1, 11),
                    AllocatedTime = 4200,
                },
                new TaskEntity
                {
                    UserId = UnitTestHelper.AppUsers[1].Id,
                    Title = "Do exercise",
                    InitialDate = new DateTime(2023, 1, 12),
                    AllocatedTime = 3000,
                },
            };

            // act
            var act = async () =>
            {
                await taskRepository.AddRangeAsync(tasks);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Finds tasks.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task FindAsync_FindsTasks()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var expTasks = UnitTestHelper.Tasks;

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var expTasks = UnitTestHelper.Tasks;

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var expTask = UnitTestHelper.Tasks[0];

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var task = UnitTestHelper.Tasks[0];
            int expectedCount = UnitTestHelper.Tasks.Count - 1;

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var tasks = UnitTestHelper.Tasks;

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var expTask = UnitTestHelper.Tasks[0];
            expTask.Title = "New Title";
            expTask.AllocatedTime = 3600;
            expTask.FrequencyId = UnitTestHelper.Frequencies[6].Id;

            // act
            taskRepository.Update(expTask);
            context.SaveChanges();
            var actTask = await taskRepository.GetByIdAsync(expTask.Id);

            // assert
            Assert.Equal(expTask, actTask, new TaskComparer());
        }

        /// <summary>
        /// Doesn`t update task because frequency doesn`t exist.
        /// </summary>
        [Fact]
        public void Update_ThrowsDbUpdateException_FrequencyDoesntExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var taskRepository = new TaskRepository(context);
            var expTask = UnitTestHelper.Tasks[0];
            expTask.Title = "New Title";
            expTask.AllocatedTime = 3600;
            expTask.FrequencyId = Guid.Empty;

            // act
            var act = () =>
            {
                taskRepository.Update(expTask);
                context.SaveChanges();
            };

            // assert
            Assert.Throws<DbUpdateException>(act);
        }
    }
}
