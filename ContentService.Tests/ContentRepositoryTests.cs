using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ContentService.Data;
using ContentService.Models;

namespace ContentService.Tests.Data
{
    [TestFixture]
    public class ContentRepositoryTests
    {
        private ContentDbContext _context;
        private ContentRepository _repository;
        private Mock<ILogger<ContentRepository>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ContentDbContext>()
                .UseInMemoryDatabase(databaseName: "ContentServiceTestDb")
                .Options;

            _context = new ContentDbContext(options);
            _mockLogger = new Mock<ILogger<ContentRepository>>();
            _repository = new ContentRepository(_context, _mockLogger.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllContentsAsync_ShouldReturnAllContents()
        {
            // Arrange
            var contents = new List<Content>
            {
                new Content { Id = 1, Title = "Yüzüklerin Efendisi 1", Body = "Yüzük kardeşliği", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Content { Id = 2, Title = "Yüzüklerin Efendisi 2", Body = "Kralın Dönüşü", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };
            await _context.Contents.AddRangeAsync(contents);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllContentsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetContentByIdAsync_ShouldReturnContent_WhenContentExists()
        {
            // Arrange
            var content = new Content { Id = 1, Title = "Yüzüklerin Efendisi 1", Body = "Yüzük kardeşliği", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            await _context.Contents.AddAsync(content);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetContentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(content.Id, result.Id);
        }

        [Test]
        public async Task GetContentByIdAsync_ShouldReturnNull_WhenContentDoesNotExist()
        {
            // Act
            var result = await _repository.GetContentByIdAsync(99);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddContentAsync_ShouldAddContent()
        {
            // Arrange
            var content = new Content { Id = 1, Title = "Yüzüklerin Efendisi 1", Body = "Yüzük kardeşliği", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

            // Act
            await _repository.AddContentAsync(content);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _context.Contents.FindAsync(1);
            Assert.NotNull(result);
            Assert.AreEqual(content.Id, result.Id);
        }

        [Test]
        public async Task UpdateContentAsync_ShouldUpdateContent()
        {
            // Arrange
            var content = new Content { Id = 1, Title = "Yüzüklerin Efendisi 1", Body = "Yüzük kardeşliği", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            await _context.Contents.AddAsync(content);
            await _context.SaveChangesAsync();

            content.Title = "Updated Content";

            // Act
            await _repository.UpdateContentAsync(content);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _context.Contents.FindAsync(1);
            Assert.NotNull(result);
            Assert.AreEqual("Updated Content", result.Title);
        }

        [Test]
        public async Task DeleteContentAsync_ShouldDeleteContent()
        {
            // Arrange
            var content = new Content { Id = 1, Title = "Yüzüklerin Efendisi 1", Body = "Yüzük kardeşliği", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            await _context.Contents.AddAsync(content);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteContentAsync(1);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _context.Contents.FindAsync(1);
            Assert.IsNull(result);
        }
    }
}

