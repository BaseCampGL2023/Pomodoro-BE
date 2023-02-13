// <copyright file="FrequencyTypeRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Enums;
using Pomodoro.DataAccess.Repositories.Realizations;
using Pomodoro.Tests.EqualityComparers;

namespace Pomodoro.Tests.DataAccessTests
{
    /// <summary>
    /// FrequencyType repository test class.
    /// </summary>
    public class FrequencyTypeRepositoryTests
    {
        /// <summary>
        /// Adds frequency type to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_AddsFrequencyTypeToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var frequencyType = new FrequencyType
            {
                Id = new Guid(8, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                Value = FrequencyValue.Month,
            };
            int expectedCount = context.FrequencyTypes.Count() + 1;

            // act
            await frequencyTypeRepository.AddAsync(frequencyType);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.FrequencyTypes.Count());
        }

        /// <summary>
        /// Adds frequency types to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsFrequencyTypesToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var frequencyTypes = new List<FrequencyType>
            {
                new FrequencyType
                {
                    Id = new Guid(8, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Month,
                },
                new FrequencyType
                {
                    Id = new Guid(9, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Value = FrequencyValue.Year,
                },
            };
            int expectedCount = context.FrequencyTypes.Count() + frequencyTypes.Count;

            // act
            await frequencyTypeRepository.AddRangeAsync(frequencyTypes);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.FrequencyTypes.Count());
        }

        /// <summary>
        /// Finds frequency types.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task FindAsync_FindsFrequencyTypes()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var expFrequencyTypes = context.FrequencyTypes.Take(1).ToList();

            // act
            var actFrequencyTypes = await frequencyTypeRepository.FindAsync(x => x.Value == expFrequencyTypes[0].Value);

            // assert
            Assert.Equal(expFrequencyTypes, actFrequencyTypes, new FrequencyTypeComparer());
        }

        /// <summary>
        /// Returns all frequency types.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_ReturnsAllFrequencyTypes()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var expFrequencyTypes = context.FrequencyTypes.ToList();

            // act
            var actFrequencyTypes = await frequencyTypeRepository.GetAllAsync();

            // assert
            Assert.Equal(expFrequencyTypes, actFrequencyTypes, new FrequencyTypeComparer());
        }

        /// <summary>
        /// Gets frequency type by id.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_ReturnsFrequencyType()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var expFrequencyType = context.FrequencyTypes.First();

            // act
            var actFrequencyType = await frequencyTypeRepository.GetByIdAsync(expFrequencyType.Id);

            // assert
            Assert.Equal(expFrequencyType, actFrequencyType, new FrequencyTypeComparer());
        }

        /// <summary>
        /// Remove frequency type from db.
        /// </summary>
        [Fact]
        public void Remove_RemovesFrequencyType()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var frequencyType = context.FrequencyTypes.First();
            int expectedCount = context.FrequencyTypes.Count() - 1;

            // act
            frequencyTypeRepository.Remove(frequencyType);
            context.SaveChanges();

            // assert
            Assert.Equal(expectedCount, context.FrequencyTypes.Count());
        }

        /// <summary>
        /// Removes frequency types from db.
        /// </summary>
        [Fact]
        public void RemoveRange_RemovesFrequencyTypes()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var frequencyTypes = context.FrequencyTypes.ToList();

            // act
            frequencyTypeRepository.RemoveRange(frequencyTypes);
            context.SaveChanges();

            // assert
            Assert.Empty(context.FrequencyTypes);
        }

        /// <summary>
        /// Updates frequency type.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task Update_UpdatesFrequencyType()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var expFrequency = new FrequencyType
            {
                Id = new Guid(7, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                Value = FrequencyValue.Year,
            };

            // act
            frequencyTypeRepository.Update(expFrequency);
            context.SaveChanges();
            var actFrequencyType = await frequencyTypeRepository.GetByIdAsync(expFrequency.Id);

            // assert
            Assert.Equal(expFrequency, actFrequencyType, new FrequencyTypeComparer());
        }
    }
}
