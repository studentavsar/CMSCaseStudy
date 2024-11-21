using System;
using System.Threading;
using System.Threading.Tasks;
using ContentService.Commands;
using ContentService.Handlers;
using ContentService.Models;
using ContentService.Data;
using Moq;
using NUnit.Framework;

namespace ContentService.Tests.Handlers
{
    [TestFixture]
    public class AddContentCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IContentRepository> _mockContentRepository;
        private AddContentCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockContentRepository = new Mock<IContentRepository>();

            _mockUnitOfWork.Setup(u => u.Contents).Returns(_mockContentRepository.Object);
            _handler = new AddContentCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldAddContent()
        {
            // Arrange
            var content = new Content
            {
                Id = 1,
                Title = "Test Content",
                Body = "This is a test content body.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var command = new AddContentCommand(content);

            _mockContentRepository.Setup(r => r.AddContentAsync(It.IsAny<Content>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(content.Title, result.Title);
            Assert.AreEqual(content.Body, result.Body);
            _mockContentRepository.Verify(r => r.AddContentAsync(It.IsAny<Content>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
