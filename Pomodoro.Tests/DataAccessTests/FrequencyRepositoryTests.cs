// <copyright file="FrequencyRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
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
    /// Frequency repository test class.
    /// </summary>
    public class FrequencyRepositoryTests
    {
        /// <summary>
        /// Adds frequency to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_AddsFrequencyToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyRepository = new FrequencyRepository(context);
            var frequency = new Frequency
            {
                FrequencyTypeId = UnitTestHelper.FrequencyTypes[6].Id,
                Every = 2,
            };
            int expectedCount = UnitTestHelper.Frequencies.Count + 1;

            // act
            await frequencyRepository.AddAsync(frequency);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.Frequencies.Count());
        }

        /// <summary>
        /// Doesn`t add frequency to database because frequency type doesn`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_ThrowsDbUpdateException_FrequencyTypeDoesntExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyRepository = new FrequencyRepository(context);
            var frequency = new Frequency
            {
                FrequencyTypeId = Guid.Empty,
                Every = 2,
            };

            // act
            var act = async () =>
            {
                await frequencyRepository.AddAsync(frequency);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Adds frequencies to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsFrequenciesToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyRepository = new FrequencyRepository(context);
            var frequencies = new List<Frequency>
            {
                new Frequency
                {
                    FrequencyTypeId = UnitTestHelper.FrequencyTypes[5].Id,
                    Every = 2,
                },
                new Frequency
                {
                    FrequencyTypeId = UnitTestHelper.FrequencyTypes[6].Id,
                    Every = 3,
                },
            };
            int expectedCount = UnitTestHelper.Frequencies.Count + frequencies.Count;

            // act
            await frequencyRepository.AddRangeAsync(frequencies);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.Frequencies.Count());
        }

        /// <summary>
        /// Doesn`t add frequencies to database because frequency types don`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_ThrowsDbUpdateException_FrequencyTypesDontExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyRepository = new FrequencyRepository(context);
            var frequencies = new List<Frequency>
            {
                new Frequency
                {
                    FrequencyTypeId = Guid.Empty,
                    Every = 2,
                },
                new Frequency
                {
                    FrequencyTypeId = Guid.NewGuid(),
                    Every = 3,
                },
            };

            // act
            var act = async () =>
            {
                await frequencyRepository.AddRangeAsync(frequencies);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Finds frequencies.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task FindAsync_FindsFrequencies()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyRepository = new FrequencyRepository(context);
            var expFrequencies = UnitTestHelper.Frequencies.Where(f => f.Every == UnitTestHelper.Frequencies[1].Every)
                                                           .ToList();

            // act
            var actFrequencies = await frequencyRepository.FindAsync(x => x.Every == expFrequencies[0].Every);

            // assert
            Assert.Equal(expFrequencies, actFrequencies, new FrequencyComparer());
        }

        /// <summary>
        /// Returns all frequencies.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_ReturnsAllFrequencies()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyRepository = new FrequencyRepository(context);
            var expFrequencies = UnitTestHelper.Frequencies;

            // act
            var actFrequencies = await frequencyRepository.GetAllAsync();

            // assert
            Assert.Equal(expFrequencies, actFrequencies, new FrequencyComparer());
        }

        /// <summary>
        /// Gets frequency by id.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_ReturnsFrequency()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyRepository = new FrequencyRepository(context);
            var expFrequency = UnitTestHelper.Frequencies[0];

            // act
            var actFrequency = await frequencyRepository.GetByIdAsync(expFrequency.Id);

            // assert
            Assert.Equal(expFrequency, actFrequency, new FrequencyComparer());
        }

        /// <summary>
        /// Remove frequency from db.
        /// </summary>
        [Fact]
        public void Remove_RemovesFrequency()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyRepository = new FrequencyRepository(context);
            var frequency = UnitTestHelper.Frequencies[0];
            int expectedCount = UnitTestHelper.Frequencies.Count - 1;

            // act
            frequencyRepository.Remove(frequency);
            context.SaveChanges();

            // assert
            Assert.Equal(expectedCount, context.Frequencies.Count());
        }

        /// <summary>
        /// Removes frequencies from db.
        /// </summary>
        [Fact]
        public void RemoveRange_RemovesFrequencies()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyRepository = new FrequencyRepository(context);
            var frequencies = UnitTestHelper.Frequencies;

            // act
            frequencyRepository.RemoveRange(frequencies);
            context.SaveChanges();

            // assert
            Assert.Empty(context.Frequencies);
        }

        /// <summary>
        /// Updates frequency.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task Update_UpdatesFrequency()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyRepository = new FrequencyRepository(context);
            var expFrequency = UnitTestHelper.Frequencies[0];
            expFrequency.FrequencyTypeId = UnitTestHelper.FrequencyTypes[5].Id;
            expFrequency.Every = 2;

            // act
            frequencyRepository.Update(expFrequency);
            context.SaveChanges();
            var actFrequency = await frequencyRepository.GetByIdAsync(expFrequency.Id);

            // assert
            Assert.Equal(expFrequency, actFrequency, new FrequencyComparer());
        }

        /// <summary>
        /// Doesn`t update frequency because frequency type doesn`t exist.
        /// </summary>
        [Fact]
        public void Update_ThrowsDbUpdateException_FrequencyTypeDoesntExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyRepository = new FrequencyRepository(context);
            var expFrequency = UnitTestHelper.Frequencies[0];
            expFrequency.FrequencyTypeId = Guid.Empty;
            expFrequency.Every = 2;

            // act
            var act = () =>
            {
                frequencyRepository.Update(expFrequency);
                context.SaveChanges();
            };

            // assert
            Assert.Throws<DbUpdateException>(act);
        }
    }
}
