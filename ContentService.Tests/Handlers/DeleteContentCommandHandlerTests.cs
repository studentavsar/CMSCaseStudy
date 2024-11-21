using System.Threading;
using System.Threading.Tasks;
using ContentService.Commands;
using ContentService.Handlers;
using ContentService.Data;
using Moq;
using NUnit.Framework;
using MediatR;

namespace ContentService.Tests.Handlers
{
    [TestFixture]
    public class DeleteContentCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IContentRepository> _mockContentRepository;
        private DeleteContentCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockContentRepository = new Mock<IContentRepository>();

            _mockUnitOfWork.Setup(u => u.Contents).Returns(_mockContentRepository.Object);
            _handler = new DeleteContentCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldDeleteContent()
        {
            // Arrange
            var command = new DeleteContentCommand(1);

            _mockContentRepository.Setup(r => r.DeleteContentAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(Unit.Value, result);
            _mockContentRepository.Verify(r => r.DeleteContentAsync(It.IsAny<int>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
