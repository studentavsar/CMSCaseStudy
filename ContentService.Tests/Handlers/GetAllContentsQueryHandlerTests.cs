using System.Collections.Generic;
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
    public class GetAllContentsQueryHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IContentRepository> _mockContentRepository;
        private GetAllContentsQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockContentRepository = new Mock<IContentRepository>();

            _mockUnitOfWork.Setup(u => u.Contents).Returns(_mockContentRepository.Object);
            _handler = new GetAllContentsQueryHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldReturnAllContents()
        {
            // Arrange
            var contents = new List<Content>
            {
                new Content
                {
                    Id = 1,
                    Title = "Test Content 1",
                    Body = "This is the body of test content 1.",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Content
                {
                    Id = 2,
                    Title = "Test Content 2",
                    Body = "This is the body of test content 2.",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            _mockContentRepository.Setup(r => r.GetAllContentsAsync()).ReturnsAsync(contents);

            var query = new GetAllContentsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(contents, result);
            _mockContentRepository.Verify(r => r.GetAllContentsAsync(), Times.Once);
        }
    }
}
