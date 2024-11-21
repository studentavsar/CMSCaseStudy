using System.Threading;
using System.Threading.Tasks;
using UserService.Handlers;
using UserService.Models;
using UserService.Queries;
using UserService.Data;
using Moq;
using NUnit.Framework;

namespace UserService.Tests.Handlers
{
    [TestFixture]
    public class GetUserByIdQueryHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IUserRepository> _mockUserRepository;
        private GetUserByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();

            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepository.Object);
            _handler = new GetUserByIdQueryHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldReturnUser()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "testuser@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

            var query = new GetUserByIdQuery(1);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.Username, result.Username);
            Assert.AreEqual(user.Email, result.Email);
            _mockUserRepository.Verify(r => r.GetUserByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task Handle_GivenInvalidRequest_ShouldReturnNull()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            var query = new GetUserByIdQuery(99);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNull(result);
            _mockUserRepository.Verify(r => r.GetUserByIdAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
