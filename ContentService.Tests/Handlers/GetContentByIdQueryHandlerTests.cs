using System.Threading;
using System.Threading.Tasks;
using ContentService.Handlers;
using ContentService.Models;
using ContentService.Queries;
using ContentService.Data;
using Moq;
using NUnit.Framework;

namespace ContentService.Tests.Handlers
{
    [TestFixture]
    public class GetContentByIdQueryHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IContentRepository> _mockContentRepository;
        private GetContentByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockContentRepository = new Mock<IContentRepository>();

            _mockUnitOfWork.Setup(u => u.Contents).Returns(_mockContentRepository.Object);
            _handler = new GetContentByIdQueryHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldReturnContent()
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

            _mockContentRepository.Setup(r => r.GetContentByIdAsync(It.IsAny<int>())).ReturnsAsync(content);

            var query = new GetContentByIdQuery(1);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(content.Id, result.Id);
            Assert.AreEqual(content.Title, result.Title);
            Assert.AreEqual(content.Body, result.Body);
            _mockContentRepository.Verify(r => r.GetContentByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task Handle_GivenInvalidRequest_ShouldReturnNull()
        {
            // Arrange
            _mockContentRepository.Setup(r => r.GetContentByIdAsync(It.IsAny<int>())).ReturnsAsync((Content)null);

            var query = new GetContentByIdQuery(99);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNull(result);
            _mockContentRepository.Verify(r => r.GetContentByIdAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
