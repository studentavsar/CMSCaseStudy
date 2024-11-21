using System.Threading;
using System.Threading.Tasks;
using UserService.Handlers;
using UserService.Commands;
using UserService.Data;
using Moq;
using NUnit.Framework;
using MediatR;

namespace UserService.Tests.Handlers
{
    [TestFixture]
    public class DeleteUserCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IUserRepository> _mockUserRepository;
        private DeleteUserCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();

            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepository.Object);
            _handler = new DeleteUserCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldDeleteUser()
        {
            // Arrange
            var command = new DeleteUserCommand(1);

            _mockUserRepository.Setup(r => r.DeleteUserAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(Unit.Value, result);
            _mockUserRepository.Verify(r => r.DeleteUserAsync(It.IsAny<int>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
