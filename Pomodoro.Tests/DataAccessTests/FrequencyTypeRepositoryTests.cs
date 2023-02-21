// <copyright file="FrequencyTypeRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var frequencyType = new FrequencyType
            {
                Value = (FrequencyValue)7,
            };
            int expectedCount = UnitTestHelper.FrequencyTypes.Count + 1;

            // act
            await frequencyTypeRepository.AddAsync(frequencyType);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.FrequencyTypes.Count());
        }

        /// <summary>
        /// Doesn`t add frequency type to database because frequency value already exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_ThrowsDbUpdateException_FrequencyValueAlreadyExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var frequencyType = new FrequencyType
            {
                Value = FrequencyValue.Day,
            };

            // act
            var act = async () =>
            {
                await frequencyTypeRepository.AddAsync(frequencyType);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Adds frequency types to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsFrequencyTypesToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var frequencyTypes = new List<FrequencyType>
            {
                new FrequencyType
                {
                    Value = (FrequencyValue)7,
                },
                new FrequencyType
                {
                    Value = (FrequencyValue)8,
                },
            };
            int expectedCount = UnitTestHelper.FrequencyTypes.Count + frequencyTypes.Count;

            // act
            await frequencyTypeRepository.AddRangeAsync(frequencyTypes);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.FrequencyTypes.Count());
        }

        /// <summary>
        /// Doesn`t add frequency types to database because frequency values already exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_ThrowsDbUpdateException_FrequencyValuesAlreadyExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var frequencyTypes = new List<FrequencyType>
            {
                new FrequencyType
                {
                    Value = FrequencyValue.Workday,
                },
                new FrequencyType
                {
                    Value = FrequencyValue.Day,
                },
            };

            // act
            var act = async () =>
            {
                await frequencyTypeRepository.AddRangeAsync(frequencyTypes);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Finds frequency types.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task FindAsync_FindsFrequencyTypes()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var expFrequencyTypes = UnitTestHelper.FrequencyTypes.Take(1).ToList();

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var expFrequencyTypes = UnitTestHelper.FrequencyTypes;

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var expFrequencyType = UnitTestHelper.FrequencyTypes[0];

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var frequencyType = UnitTestHelper.FrequencyTypes[0];
            int expectedCount = UnitTestHelper.FrequencyTypes.Count - 1;

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var frequencyTypes = UnitTestHelper.FrequencyTypes;

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var expFrequencyType = UnitTestHelper.FrequencyTypes[0];
            expFrequencyType.Value = (FrequencyValue)7;

            // act
            frequencyTypeRepository.Update(expFrequencyType);
            context.SaveChanges();
            var actFrequencyType = await frequencyTypeRepository.GetByIdAsync(expFrequencyType.Id);

            // assert
            Assert.Equal(expFrequencyType, actFrequencyType, new FrequencyTypeComparer());
        }

        /// <summary>
        /// Doesn`t update frequency type because frequency value already exist.
        /// </summary>
        [Fact]
        public void Update_ThrowsDbUpdateException_FrequencyValueAlreadyExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var frequencyTypeRepository = new FrequencyTypeRepository(context);
            var expFrequencyType = UnitTestHelper.FrequencyTypes[0];
            expFrequencyType.Value = FrequencyValue.Year;

            // act
            var act = () =>
            {
                frequencyTypeRepository.Update(expFrequencyType);
                context.SaveChanges();
            };

            // assert
            Assert.Throws<DbUpdateException>(act);
        }
    }
}
