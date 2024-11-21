using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Commands;
using UserService.Models;
using UserService.Queries;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;
        private readonly IValidator<User> _validator;

        public UsersController(IMediator mediator, ILogger<UsersController> logger, IValidator<User> validator)
        {
            _mediator = mediator;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _mediator.Send(new GetAllUsersQuery());
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all contents.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _mediator.Send(new GetUserByIdQuery(id));
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting content by id.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            ValidationResult result = await _validator.ValidateAsync(user);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                var createdUser = await _mediator.Send(new AddUserCommand(user));
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new content.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            ValidationResult result = await _validator.ValidateAsync(user);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                await _mediator.Send(new UpdateUserCommand(id, user));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the content.");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _mediator.Send(new DeleteUserCommand(id));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the content.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
