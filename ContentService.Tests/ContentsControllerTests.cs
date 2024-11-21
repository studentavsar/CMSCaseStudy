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
using ContentService.Commands;
using ContentService.Controllers;
using ContentService.Models;
using ContentService.Queries;

namespace ContentService.Tests.Controllers
{
    [TestFixture]
    public class ContentsControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private Mock<ILogger<ContentsController>> _mockLogger;
        private Mock<IValidator<Content>> _mockValidator;
        private ContentsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<ContentsController>>();
            _mockValidator = new Mock<IValidator<Content>>();

            _controller = new ContentsController(_mockMediator.Object, _mockLogger.Object, _mockValidator.Object);
        }

        [Test]
        public async Task GetAllContents_ShouldReturnOkResult_WithListOfContents()
        {
            // Arrange
            var contents = new List<Content>
            {
                new Content { Id = 1, Title = "Yüzüklerin Efendisi 1", Body = "Yüzük kardeşliği" },
                new Content { Id = 2, Title = "Yüzüklerin Efendisi 2", Body = "Kralın Dönüşü" }
            };
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAllContentsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(contents);

            // Act
            var result = await _controller.GetAllContents();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(contents, okResult.Value);
        }

        [Test]
        public async Task GetContentById_ShouldReturnOkResult_WithContent()
        {
            // Arrange
            var content = new Content { Id = 1, Title = "Yüzüklerin Efendisi 1", Body = "Yüzük kardeşliği" };
            _mockMediator.Setup(m => m.Send(It.IsAny<GetContentByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(content);

            // Act
            var result = await _controller.GetContentById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(content, okResult.Value);
        }

        [Test]
        public async Task GetContentById_ShouldReturnNotFound_WhenContentDoesNotExist()
        {
            // Arrange
            _mockMediator.Setup(m => m.Send(It.IsAny<GetContentByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((Content)null);

            // Act
            var result = await _controller.GetContentById(99);

            // Assert
            var notFoundResult = result as NotFoundResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task AddContent_ShouldReturnCreatedAtActionResult_WithCreatedContent()
        {
            // Arrange
            var content = new Content { Id = 1, Title = "Yüzüklerin Efendisi 1", Body = "Yüzük kardeşliği" };
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Content>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
            _mockMediator.Setup(m => m.Send(It.IsAny<AddContentCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(content);

            // Act
            var result = await _controller.AddContent(content);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
            Assert.AreEqual(content, createdAtActionResult.Value);
        }

        [Test]
        public async Task AddContent_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var content = new Content { Id = 1, Title = "Yüzüklerin Efendisi 1", Body = "Yüzük kardeşliği" };
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Title", "Title is required") };
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Content>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.AddContent(content);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual(validationFailures, badRequestResult.Value);
        }

        [Test]
        public async Task UpdateContent_ShouldReturnNoContent()
        {
            // Arrange
            var content = new Content { Id = 1, Title = "Yüzüklerin Efendisi 1", Body = "Yüzük kardeşliği" };
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Content>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            // Act
            var result = await _controller.UpdateContent(1, content);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [Test]
        public async Task UpdateContent_ShouldReturnBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var content = new Content { Id = 1, Title = "Yüzüklerin Efendisi 1", Body = "Yüzük kardeşliği" };

            // Act
            var result = await _controller.UpdateContent(2, content);

            // Assert
            var badRequestResult = result as BadRequestResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task UpdateContent_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var content = new Content { Id = 1, Title = "Yüzüklerin Efendisi 1", Body = "Yüzük kardeşliği" };
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Title", "Title is required") };
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Content>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.UpdateContent(1, content);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual(validationFailures, badRequestResult.Value);
        }

        [Test]
        public async Task DeleteContent_ShouldReturnNoContent()
        {
            // Act
            var result = await _controller.DeleteContent(1);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }
    }
}


