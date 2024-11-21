using FluentValidation;
using ContentService.Models;

namespace ContentService.Validators
{
    public class ContentValidator : AbstractValidator<Content>
    {
        public ContentValidator()
        {
            RuleFor(content => content.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(5, 100).WithMessage("Title must be between 5 and 100 characters.");

            RuleFor(content => content.Body)
                .NotEmpty().WithMessage("Body is required.")
                .Length(10, 1000).WithMessage("Body must be between 10 and 1000 characters.");
        }
    }
}

