using System.Collections.Generic;
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
    public class GetAllUsersQueryHandlerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IUserRepository> _mockUserRepository;
        private GetAllUsersQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();

            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepository.Object);
            _handler = new GetAllUsersQueryHandler(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Handle_GivenValidRequest_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "testuser1",
                    Email = "testuser1@example.com",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 2,
                    Username = "testuser2",
                    Email = "testuser2@example.com",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            _mockUserRepository.Setup(r => r.GetAllUsersAsync()).ReturnsAsync(users);

            var query = new GetAllUsersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(users, result);
            _mockUserRepository.Verify(r => r.GetAllUsersAsync(), Times.Once);
        }
    }
}
