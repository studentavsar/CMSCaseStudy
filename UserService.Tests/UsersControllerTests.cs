using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UserService.Commands;
using UserService.Controllers;
using UserService.Models;
using UserService.Queries;

namespace UserService.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private Mock<ILogger<UsersController>> _mockLogger;
        private Mock<IValidator<User>> _mockValidator;
        private UsersController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<UsersController>>();
            _mockValidator = new Mock<IValidator<User>>();

            _controller = new UsersController(_mockMediator.Object, _mockLogger.Object, _mockValidator.Object);
        }

        [Test]
        public async Task GetAllUsers_ShouldReturnOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Email = "user1@example.com" },
                new User { Id = 2, Username = "user2", Email = "user2@example.com" }
            };
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(users, okResult.Value);
        }

        [Test]
        public async Task GetUserById_ShouldReturnOkResult_WithUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com" };
            _mockMediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(user, okResult.Value);
        }

        [Test]
        public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockMediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUserById(99);

            // Assert
            var notFoundResult = result as NotFoundResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task AddUser_ShouldReturnCreatedAtActionResult_WithCreatedUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com" };
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
            _mockMediator.Setup(m => m.Send(It.IsAny<AddUserCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            // Act
            var result = await _controller.AddUser(user);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
            Assert.AreEqual(user, createdAtActionResult.Value);
        }

        [Test]
        public async Task AddUser_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com" };
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Username", "Username is required") };
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.AddUser(user);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual(validationFailures, badRequestResult.Value);
        }

        [Test]
        public async Task UpdateUser_ShouldReturnNoContent()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com" };
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            // Act
            var result = await _controller.UpdateUser(1, user);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [Test]
        public async Task UpdateUser_ShouldReturnBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com" };

            // Act
            var result = await _controller.UpdateUser(2, user);

            // Assert
            var badRequestResult = result as BadRequestResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task UpdateUser_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com" };
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Username", "Username is required") };
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.UpdateUser(1, user);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual(validationFailures, badRequestResult.Value);
        }

        [Test]
        public async Task DeleteUser_ShouldReturnNoContent()
        {
            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }
    }
}

