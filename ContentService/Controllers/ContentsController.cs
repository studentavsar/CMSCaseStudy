using ContentService.Commands;
using ContentService.Models;
using ContentService.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ContentService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ContentsController> _logger;
        private readonly IValidator<Content> _validator;

        public ContentsController(IMediator mediator, ILogger<ContentsController> logger, IValidator<Content> validator)
        {
            _mediator = mediator;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContents()
        {
            try
            {
                var contents = await _mediator.Send(new GetAllContentsQuery());
                return Ok(contents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all contents.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentById(int id)
        {
            try
            {
                var content = await _mediator.Send(new GetContentByIdQuery(id));
                if (content == null)
                {
                    return NotFound();
                }
                return Ok(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting content by id.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddContent(Content content)
        {
            ValidationResult result = await _validator.ValidateAsync(content);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                var createdContent = await _mediator.Send(new AddContentCommand(content));
                return CreatedAtAction(nameof(GetContentById), new { id = createdContent.Id }, createdContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new content.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContent(int id, Content content)
        {
            if (id != content.Id)
            {
                return BadRequest();
            }

            ValidationResult result = await _validator.ValidateAsync(content);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                await _mediator.Send(new UpdateContentCommand(id, content));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the content.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(int id)
        {
            try
            {
                await _mediator.Send(new DeleteContentCommand(id));
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

