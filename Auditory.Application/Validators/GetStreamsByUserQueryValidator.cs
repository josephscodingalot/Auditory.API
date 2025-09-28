using Auditory.Application.Queries;
using FluentValidation;

namespace Auditory.Application.Validators;

public class GetStreamsByUserQueryValidator : AbstractValidator<GetStreamsByUserQuery>
{
    public GetStreamsByUserQueryValidator()
    {
        RuleFor(x => x.userName)
            .NotEmpty().WithMessage("User name must not be empty.")
            .MinimumLength(3).WithMessage("User name must be at least 3 characters long.");
    }
}