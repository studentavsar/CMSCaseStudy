using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UserService.Data;
using UserService.Models;

namespace UserService.Tests.Data
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private UserDbContext _context;
        private UserRepository _repository;
        private Mock<ILogger<UserRepository>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceTestDb")
                .Options;

            _context = new UserDbContext(options);
            _mockLogger = new Mock<ILogger<UserRepository>>();
            _repository = new UserRepository(_context, _mockLogger.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Email = "user1@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new User { Id = 2, Username = "user2", Email = "user2@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllUsersAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(user.Id, result.Id);
        }

        [Test]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.GetUserByIdAsync(99);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddUserAsync_ShouldAddUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

            // Act
            await _repository.AddUserAsync(user);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _context.Users.FindAsync(1);
            Assert.NotNull(result);
            Assert.AreEqual(user.Id, result.Id);
        }

        [Test]
        public async Task UpdateUserAsync_ShouldUpdateUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            user.Username = "updateduser";

            // Act
            await _repository.UpdateUserAsync(user);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _context.Users.FindAsync(1);
            Assert.NotNull(result);
            Assert.AreEqual("updateduser", result.Username);
        }

        [Test]
        public async Task DeleteUserAsync_ShouldDeleteUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteUserAsync(1);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _context.Users.FindAsync(1);
            Assert.IsNull(result);
        }
    }
}
