using Claims.WebApi.Contracts;
using FluentValidation;

namespace Claims.WebApi.Validators
{
    public class CoverRequestValidator : AbstractValidator<CreateCoverRequest>
    {
        public CoverRequestValidator()
        {
            RuleFor(request => request.StartDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("StartDate cannot be in the past");

            RuleFor(request => request.EndDate)
                .Must((request, endDate) => endDate <= request.StartDate.AddYears(1))
                .WithMessage("Total insurance period cannot exceed 1 year");
        }
    }
}