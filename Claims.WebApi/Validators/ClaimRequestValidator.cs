using Claims.WebApi.Contracts;
using FluentValidation;

namespace Claims.WebApi.Validators
{
    public class ClaimRequestValidator : AbstractValidator<CreateClaimRequest>
    {
        public ClaimRequestValidator()
        {
            RuleFor(request => request.DamageCost)
                .LessThanOrEqualTo(100_000)
                .WithMessage("DamageCost cannot exceed 100.000");
        }
    }
}

