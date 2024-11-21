using System.Threading;
using System.Threading.Tasks;
using ContentService.Handlers;
using ContentService.Models;
using ContentService.Commands;
using ContentService.Data;
using Moq;
using NUnit.Framework;
using MediatR;

namespace ContentService.Tests.Handlers
{
    [TestFixture]
    public class UpdateContentCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IContentRepository> _mockContentRepository;
        private UpdateContentCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockContentRepository = new Mock<IContentRepository>();

            _mockUnitOfWork.Setup(u => u.Contents).Returns(_mockContentRepository.Object);
            _handler = new UpdateContentCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldUpdateContent()
        {
            // Arrange
            var content = new Content
            {
                Id = 1,
                Title = "Updated Content",
                Body = "This is the updated content body.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var command = new UpdateContentCommand(content.Id, content);

            _mockContentRepository.Setup(r => r.UpdateContentAsync(It.IsAny<Content>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(Unit.Value, result);
            _mockContentRepository.Verify(r => r.UpdateContentAsync(It.IsAny<Content>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
