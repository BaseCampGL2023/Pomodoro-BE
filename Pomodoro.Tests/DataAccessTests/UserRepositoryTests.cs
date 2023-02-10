// <copyright file="UserRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using Pomodoro.DataAccess.EF;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Realizations;
using Pomodoro.Tests.EqualityComparers;

namespace Pomodoro.Tests.DataAccessTests
{
    /// <summary>
    /// Unit test sample class.
    /// </summary>
    public class UserRepositoryTests
    {
        /// <summary>
        /// Adds user to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_AddsUserToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var userRepository = new UserRepository(context);
            var user = new AppUser
            {
                Id = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                Name = "Jane",
                Email = "jane@gmail.com",
            };

            // act
            await userRepository.AddAsync(user);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(3, context.AppUsers.Count());
        }

        /// <summary>
        /// Adds users to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsUsersToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var userRepository = new UserRepository(context);
            var users = new List<AppUser>
            {
                new AppUser
                {
                    Id = new Guid(3, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Name = "Jane",
                    Email = "jane@gmail.com",
                },
                new AppUser
                {
                    Id = new Guid(4, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Name = "Bob",
                    Email = "bob@gmail.com",
                },
            };

            // act
            await userRepository.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(4, context.AppUsers.Count());
        }

        /// <summary>
        /// Finds users.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task FindAsync_FindsUsers()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var userRepository = new UserRepository(context);
            var expUsers = new List<AppUser>
            {
                new AppUser
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Name = "Viktor",
                    Email = "vitia@gmail.com",
                },
                new AppUser
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Name = "Mia",
                    Email = "mia@gmail.com",
                },
            };

            // act
            var actUsers = await userRepository.FindAsync(x => x.Name == expUsers[0].Name
                                                            || x.Name == expUsers[1].Name);

            // assert
            Assert.Equal(expUsers, actUsers, new AppUserComparer());
        }

        /// <summary>
        /// Does not find any user.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task FindAsync_DoesNotFindAnyUser()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var userRepository = new UserRepository(context);

            // act
            var actUser = await userRepository.FindAsync(u => u.Name == "Will8795");

            // assert
            Assert.Empty(actUser);
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var userRepository = new UserRepository(context);
            var expUsers = new List<AppUser>
            {
                new AppUser
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Name = "Viktor",
                    Email = "vitia@gmail.com",
                },
                new AppUser
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Name = "Mia",
                    Email = "mia@gmail.com",
                },
            };

            // act
            var actUsers = await userRepository.GetAllAsync();

            // assert
            Assert.Equal(expUsers, actUsers, new AppUserComparer());
        }

        /// <summary>
        /// Gets user by id.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetByIdAsync_ReturnsUsers()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var userRepository = new UserRepository(context);
            var expUser = new AppUser
            {
                Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                Name = "Viktor",
                Email = "vitia@gmail.com",
            };

            // act
            var actUses = await userRepository.GetByIdAsync(expUser.Id);

            // assert
            Assert.Equal(expUser, actUses, new AppUserComparer());
        }

        /// <summary>
        /// Remove user from db.
        /// </summary>
        [Fact]
        public void Remove_RemovesUser()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var userRepository = new UserRepository(context);
            var user = new AppUser
            {
                Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                Name = "Viktor",
                Email = "vitia@gmail.com",
            };

            // act
            userRepository.Remove(user);
            context.SaveChanges();

            // assert
            Assert.Equal(1, context.AppUsers.Count());
        }

        /// <summary>
        /// Removes users from db.
        /// </summary>
        [Fact]
        public void RemoveRange_RemovesUsers()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var userRepository = new UserRepository(context);
            var users = new List<AppUser>
            {
                new AppUser
                {
                    Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Name = "Viktor",
                    Email = "vitia@gmail.com",
                },
                new AppUser
                {
                    Id = new Guid(2, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                    Name = "Mia",
                    Email = "mia@gmail.com",
                },
            };

            // act
            userRepository.RemoveRange(users);
            context.SaveChanges();

            // assert
            Assert.Empty(context.AppUsers);
        }

        /// <summary>
        /// Updates user.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task Update_UpdatesUser()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var userRepository = new UserRepository(context);
            var expUser = new AppUser
            {
                Id = new Guid(1, 2, 3, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }),
                Name = "ViktorNew",
                Email = "vitianew@gmail.com",
            };

            // act
            userRepository.Update(expUser);
            context.SaveChanges();
            var actUses = await userRepository.GetByIdAsync(expUser.Id);

            // assert
            Assert.Equal(expUser, actUses, new AppUserComparer());
        }
    }
}
