// <copyright file="UserRepositoryTests.cs" company="PomodoroGroup_GL_BaseCamp">
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
    /// User repository test class.
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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var identityUserId = context.Users.Add(new PomoIdentityUser()).Entity.Id;
            var user = new AppUser
            {
                Name = "Jane",
                Email = "jane@gmail.com",
                PomoIdentityUserId = identityUserId,
            };
            int expectedCount = UnitTestHelper.AppUsers.Count + 1;

            // act
            await userRepository.AddAsync(user);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.AppUsers.Count());
        }

        /// <summary>
        /// Doesn`t add user to database because identity user doesn`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_ThrowsDbUpdateException_IdentityUserDoesntExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var user = new AppUser
            {
                Name = "Jane",
                Email = "jane@gmail.com",
            };

            // act
            var act = async () =>
            {
                await userRepository.AddAsync(user);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Doesn`t add user to database because of not unique user`s id.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddAsync_ThrowsDbUpdateException_UserIdNotUnique()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var identityUserId = context.Users.Add(new PomoIdentityUser()).Entity.Id;
            var user = new AppUser
            {
                Id = UnitTestHelper.AppUsers[0].Id,
                Name = "Jane",
                Email = "jane@gmail.com",
                PomoIdentityUserId = identityUserId,
            };

            // act
            var act = async () =>
            {
                await userRepository.AddAsync(user);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Adds users to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsUsersToDatabase()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var identityUserId1 = context.Users.Add(new PomoIdentityUser()).Entity.Id;
            var identityUserId2 = context.Users.Add(new PomoIdentityUser()).Entity.Id;
            var users = new List<AppUser>
            {
                new AppUser
                {
                    Name = "Jane",
                    Email = "jane@gmail.com",
                    PomoIdentityUserId = identityUserId1,
                },
                new AppUser
                {
                    Name = "Bob",
                    Email = "bob@gmail.com",
                    PomoIdentityUserId = identityUserId2,
                },
            };
            int expectedCount = UnitTestHelper.AppUsers.Count + users.Count;

            // act
            await userRepository.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // assert
            Assert.Equal(expectedCount, context.AppUsers.Count());
        }

        /// <summary>
        /// Doesn`t add users to database because identity users don`t exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsUsersToDatabase_IdentityUsersDoesntExist()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var users = new List<AppUser>
            {
                new AppUser
                {
                    Name = "Jane",
                    Email = "jane@gmail.com",
                },
                new AppUser
                {
                    Name = "Bob",
                    Email = "bob@gmail.com",
                },
            };

            // act
            var act = async () =>
            {
                await userRepository.AddRangeAsync(users);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Doesn`t add users to database because of not unique users` id.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task AddRangeAsync_AddsUsersToDatabase_UsersIdNotUnique()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var identityUserId1 = context.Users.Add(new PomoIdentityUser()).Entity.Id;
            var identityUserId2 = context.Users.Add(new PomoIdentityUser()).Entity.Id;
            var users = new List<AppUser>
            {
                new AppUser
                {
                    Id = UnitTestHelper.AppUsers[0].Id,
                    Name = "Jane",
                    Email = "jane@gmail.com",
                    PomoIdentityUserId = identityUserId1,
                },
                new AppUser
                {
                    Id = UnitTestHelper.AppUsers[1].Id,
                    Name = "Bob",
                    Email = "bob@gmail.com",
                    PomoIdentityUserId = identityUserId2,
                },
            };

            // act
            var act = async () =>
            {
                await userRepository.AddRangeAsync(users);
                await context.SaveChangesAsync();
            };

            // assert
            await Assert.ThrowsAsync<DbUpdateException>(act);
        }

        /// <summary>
        /// Finds users.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task FindAsync_FindsUsers()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var expUsers = UnitTestHelper.AppUsers.Take(1).ToList();

            // act
            var actUsers = await userRepository.FindAsync(x => x.Name == expUsers[0].Name);

            // assert
            Assert.Equal(expUsers, actUsers, new AppUserComparer());
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var expUsers = UnitTestHelper.AppUsers;

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
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var expUser = UnitTestHelper.AppUsers[0];

            // act
            var actUser = await userRepository.GetByIdAsync(expUser.Id);

            // assert
            Assert.Equal(expUser, actUser, new AppUserComparer());
        }

        /// <summary>
        /// Remove user from db.
        /// </summary>
        [Fact]
        public void Remove_RemovesUser()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var user = UnitTestHelper.AppUsers[0];
            int expectedCount = UnitTestHelper.AppUsers.Count() - 1;

            // act
            userRepository.Remove(user);
            context.SaveChanges();

            // assert
            Assert.Equal(expectedCount, context.AppUsers.Count());
        }

        /// <summary>
        /// Removes users from db.
        /// </summary>
        [Fact]
        public void RemoveRange_RemovesUsers()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var users = UnitTestHelper.AppUsers.ToList();

            // act
            userRepository.RemoveRange(users);
            context.SaveChanges();

            // assert
            Assert.Empty(context.AppUsers);
        }

        /// <summary>
        /// Updates user`s name and email.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents an asynchronous operation.</returns>
        [Fact]
        public async Task Update_UpdatesUser()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var expUser = new AppUser
            {
                Id = UnitTestHelper.AppUsers[0].Id,
                Name = "new" + UnitTestHelper.AppUsers[0].Name,
                Email = "new" + UnitTestHelper.AppUsers[0].Email,
                PomoIdentityUserId = UnitTestHelper.AppUsers[0].PomoIdentityUserId,
            };

            // act
            userRepository.Update(expUser);
            context.SaveChanges();
            var actUser = await userRepository.GetByIdAsync(expUser.Id);

            // assert
            Assert.Equal(expUser, actUser, new AppUserComparer());
        }

        /// <summary>
        /// Doesn`t update user because identityUser is taken.
        /// </summary>
        [Fact]
        public void Update_DoesntUpdateUser_IdentityUserIsTaken()
        {
            // arrange
            using var context = new AppDbContext(UnitTestHelper.DbOptions);
            var userRepository = new UserRepository(context);
            var expUser = new AppUser
            {
                Id = UnitTestHelper.AppUsers[0].Id,
                Name = "new" + UnitTestHelper.AppUsers[0].Name,
                Email = "new" + UnitTestHelper.AppUsers[0].Email,
                PomoIdentityUserId = UnitTestHelper.IdentityUsers[1].Id,
            };

            // act
            var act = () =>
            {
                userRepository.Update(expUser);
                context.SaveChanges();
            };

            // assert
            Assert.Throws<DbUpdateException>(act);
        }
    }
}
