using System.Threading;
using System.Threading.Tasks;
using UserService.Handlers;
using UserService.Models;
using UserService.Commands;
using UserService.Data;
using Moq;
using NUnit.Framework;
using MediatR;

namespace UserService.Tests.Handlers
{
    [TestFixture]
    public class UpdateUserCommandHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IUserRepository> _mockUserRepository;
        private UpdateUserCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();

            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepository.Object);
            _handler = new UpdateUserCommandHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldUpdateUser()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "updateduser",
                Email = "updateduser@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var command = new UpdateUserCommand(user.Id, user);

            _mockUserRepository.Setup(r => r.UpdateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(Unit.Value, result);
            _mockUserRepository.Verify(r => r.UpdateUserAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
