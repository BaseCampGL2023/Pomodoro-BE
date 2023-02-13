// <copyright file="FrequencyRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

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
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyRepository = new FrequencyRepository(context);
            var frequency = new Frequency
            {
                Id = new Guid(2, 8, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                FrequencyTypeId = new Guid(7, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                Every = 2,
            };
            int expectedCount = context.Frequencies.Count() + 1;

            // act
            await frequencyRepository.AddAsync(frequency);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.Frequencies.Count());
        }

        /// <summary>
        /// Adds frequencies to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsFrequenciesToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyRepository = new FrequencyRepository(context);
            var frequencies = new List<Frequency>
            {
                new Frequency
                {
                    Id = new Guid(2, 8, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(6, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 2,
                },
                new Frequency
                {
                    Id = new Guid(2, 9, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    FrequencyTypeId = new Guid(7, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Every = 3,
                },
            };
            int expectedCount = context.Frequencies.Count() + frequencies.Count;

            // act
            await frequencyRepository.AddRangeAsync(frequencies);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.Frequencies.Count());
        }

        /// <summary>
        /// Finds frequencies.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task FindAsync_FindsFrequencies()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyRepository = new FrequencyRepository(context);
            var expFrequencies = context.Frequencies.Skip(1).ToList();

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
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyRepository = new FrequencyRepository(context);
            var expFrequencies = context.Frequencies.ToList();

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
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyRepository = new FrequencyRepository(context);
            var expFrequency = context.Frequencies.First();

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
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyRepository = new FrequencyRepository(context);
            var frequency = context.Frequencies.First();
            int expectedCount = context.Frequencies.Count() - 1;

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
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyRepository = new FrequencyRepository(context);
            var frequencies = context.Frequencies.ToList();

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
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyRepository = new FrequencyRepository(context);
            var expFrequency = new Frequency
            {
                Id = new Guid(2, 7, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                FrequencyTypeId = new Guid(6, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                Every = 2,
            };

            // act
            frequencyRepository.Update(expFrequency);
            context.SaveChanges();
            var actFrequency = await frequencyRepository.GetByIdAsync(expFrequency.Id);

            // assert
            Assert.Equal(expFrequency, actFrequency, new FrequencyComparer());
        }
    }
}
